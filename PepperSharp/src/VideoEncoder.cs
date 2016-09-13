using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace PepperSharp
{
    public class VideoEncoder : Resource
    {

        /// <summary>
        /// Event raised when the VideoEncoder issues a GetSupportedProfiles call.
        /// </summary>
        public event EventHandler<VideoProfileInfo> HandleEncoderProbed;

        /// <summary>
        /// Event raised when the VideoEncoder issues an Initialize call.
        /// </summary>
        public event EventHandler<PPError> HandleInitialize;

        /// <summary>
        /// Event raised when the VideoEncoder issues a GetVideoFrame call.
        /// </summary>
        public event EventHandler<VideoFrameInfo> HandleVideoFrame;

        /// <summary>
        /// Event raised when the VideoEncoder issues an Encode call.
        /// </summary>
        public event EventHandler<PPError> HandleEncode;

        /// <summary>
        /// Event raised when the VideoEncoder issues a GetBitstreamBuffer call.
        /// </summary>
        public event EventHandler<BitstreamBufferInfo> HandleBitstreamBuffer;

        /**
        * The <code>BitstreamBufferInof</code> struct represents all information about a file,
        * such as size, type, and creation time.
        */
        public class BitstreamBufferInfo : EventArgs
        {
            public PPError Result { get; private set; }
            /**
             * The size, in bytes, of the bitstream data.
            */
            public uint Size { get; private set; }
            /**
             * The base address of the bitstream data.
             */
            public byte[] Buffer { get; private set; }
            /**
             * Whether the buffer represents a key frame.
             */
            public bool KeyFrame { get; private set; }

            internal PPBitstreamBuffer BitstreamBuffer { get; set; }

            internal BitstreamBufferInfo(PPError result, PPBitstreamBuffer bitstreamBuffer)
            {
                Result = result;
                Size = bitstreamBuffer.size;
                if (Size > 0)
                {
                    var bufferMap = new byte[Size];
                    Marshal.Copy(bitstreamBuffer.buffer, bufferMap, 0, bufferMap.Length);
                    Buffer = bufferMap;
                }
                else
                    Buffer = new byte[0];

                KeyFrame = bitstreamBuffer.key_frame == PPBool.True;
                BitstreamBuffer = bitstreamBuffer;
            }

        }

        /// <summary>
        /// Event raised when the VideoEncoder issues Close.
        /// </summary>
        public event EventHandler<PPError> HandleClose;


        public sealed class VideoProfileInfo : EventArgs
        {
            public PPError Result { get; private set; }
            public ReadOnlyCollection<VideoProfileDescription> VideoProfileDescriptions { get; private set; }

            internal VideoProfileInfo(PPError result, ReadOnlyCollection<VideoProfileDescription> entries)
            {
                Result = result;
                VideoProfileDescriptions = entries;
            }
        }

        public VideoEncoder(Instance instance)
        {
            handle = PPBVideoEncoder.Create(instance);
        }

        /// <summary>
        /// Gets an array of supported video encoder profiles.
        /// These can be used to choose a profile before calling Initialize().
        /// </summary>
        /// <returns>If >= 0, the number of supported profiles returned, otherwise an
        /// error code.
        /// </returns>
        public PPError GetSupportedProfiles()
        {
            var probed = new Action<PPError, PPVideoProfileDescription01[]>
                ((result, profiles) =>
                {
                    var sv = new List<VideoProfileDescription>();
                    if (profiles != null)
                    {
                        for (int i = 0; i < profiles.Length; ++i)
                        {
                            sv.Add(new VideoProfileDescription(profiles[i]));
                        }
                    }

                    OnEncoderProbed(result, sv.AsReadOnly());
                });
            var encoderProbedCallback = new CompletionCallbackWithOutput<PPVideoProfileDescription01[]>(new CompletionCallbackWithOutputFunc<PPVideoProfileDescription01[]>(probed));
            return (PPError)PPBVideoEncoder.GetSupportedProfiles(this, encoderProbedCallback, encoderProbedCallback.Callback);
        }

        protected void OnEncoderProbed(PPError result, ReadOnlyCollection<VideoProfileDescription> entries)
            => HandleEncoderProbed?.Invoke(this, new VideoProfileInfo(result, entries));

        /// <summary>
        /// Gets an array of supported video encoder profiles.
        /// These can be used to choose a profile before calling Initialize().
        /// </summary>
        /// <param name="messageLoop">Optional MessageLoop instance used to run the command on.</param>
        /// <returns>A VideProfileInfo object <see cref="VideoProfileInfo"/></returns>
        public Task<VideoProfileInfo> GetSupportedProfilesAsync(MessageLoop messageLoop = null)
            => GetSupportedProfilesAsyncCore(messageLoop);

        private async Task<VideoProfileInfo> GetSupportedProfilesAsyncCore(MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<VideoProfileInfo>();
            EventHandler<VideoProfileInfo> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleEncoderProbed += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    GetSupportedProfiles();
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var output = new ArrayOutputAdapterWithStorage<PPVideoProfileDescription01[]>();
                        var result = (PPError)PPBVideoEncoder.GetSupportedProfiles(this, output.PPArrayOutput,
                            new BlockUntilComplete()
                        );

                        var profiles = new List<VideoProfileDescription>();
                        foreach (var entry in output.Output)
                        {
                            profiles.Add(new VideoProfileDescription(entry));
                        }
                        tcs.TrySetResult(new VideoProfileInfo(result, profiles.AsReadOnly()));
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
                return new VideoProfileInfo(PPError.Aborted, new List<VideoProfileDescription>().AsReadOnly());
            }
            finally
            {
                HandleEncoderProbed -= handler;
            }
        }

        /// <summary>
        /// Initializes a video encoder resource. This should be called after
        /// GetSupportedProfiles() and before any functions below.
        /// </summary>
        /// <param name="inputFormat">The <code>VideoFrame_Format</code> of the
        /// frames which will be encoded.</param>
        /// <param name="inputVisibleSize">A <code>Size</code> specifying the
        /// dimensions of the visible part of the input frames.</param>
        /// <param name="outputProfile">A <code>VideoProfile</code> specifying the
        /// codec profile of the encoded output stream.</param>
        /// <param name="initialBitrate">The initial bitrate of the encoded output stream</param>
        /// <param name="acceleration">A <code>HardwareAcceleration</code> specifying
        /// whether to use a hardware accelerated or a software implementation.</param>
        /// <returns>Error code.  Returns NotSupported if video encoding is not available, or the
        /// requested codec profile is not supported.
        /// Returns NoMemory if frame and bitstream buffers can't be created.</returns>
        public PPError Initialize(VideoFrameFormat inputFormat,
                     PPSize inputVisibleSize,
                     VideoProfile outputProfile,
                     uint initialBitrate,
                     HardwareAcceleration acceleration)
            => (PPError)PPBVideoEncoder.Initialize(this, (PPVideoFrameFormat)inputFormat, 
                inputVisibleSize,
                (PPVideoProfile) outputProfile,
                initialBitrate, 
                (PPHardwareAcceleration)acceleration, new CompletionCallback(OnInitialize));

        protected virtual void OnInitialize(PPError result)
            => HandleInitialize?.Invoke(this, result);

        /// <summary>
        /// Initializes a video encoder resource asynchronously. This should be called after
        /// GetSupportedProfiles() and before any functions below.
        /// </summary>
        /// <param name="inputFormat">The <code>VideoFrame_Format</code> of the
        /// frames which will be encoded.</param>
        /// <param name="inputVisibleSize">A <code>Size</code> specifying the
        /// dimensions of the visible part of the input frames.</param>
        /// <param name="outputProfile">A <code>VideoProfile</code> specifying the
        /// codec profile of the encoded output stream.</param>
        /// <param name="initialBitrate">The initial bitrate of the encoded output stream</param>
        /// <param name="acceleration">A <code>HardwareAcceleration</code> specifying
        /// whether to use a hardware accelerated or a software implementation.</param>
        /// <param name="messageLoop">Optional MessageLoop instance that can be used to post the command to</param>
        /// <returns>Error code.  Returns NotSupported if video encoding is not available, or the
        /// requested codec profile is not supported.
        /// Returns NoMemory if frame and bitstream buffers can't be created.</returns>
        public Task<PPError> InitializeAsync(VideoFrameFormat inputFormat,
                    PPSize inputVisibleSize,
                     VideoProfile outputProfile,
                     uint initialBitrate,
                     HardwareAcceleration acceleration, 
                     MessageLoop messageLoop = null)
            => InitializeAsyncCore(inputFormat, inputVisibleSize, outputProfile, initialBitrate, acceleration, messageLoop);

        private async Task<PPError> InitializeAsyncCore(VideoFrameFormat inputFormat,
                     PPSize inputVisibleSize,
                     VideoProfile outputProfile,
                     uint initialBitrate,
                     HardwareAcceleration acceleration, 
                     MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<PPError>();
            EventHandler<PPError> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleInitialize += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    Initialize(inputFormat, inputVisibleSize, outputProfile, initialBitrate, acceleration);
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var result = (PPError)PPBVideoEncoder.Initialize(this,
                            (PPVideoFrameFormat)inputFormat,
                            inputVisibleSize,
                            (PPVideoProfile)outputProfile,
                            initialBitrate,
                            (PPHardwareAcceleration)acceleration,
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
                HandleInitialize -= handler;
            }
        }

        /// <summary>
        /// Gets the number of input video frames that the encoder may hold while
        /// encoding. If the plugin is providing the video frames, it should have at
        /// least this many available.  
        /// 
        /// <remarks>If Initialize() has not successfully completed will return an error code</remarks>
        /// </summary>
        public int FramesRequired
            => PPBVideoEncoder.GetFramesRequired(this);

        /// <summary>
        /// Gets the coded size of the video frames required by the encoder. Coded
        /// size is the logical size of the input frames, in pixels.  The encoder may
        /// have hardware alignment requirements that make this different from
        /// |inputVisibleSize|, as requested in the call to Initialize().
        /// </summary>
        /// <param name="codedSize"></param>
        /// <returns>Returns Failed if Initialize has not been called</returns>
        public PPError GetFrameCodedSize(out PPSize codedSize)
        { 
            return (PPError)PPBVideoEncoder.GetFrameCodedSize(this, out codedSize);
        }

        /// <summary>
        /// Gets a blank video frame which can be filled with video data and passed
        /// to the encoder.
        /// </summary>
        /// <returns>Error code</returns>
        public PPError GetVideoFrame()
        {
            var OnEncoderFrameCallback = new CompletionCallbackWithOutput<PPResource>(OnVideoFrame);
            return (PPError)PPBVideoEncoder.GetVideoFrame(this, 
                out OnEncoderFrameCallback.OutputAdapter.output, 
                OnEncoderFrameCallback.Callback);
        }

        protected void OnVideoFrame(PPError result, PPResource videoFrame)
            => HandleVideoFrame?.Invoke(this, new VideoFrameInfo(result, videoFrame));

        /// <summary>
        /// Gets a blank video frame asynchronously which can be filled with video data and passed
        /// to the encoder.
        /// </summary>
        /// <param name="messageLoop">Optional MessageLoop instance that can be used to post the command to</param>
        /// <returns>VideoFrameInfo object that contains a blank video frame.  <see cref="VideoFrameInfo"/></returns>
        public Task<VideoFrameInfo> GetVideoFrameAsync(MessageLoop messageLoop = null)
            => GetVideoFrameAsyncCore(messageLoop);

        private async Task<VideoFrameInfo> GetVideoFrameAsyncCore(MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<VideoFrameInfo>();
            EventHandler<VideoFrameInfo> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleVideoFrame += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    GetVideoFrame();
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var output = new APIArgumentAdapter<PPResource>();
                        var result = (PPError)PPBVideoEncoder.GetVideoFrame(this, out output.output,
                            new BlockUntilComplete()
                        );

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
                HandleVideoFrame -= handler;
            }
        }

        /// <summary>
        /// Encodes a video frame.
        /// </summary>
        /// <param name="videoFrame">The <code>VideoFrame</code> to be encoded.</param>
        /// <param name="forceKeyframe">A bool specifying whether the encoder
        /// should emit a key frame for this video frame.</param>
        /// <returns>Returns Failed if Initialize() has not successfully completed.</returns>
        public PPError Encode(PPResource videoFrame,
                 bool forceKeyframe)
            => (PPError)PPBVideoEncoder.Encode(this, videoFrame, forceKeyframe ? PPBool.True : PPBool.False,
                new CompletionCallback(OnEncode));

        protected void OnEncode(PPError result)
            => HandleEncode?.Invoke(this, result);

        /// <summary>
        /// Encodes a video frame asynchronously.
        /// </summary>
        /// <param name="videoFrame">The <code>VideoFrame</code> to be encoded.</param>
        /// <param name="forceKeyframe">A bool specifying whether the encoder
        /// should emit a key frame for this video frame.</param>
        /// <param name="messageLoop">Optional MessageLoop instance that can be used to post the command to</param>
        /// <returns>Returns Failed if Initialize() has not successfully completed.</returns>
        public Task<PPError> EncodeAsync(PPResource videoFrame,
                 bool forceKeyframe, MessageLoop messageLoop = null)
            => EncodeAsyncCore(videoFrame, forceKeyframe, messageLoop);

        private async Task<PPError> EncodeAsyncCore(PPResource videoFrame,
                 bool forceKeyframe, MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<PPError>();
            EventHandler<PPError> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleEncode += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    Encode(videoFrame, forceKeyframe);
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var result = (PPError)PPBVideoEncoder.Encode(this, 
                            videoFrame, 
                            forceKeyframe ? PPBool.True : PPBool.False,
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
                HandleEncode -= handler;
            }
        }

        /// <summary>
        /// Gets the next encoded bitstream buffer from the encoder.
        /// </summary>
        /// <returns>Returns Failed if Initialize() has not successfully completed.
        /// Returns InProgress if a prior call to GetBitstreamBuffer() has
        /// not completed.</returns>
        public PPError GetBitstreamBuffer()
        {
            var callback = new CompletionCallbackWithOutput<PPBitstreamBuffer>(OnBitstreamBuffer);
            return (PPError)PPBVideoEncoder.GetBitstreamBuffer(this, out callback.OutputAdapter.output, callback);
        }

        protected void OnBitstreamBuffer(PPError result, PPBitstreamBuffer bitstreamBuffer)
            => HandleBitstreamBuffer?.Invoke(this, new BitstreamBufferInfo(result, bitstreamBuffer));

        /// <summary>
        /// Gets the next encoded bitstream buffer from the encoder asynchronously.
        /// </summary>
        /// <param name="messageLoop">Optional MessageLoop instance that can be used to post the command to</param>
        /// <returns>Returns Failed if Initialize() has not successfully completed.
        /// Returns InProgress if a prior call to GetBitstreamBuffer() has
        /// not completed.</returns>
        public Task<BitstreamBufferInfo> GetBitstreamBufferAsync(MessageLoop messageLoop = null)
            => GetBitstreamBufferAsyncCore(messageLoop);

        private async Task<BitstreamBufferInfo> GetBitstreamBufferAsyncCore(MessageLoop messageLoop = null)
        {
            var tcs = new TaskCompletionSource<BitstreamBufferInfo>();
            EventHandler<BitstreamBufferInfo> handler = (s, e) => { tcs.TrySetResult(e); };

            try
            {
                HandleBitstreamBuffer += handler;

                if (MessageLoop == null && messageLoop == null)
                {
                    GetBitstreamBuffer();
                }
                else
                {
                    Action<PPError> action = new Action<PPError>((e) =>
                    {
                        var output = new APIArgumentAdapter<PPBitstreamBuffer>();
                        var result = (PPError)PPBVideoEncoder.GetBitstreamBuffer(this, out output.output,
                            new BlockUntilComplete()
                        );

                        tcs.TrySetResult(new BitstreamBufferInfo(result, output.Output));
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
                return new BitstreamBufferInfo(PPError.Aborted, new PPBitstreamBuffer());
            }
            finally
            {
                HandleBitstreamBuffer -= handler;
            }
        }


        /// <summary>
        /// Recycles a bitstream buffer back to the encoder.
        /// </summary>
        /// <param name="bitstreamBuffer">A <code>BitstreamBufferInfo</code> that is no
        /// longer needed by the plugin.</param>
        public void RecycleBitstreamBuffer(BitstreamBufferInfo bitstreamBuffer)
            => PPBVideoEncoder.RecycleBitstreamBuffer(this, bitstreamBuffer.BitstreamBuffer);

        /// <summary>
        /// Requests a change to encoding parameters. This is only a request,
        /// fulfilled on a best-effort basis.
        /// </summary>
        /// <param name="bitRate">The requested new bitrate, in bits per second.</param>
        /// <param name="frameRate">The requested new framerate, in frames per second.</param>
        public void RequestEncodingParametersChange(uint bitRate, uint frameRate)
            => PPBVideoEncoder.RequestEncodingParametersChange(this, bitRate, frameRate);


        /// <summary>
        /// Closes the video encoder, and cancels any pending encodes. Any pending
        /// callbacks will still run, reporting <code>Aborted</code> . It is
        /// not valid to call any encoder functions after a call to this method.
        /// <strong>Note:</strong> Destroying the video encoder closes it implicitly,
        /// so you are not required to call Close().
        /// </summary>
        public void Close()
        {
            PPBVideoEncoder.Close(this);
            OnClose(PPError.Ok);
        }

        protected virtual void OnClose(PPError result)
            => HandleClose?.Invoke(this, result);

        /// <summary>
        /// Closes the video encoder asynchronously, and cancels any pending encodes. Any pending
        /// callbacks will still run, reporting <code>Aborted</code> . It is
        /// not valid to call any encoder functions after a call to this method.
        /// <strong>Note:</strong> Destroying the video encoder closes it implicitly,
        /// so you are not required to call Close().
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
                        PPBVideoEncoder.Close(this);
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
                    HandleEncoderProbed = null;
                    HandleInitialize = null;
                    HandleVideoFrame = null;
                    HandleEncode = null;
                    HandleClose = null;
                    HandleVideoFrame = null;

                }
            }
            base.Dispose(disposing);
        }

        #endregion

    }

    /**
    * The <code>VideoFrameInfo</code> class represents all information
    */
    public class VideoFrameInfo : EventArgs
    {
        public PPError Result { get; private set; }
        public VideoFrame VideoFrame { get; set; }

        public VideoFrameInfo(PPError result, PPResource videoFrame)
        {
            Result = result;
            VideoFrame = new VideoFrame(PassRef.PassRef, videoFrame);
        }

    }

    public class VideoProfileDescription
    {
        /**
        * The codec profile.
        */
        public VideoProfile Profile { get; private set; }
        /**
         * Dimensions of the maximum resolution of video frames, in pixels.
         */
        public PPSize MaxResolution { get; private set; }
        /**
         * The numerator of the maximum frame rate.
         */
        public uint MaxFramerateNumerator { get; private set; }
        /**
         * The denominator of the maximum frame rate.
         */
        public uint MaxFramerateDenominator { get; private set; }
        /**
         * Whether the profile is hardware accelerated.
         */
        public HardwareAcceleration HardwareAcceleration { get; private set; }

        internal VideoProfileDescription(PPVideoProfileDescription01 profileDescription)
        {
            Profile = (VideoProfile)profileDescription.profile;
            MaxResolution = profileDescription.max_resolution;
            MaxFramerateNumerator = profileDescription.max_framerate_numerator;
            MaxFramerateDenominator = profileDescription.max_framerate_denominator;
            HardwareAcceleration = (HardwareAcceleration)profileDescription.acceleration;
        }
    }

    public enum VideoProfile
    {
        H264baseline = 0,
        H264main = 1,
        H264extended = 2,
        H264high = 3,
        H264high10profile = 4,
        H264high422profile = 5,
        H264high444predictiveprofile = 6,
        H264scalablebaseline = 7,
        H264scalablehigh = 8,
        H264stereohigh = 9,
        H264multiviewhigh = 10,
        Vp8Any = 11,
        Vp9Any = 12,
        Max = Vp9Any
    }

    /**
    * Hardware acceleration options.
    */
    public enum HardwareAcceleration
    {
        /** Create a hardware accelerated resource only. */
        Only = 0,
        /**
         * Create a hardware accelerated resource if possible. Otherwise, fall back
         * to the software implementation.
         */
        Withfallback = 1,
        /** Create the software implementation only. */
        None = 2,
        Last = None
    }

}
