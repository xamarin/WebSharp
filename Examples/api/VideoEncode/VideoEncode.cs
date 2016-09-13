using System;
using System.Collections.Generic;

using PepperSharp;

namespace VideoEncode
{
    public class VideoEncode : Instance
    {

        bool IsEncoding { get; set; }
        bool IsEncodeTicking { get; set; }
        bool IsReceivingTrackFrames { get; set; }

        VideoEncoder videoEncoder;

        MediaStreamVideoTrack videoTrack;

        VideoProfile videoProfile;
        VideoFrameFormat frameFormat;

        PPSize requestedSize;
        PPSize frameSize;
        PPSize encoderSize;
        int encodedFrames;

        Queue<long> frameTimeStampQueue = new Queue<long>();

        VideoFrame currentTrackFrame;

        Dictionary<string, VideoProfile> profileFromString;
        Dictionary<VideoProfile, string> profileToString;
        IVFWriter ivfWriter = new IVFWriter();

        double lastEncodeTick;

        public VideoEncode(IntPtr handle) : base(handle)
        {
#if USE_VP8_INSTEAD_OF_H264
            videoProfile = VideoProfile.Vp8Any;
#else
            videoProfile = VideoProfile.H264main;
#endif
            frameFormat = VideoFrameFormat.I420;
            InitializeVideoProfiles();
            ProbeEncoder();

            HandleMessage += OnHandleMessage;
            Initialize += OnInitialize;
        }

        private void OnInitialize(object sender, InitializeEventArgs args)
        {
            LogToConsole(PPLogLevel.Log, "VideoEncode");
        }

        private void OnHandleMessage(object sender, Var varMessage)
        {
            if (!varMessage.IsDictionary)
            {
                LogToConsole(PPLogLevel.Error, "Invalid message!");
                return;
            }

            var dictMessage = new VarDictionary(varMessage);
            string command = dictMessage.Get("command").AsString();

            if (command == "start")
            {
                requestedSize = new PPSize(dictMessage.Get("width").AsInt(),
                                           dictMessage.Get("height").AsInt());
                var var_track = dictMessage.Get("track");
                if (!var_track.IsResource)
                {
                    LogToConsole(PPLogLevel.Error, "Given track is not a resource");
                    return;
                }

                var resourceTrack = new MediaStreamVideoTrack(var_track.AsResource());
                if (!resourceTrack.IsEmpty)
                {
                    videoTrack = resourceTrack;
                    videoTrack.HandleConfigure += OnConfigureTrack;
                    videoTrack.HandleFrame += OnTrackFrame;
                    videoEncoder = new VideoEncoder(this);
                    videoProfile = VideoProfileFromString(dictMessage.Get("profile").AsString());
                    ConfigureTrack();
                }
            }
            else if (command == "stop")
            {
                StopEncode();
                Log("stopped");
            }
            else
            {
                LogToConsole(PPLogLevel.Error, "Invalid command!");
            }
        }

        void StopEncode()
        {
            videoEncoder.Close();
            StopTrackFrames();
            videoTrack.Close();
            IsEncoding = false;
            encodedFrames = 0;
        }

        void StopTrackFrames()
        {
            IsReceivingTrackFrames = false;
            if (!currentTrackFrame.IsEmpty)
            {
                videoTrack.RecycleFrame(currentTrackFrame);
                currentTrackFrame.Detach();
            }
        }

        void ConfigureTrack()
        {
            if (encoderSize == PPSize.Zero)
                frameSize = requestedSize;
            else
                frameSize = encoderSize;

            var attribList = new MediaStreamVideoTrackAttributes()
            {
                Format = frameFormat,
                Width = frameSize.Width,
                Height = frameSize.Height
            };

            videoTrack.Configure(attribList);

        }
        private void OnConfigureTrack(object sender, PPError result)
        {
            if (result != PPError.Ok)
            {
                LogError(result, "Cannot configure track");
                return;
            }

            if (IsEncoding)
            {
                StartTrackFrames();
                ScheduleNextEncode();
            }
            else
                StartEncoder();
        }

        void StartTrackFrames()
        {
            IsReceivingTrackFrames = true;
            videoTrack.GetFrame();

        }

        static double Clamp(double min, double max, double value)
        {
            return Math.Max(Math.Min(value, max), min);
        }

        void ScheduleNextEncode()
        {
            
            // Avoid scheduling more than once at a time.
            if (IsEncodeTicking)
                return;
            var now = Core.Time;
            double tick = 1.0 / 30;
            // If the callback was triggered late, we need to account for that
            // delay for the next tick.
            
            var delta = tick - Clamp(0, tick, now - lastEncodeTick - tick);
            Core.CallOnMainThread(GetEncoderFrameTick, (int)(delta * 1000));

            lastEncodeTick = now;
            IsEncodeTicking = true;
        }

