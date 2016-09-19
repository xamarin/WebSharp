using System;
using System.Runtime.InteropServices;
using PepperSharp;

namespace MouseCursor
{
    public class MouseCursor : Instance
    {

        public MouseCursor(IntPtr handle) : base(handle)
        {
            HandleMessage += OnHandleMessage;
            Initialize += OnInitialize;
        }

        ~MouseCursor() { System.Console.WriteLine("MouseCursor destructed"); }

        private void OnInitialize(object sender, InitializeEventArgs args)
        {
            LogToConsole(PPLogLevel.Log, "Hello from MouseCursor using C#");
            MakeCustomCursor();
        }

        private void OnHandleMessage(object sender, Var varMessage)
        {
            if (!varMessage.IsInt)
            {
                Console.WriteLine("Unexpected message.");
            }

            PPMouseCursorType cursor = (PPMouseCursorType)varMessage.AsInt();
            if (cursor == PPMouseCursorType.Custom)
            {
                var hotSpot = new PPPoint(16,16);
                SetCursor(cursor, customCursor, hotSpot);
            }
            else
            { 
                SetCursor(cursor);
            }

        }

        ImageData customCursor;
        void MakeCustomCursor()
        {
            var size = new PPSize(32,32);

            customCursor = new ImageData(this, PPImageDataFormat.BgraPremul, size, true);

            DrawCircle(16, 16, 9, 14, 0.8f, 0.8f, 0);
            DrawCircle(11, 12, 2, 3, 0, 0, 0);
            DrawCircle(21, 12, 2, 3, 0, 0, 0);
            DrawHorizontalLine(12, 20, 21, 0.5f, 0, 0, 1.0f);
        }

        void DrawCircle(int cx, int cy, float alpha_radius, float radius,
                float r, float g, float b)
        {
            int[] data = null;
            IntPtr dataPtr = customCursor.Data;
            if (dataPtr == IntPtr.Zero)
                return;

            var size = customCursor.Size;

            data = new int[size.Width * customCursor.Size.Height];

            Marshal.Copy(dataPtr, data, 0, data.Length);

            // It's less efficient to loop over the entire image this way, but the
            // image is small, and this is simpler.
            for (int y = 0; y < size.Width; ++y)
            {
                for (int x = 0; x < size.Width; ++x)
                {
                    int dx = (x - cx);
                    int dy = (y - cy);
                    float dist = (float)Math.Sqrt(dx * dx + dy * dy);

                    if (dist < radius)
                    {
                        float a;
                        if (dist > alpha_radius)
                        {
                            a = 1.0f - (dist - alpha_radius) / (radius - alpha_radius);
                        }
                        else
                        {
                            a = 1.0f;
                        }

                        data[y * size.Width + x] = (int)MakeColor(r, g, b, a);
                    }
                }
            }

            Marshal.Copy(data, 0, dataPtr, data.Length);

        }

        void DrawHorizontalLine(int x1, int x2, int y,
                        float r, float g, float b, float a)
        {

            int[] data = null;
            IntPtr dataPtr = customCursor.Data;
            if (dataPtr == IntPtr.Zero)
                return;

            var size = customCursor.Size;

            data = new int[size.Width * customCursor.Size.Height];
            data = new int[size.Width * customCursor.Size.Height];

            Marshal.Copy(dataPtr, data, 0, data.Length);

            for (int x = x1; x <= x2; ++x)
            {
                data[y * size.Width + x] = (int)MakeColor(r, g, b, a);
            }

            Marshal.Copy(data, 0, dataPtr, data.Length);
        }

        uint MakeColor(float r, float g, float b, float a)
        {
            // Since we're using premultiplied alpha
            // (PP_IMAGEDATAFORMAT_BGRA_PREMUL), we have to multiply each
            // color component by the alpha value.
            byte a8 = (byte)(255 * a);
            byte r8 = (byte)(255 * r * a);
            byte g8 = (byte)(255 * g * a);
            byte b8 = (byte)(255 * b * a);
            return (uint)((a8 << 24) | (r8 << 16) | (g8 << 8) | b8);
        }

    }
}
