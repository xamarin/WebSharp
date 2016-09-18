using System;
using System.Threading.Tasks;

namespace PepperSharp
{
    public class MediaStreamAudioTrack : Resource
    {

        /// <summary>
        /// Event raised when the MediaStreamAudioTrack issues a Configure call.
        /// </summary>
        public event EventHandler<PPError> HandleConfigure;

        /// <summary>
        /// Event raised when the MediaStreamAudioTrack issues GetBuffer.
        /// </summary>
        public event EventHandler<AudioBufferInfo> HandleBuffer;

        /// <summary>
        /// Event raised when the MediaStreamAudioTrack issues Close.
        /// </summary>
        public event EventHandler<PPError> HandleClose;

        /**
        * The <code>AudioBufferInfo</code> class represents the info of a audio buffer event
        */
        public class AudioBufferInfo : EventArgs
        {
            public PPError Result { get; private set; }
            public AudioBuffer AudioBuffer { get; set; }

            public AudioBufferInfo(PPError result, PPResource audioBuffer)
            {
                Result = result;
                AudioBuffer = new AudioBuffer(PassRef.PassRef, audioBuffer);// new VideoFrame(PassRef.PassRef, videoFrame);
            }

        }

        public MediaStreamAudioTrack(MediaStreamAudioTrack mediaStreamAudioTrack) : base(mediaStreamAudioTrack)
        { }

        public MediaStreamAudioTrack(PPResource resource) : base(resource)
        {
            if (PPBMediaStreamAudioTrack.IsMediaStreamAudioTrack(resource) == PPBool.False)
                throw new ArgumentException($"{nameof(resource)} is not a valid MediaStreamAudioTrack");
        }

        /// <summary>
        /// Configures underlying buffer buffers for incoming audio samples.
        /// If the application doesn't want to drop samples, then the
        /// <code>Buffers</code> should be
        /// chosen such that inter-buffer processing time variability won't overrun
        /// all input buffers. If all buffers are filled, then samples will be
        /// dropped. The application can detect this by examining the timestamp on
        /// returned buffers. If <code>Configure()</code> is not called, default
        /// settings will be used. Calls to Configure while the plugin holds
        /// buffers will fail.
        /// Example usage from plugin code:
        /// <code>
        /// var attribs = new MediaStreamAudioTrackAttributes() {
        ///     Buffers = 4,
        ///     Duration, 10
        ///     };
        /// track.Configure(attribs);
        /// </code>
        /// </summary>
        /// <param name="attributes">A MediaStreamAudioTrackAttributes instance</param>
        /// <returns>Error code</returns>
        public PPError Configure(MediaStreamAudioTrackAttributes attributes)
            => (PPError)PPBMediaStreamVideoTrack.Configure(this, attributes.ToAttributes(), new CompletionCallback(OnConfigure));

        protected void OnConfigure(PPError result)
            => HandleConfigure?.Invoke(this, result);

        /// <summary>
        /// Configures underlying buffer buffers for incoming audio samples asynchronously.
        /// If the application doesn't want to drop samples, then the
        /// <code>Buffers</code> should be
        /// chosen such that inter-buffer processing time variability won't overrun
        /// all input buffers. If all buffers are filled, then samples will be
        /// dropped. The application can detect this by examining the timestamp on
        /// returned buffers. If <code>Configure()</code> is not called, default
        /// settings will be used. Calls to Configure while the plugin holds
        /// buffers will fail.
        /// Example usage from plugin code:
        /// <code>
        /// var attribs = new MediaStreamAudioTrackAttributes() {
        ///     Buffers = 4,
        ///     Duration, 10
        ///     };
        /// track.Configure(attribs);
        /// </code>
        /// </summary>
        /// <param name="attributes">A MediaStreamAudioTrackAttributes instance</param>
        /// <param name="messageLoop">Optional MessageLoop instance that can be used to post the command to</param>
        /// <returns>Error code</returns>
        public Task<PPError> ConfigureAsync(MediaStreamAudioTrackAttributes attributes, MessageLoop messageLoop = null)
            => ConfigureAsyncCore(attributes, messageLoop);