        private void GetEncoderFrameTick(PPError result)
        {
            IsEncodeTicking = false;
            if (IsEncoding)
            {
                if (currentTrackFrame != null && !currentTrackFrame.IsEmpty)
                {
                    var frame = new VideoFrame(currentTrackFrame);
                    currentTrackFrame.Detach();
                    GetEncoderFrame(frame);
                }
                ScheduleNextEncode();
            }
        }

        VideoFrame encoderTrackFrame;
        void GetEncoderFrame(VideoFrame trackframe)
        {
            encoderTrackFrame = trackframe;
            videoEncoder.GetVideoFrame();

        }

        private void HandleEncoderFrame(object sender, VideoFrameInfo videoFrameInfo)
        {
            var result = videoFrameInfo.Result;
            if (result == PPError.Aborted)
            {
                videoTrack.RecycleFrame(encoderTrackFrame);
                return;
            }
            if (result != PPError.Ok)
            {
                videoTrack.RecycleFrame(encoderTrackFrame);
                LogError(result, $"Cannot get video frame from video encoder {videoFrameInfo.VideoFrame} / {encoderTrackFrame}");
                return;
            }

            encoderTrackFrame.GetSize(out frameSize);

            if (frameSize != encoderSize)
            {
                videoTrack.RecycleFrame(encoderTrackFrame);
                LogError(PPError.Failed, "MediaStreamVideoTrack frame size incorrect");
                return;
            }

            if (CopyVideoFrame(videoFrameInfo.VideoFrame, encoderTrackFrame) == PPError.Ok)
                EncodeFrame(videoFrameInfo.VideoFrame);
            videoTrack.RecycleFrame(encoderTrackFrame);

        }

        PPError CopyVideoFrame(VideoFrame dest, VideoFrame src)
        {

            var destSize = dest.DataBufferSize;
            var srcSize = src.DataBufferSize;
            if (destSize < srcSize)
            {
                LogError(PPError.Failed, $"Incorrect destination video frame buffer size : {destSize} < {srcSize}");
                return PPError.Failed;
            }
            dest.TimeStamp = src.TimeStamp;
            dest.DataBuffer = src.DataBuffer;
            return PPError.Ok;
        }

        void EncodeFrame(PPResource frame) {
            frameTimeStampQueue.Enqueue((long)(PPBVideoFrame.GetTimestamp(frame) * 1000));
            videoEncoder.Encode(frame, false);

        }

        private void OnEncodeDone(object sender, PPError result)
        {
            if (result == PPError.Aborted)
                return;
            if (result != PPError.Ok)
                LogError(result, "Encode failed");
        }

        void OnTrackFrame(object sender, VideoFrameInfo frameInfo)
        {
            var result = frameInfo.Result;
            if (result == PPError.Aborted)
            {
                return;
            }

            if (currentTrackFrame != null && !currentTrackFrame.IsEmpty)
            {
                videoTrack.RecycleFrame(currentTrackFrame);
                currentTrackFrame.Detach();
            }

            if (result != PPError.Ok)
            {
                LogError(result, "Cannot get video frame from video track");
                return;
            }

            currentTrackFrame = new VideoFrame(frameInfo.VideoFrame);
            if (IsReceivingTrackFrames)
            {
                videoTrack.GetFrame();
            }
        }

        void StartEncoder()
        {
            if (!videoEncoder.IsEmpty)
                videoEncoder.Dispose();

            videoEncoder = new VideoEncoder(this);
            videoEncoder.HandleVideoFrame += HandleEncoderFrame;
            videoEncoder.HandleEncode += OnEncodeDone;
            videoEncoder.HandleBitstreamBuffer += HandleBitstreamBuffer;

            frameTimeStampQueue.Clear();

            videoEncoder.HandleInitialize += OnInitializedEncoder;
            var error = videoEncoder.Initialize(
                frameFormat, frameSize, videoProfile, 2000000,
                HardwareAcceleration.Withfallback);

            if (error != PPError.OkCompletionpending)
            {
                LogError(error, "Cannot initialize encoder");
                return;
            }
        }

        void OnInitializedEncoder(object sender, PPError result)
        {
            if ((PPError)result != PPError.Ok)
            {
                LogError(result, "Encoder initialization failed");
                return;
            }

            IsEncoding = true;
            Log("started");

            if (videoEncoder.GetFrameCodedSize(out encoderSize) != PPError.Ok)
            {
                LogError(result, "Cannot get encoder coded frame size");
                return;
            }

            var bitResult = videoEncoder.GetBitstreamBuffer();
            if (encoderSize != frameSize)
            {
                ConfigureTrack();
            }
            else
            {
                StartTrackFrames();
                ScheduleNextEncode();
            }

        }

        private void HandleBitstreamBuffer(object sender, VideoEncoder.BitstreamBufferInfo bitstreamBufferInfo)
        {
            var result = bitstreamBufferInfo.Result;
            if (result == PPError.Aborted)
                return;

            if (result != PPError.Ok)
            {
                LogError(result, "Cannot get bitstream buffer");
                return;
            }

            encodedFrames++;

            PostDataMessage(bitstreamBufferInfo.Buffer, bitstreamBufferInfo.Size);
            videoEncoder.RecycleBitstreamBuffer(bitstreamBufferInfo);

            videoEncoder.GetBitstreamBuffer();
        }

