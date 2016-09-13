using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PepperSharp
{
    public class MediaStreamVideoTrack : Resource
    {

        /// <summary>
        /// Event raised when the MediaStreamVideoTrack issues a Configure call.
        /// </summary>
        public event EventHandler<PPError> HandleConfigure;

        /// <summary>
        /// Event raised when the MediaStreamVideoTrack issues Close.
        /// </summary>
        public event EventHandler<PPError> HandleClose;

        /// <summary>
        /// Event raised when the MediaStreamVideoTrack issues GetFrame.
        /// </summary>
        public event EventHandler<VideoFrameInfo> HandleFrame;

        /// <summary>
        /// Event raised when the MediaStreamVideoTrack issues GetEmptyFrame.
        /// </summary>
        public event EventHandler<VideoFrameInfo> HandleEmptyFrame;

        public MediaStreamVideoTrack (Instance instance)
        {
            handle = PPBMediaStreamVideoTrack.Create(instance);
        }

        public MediaStreamVideoTrack(PPResource resource) : base(resource)
        { }

        /// <summary>
        /// Configures underlying frame buffers for incoming frames.
        /// If the application doesn't want to drop frames, then the
        /// <code>BufferedFrames</code> should be
        /// chosen such that inter-frame processing time variability won't overrun the
        /// input buffer. If the buffer is overfilled, then frames will be dropped.
        /// The application can detect this by examining the timestamp on returned
        /// frames. If some attributes are not specified, default values will be used
        /// for those unspecified attributes. If <code>Configure()</code> is not
        /// called, default settings will be used.
        /// Example usage from plugin code:
        /// <code>
        /// var attribList = new MediaStreamVideoTrackAttributes()
        /// {
        ///     BufferedFrames = 4
        /// };
        /// track.Configure(attribList);
        /// </code>
        /// </summary>
        /// <param name="attributes">A MediaStreamVideoTrackAttributes instance</param>
        /// <returns>Error code.  Returns <code>InProgress</code> if there is a pending call of
        /// <code>Configure()</code> or <code>GetFrame()</code>, or the plugin
        /// holds some frames which are not recycled with <code>RecycleFrame()</code>.
        /// If an error is returned, all attributes and the underlying buffer will not
        /// be changed.</returns>
        public PPError Configure(MediaStreamVideoTrackAttributes attributes)
            => (PPError)PPBMediaStreamVideoTrack.Configure(this, attributes.ToAttributes(), new CompletionCallback(OnConfigure));

        protected void OnConfigure(PPError result)
            => HandleConfigure?.Invoke(this, result);

        /// <summary>
        /// Configures underlying frame buffers for incoming frames asynchronously.
        /// If the application doesn't want to drop frames, then the
        /// <code>BufferedFrames</code> should be
        /// chosen such that inter-frame processing time variability won't overrun the
        /// input buffer. If the buffer is overfilled, then frames will be dropped.
        /// The application can detect this by examining the timestamp on returned
        /// frames. If some attributes are not specified, default values will be used
        /// for those unspecified attributes. If <code>Configure()</code> is not
        /// called, default settings will be used.
        /// Example usage from plugin code:
        /// <code>
        /// var attribList = new MediaStreamVideoTrackAttributes()
        /// {
        ///     BufferedFrames = 4
        /// };
        /// await track.ConfigureAsync(attribList);
        /// </code>
        /// </summary>
        /// <param name="attributes">A MediaStreamVideoTrackAttributes instance</param>
        /// <param name="messageLoop">Optional MessageLoop instance used to run the command on.</param>
        /// <returns>Error code.  Returns <code>InProgress</code> if there is a pending call of
        /// <code>Configure()</code> or <code>GetFrame()</code>, or the plugin
        /// holds some frames which are not recycled with <code>RecycleFrame()</code>.
        /// If an error is returned, all attributes and the underlying buffer will not
        /// be changed.</returns>
        public Task<PPError> ConfigureAsync(MediaStreamVideoTrackAttributes attributes, MessageLoop messageLoop = null)
            => ConfigureAsyncCore(attributes, messageLoop);

        private async Task<PPError> ConfigureAsyncCore(MediaStreamVideoTrackAttributes attributes, MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<PPError>();
            EventHandler<PPError> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleConfigure += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    Configure(attributes);
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var result = (PPError)PPBMediaStreamVideoTrack.Configure(this, attributes.ToAttributes(),
                            new BlockUntilComplete()
                        );
                        tcs.TrySetResult(result);
                    }
                    );

                    InvokeHelper(action, messageLoop);
                }
                return await tcs.Task;

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                tcs.SetException(exc);
                return PPError.Aborted;
            }
            finally
            {
                HandleConfigure -= handler;
            }
        }

        /// <summary>
        /// Gets a MediaStreamVideoTrackAttributes instance with the attributes that the
        /// MediaStreamVideoTrack is configured for.
        /// </summary>
        public MediaStreamVideoTrackAttributes Attributes
        {
            get
            {
                int output = 0;
                var attributes = new MediaStreamVideoTrackAttributes();

                PPBMediaStreamVideoTrack.GetAttrib(this, PPMediaStreamVideoTrackAttrib.Format, out output);
                attributes.Format = (VideoFrameFormat)output;
                PPBMediaStreamVideoTrack.GetAttrib(this, PPMediaStreamVideoTrackAttrib.Width, out output);
                attributes.Width = output;
                PPBMediaStreamVideoTrack.GetAttrib(this, PPMediaStreamVideoTrackAttrib.Height, out output);
                attributes.Height = output;
                PPBMediaStreamVideoTrack.GetAttrib(this, PPMediaStreamVideoTrackAttrib.BufferedFrames, out output);
                attributes.BufferedFrames = output;
                return attributes;
            }
        }

        /// <summary>
        /// Gets the track ID of the underlying MediaStream video track.
        /// </summary>
        public string Id
            => ((Var)PPBMediaStreamVideoTrack.GetId(this)).AsString();

        /// <summary>
        /// Checks whether the underlying MediaStream track has ended.
        /// Calls to GetFrame while the track has ended are safe to make and will
        /// complete, but will fail.
        /// </summary>
        public bool HasEnded
            => PPBMediaStreamVideoTrack.HasEnded(this) == PPBool.True;


        /// <summary>
        /// Recycles a frame returned by <code>GetFrame()</code>, so the track can
        /// reuse the underlying buffer of this frame. And the frame will become
        /// invalid. The caller should release all references it holds to
        /// <code>frame</code> and not use it anymore.
        /// </summary>
        /// <param name="frame">A VideoFrame returned by <code>GetFrame()</code>.</param>
        /// <returns>Error code</returns>
        public PPError RecycleFrame(VideoFrame frame)
            => (PPError)PPBMediaStreamVideoTrack.RecycleFrame(this, frame);

        /// <summary>
        /// Closes the MediaStream video track, and disconnects it from video source.
        /// After calling <code>Close()</code>, no new frames will be received.
        /// </summary>
        public void Close()
        {
            PPBMediaStreamVideoTrack.Close(this);
            OnClose(PPError.Ok);
        }

        protected virtual void OnClose(PPError result)
            => HandleClose?.Invoke(this, result);

        /// <summary>
        /// Closes the MediaStream video track asynchronously, and disconnects it from video source.
        /// After calling <code>Close()</code>, no new frames will be received.
        /// </summary>
        /// <param name="messageLoop">Optional MessageLoop instance that can be used to post the command to</param>
        public Task<PPError> CloseAsync(MessageLoop messageLoop = null)
            => CloseAsyncCore(messageLoop);

        private async Task<PPError> CloseAsyncCore(MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<PPError>();
            EventHandler<PPError> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleClose += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    Close();
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        PPBMediaStreamVideoTrack.Close(this);
                        tcs.TrySetResult(PPError.Ok);
                    }
                    );
                    InvokeHelper(action, messageLoop);
                }
                return await tcs.Task;

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                tcs.SetException(exc);
                return PPError.Aborted;
            }
            finally
            {
                HandleClose -= handler;
            }
        }

        /// <summary>
        /// Gets the next video frame from the MediaStream track.
        /// If internal processing is slower than the incoming frame rate, new frames
        /// will be dropped from the incoming stream. Once the input buffer is full,
        /// frames will be dropped until <code>RecycleFrame()</code> is called to free
        /// a spot for another frame to be buffered.
        /// If there are no frames in the input buffer,
        /// <code>OkCompletionPending</code> will be returned immediately and the
        /// <code>HandleFrame</code> event handler will be called when a new frame is received or some
        /// error happens.
        /// </summary>
        /// <returns>Error code</returns>
        public PPError GetFrame()
        {
            var action = new Action<PPError, PPResource>((result, resource) =>
            {
                OnGetFrame(new VideoFrameInfo(result, resource));
            });
            var callback = new CompletionCallbackWithOutput<PPResource>(new CompletionCallbackWithOutputFunc<PPResource>(action));
            return (PPError)PPBMediaStreamVideoTrack.GetFrame(this, out callback.OutputAdapter.output, callback);
        }

        protected virtual void OnGetFrame(VideoFrameInfo videoFrame)
            => HandleFrame?.Invoke(this, videoFrame);

        /// <summary>
        /// Gets the next video frame from the MediaStream track asynchronously.
        /// If internal processing is slower than the incoming frame rate, new frames
        /// will be dropped from the incoming stream. Once the input buffer is full,
        /// frames will be dropped until <code>RecycleFrame()</code> is called to free
        /// a spot for another frame to be buffered.
        /// If there are no frames in the input buffer,
        /// <code>OkCompletionPending</code> will be returned immediately.
        /// </summary>
        /// <param name="messageLoop">Optional MessageLoop instance that can be used to post the command to</param>
        public Task<VideoFrameInfo> GetFrameAsync(MessageLoop messageLoop = null)
            => GetFrameAsyncCore(messageLoop);

        private async Task<VideoFrameInfo> GetFrameAsyncCore(MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<VideoFrameInfo>();
            EventHandler<VideoFrameInfo> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleFrame += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    GetFrame();
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var output = new APIArgumentAdapter<PPResource>();
                        var result = (PPError)PPBMediaStreamVideoTrack.GetFrame(this, out output.output,
                            new BlockUntilComplete());
                        tcs.TrySetResult(new VideoFrameInfo(result, output.Output));
                    }
                    );
                    InvokeHelper(action, messageLoop);
                }
                return await tcs.Task;

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                tcs.SetException(exc);
                return new VideoFrameInfo(PPError.Aborted, PPResource.Empty);
            }
            finally
            {
                HandleFrame -= handler;
            }
        }

        /// <summary>
        // Gets a free frame for output. The frame is allocated by
        // <code>Configure()</code>. The caller should fill it with frame data, and
        // then use |PutFrame()| to send the frame back.
        /// </summary>
        /// <returns>Error code</returns>
        public PPError GetEmptyFrame()
        {
            var action = new Action<PPError, PPResource>((result, resource) =>
            {
                OnGetEmptyFrame(new VideoFrameInfo(result, resource));
            });
            var callback = new CompletionCallbackWithOutput<PPResource>(new CompletionCallbackWithOutputFunc<PPResource>(action));
            return (PPError)PPBMediaStreamVideoTrack.GetEmptyFrame(this, out callback.OutputAdapter.output, callback);
        }

        protected virtual void OnGetEmptyFrame(VideoFrameInfo videoFrame)
            => HandleFrame?.Invoke(this, videoFrame);

        /// <summary>
        // Gets a free frame for output asynchonously. The frame is allocated by
        // <code>Configure()</code>. The caller should fill it with frame data, and
        // then use |PutFrame()| to send the frame back.
        /// </summary>
        /// <param name="messageLoop">Optional MessageLoop instance that can be used to post the command to</param>
        public Task<VideoFrameInfo> GetEmptyFrameAsync(MessageLoop messageLoop = null)
            => GetEmptyFrameAsyncCore(messageLoop);

        private async Task<VideoFrameInfo> GetEmptyFrameAsyncCore(MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<VideoFrameInfo>();
            EventHandler<VideoFrameInfo> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleEmptyFrame += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    GetFrame();
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var output = new APIArgumentAdapter<PPResource>();
                        var result = (PPError)PPBMediaStreamVideoTrack.GetEmptyFrame(this, out output.output,
                            new BlockUntilComplete());
                        tcs.TrySetResult(new VideoFrameInfo(result, output.Output));
                    }
                    );
                    InvokeHelper(action, messageLoop);
                }
                return await tcs.Task;

            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
                tcs.SetException(exc);
                return new VideoFrameInfo(PPError.Aborted, PPResource.Empty);
            }
            finally
            {
                HandleEmptyFrame -= handler;
            }
        }

        /// <summary>
        /// Sends a frame returned by |GetEmptyFrame()| to the output track.
        /// After this function, the |frame| should not be used anymore and the
        /// caller should release the reference that it holds.
        /// </summary>
        /// <param name="frame"></param>
        /// <returns>Error code</returns>
        public PPError PutFrame(VideoFrame frame)
            => (PPError)PPBMediaStreamVideoTrack.PutFrame(this, frame);

        #region Implement IDisposable.

        protected override void Dispose(bool disposing)
        {
            if (!IsEmpty)
            {
                if (disposing)
                {
                    HandleConfigure = null;
                    HandleClose = null;
                    HandleEmptyFrame = null;
                    HandleFrame = null;
                }
            }
            base.Dispose(disposing);
        }

        #endregion


    }

    /// <summary>
    /// The MediaStreamVideoTrackAttributes allows setting of the MediaStreamVideoTrack
    /// configuration.
    /// </summary>
    public class MediaStreamVideoTrackAttributes
    {

        /// <summary>
        /// The maximum number of frames to hold in the input buffer.
        /// Note: this is only used as advisory; the browser may allocate more or fewer
        /// based on available resources.How many frames to buffer depends on usage -
        /// request at least 2 to make sure latency doesn't cause lost frames. If
        /// the plugin expects to hold on to more than one frame at a time(e.g.to do
        /// multi-frame processing), it should request that many more.
        /// If this attribute is not specified or value 0 is specified for this
        /// attribute, the default value will be used.
        /// </summary>
        public int BufferedFrames { get; set; }

        /// <summary>
        /// The width of video frames in pixels. It should be a multiple of 4.
        /// If the specified size is different from the video source (webcam),
        /// frames will be scaled to specified size.
        /// If this attribute is not specified or value 0 is specified, the original
        /// frame size of the video track will be used.
        /// 
        /// Maximum value: 4096 (4K resolution).
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// The height of video frames in pixels.It should be a multiple of 4.
        /// If the specified size is different from the video source(webcam),
        /// frames will be scaled to specified size.
        /// If this attribute is not specified or value 0 is specified, the original
        /// frame size of the video track will be used.
        /// 
        /// Maximum value: 4096 (4K resolution).
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// The format of video frames. The attribute value is
        /// a <code>VideoFrameFormat</code>. If the specified format is different
        /// from the video source (webcam), frames will be converted to specified
        /// format.
        /// If this attribute is not specified or value
        /// <code>Unknown</code> is specified, the orignal frame
        /// format of the video track will be used.
        /// </summary>
        public VideoFrameFormat Format { get; set; } = VideoFrameFormat.Unknown;

        public MediaStreamVideoTrackAttributes()
        { }

        internal int[] ToAttributes()
        {
            return new int[]
            {
                (int)PPMediaStreamVideoTrackAttrib.BufferedFrames, BufferedFrames,
                (int)PPMediaStreamVideoTrackAttrib.Width, Width,
                (int)PPMediaStreamVideoTrackAttrib.Height, Height,
                (int)PPMediaStreamVideoTrackAttrib.Format, (int)Format,
                (int)PPMediaStreamVideoTrackAttrib.None
            };
        }


    }

}
