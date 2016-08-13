using System;
using System.Runtime.InteropServices;

using PepperSharp;

namespace GamePad
{
    public class GamePad : Instance
    {

        Graphics2D graphics_2d_context_;
        ImageData pixel_buffer_;

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
            CreateContext(position.size);
            
            // Delete the old pixel buffer and create a new one.
            if (pixel_buffer_ != null && !pixel_buffer_.IsEmpty)
                pixel_buffer_.Dispose();

            //pixel_buffer_.ppresource = 0;
            if (IsContextValid)
            {
                pixel_buffer_ = new ImageData(this, PPImageDataFormat.BgraPremul,
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

            var stride = image.Size.width;
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

        PPGamepadsSampleData gamepad_data = new PPGamepadsSampleData();
        void Paint()
        {
            // Clear the background.
            FillRect(pixel_buffer_, 0, 0, Width, Height, 0xfff0f0f0);

            // Get current gamepad data.
            PPBGamepad.Sample(this, out gamepad_data);

            //// Draw the current state for each connected gamepad.
            for (int p = 0; p < gamepad_data.Length; ++p)
            {
                int width2 = (int)(Width / gamepad_data.Length / 2);
                int height2 = (int)(Height / 2);
                int offset = width2 * 2 * p;
                var pad = gamepad_data[p];

                if (!pad.IsConnected)
                    continue;

                // Draw axes.
                for (int i = 0; i < pad.AxesCount; i += 2)
                {
                    int x = (int)(pad.Axes(i + 0) * width2 + width2) + offset;
                    int y = (int)(pad.Axes(i + 1) * height2 + height2);
                    uint box_bgra = 0x80000000;  // Alpha 50%.
                    FillRect(pixel_buffer_, x - 3, y - 3, 7, 7, box_bgra);
                }

                // Draw buttons.
                for (int i = 0; i < pad.ButtonCount; ++i)
                {
                    float button_val = pad.Button(i);
                    uint colour = (uint)((button_val * 192) + 63) << 24;
                    int x = i * 8 + 10 + offset;
                    int y = 10;
                    FillRect(pixel_buffer_, x - 3, y - 3, 7, 7, colour);
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
            graphics_2d_context_.PaintImageData(pixel_buffer_, PPPoint.Zero, srcRect);

            if (FlushPending)
                return;

            FlushPending = true;

            graphics_2d_context_.Flush(new CompletionCallback
                (
                    (result) =>
                    {
                        FlushPending = false;
                        Paint();

                    }

                ));
        }

        void DestroyContext()
        {
            if (!IsContextValid)
                return;
            if (graphics_2d_context_ != null)
                graphics_2d_context_.Dispose();
        }

        void CreateContext(PPSize size)
        {
            if (IsContextValid)
                return;

            var isAlwaysOpaque = false;
            graphics_2d_context_ = new Graphics2D(this, size, isAlwaysOpaque);

            if (!BindGraphics(graphics_2d_context_))
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
                return graphics_2d_context_ != null ? !graphics_2d_context_.IsEmpty : false;
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
                    return graphics_2d_context_.Size;
                }
                return PPSize.Zero;
            }

        }
    }
}
