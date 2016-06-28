using System;
using System.Runtime.InteropServices;
using PepperSharp;

namespace MouseCursor
{
    public class MouseCursor : PPInstance
    {

        public MouseCursor(IntPtr handle) : base(handle) { }

        ~MouseCursor() { System.Console.WriteLine("MouseCursor destructed"); }

        public override bool Init(int argc, string[] argn, string[] argv)
        {
            PPB_Console.Log(Instance, PP_LogLevel.Log, "Hello from MouseCursor using C#");
            MakeCustomCursor();

            return true;
        }

        public override void HandleMessage(object message)
        {
            if (!(message is Int32))
            {
                Console.WriteLine("Unexpected message.");
            }

            PP_MouseCursor_Type cursor = (PP_MouseCursor_Type)message;
            if (cursor == PP_MouseCursor_Type.Custom)
            {
                var hotSpot = new PPPoint(16,16);
                PPB_MouseCursor.SetCursor(Instance, cursor, custom_cursor_, hotSpot);
            }
            else
            {
                var refPoint = new PPPoint();
                PPB_MouseCursor.SetCursor(Instance, cursor, new PP_Resource(), refPoint);
            }

        }

        PP_Resource custom_cursor_;
        void MakeCustomCursor()
        {
            var size = new PPSize(32,32);

            custom_cursor_ = PPB_ImageData.Create(Instance, PP_ImageDataFormat.Bgra_premul, size, PP_Bool.PP_TRUE);

            DrawCircle(16, 16, 9, 14, 0.8f, 0.8f, 0);
            DrawCircle(11, 12, 2, 3, 0, 0, 0);
            DrawCircle(21, 12, 2, 3, 0, 0, 0);
            DrawHorizontalLine(12, 20, 21, 0.5f, 0, 0, 1.0f);
        }

        void DrawCircle(int cx, int cy, float alpha_radius, float radius,
                float r, float g, float b)
        {
            var desc = new PP_ImageDataDesc();
            int[] data = null;
            IntPtr dataPtr = IntPtr.Zero;
            if (PPB_ImageData.Describe(custom_cursor_, out desc) == PP_Bool.PP_FALSE)
                return;

            dataPtr = PPB_ImageData.Map(custom_cursor_);
            if (dataPtr == IntPtr.Zero)
                return;

            var size = desc.size;

            data = new int[size.width * desc.size.height];

            Marshal.Copy(dataPtr, data, 0, data.Length);

            // It's less efficient to loop over the entire image this way, but the
            // image is small, and this is simpler.
            for (int y = 0; y < size.width; ++y)
            {
                for (int x = 0; x < size.width; ++x)
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

                        data[y * size.width + x] = (int)MakeColor(r, g, b, a);
                    }
                }
            }

            Marshal.Copy(data, 0, dataPtr, data.Length);

        }

        void DrawHorizontalLine(int x1, int x2, int y,
                        float r, float g, float b, float a)
        {
            var desc = new PP_ImageDataDesc();
            int[] data = null;
            IntPtr dataPtr = IntPtr.Zero;
            if (PPB_ImageData.Describe(custom_cursor_, out desc) == PP_Bool.PP_FALSE)
                return;

            dataPtr = PPB_ImageData.Map(custom_cursor_);
            if (dataPtr == IntPtr.Zero)
                return;

            var size = desc.size;

            data = new int[size.width * desc.size.height];

            Marshal.Copy(dataPtr, data, 0, data.Length);

            for (int x = x1; x <= x2; ++x)
            {
                data[y * size.width + x] = (int)MakeColor(r, g, b, a);
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
