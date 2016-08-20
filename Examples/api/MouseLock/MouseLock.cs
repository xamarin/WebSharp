using System;
using System.Runtime.InteropServices;
using PepperSharp;

namespace MouseLockInstance
{
    public class MouseLockInstance : MouseLock
    {

        PPSize size_ = PPSize.Zero;
        bool mouse_locked_;
        PPPoint mouse_movement_ = PPPoint.Zero;
        const int kCentralSpotRadius = 5;
        const uint kReturnKeyCode = 13;
        const uint kBackgroundColor = 0xff606060;
        const uint kForegroundColor = 0xfff08080;
        bool is_context_bound_;
        bool was_fullscreen_;
        int[] background_scanline_ = null;
        bool waiting_for_flush_completion_;

        // Indicate the direction of the mouse location relative to the center of the
        // view.  These values are used to determine which 2D quadrant the needle lies
        // in.
        enum MouseDirection
        {
            Left = 0,
            Right = 1,
            Up = 2,
            Down = 3
        }

        Graphics2D deviceContext;


        public MouseLockInstance(IntPtr handle) : base(handle)
        {
            // Setup our listeners for mouselock
            MouseLocked += OnMouseLocked;
            MouseUnLocked += OnMouseUnLocked;

            ViewChanged += OnViewChanged;
            Initialize += OnInitialize;

            MouseDown += OnMouseDown;
            MouseMove += OnMouseMove;

            KeyDown += OnKeyDown;
        }

        ~MouseLockInstance() { System.Console.WriteLine("MouseLock destructed"); }

        private void OnInitialize(object sender, InitializeEventArgs args)
        {
            LogToConsole(PPLogLevel.Log, "Hello from MouseLock using C#");
            RequestInputEvents(PPInputEventClass.Mouse | PPInputEventClass.Keyboard);
        }

        private void OnViewChanged(object sender, View view)
        {
            var viewRect = view.Rect;

            // DidChangeView can get called for many reasons, so we only want to
            // rebuild the device context if we really need to.

            if ((size_ == viewRect.Size) &&
                (was_fullscreen_ == view.IsFullScreen) && 
                is_context_bound_)
            {
                Log($"DidChangeView SKIP {viewRect.size.width}, {viewRect.size.height} " +
                    $"FULL= {view.IsFullScreen} " +
                    $"CTX Bound={is_context_bound_}");
                return;
            }

            Log($"DidChangeView DO {viewRect.Size.Width}, {viewRect.Size.Height} " +
                $"FULL={view.IsFullScreen} " +
                    $"CTX Bound={is_context_bound_}");

            size_ = viewRect.Size;
            deviceContext = new Graphics2D(this, size_, false);
            waiting_for_flush_completion_ = false;

            is_context_bound_ = BindGraphics(deviceContext);
            if (!is_context_bound_)
            {
                Log("Could not bind to 2D context\n.");
                return;
            }
            else
            {
                Log($"Bound to 2D context size {size_}.\n");
            }

            // Create a scanline for fill.
            background_scanline_ = new int[size_.Width];
            var bg_pixel = background_scanline_;
            for (int x = 0; x < size_.Width; ++x)
            {
                unchecked { bg_pixel[x] = (int)kBackgroundColor; };
            }

            // Remember if we are fullscreen or not
            was_fullscreen_ = view.IsFullScreen;

            // Paint this context
            Paint();
        }

        private void OnMouseUnLocked(object sender, EventArgs e)
        {
            if (mouse_locked_)
            {
                Log("Mouselock unlocked.\n");
                mouse_locked_ = false;
                Paint();
            }
        }

        private void OnMouseLocked(object sender, PPError result)
        {
            mouse_locked_ = result == PPError.Ok;
            if (result != PPError.Ok)
            {
                Log($"Mouselock failed with error number {result}.\n");
            }
            mouse_movement_ = PPPoint.Zero;
            Paint();

        }

        ImageData PaintImage(PPSize size)
        {
            
            var image = new ImageData(this, PPImageDataFormat.BgraPremul, size, false);

            if (image.IsEmpty) // || image.data() == NULL)
            {
                Log("Skipping image.\n");
                return image;
            }

            ClearToBackground(image);

            DrawCenterSpot(image, kForegroundColor);
            DrawNeedle(image, kForegroundColor);
            return image;
        }

