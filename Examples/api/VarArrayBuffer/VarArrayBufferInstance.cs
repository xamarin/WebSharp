using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Diagnostics;
using PepperSharp;

namespace VarArrayBufferInstance
{
    public class VarArrayBufferInstance : Instance
    {
        const uint BLUE = 0xff4040ffu;
        const uint BLACK = 0xff000000u;
        const uint HISTOGRAM_SIZE = 256u;

        Graphics2D graphics2DContext;

        /// A queue of images to paint. We must maintain a queue because we can not
        /// call pp::Graphics2D::Flush while a Flush is already pending.
        Queue<ImageData> paintQueue = new Queue<ImageData>();

        /// The size of our rectangle in the DOM, as of the last time DidChangeView
        /// was called.
        PPSize size = new PPSize();

        /// true if we are flushing.
        bool isFlushing;

        /// Stores the most recent histogram so that we can re-draw it if we get
        /// resized.
        double[] histogram = new double[HISTOGRAM_SIZE];

        public VarArrayBufferInstance(IntPtr handle) : base(handle)
        {
            ViewChanged += OnViewChanged;
            HandleMessage += OnHandleMessage;
            Initialize += OnInitialize;
        }

        private void OnInitialize(object sender, InitializeEventArgs args)
        {
            LogToConsole(PPLogLevel.Log, "VarArrayBuffer");
        }

        /// Handler for messages coming in from the browser via postMessage().  The
        /// @a var_message can contain anything: a JSON string; a string that encodes
        /// method names and arguments; etc.
        ///
        /// In this case, we only handle <code>pp::VarArrayBuffer</code>s. When we
        /// receive one, we compute and display a histogram based on its contents.
        ///
        /// @param[in] var_message The message posted by the browser.
        private void OnHandleMessage(object sender, Var var_message)
        {
            if (var_message.IsArrayBuffer)
            {
                var buffer = new VarArrayBuffer(var_message);
                ComputeHistogram(buffer);
                DrawHistogram();
            }
        }

        private void OnViewChanged(object sender, View view)
        {
            var viewRect = view.Rect;

            if (size != viewRect.Size)
            {
                size = viewRect.Size;
                bool isAlwaysOpaque = true;
                graphics2DContext = new Graphics2D(this, viewRect.Size, isAlwaysOpaque);
                graphics2DContext.Flushed += DidFlush;
                BindGraphics(graphics2DContext);

                // The images in our queue are the wrong size, so we won't paint them.
                // We'll only draw the most recently computed histogram.
                paintQueue.Clear();
                DrawHistogram();
            }

        }

        void ComputeHistogram(VarArrayBuffer buffer)
        {

            var bufferSize = buffer.ByteLength;

            if (bufferSize == 0)
                return;

            var bufferData = buffer.Map();

            var max = double.MinValue;
            for (uint i = 0; i < bufferSize; i++)
            {
                var index = bufferData[i];
                histogram[index] += 1.0;

                if (histogram[index] > max)
                    max = histogram[index];
            }

            //Normalize
            for (uint i=0; i<HISTOGRAM_SIZE;i++)
            {
                histogram[i] /= max;
            }

        }

        void DrawHistogram()
        {
            var imageData = MakeBlankImageData(size);

            if (imageData.Data == IntPtr.Zero)
            {
                return;
            }
            for (uint i = 0; i < Math.Min(HISTOGRAM_SIZE, imageData.Size.Width); i++)
            {
                DrawBar(i, histogram[i], imageData);
            }

            if (!isFlushing)
                PaintAndFlush(imageData);
            else
                paintQueue.Enqueue(imageData);
        }

        /// Draw a bar of the appropriate height based on <code>value</code> at
        /// <code>column</code> in <code>image_data</code>. <code>value</code> must be
        /// in the range [0, 1].
        void DrawBar(uint column, double value, ImageData imageData)
        {
            Debug.Assert((value >= 0.0) && (value <= 1.0));
            //var desc = new PPImageDataDesc();

            int[] imageBuffer = null;
            IntPtr dataPtr = imageData.Data;
            if (dataPtr == IntPtr.Zero)
                return;

            imageBuffer = new int[(imageData.Size.Width * imageData.Size.Height)];
            Marshal.Copy(dataPtr, imageBuffer, 0, imageBuffer.Length);

            var imageHeight = imageData.Size.Height;
            var imageWidth = imageData.Size.Width;

            Debug.Assert(column < imageWidth);

            var barHeight = (int)(value * imageHeight);

            var blue = (int)MakeColor(BLUE);

            for (int i = 0; i < barHeight; i++)
            {
                var row = imageHeight - 1 - i;
                imageBuffer[row * imageWidth + column] = blue;
            }

            Marshal.Copy(imageBuffer, 0, dataPtr, imageBuffer.Length);
        }

        uint MakeColor(uint colorARGB)
        {
            byte a = (byte)((colorARGB >> 24 & 255));
            byte r = (byte)((colorARGB >> 16 & 255));
            byte g = (byte)((colorARGB >> 8 & 255));
            byte b = (byte)(colorARGB >> 0 & 255);

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

        /// Create and return a blank (all-black) <code>pp::ImageData</code> of the
        /// given <code>size</code>.
        ImageData MakeBlankImageData(PPSize size) {

            bool isInitToZero = false;
            var image_data = new ImageData(this, PPImageDataFormat.BgraPremul, size, isInitToZero);
            
            IntPtr dataPtr = image_data.Data;

            var imageBuffer = new int[(image_data.Size.Width * image_data.Size.Height)];

            var black = (int)MakeColor(BLACK);
            for (int i = 0; i < size.Area; ++i)
                imageBuffer[i] = black;

            Marshal.Copy(imageBuffer, 0, dataPtr, imageBuffer.Length);
            return image_data;
        }

        void PaintAndFlush(ImageData image_data)
        {
            Debug.Assert(!isFlushing);
            graphics2DContext.ReplaceContents(image_data);
            graphics2DContext.Flush();
            isFlushing = true;
        }

        /// The callback that gets invoked when a flush completes. This is bound to a
        /// <code>CompletionCallback</code> and passed as a parameter to
        /// <code>Flush</code>.
        void DidFlush(object sender, PPError error_code)
        {
            isFlushing = false;
            // If there are no images in the queue, we're done for now.
            if (paintQueue.Count == 0)
                return;
            // Otherwise, pop the next image off the queue and draw it.
            var imageData = paintQueue.Peek();
            paintQueue.Dequeue();
            PaintAndFlush(imageData);
        }
    }
}