        private async Task<PPError> ConfigureAsyncCore(MediaStreamAudioTrackAttributes attributes, MessageLoop messageLoop = null)
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
                        var result = (PPError)PPBMediaStreamAudioTrack.Configure(this, attributes.ToAttributes(),
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
        /// Gets a MediaStreamAudioTrackAttributes instance with the attributes that the
        /// MediaStreamAudioTrack is configured for.
        /// </summary>
        public MediaStreamAudioTrackAttributes Attributes
        {
            get
            {
                int output = 0;
                var attributes = new MediaStreamAudioTrackAttributes();

                PPBMediaStreamAudioTrack.GetAttrib(this, PPMediaStreamAudioTrackAttrib.Buffers, out output);
                attributes.Buffers = output;
                PPBMediaStreamAudioTrack.GetAttrib(this, PPMediaStreamAudioTrackAttrib.Channels, out output);
                attributes.Channels = output;
                PPBMediaStreamAudioTrack.GetAttrib(this, PPMediaStreamAudioTrackAttrib.Duration, out output);
                attributes.Duration = output;
                PPBMediaStreamAudioTrack.GetAttrib(this, PPMediaStreamAudioTrackAttrib.SampleRate, out output);
                attributes.SampleRate = (AudioBufferSampleRate)output;
                PPBMediaStreamAudioTrack.GetAttrib(this, PPMediaStreamAudioTrackAttrib.SampleSize, out output);
                attributes.SampleSize = (AudioBufferSampleSize)output;
                return attributes;
            }
        }

        /// <summary>
        /// Gets the track ID of the underlying MediaStream audio track.
        /// </summary>
        public string Id
            => ((Var)PPBMediaStreamAudioTrack.GetId(this)).AsString();

        /// <summary>
        /// Checks whether the underlying MediaStream track has ended.
        /// Calls to GetBuffer while the track has ended are safe to make and will
        /// complete, but will fail.
        /// </summary>
        public bool HasEnded
            => PPBMediaStreamAudioTrack.HasEnded(this) == PPBool.True;


        /// <summary>
        /// Recycles a buffer returned by <code>GetBuffer()</code>, so the track can
        /// reuse the buffer. And the buffer will become invalid. The caller should
        /// release all references it holds to <code>buffer</code> and not use it
        /// anymore.
        /// </summary>
        /// <param name="buffer">A AudioBuffer returned by <code>GetBuffer()</code>.</param>
        /// <returns>Error code</returns>
        public PPError RecycleBuffer(PPResource buffer)
            => (PPError)PPBMediaStreamAudioTrack.RecycleBuffer(this, buffer);

        /// <summary>
        /// Gets the next audio buffer from the MediaStream track.
        /// If internal processing is slower than the incoming buffer rate,
        /// new buffers will be dropped from the incoming stream. Once all buffers
        /// are full, audio samples will be dropped until <code>RecycleBuffer()</code>
        /// is called to free a spot for another buffer.
        /// If there are no audio data in the input buffer,
        /// <code>CompletionPending</code> will be returned immediately and the
        /// <code>HandleBuffer</code> event handler will be called when a new buffer of audio samples
        /// is received or some error happens.
        /// </summary>
        /// <returns>Error code</returns>
        public PPError GetBuffer()
        {
            var action = new Action<PPError, PPResource>(
                (result, resource) =>
                {
                    OnGetBuffer(new AudioBufferInfo(result,resource));
                }
                );
            var callback = new CompletionCallbackWithOutput<PPResource>(new CompletionCallbackWithOutputFunc<PPResource>(action));
            return (PPError)PPBMediaStreamAudioTrack.GetBuffer(this, out callback.OutputAdapter.output, callback);
        }

        protected void OnGetBuffer(AudioBufferInfo audioBufferInfo)
            => HandleBuffer?.Invoke(this, audioBufferInfo);

        /// <summary>
        /// Gets the next audio buffer from the MediaStream track.
        /// If internal processing is slower than the incoming buffer rate,
        /// new buffers will be dropped from the incoming stream. Once all buffers
        /// are full, audio samples will be dropped until <code>RecycleBuffer()</code>
        /// is called to free a spot for another buffer.
        /// </summary>
        /// <param name="messageLoop">Optional MessageLoop instance that can be used to post the command to</param>
        /// <returns>AudioBufferInfo instance</returns>
        public Task<AudioBufferInfo> GetBufferAsync(MessageLoop messageLoop = null)
            => GetBufferAsyncCore(messageLoop);

        private async Task<AudioBufferInfo> GetBufferAsyncCore(MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<AudioBufferInfo>();
            EventHandler<AudioBufferInfo> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleBuffer += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    GetBuffer();
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var output = new APIArgumentAdapter<PPResource>();
                        var result = (PPError)PPBMediaStreamAudioTrack.GetBuffer(this, out output.output,
                            new BlockUntilComplete());
                        tcs.TrySetResult(new AudioBufferInfo(result, output.Output));
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
                return new AudioBufferInfo(PPError.Aborted, PPResource.Empty);
            }
            finally
            {
                HandleBuffer -= handler;
            }
        }