        void ProbeEncoder()
        {
            videoEncoder = new VideoEncoder(this);
            videoEncoder.HandleEncoderProbed += HandleEncoderProbed;
            videoEncoder.GetSupportedProfiles();
        }

        private void HandleEncoderProbed(object sender, VideoEncoder.VideoProfileInfo profileInfo)
        {
            var dict = new VarDictionary();
            dict.Set("name", "supportedProfiles");
            VarArray js_profiles = new VarArray();
            dict.Set("profiles", js_profiles);

            if (profileInfo.Result < 0)
            {
                LogError(profileInfo.Result, "Cannot get supported profiles");
                PostMessage(dict);
            }

            if (profileInfo.VideoProfileDescriptions.Count > 0)
            {
                var idx = 0u;
                foreach (var profile in profileInfo.VideoProfileDescriptions)
                    js_profiles.Set(idx++, VideoProfileToString(profile.Profile));
            }
            PostMessage(dict);
        }

        void PostDataMessage(byte[] buffer, uint size)
        {
            var dictionary = new VarDictionary();

            dictionary.Set("name", "data");

            VarArrayBuffer array_buffer = null;
            byte[] dataPtr = null;
            uint dataOffset = 0;
            if (videoProfile == VideoProfile.Vp8Any ||
                videoProfile == VideoProfile.Vp9Any)
            {
                uint frameOffset = 0;
                if (encodedFrames == 1)
                {
                    array_buffer = new VarArrayBuffer(size + ivfWriter.GetFileHeaderSize() + ivfWriter.GetFrameHeaderSize());
                    dataPtr = array_buffer.Map();
                    frameOffset = ivfWriter.WriteFileHeader(dataPtr, VideoProfileToString(videoProfile).ToUpper(),
                        frameSize.Width, frameSize.Height);
                }
                else
                {
                    array_buffer = new VarArrayBuffer(size + ivfWriter.GetFrameHeaderSize());
                    dataPtr = array_buffer.Map();
                }
                var timestamp = frameTimeStampQueue.Peek();
                frameTimeStampQueue.Dequeue();
                dataOffset =
                    frameOffset +
                    ivfWriter.WriteFrameHeader(dataPtr, frameOffset, timestamp, size);
            }

            Array.Copy(buffer, 0, dataPtr, dataOffset, size);

            // Make sure we flush the buffer data back to unmanaged memeory.
            array_buffer.Flush();
            array_buffer.Unmap();
            dictionary.Set("data", array_buffer);

            PostMessage(dictionary);
        }

        void LogError(PPError error, string message)
        {
            var msg = $"Error: {error} : {message}";
            Log(msg);
        }

        void LogError(int error, string message)
        {
            LogError((PPError)error, message);

        }

        void Log(string message)
        {
            var dictionary = new VarDictionary();
            dictionary.Set("name", "log");
            dictionary.Set("message", message);

            PostMessage(dictionary);
        }

        void AddVideoProfile(VideoProfile profile,
                                           string profile_str)
        {
            profileToString.Add(profile, profile_str);
            profileFromString.Add(profile_str, profile);
        }

        void InitializeVideoProfiles()
        {
            profileFromString = new Dictionary<string, VideoProfile>();
            profileToString = new Dictionary<VideoProfile, string>();

            AddVideoProfile(VideoProfile.H264baseline, "h264baseline");
            AddVideoProfile(VideoProfile.H264main, "h264main");
            AddVideoProfile(VideoProfile.H264extended, "h264extended");
            AddVideoProfile(VideoProfile.H264high, "h264high");
            AddVideoProfile(VideoProfile.H264high10profile, "h264high10");
            AddVideoProfile(VideoProfile.H264high422profile, "h264high422");
            AddVideoProfile(VideoProfile.H264high444predictiveprofile,
                            "h264high444predictive");
            AddVideoProfile(VideoProfile.H264scalablebaseline, "h264scalablebaseline");
            AddVideoProfile(VideoProfile.H264scalablehigh, "h264scalablehigh");
            AddVideoProfile(VideoProfile.H264stereohigh, "h264stereohigh");
            AddVideoProfile(VideoProfile.H264multiviewhigh, "h264multiviewhigh");
            AddVideoProfile(VideoProfile.Vp8Any, "vp8");
            AddVideoProfile(VideoProfile.Vp9Any, "vp9");
        }

        VideoProfile VideoProfileFromString(string str)
        {
            if (!profileFromString.ContainsKey(str))
                return VideoProfile.Vp8Any;

            return profileFromString[str];

        }

        string VideoProfileToString(VideoProfile profile)
        {
            if (!profileToString.ContainsKey(profile))
                return "unknown";

            return profileToString[profile];
        }

    }
}