        void ClearToBackground(ImageData image)
        {
            if (image.IsEmpty)
            {
                Log("ClearToBackground with NULL image.");
                return;
            }
            if (background_scanline_ == null)
            {
                Log("ClearToBackground with no scanline.");
                return;
            }

            if (image.IsEmpty)
                return;

            int[] data = null;
            IntPtr dataPtr = image.Data;
            if (dataPtr == IntPtr.Zero)
                return;
            data = new int[(image.Size.Width * image.Size.Height)];

            Marshal.Copy(dataPtr, data, 0, data.Length);

            int image_height = image.Size.Height; ;
            int image_width = image.Size.Width; ;

            for (int y = 0; y < image_height; ++y)
            {
                Array.Copy(background_scanline_, 0, data, y * image_width, background_scanline_.Length);
            }

            Marshal.Copy(data, 0, dataPtr, data.Length);

        }

        void DrawCenterSpot(ImageData image,
                                       uint spot_color)
        {
            if (image.IsEmpty)
            {
                Log("DrawCenterSpot with NULL image");
                return;
            }

            int[] data = null;
            IntPtr dataPtr = image.Data;
            if (dataPtr == IntPtr.Zero)
                return;
            data = new int[(image.Size.Width * image.Size.Height)];

            Marshal.Copy(dataPtr, data, 0, data.Length);

            // Draw the center spot.  The ROI is bounded by the size of the spot, plus
            // one pixel.
            int center_x = image.Size.Width / 2;
            int center_y = image.Size.Height / 2;
            int region_of_interest_radius = kCentralSpotRadius + 1;

            var left_top = new PPPoint(Math.Max(0, center_x - region_of_interest_radius),
                Math.Max(0, center_x - region_of_interest_radius));

            var right_bottom = new PPPoint(Math.Min(image.Size.Width, center_x + region_of_interest_radius),
                Math.Min(image.Size.Height, center_y + region_of_interest_radius));

            for (int y = left_top.Y; y < right_bottom.Y; ++y)
            {
                for (int x = left_top.X; x < right_bottom.X; ++x)
                {
                    if (MouseLockInstance.GetDistance(x, y, center_x, center_y) < kCentralSpotRadius)
                    {
                        unchecked { data[y * image.Stride / 4 + x] = (int)spot_color; }
                    }
                }
            }

            Marshal.Copy(data, 0, dataPtr, data.Length);
            
        }

        void DrawNeedle(ImageData image,
                                   uint needle_color)
        {
            if (image.IsEmpty)
            {
                Log("DrawNeedle with NULL image");
                return;
            }

            int[] data = null;
            IntPtr dataPtr = image.Data;
            if (dataPtr == IntPtr.Zero)
                return;
            data = new int[(image.Size.Width * image.Size.Height)];

            Marshal.Copy(dataPtr, data, 0, data.Length);

            if (GetDistance(mouse_movement_.X, mouse_movement_.Y, 0, 0) <=
                kCentralSpotRadius)
            {
                return;
            }

            int abs_mouse_x = Math.Abs(mouse_movement_.X);
            int abs_mouse_y = Math.Abs(mouse_movement_.Y);
            int center_x = image.Size.Width / 2;
            int center_y = image.Size.Height / 2;

            var vertex = new PPPoint(mouse_movement_.X + center_x,
                                    mouse_movement_.Y + center_y);

            var anchor_1 = new PPPoint();
            var anchor_2 = new PPPoint();

            
            MouseDirection direction = MouseDirection.Left;

            if (abs_mouse_x >= abs_mouse_y)
            {
                anchor_1.X = (center_x);
                anchor_1.Y = (center_y - kCentralSpotRadius);
                anchor_2.X = (center_x);
                anchor_2.Y = (center_y + kCentralSpotRadius);
                direction = (mouse_movement_.X < 0) ? MouseDirection.Left : MouseDirection.Right;
                if (direction == MouseDirection.Left)
                    anchor_1.Swap(ref anchor_2);
            }
            else
            {
                anchor_1.X = (center_x + kCentralSpotRadius);
                anchor_1.Y = (center_y);
                anchor_2.X = (center_x - kCentralSpotRadius);
                anchor_2.Y = (center_y);
                direction = (mouse_movement_.Y < 0) ? MouseDirection.Up : MouseDirection.Down;
                if (direction == MouseDirection.Up)
                    anchor_1.Swap(ref anchor_2);
            }

            var left_top = new PPPoint(Math.Max(0, center_x - abs_mouse_x),
                                        Math.Max(0, center_y - abs_mouse_y));

            var right_bottom = new PPPoint(Math.Min(image.Size.Width, center_x + abs_mouse_x),
                                    Math.Min(image.Size.Height, center_y + abs_mouse_y));

            for (int y = left_top.Y; y < right_bottom.Y; ++y)
            {
                for (int x = left_top.X; x < right_bottom.X; ++x)
                {
                    bool within_bound_1 = ((y - anchor_1.Y) * (vertex.X - anchor_1.X)) >
                      ((vertex.Y - anchor_1.Y) * (x - anchor_1.X));
                    bool within_bound_2 = ((y - anchor_2.Y) * (vertex.X - anchor_2.X)) <
                                          ((vertex.Y - anchor_2.Y) * (x - anchor_2.X));
                    bool within_bound_3 = (direction == MouseDirection.Up && y < center_y) ||
                                          (direction == MouseDirection.Down && y > center_y) ||
                                          (direction == MouseDirection.Left && x < center_x) ||
                                          (direction == MouseDirection.Right && x > center_x);

                    if (within_bound_1 && within_bound_2 && within_bound_3)
                    {
                        unchecked { data[y * image.Stride / 4 + x] = (int)needle_color; }
                    }
                }
            }

            Marshal.Copy(data, 0, dataPtr, data.Length);
        }

