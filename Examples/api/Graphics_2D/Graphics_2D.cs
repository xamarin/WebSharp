using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using PepperSharp;

namespace Graphics_2D
{
    public class Graphics_2D : Instance
    {

        Graphics2D context;
        PPSize size;
        PPPoint mouse;
        bool mouseDown;
        byte[] buffer;
        uint[] palette = new uint[256];
        float deviceScale;
        Random random;
        const int mouseRadius = 15;

        public Graphics_2D(IntPtr handle) : base(handle)
        {
            ViewChanged += OnViewChanged;
            Initialize += OnInitialize;
            MouseDown += OnMouseDownOrMove;
            MouseMove += OnMouseDownOrMove;
            MouseUp += OnMouseUp;
            
        }
        bool first = true;
        private void OnViewChanged(object sender, View view)
        {
            var viewRect = view.Rect;
            deviceScale = view.DeviceScale;
            var newSize = new PPSize((int)(viewRect.size.width * deviceScale),
                                    (int)(viewRect.size.height * deviceScale));

            if (!CreateContext(newSize))
                return;

            // When flush_context_ is null, it means there is no Flush callback in
            // flight. This may have happened if the context was not created
            // successfully, or if this is the first call to DidChangeView (when the
            // module first starts). In either case, start the main loop.
            if (first) 
            {
                first = false;
                MainLoop (0);
            }
        }

        ~Graphics_2D() { System.Console.WriteLine("Graphics_2D destructed"); }

        private void OnInitialize(object sender, InitializeEventArgs args)
        {
            LogToConsole(PPLogLevel.Log, $"Hello from PepperSharp using C#");
            RequestInputEvents(PPInputEventClass.Mouse);
            int seed = 1;
            random = new Random(seed);
            CreatePalette();
        }


        private void OnMouseDownOrMove(object sender, MouseEventArgs e)
        {
            if (buffer == null)
            {
                e.Handled = true;
                return;
            }

            if (e.Buttons == PPInputEventMouseButton.None)
            {
                e.Handled = true;
                return;
            }

            PPPoint pos = e.Position;
            mouse.x = (int)(pos.X * deviceScale);
            mouse.y = (int)(pos.Y * deviceScale);

            mouseDown = true;

            e.Handled = true;
        }

        private void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (buffer == null)
            {
                e.Handled = true;
                return;
            }

            mouseDown = false;
            e.Handled = true;
        }

        bool CreateContext(PPSize new_size)
        {
            
            context = new Graphics2D(this, new_size, true);

            // Call SetScale before BindGraphics so the image is scaled correctly on
            // HiDPI displays.
            context.Scale = (1.0f / deviceScale);

            var osize = context.Size;

            if (!BindGraphics(context))
            {
                Console.WriteLine("Unable to bind 2d context!");
                return false;
            }

            // Allocate a buffer of palette entries of the same size as the new context.
            buffer = new byte[new_size.Width * new_size.Height];
            size = new_size;

            return true;
        }

        void CreatePalette()
        {
            for (int i = 0; i < 64; ++i)
            {
                // Black -> Red
                palette[i] = MakeColor((byte)(i * 2), 0, 0);
                palette[i + 64] = MakeColor((byte)(128 + i * 2), 0, 0);
                // Red -> Yellow
                palette[i + 128] = MakeColor(255, (byte)(i * 4), 0);
                // Yellow -> White
                palette[i + 192] = MakeColor(255, 255, (byte)(i * 4));
            }

        }

        uint MakeColor(byte r, byte g, byte b)
        {
            byte a = 255;
            var format = ImageData.NativeImageDataFormat;
            
            if (format == PPImageDataFormat.BgraPremul)
            {
                return (uint)((a << 24) | (r << 16) | (g << 8) | b);
            }
            else
            {
                return (uint)((a << 24) | (b << 16) | (g << 8) | r);
            }

        }

        const uint RAND_MAX = 0x7fff;
        byte RandUint8(byte min, byte max)
        {
            var r = random.Next();
            var result = (byte)((r * (max - min + 1) / RAND_MAX) + min);
            return result;
        }

        void Update()
        {
            // Old-school fire technique cribbed from
            // http://ionicsolutions.net/2011/12/30/demo-fire-effect/
            UpdateCoals();
            DrawMouse();
            UpdateFlames();
        }

