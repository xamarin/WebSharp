using System;
using System.Runtime.InteropServices;

using PepperSharp;

namespace GamePad
{
    public class GamePad : Instance
    {

        Graphics2D graphics2DContext;
        ImageData pixelBuffer;

        public GamePad(IntPtr handle) : base(handle)
        {
            ViewChanged += OnViewChanged;
            Initialize += OnInitialize;
        }

        // Indicate whether a flush is pending.  This can only be called from the
        // main thread; it is not thread safe.
        bool FlushPending { get; set; }

        private void OnInitialize(object sender, InitializeEventArgs args)
        {
            LogToConsoleWithSource(PPLogLevel.Log, "GamePad", "There be dragons.");
        }

        private void OnViewChanged(object sender, View view)
        {
            var position = view.Rect;

            if (position.Size.Width == Width &&
                position.Size.Height == Height)
                return;  // Size didn't change, no need to update anything.
           
            // Create a new device context with the new size.
            DestroyContext();
            CreateContext(position.Size);
            
            // Delete the old pixel buffer and create a new one.
            if (pixelBuffer != null && !pixelBuffer.IsEmpty)
                pixelBuffer.Dispose();

            if (IsContextValid)
            {
                pixelBuffer = new ImageData(this, PPImageDataFormat.BgraPremul,
                    Size, false);
            }

            Paint();
        }

        void FillRect(ImageData image,
              int left,
              int top,
              int width,
              int height,
              uint color)
        {

            int[] data = null;
            IntPtr dataPtr = image.Data;
            if (dataPtr == IntPtr.Zero)
                return;

            data = new int[(image.Size.Width * image.Size.Height)];

            Marshal.Copy(dataPtr, data, 0, data.Length);

            var stride = image.Size.Width;
            for (int y = Math.Max(0, top);
                 y < Math.Min(image.Size.Height - 1, top + height);
                 y++)
            {
                for (int x = Math.Max(0, left);
                     x < Math.Min(image.Size.Width - 1, left + width);
                     x++)
                {
                    unchecked { data[y * stride + x] = (int)color; }
                }
            }

            Marshal.Copy(data, 0, dataPtr, data.Length);

        }

        GamepadsSampleData gamepadData = new GamepadsSampleData();
        void Paint()
        {
            // Clear the background.
            FillRect(pixelBuffer, 0, 0, Width, Height, 0xfff0f0f0);

            // Get current gamepad data.
            GamepadsSample(out gamepadData);

            //// Draw the current state for each connected gamepad.
            for (int p = 0; p < gamepadData.Length; ++p)
            {
                int width2 = (int)(Width / gamepadData.Length / 2);
                int height2 = (int)(Height / 2);
                int offset = width2 * 2 * p;
                var pad = gamepadData[p];

                if (!pad.IsConnected)
                    continue;

                // Draw axes.
                for (int i = 0; i < pad.AxesCount; i += 2)
                {
                    int x = (int)(pad.Axes(i + 0) * width2 + width2) + offset;
                    int y = (int)(pad.Axes(i + 1) * height2 + height2);
                    uint box_bgra = 0x80000000;  // Alpha 50%.
                    FillRect(pixelBuffer, x - 3, y - 3, 7, 7, box_bgra);
                }

                // Draw buttons.
                for (int i = 0; i < pad.ButtonCount; ++i)
                {
                    float button_val = pad.Button(i);
                    uint colour = (uint)((button_val * 192) + 63) << 24;
                    int x = i * 8 + 10 + offset;
                    int y = 10;
                    FillRect(pixelBuffer, x - 3, y - 3, 7, 7, colour);
                }
            }

            // Output to the screen.
            FlushPixelBuffer();
        }

        void FlushPixelBuffer()
        {
            if (!IsContextValid)
                return;

            // Note that the pixel lock is held while the buffer is copied into the
            // device context and then flushed.
            PPRect srcRect = new PPRect(Size);
            graphics2DContext.PaintImageData(pixelBuffer, PPPoint.Zero, srcRect);

            if (FlushPending)
                return;

            FlushPending = true;

            graphics2DContext.Flush ();
        }

        void DestroyContext()
        {
            if (!IsContextValid)
                return;
            if (graphics2DContext != null)
                graphics2DContext.Dispose();
        }

        void CreateContext(PPSize size)
        {
            if (IsContextValid)
                return;

            var isAlwaysOpaque = false;
            graphics2DContext = new Graphics2D(this, size, isAlwaysOpaque);

            graphics2DContext.Flushed +=
                (s, e) => {
                    FlushPending = false;
                    Paint ();
                };

            if (!BindGraphics(graphics2DContext))
            {
                LogToConsole(PPLogLevel.Error, "Couldn't bind the device context\n");
            }
            else
                LogToConsole(PPLogLevel.Log, "Bound the device context\n");
        }

        bool IsContextValid
        {
            get
            {
                return graphics2DContext != null ? !graphics2DContext.IsEmpty : false;
            }
        }

        int Width
        {
            get
            {
                return Size.Width;
            }
        }

        int Height
        {
            get
            {
                return Size.Height;
            }
        }

        PPSize Size
        {
            get
            {
                if (IsContextValid)
                {
                    return graphics2DContext.Size;
                }
                return PPSize.Zero;
            }

        }
    }
}