        public void Close()
        {
            PPBMediaStreamAudioTrack.Close(this);
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
                        PPBMediaStreamAudioTrack.Close(this);
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

        #region Implement IDisposable.

        protected override void Dispose(bool disposing)
        {
            if (!IsEmpty)
            {
                if (disposing)
                {
                    HandleBuffer = null;
                    HandleClose = null;
                    HandleConfigure = null;
                }
            }
            base.Dispose(disposing);
        }

        #endregion

    }

    /// <summary>
    /// The MediaStreamAudioTrackAttributes allows setting of the MediaStreamAudioTrack
    /// configuration.
    /// </summary>
    public class MediaStreamAudioTrackAttributes
    {

        /// <summary>
        /// The maximum number of buffers to hold audio samples.
        /// Note: this is only used as advisory; the browser may allocate more or fewer
        /// based on available resources.How many buffers depends on usage -
        /// request at least 2 to make sure latency doesn't cause lost samples. If
        /// the plugin expects to hold on to more than one buffer at a time(e.g.to do
        /// multi-buffer processing), it should request that many more.
        /// </summary>
        public int Buffers { get; set; }

        /// <summary>
        /// The sample rate of audio data in buffers. The attribute value is a
        /// <code>AudioBufferSampleRate</code>.
        /// </summary>
        public AudioBufferSampleRate SampleRate { get; set; } = AudioBufferSampleRate.Unknown;

        /// <summary>
        /// The sample size of audio data in buffers in bytes. The attribute value is a
        /// <code>AudioBufferSampleSize</code>.
        /// </summary>
        public AudioBufferSampleSize SampleSize { get; set; } = AudioBufferSampleSize.Unknown;

        /// <summary>
        /// The number of channels in audio buffers.
        ///
        /// Supported values: 1, 2
        /// </summary>
        public int Channels { get; set; }

        /// <summary>
        /// The duration of an audio buffer in milliseconds.
        /// 
        /// Valid range: 10 to 10000
        /// </summary>
        public int Duration { get; set; }

        public MediaStreamAudioTrackAttributes()
        { }

        internal int[] ToAttributes()
        {
            return new int[]
            {
                (int)PPMediaStreamAudioTrackAttrib.Buffers, Buffers,
                (int)PPMediaStreamAudioTrackAttrib.SampleRate, (int)SampleRate,
                (int)PPMediaStreamAudioTrackAttrib.SampleSize, (int)SampleSize,
                (int)PPMediaStreamAudioTrackAttrib.Channels, Channels,
                (int)PPMediaStreamAudioTrackAttrib.Duration, Duration,
                (int)PPMediaStreamAudioTrackAttrib.None
            };
        }


    }

    /**
    * AudioBuffer_SampleRate is an enumeration of the different audio sample
    * rates.
    */
    public enum AudioBufferSampleRate
    {
        Unknown = 0,
        _8000 = 8000,
        _16000 = 16000,
        _22050 = 22050,
        _32000 = 32000,
        _44100 = 44100,
        _48000 = 48000,
        _96000 = 96000,
        _192000 = 192000
    }

    /**
    * AudioBufferSampleSize is an enumeration of the different audio sample
    * sizes.
    */
    public enum AudioBufferSampleSize
    {
        Unknown = 0,
        _16Bits = 2
    }
}
