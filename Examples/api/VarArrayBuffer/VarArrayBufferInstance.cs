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

        PPResource graphics2DContext;

        /// A queue of images to paint. We must maintain a queue because we can not
        /// call pp::Graphics2D::Flush while a Flush is already pending.
        Queue<PPResource> paintQueue = new Queue<PPResource>();

        /// The size of our rectangle in the DOM, as of the last time DidChangeView
        /// was called.
        PPSize size = new PPSize();

        /// true if we are flushing.
        bool isFlushing;

        /// Stores the most recent histogram so that we can re-draw it if we get
        /// resized.
        double[] histogram = new double[HISTOGRAM_SIZE];

        public VarArrayBufferInstance(IntPtr handle) : base(handle) { }

        public override bool Init(int argc, string[] argn, string[] argv)
        {
            LogToConsole(PPLogLevel.Log, "VarArrayBuffer");
            return true;
        }

        /// Handler for messages coming in from the browser via postMessage().  The
        /// @a var_message can contain anything: a JSON string; a string that encodes
        /// method names and arguments; etc.
        ///
        /// In this case, we only handle <code>pp::VarArrayBuffer</code>s. When we
        /// receive one, we compute and display a histogram based on its contents.
        ///
        /// @param[in] var_message The message posted by the browser.
        public override void HandleMessage(PPVar message)
        {
            var var_message = (Var)message;
            if (var_message.IsArrayBuffer)
            {
                var buffer = new VarArrayBuffer(var_message);
                ComputeHistogram(buffer);
                DrawHistogram();
            }
        }

        public override void DidChangeView(PPResource view)
        {
            var viewRect = new PPRect();
            var result = PPBView.GetRect(view, out viewRect);

            if (size != viewRect.Size)
            {
                size = viewRect.Size;
                Console.WriteLine(size);
                bool is_AlwaysOpaque = true;
                var isAlwaysOpaque = new PPBool();
                isAlwaysOpaque = is_AlwaysOpaque ? PPBool.True : PPBool.False;
                graphics2DContext = PPBGraphics2D.Create(this, viewRect.Size, isAlwaysOpaque);

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
            var desc = new PPImageDataDesc();

            if (PPBImageData.Describe(imageData, out desc) == PPBool.False)
            {
                return;
            }
            for (uint i = 0; i < Math.Min(HISTOGRAM_SIZE, desc.size.Width); i++)
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
        void DrawBar(uint column, double value, PPResource imageData)
        {
            Debug.Assert((value >= 0.0) && (value <= 1.0));
            var desc = new PPImageDataDesc();

            int[] imageBuffer = null;
            IntPtr dataPtr = IntPtr.Zero;
            if (PPBImageData.Describe(imageData, out desc) == PPBool.True)
            {
                dataPtr = PPBImageData.Map(imageData);
                if (dataPtr == IntPtr.Zero)
                    return;

                imageBuffer = new int[(desc.size.width * desc.size.height)];

                Marshal.Copy(dataPtr, imageBuffer, 0, imageBuffer.Length);

            }

            var imageHeight = desc.size.Height;
            var imageWidth = desc.size.Width;

            Debug.Assert(column < imageWidth);

            var barHeight = (int)(value * imageHeight);

            var blue = (int)MakeColor(BLUE);

            for (int i = 0; i < barHeight; i++)
            {
                var row = imageHeight - 1 - i;
                imageBuffer[row * imageWidth + column] = blue;
            }

            Marshal.Copy(imageBuffer, 0, dataPtr, imageBuffer.Length);
            PPBImageData.Unmap(imageData);
        }

        uint MakeColor(uint colorARGB)
        {
            byte a = (byte)((colorARGB >> 24 & 255));
            byte r = (byte)((colorARGB >> 16 & 255));
            byte g = (byte)((colorARGB >> 8 & 255));
            byte b = (byte)(colorARGB >> 0 & 255);

            var format = PPBImageData.GetNativeImageDataFormat();

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
        PPResource MakeBlankImageData(PPSize size) {

            bool initToZero = false;
            var isInitToZero = new PPBool();
            isInitToZero = initToZero ? PPBool.True : PPBool.False;
            var image_data = PPBImageData.Create(this, PPImageDataFormat.BgraPremul, size, isInitToZero);
            
            var desc = new PPImageDataDesc();

            int[] imageBuffer = null;
            IntPtr dataPtr = IntPtr.Zero;
            if (PPBImageData.Describe(image_data, out desc) == PPBool.True)
            {
                dataPtr = PPBImageData.Map(image_data);
                if (dataPtr == IntPtr.Zero)
                    return new PPResource();

                imageBuffer = new int[(desc.size.width * desc.size.height)];

                Marshal.Copy(dataPtr, imageBuffer, 0, imageBuffer.Length);

            }

            var black = (int)MakeColor(BLACK);
            for (int i = 0; i < size.Area; ++i)
                imageBuffer[i] = black;

            Marshal.Copy(imageBuffer, 0, dataPtr, imageBuffer.Length);
            PPBImageData.Unmap(image_data);
            return image_data;
        }

        void PaintAndFlush(PPResource image_data)
        {
            Debug.Assert(!isFlushing);
            PPBGraphics2D.ReplaceContents(graphics2DContext, image_data);
            //var flushCallback = new CompletionCallback(DidFlush);
            PPBGraphics2D.Flush(graphics2DContext, new CompletionCallback(DidFlush).Callback);
            isFlushing = true;
        }

        /// The callback that gets invoked when a flush completes. This is bound to a
        /// <code>CompletionCallback</code> and passed as a parameter to
        /// <code>Flush</code>.
        void DidFlush(IntPtr userData, int error_code)
        {
            isFlushing = false;
            // If there are no images in the queue, we're done for now.
            if (paintQueue.Count == 0)
                return;
            // Otherwise, pop the next image off the queue and draw it.
            var imageData = paintQueue.Peek();
            paintQueue.Dequeue();
            PaintAndFlush(imageData);
            PPBCore.ReleaseResource(imageData);
        }
    }
}
