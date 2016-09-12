using System;
using System.Threading.Tasks;

namespace PepperSharp
{
    public class Graphics2D : Resource
    {
        /// <summary>
        /// Function for getting and setting the size of the 2D graphics context.
        /// </summary>
        public PPSize Size { get; private set; }

        /// <summary>
        /// Event raised when the Graphics2D issues a Flush on the context.
        /// </summary>
        public event EventHandler<PPError> Flushed;

        public Graphics2D(PPResource resource) : base(resource)
        { }


        /// <summary>
        /// A constructor allocating a new 2D graphics context with the given size
        /// in the browser, resulting object will be IsEmpty if the allocation
        /// failed.
        ///
        /// </summary>
        /// <param name="instance">The instance with which this resource will be associated.</param>
        /// <param name="size">The size of the 2D graphics context in the browser, measured in pixels. See <code>Scale</code> for more information.</param>
        /// <param name="isAlwaysOpaque">Set the <code>isAlwaysOpaque</code> flag
        /// to true if you know that you will be painting only opaque data to this
        /// context. This option will disable blending when compositing the module
        /// with the web page, which might give higher performance on some computers.
        ///
        /// If you set <code>isAlwaysOpaque</code>, your alpha channel should
        /// always be set to 0xFF or there may be painting artifacts. The alpha values
        /// overwrite the destination alpha values without blending when
        /// <code>isAlwaysOpaque</code> is true.
        /// </param>
        public Graphics2D(Instance instance, PPSize size, bool isAlwaysOpaque)
        {
            handle = PPBGraphics2D.Create(instance, size, (isAlwaysOpaque) ? PPBool.True : PPBool.False);
            if (!handle.IsEmpty)
            {
                Size = size;
            }
        }

        /// <summary>
        /// PaintImageData() enqueues a paint command of the given image into
        /// the context. This command has no effect until you call Flush(). As a
        /// result, what counts is the contents of the bitmap when you call Flush,
        /// not when you call this function.
        ///
        /// The provided image will be placed at <code>top_left</code> from the top
        /// left of the context's internal backing store. This version of
        /// PaintImageData paints the entire image. Refer to the other version of
        /// this function to paint only part of the area.
        ///
        /// The painted area of the source bitmap must fall entirely within the
        /// context. Attempting to paint outside of the context will result in an
        /// error.
        ///
        /// There are two methods most modules will use for painting. The first
        /// method is to generate a new <code>ImageData</code> and then paint it.
        /// In this case, you'll set the location of your painting to
        /// <code>top_left</code> and set <code>src_rect</code> to <code>NULL</code>.
        /// The second is that you're generating small invalid regions out of a larger
        /// bitmap representing your entire module's image.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="top_left"></param>
        public void PaintImageData(ImageData image,
                      PPPoint top_left)
        {
            PPBGraphics2D.PaintImageData(this, image, top_left, new PPRect(Size));
        }

        /// <summary>
        /// PaintImageData() enqueues a paint command of the given image into
        /// the context. This command has no effect until you call Flush(). As a
        /// result, what counts is the contents of the bitmap when you call Flush(),
        /// not when you call this function.
        ///
        /// The provided image will be placed at <code>top_left</code> from the top
        /// left of the context's internal backing store. Then the pixels contained
        /// in <code>src_rect</code> will be copied into the backing store. This
        /// means that the rectangle being painted will be at <code>src_rect</code>
        /// offset by <code>top_left</code>.
        ///
        /// The <code>src_rect</code> is specified in the coordinate system of the
        /// image being painted, not the context. For the common case of copying the
        /// entire image, you may specify an empty <code>src_rect</code>.
        ///
        /// The painted area of the source bitmap must fall entirely within the
        /// context. Attempting to paint outside of the context will result in an
        /// error. However, the source bitmap may fall outside the context, as long
        /// as the <code>src_rect</code> subset of it falls entirely within the
        /// context.
        ///
        /// There are two methods most modules will use for painting. The first
        /// method is to generate a new <code>ImageData</code> and then paint it. In
        /// this case, you'll set the location of your painting to
        /// <code>topLeft</code> and set <code>srcRect</code> to <code>NULL</code>.
        /// The second is that you're generating small invalid regions out of a larger
        /// bitmap representing your entire module. In this case, you would set the
        /// location of your image to (0,0) and then set <code>srcRect</code> to the
        /// pixels you changed.
        /// </summary>
        /// <param name="image">The <code>ImageData</code> to be painted.</param>
        /// <param name="topLeft">A <code>Point</code> representing the
        /// <code>topLeft</code> location where the <code>ImageData</code> will be
        /// painted.
        /// </param>
        /// <param name="srcRect">The rectangular area where the <code>ImageData</code>
        /// will be painted.</param>
        public void PaintImageData(ImageData image,
                                   PPPoint topLeft, PPRect srcRect)
        {
            PPBGraphics2D.PaintImageData(this, image, topLeft, srcRect);
        }