        void UpdateCoals()
        {
            int width = size.Width;
            int height = size.Height;
            uint span = 0;

            // Draw two rows of random values at the bottom.
            for (int y = height - 2; y < height; ++y)
            {
                var offset = y * width;
                for (int x = 0; x < width; ++x)
                {
                    // On a random chance, draw some longer strips of brighter colors.
                    if (span == 1 || RandUint8(1, 4) == 1)
                    {
                        if (span == 0)
                            span = RandUint8(10, 20);
                        buffer[offset + x] = RandUint8(128, 255);
                        span--;
                    }
                    else
                    {
                        buffer[offset + x] = RandUint8(32, 96);
                    }
                }
            }
        }

        void UpdateFlames()
        {
            int width = size.Width;
            int height = size.Height;
            for (int y = 1; y < height - 1; ++y)
            {
                uint offset = (uint)(y * width);
                for (int x = 1; x < width - 1; ++x)
                {
                    int sum = 0;
                    sum += buffer[offset - width + x - 1];
                    sum += buffer[offset - width + x + 1];
                    sum += buffer[offset + x - 1];
                    sum += buffer[offset + x + 1];
                    sum += buffer[offset + width + x - 1];
                    sum += buffer[offset + width + x];
                    sum += buffer[offset + width + x + 1];
                    buffer[offset - width + x] = (byte)(sum / 7);
                }
            }
        }

        void DrawMouse()
        {
            if (!mouseDown)
                return;

            int width = size.Width;
            int height = size.Height;

            // Draw a circle at the mouse position.
            int radius = (int)(mouseRadius * deviceScale);
            int cx = mouse.x;
            int cy = mouse.y;
            int minx = cx - radius <= 0 ? 1 : cx - radius;
            int maxx = cx + radius >= width ? width - 1 : cx + radius;
            int miny = cy - radius <= 0 ? 1 : cy - radius;
            int maxy = cy + radius >= height ? height - 1 : cy + radius;
            for (int y = miny; y < maxy; ++y)
            {
                for (int x = minx; x < maxx; ++x)
                {
                    if ((x - cx) * (x - cx) + (y - cy) * (y - cy) < radius * radius)
                        buffer[y * width + x] = RandUint8(192, 255);
                }
            }
        }

        void Paint()
        {
            // See the comment above the call to ReplaceContents below.
            var format = ImageData.NativeImageDataFormat;
            bool kDontInitToZero = false;
            var dontInitToZero = kDontInitToZero;
            var imageData = new ImageData(this, format, size, dontInitToZero);

            int[] data = null;
            IntPtr dataPtr = imageData.Data;
            if (dataPtr == IntPtr.Zero)
                return;

            data = new int[(imageData.Size.Width * imageData.Size.Height)];


            var num_pixels = (size.Width * size.Height);
            uint offset = 0;

            for (uint i = 0; i < num_pixels; ++i)
            {
                data[offset] = (int)palette[buffer[offset]];
                offset++;
            }

            Marshal.Copy(data, 0, dataPtr, data.Length);

            // Using Graphics2D::ReplaceContents is the fastest way to update the
            // entire canvas every frame. According to the documentation:
            //
            //   Normally, calling PaintImageData() requires that the browser copy
            //   the pixels out of the image and into the graphics context's backing
            //   store. This function replaces the graphics context's backing store
            //   with the given image, avoiding the copy.
            //
            //   In the case of an animation, you will want to allocate a new image for
            //   the next frame. It is best if you wait until the flush callback has
            //   executed before allocating this bitmap. This gives the browser the
            //   option of caching the previous backing store and handing it back to
            //   you (assuming the sizes match). In the optimal case, this means no
            //   bitmaps are allocated during the animation, and the backing store and
            //   "front buffer" (which the module is painting into) are just being
            //   swapped back and forth.
            //
            context.ReplaceContents(imageData);

        }

        void MainLoop(int dt)
        {
            if (context.IsEmpty)
            {
                return;
            }

            Update();
            Paint();

            FlushContext ();
        }

        async void FlushContext()
        {
            var flushResult = await context.FlushAsync ();
            MainLoop ((int)flushResult);
        }

    }
}
