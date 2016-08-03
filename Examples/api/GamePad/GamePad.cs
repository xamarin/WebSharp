using System;
using System.Runtime.InteropServices;

using PepperSharp;

namespace GamePad
{
    public class GamePad : Instance
    {

        PPResource graphics_2d_context_;
        PPResource pixel_buffer_;

        // Indicate whether a flush is pending.  This can only be called from the
        // main thread; it is not thread safe.
        bool FlushPending { get; set; }

        public override bool Init(int argc, string[] argn, string[] argv)
        {
            LogToConsoleWithSource(PPLogLevel.Log, "GamePad", "There be dragons.");
            return true;
        }

        public override void DidChangeView(PPResource view)
        {
            var position = new PPRect();
            var result = PPBView.GetRect(view, out position);
            if (position.Size.Width == Width &&
                position.Size.Height == Height)
                return;  // Size didn't change, no need to update anything.

            // Create a new device context with the new size.
            DestroyContext();
            CreateContext(position.size);

            // Delete the old pixel buffer and create a new one.
            if (!pixel_buffer_.IsEmpty)
                PPBCore.ReleaseResource(pixel_buffer_);

            pixel_buffer_.ppresource = 0;
            if (IsContextValid)
            {
                pixel_buffer_ = PPBImageData.Create(this, PPImageDataFormat.BgraPremul,
                    Size, PPBool.False);
            }

            Paint();
        }

        void FillRect(PPResource image,
              int left,
              int top,
              int width,
              int height,
              uint color)
        {

            var desc = new PPImageDataDesc();
            int[] data = null;
            IntPtr dataPtr = IntPtr.Zero;
            if (PPBImageData.Describe(image, out desc) == PPBool.True)
            {
                dataPtr = PPBImageData.Map(image);
                if (dataPtr == IntPtr.Zero)
                    return;
                data = new int[(desc.size.width * desc.size.height)];

                Marshal.Copy(dataPtr, data, 0, data.Length);

            }

            var stride = desc.size.width;
            for (int y = Math.Max(0, top);
                 y < Math.Min(desc.size.Height - 1, top + height);
                 y++)
            {
                for (int x = Math.Max(0, left);
                     x < Math.Min(desc.size.Width - 1, left + width);
                     x++)
                {
                    unchecked { data[y * stride + x] = (int)color; }
                }
            }

            Marshal.Copy(data, 0, dataPtr, data.Length);

            // Clean up after ourselves
            PPBImageData.Unmap(image);
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
            PPBGraphics2D.PaintImageData(graphics_2d_context_, pixel_buffer_, PPPoint.Zero, srcRect);

            if (FlushPending)
                return;

            FlushPending = true;

            PPBGraphics2D.Flush(graphics_2d_context_, new CompletionCallback
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

            bool kIsAlwaysOpaque = false;
            var isAlwaysOpaque = new PPBool();
            isAlwaysOpaque = kIsAlwaysOpaque ? PPBool.True : PPBool.False;
            graphics_2d_context_ = PPBGraphics2D.Create(this, size, isAlwaysOpaque);

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
                    var osize = new PPSize();
                    var oopaque = new PPBool();

                    PPBGraphics2D.Describe(graphics_2d_context_, out osize, out oopaque);
                    return osize;
                }
                return PPSize.Zero;
            }

        }
    }
}