        // Return the Cartesian distance between two points.
        static float GetDistance(int point_1_x,
                           int point_1_y,
                           int point_2_x,
                           int point_2_y)
        {
            float v1 = point_1_x - point_2_x, v2 = point_1_y - point_2_y;
            return (float)Math.Sqrt((v1 * v1) + (v2 * v2));

        }

        void Paint()
        {

            // If we are already waiting to paint...
            if (waiting_for_flush_completion_)
            {
                return;
            }

            var image = PaintImage(size_);
            if (image.IsEmpty)
            {
                Log("Could not create image data\n");
                return;
            }

            deviceContext.ReplaceContents(image);
            waiting_for_flush_completion_ = true;

            deviceContext.Flush(new CompletionCallback(DidFlush));
        }

        void DidFlush(PPError result)
        {
            if (result != 0)
                Log("Flushed failed with error number %d.\n", result);
            waiting_for_flush_completion_ = false;
        }

        void Log(string format, params object[] args)
        {
            string message = string.Format(format, args);
            LogToConsole(PPLogLevel.Error, message);
        }

        private void OnMouseMove(object sender, MouseEventArgs mouseEvent)
        {
            mouse_movement_ = mouseEvent.Movement;
            Paint();
            mouseEvent.Handled = true;
        }

        private void OnMouseDown(object sender, MouseEventArgs mouseEvent)
        {
            if (mouse_locked_)
            {
                UnlockMouse();
            }
            else
            {
                LockMouse();

            }
            mouseEvent.Handled = true;
        }

        private void OnKeyDown(object sender, KeyboardEventArgs keyboardEvent)
        {
            // Switch in and out of fullscreen when 'Enter' is hit
            if (keyboardEvent.KeyCode == kReturnKeyCode)
            {
                // Ignore switch if in transition
                if (!is_context_bound_)
                {
                    keyboardEvent.Handled = true;
                    return;

                }

                if (PPBFullscreen.IsFullscreen(this) == PPBool.True)
                {
                    if (PPBFullscreen.SetFullscreen(this, PPBool.False) != PPBool.True)
                    {
                        Log("Could not leave fullscreen mode\n");
                    }
                    else
                    {
                        is_context_bound_ = false;
                    }
                }
                else
                {
                    if (PPBFullscreen.SetFullscreen(this, PPBool.True) != PPBool.True)
                    {
                        Log("Could not enter fullscreen mode\n");
                    }
                    else
                    {
                        is_context_bound_ = false;
                    }
                }

            }
            keyboardEvent.Handled = true;
        }
    }
}