        /// <summary>
        /// Scroll() enqueues a scroll of the context's backing store. This
        /// function has no effect until you call Flush(). The data within the
        /// provided clipping rectangle will be shifted by (dx, dy) pixels.
        ///
        /// This function will result in some exposed region which will have
        /// undefined contents. The module should call PaintImageData() on
        /// these exposed regions to give the correct contents.
        ///
        /// The scroll can be larger than the area of the clipping rectangle, which
        /// means the current image will be scrolled out of the rectangle. This
        /// scenario is not an error but will result in a no-op.
        /// </summary>
        /// <param name="clip">The clipping rectangle.</param>
        /// <param name="amount">The amount the area in the clipping rectangle will be shifted.</param>
        public void Scroll(PPRect clip, PPPoint amount)
        {
            PPBGraphics2D.Scroll(this, clip, amount);
        }

        /// <summary>
        /// ReplaceContents() provides a slightly more efficient way to paint the
        /// entire module's image. Normally, calling PaintImageData() requires that
        /// the browser copy the pixels out of the image and into the graphics
        /// context's backing store. This function replaces the graphics context's
        /// backing store with the given image, avoiding the copy.
        ///
        /// The new image must be the exact same size as this graphics context. If
        /// the new image uses a different image format than the browser's native
        /// bitmap format (use ImageData::GetNativeImageDataFormat() to retrieve the
        /// format), then a conversion will be done inside the browser which may slow
        /// the performance a little bit.
        ///
        /// <strong>Note:</strong> The new image will not be painted until you call
        /// Flush().
        ///
        /// After this call, you should take care to release your references to the
        /// image. If you paint to the image after ReplaceContents(), there is the
        /// possibility of significant painting artifacts because the page might use
        /// partially-rendered data when copying out of the backing store.
        ///
        /// In the case of an animation, you will want to allocate a new image for
        /// the next frame. It is best if you wait until the flush callback has
        /// executed before allocating this bitmap. This gives the browser the option
        /// of caching the previous backing store and handing it back to you
        /// (assuming the sizes match). In the optimal case, this means no bitmaps are
        /// allocated during the animation, and the backing store and "front buffer"
        /// (which the module is painting into) are just being swapped back and forth.
        /// </summary>
        /// <param name="image">The <code>ImageData</code> to be painted.</param>
        public void ReplaceContents(ImageData image)
        {
            PPBGraphics2D.ReplaceContents(this, image);
        }

        /// <summary>
        /// Flush() flushes any enqueued paint, scroll, and replace commands
        /// to the backing store. This actually executes the updates, and causes a
        /// repaint of the webpage, assuming this graphics context is bound to a
        /// module instance.
        ///
        /// Flush() runs in asynchronous mode. Subscribe to the Flushed EventHandler
        /// which will be invoked when the image has painted to the
        /// screen. While you are waiting for a <code>Flush</code> event,
        /// additional calls to Flush() will fail.
        ///
        /// Because the EventHandler Flushed is invoked only when the
        /// module's image is actually on the screen, this function provides
        /// a way to rate limit animations. By waiting until the image is on the
        /// screen before painting the next frame, you can ensure you're not
        /// flushing 2D graphics faster than the screen can be updated.
        ///
        /// <strong>Unbound contexts</strong>
        /// If the context is not bound to a module instance, you will
        /// still receive events. The event will be invoked after Flush() returns
        /// to avoid reentrancy. The callback will not wait until anything is
        /// painted to the screen because there will be nothing on the screen. The
        /// timing of this callback is not guaranteed and may be deprioritized by
        /// the browser because it is not affecting the user experience.
        ///
        /// <strong>Off-screen instances</strong>
        /// If the context is bound to an instance that is
        /// currently not visible (for example, scrolled out of view) it will
        /// behave like the "unbound context" case.
        ///
        /// <strong>Detaching a context</strong>
        /// If you detach a context from a module instance, any
        /// pending flush events will be converted into the "unbound context"
        /// case.
        ///
        /// <strong>Released contexts</strong>
        /// A Flushed event may or may not still be received even if you have released all
        /// of your references to the context. This can occur if there are internal
        /// references to the context that means it has not been internally
        /// destroyed (for example, if it is still bound to an instance) or due to
        /// other implementation details. As a result, you should be careful to
        /// check that flush events are for the context you expect and that
        /// you're capable of handling events for context that you may have
        /// released your reference to.
        ///
        /// <strong>Shutdown</strong>
        /// If a module instance is removed when a Flush is pending, the
        /// event will not be fired.
        /// </summary>
        /// <returns>PPError code - Returns <code>Ok</code> on success or
        /// <code>BadResource</code> if the graphics context is invalid,
        /// <code>BadRrgument</code> if the callback is null and
        /// flush is being called from the main thread of the module, or
        /// <code>InProgress</code> if a flush is already pending that has
        /// not issued fired its event yet.  In the failure case, nothing will be
        /// updated and no event will be fired.</returns>
        public PPError Flush()
            => (PPError)PPBGraphics2D.Flush(this, new CompletionCallback(OnFlushed));


        protected void OnFlushed (PPError result)
            => Flushed?.Invoke (this, result);

        /// <summary>
        /// Flushs the context asynchronously.  <see cref="Flush"/> for more information
        /// </summary>
        /// <returns>Error code.</returns>
        /// <param name="messageLoop">Optional MessageLoop to execute the command on.</param>
        public Task<PPError> FlushAsync (MessageLoop messageLoop = null)
            => FlushAsyncCore (messageLoop);

        private async Task<PPError> FlushAsyncCore (MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<PPError> ();
            EventHandler<PPError> handler = (s, e) => { tcs.TrySetResult (e); };

            try {
                Flushed += handler;

                if (MessageLoop == null && messageLoop == null) 
                { 
                    Flush (); 
                } 
                else 
                {
                    Action<PPError> action = new Action<PPError> ((e) => {
                        var result = (PPError)PPBGraphics2D.Flush (this, new BlockUntilComplete());
                        tcs.TrySetResult (result);
                    }
                    );
                    InvokeHelper(action, messageLoop);
                }
                return await tcs.Task;

            } 
            catch (Exception exc) 
            {
                Console.WriteLine (exc.Message);
                tcs.SetException (exc);
                return PPError.Aborted;
            } 
            finally 
            {
                Flushed -= handler;
            }

        }

        /// <summary>
        /// Gets or Sets the scale factor that will be applied when painting the
        /// graphics context onto the output device. Typically, if rendering at device
        /// resolution is desired, the context would be created with the width and
        /// height scaled up by the view's GetDeviceScale and SetScale called with a
        /// scale of 1.0 / GetDeviceScale(). For example, if the view resource passed
        /// to DidChangeView has a rectangle of (w=200, h=100) and a device scale of
        /// 2.0, one would call Create with a size of (w=400, h=200) and then call
        /// SetScale with 0.5. One would then treat each pixel in the context as a
        /// single device pixel.
        /// 
        /// Default scale factor is 1.0f
        /// </summary>
        public float Scale
        {
            get
            {
                return PPBGraphics2D.GetScale(this);
            }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException("value", "Scale factor is invalid.");

                if (PPBGraphics2D.SetScale(this, value) == PPBool.False)
                    throw new ArgumentOutOfRangeException("value", "Scale factor is invalid.");

            }
        }


        public bool SetLayerTransform(float scale,
                         PPPoint origin,
                         PPPoint translate)
        {
            return PPBGraphics2D.SetLayerTransform(this, scale, origin, translate) == PPBool.True ? true : false;
        }

        #region Implement IDisposable.

        protected override void Dispose (bool disposing)
        {
            if (!IsEmpty) 
            {
                if (disposing) 
                {
                    Flushed = null;
                }
            }

            base.Dispose (disposing);
        }

        #endregion


    }
}
