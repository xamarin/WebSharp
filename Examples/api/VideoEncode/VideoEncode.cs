using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using PepperSharp;

namespace VideoEncode
{
    public class VideoEncode : Instance
    {


        bool IsEncoding { get; set; }
        bool is_encode_ticking_;
        bool is_receiving_track_frames_;

        VideoEncoder videoEncoder;

        PPResource video_track_;

        VideoProfile videoProfile;
        VideoFrameFormat frame_format_;

        PPSize requested_size_;
        PPSize frame_size_;
        PPSize encoder_size_;
        int encoded_frames_;

        Queue<long> frameTimeStampQueue = new Queue<long>();

        VideoFrame currentTrackFrame;

        Dictionary<string, VideoProfile> profile_from_string_;
        Dictionary<VideoProfile, string> profile_to_string_;
        IVFWriter ivf_writer_ = new IVFWriter();

        double last_encode_tick_;

        public VideoEncode(IntPtr handle) : base(handle)
        {
#if USE_VP8_INSTEAD_OF_H264
            video_profile_ = PPVideoProfile.Vp8Any;
#else
            videoProfile = VideoProfile.H264main;
#endif
            frame_format_ = VideoFrameFormat.I420;
            InitializeVideoProfiles();
            ProbeEncoder();

            HandleMessage += OnHandleMessage;
            Initialize += OnInitialize;
        }

        private void OnInitialize(object sender, InitializeEventArgs args)
        {
            LogToConsole(PPLogLevel.Log, "VideoEncode");
        }

        private void OnHandleMessage(object sender, Var var_message)
        {
            if (!var_message.IsDictionary)
            {
                LogToConsole(PPLogLevel.Error, "Invalid message!");
                return;
            }

            var dict_message = new VarDictionary(var_message);
            string command = dict_message.Get("command").AsString();

            if (command == "start")
            {
                requested_size_ = new PPSize(dict_message.Get("width").AsInt(),
                                           dict_message.Get("height").AsInt());
                var var_track = dict_message.Get("track");
                if (!var_track.IsResource)
                {
                    LogToConsole(PPLogLevel.Error, "Given track is not a resource");
                    return;
                }

                var resource_track = var_track.AsResource();
                if (PPBMediaStreamVideoTrack.IsMediaStreamVideoTrack(resource_track) == PPBool.True)
                {
                    video_track_ = resource_track;
                    videoEncoder = new VideoEncoder(this);
                    videoProfile = VideoProfileFromString(dict_message.Get("profile").AsString());
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
            PPBMediaStreamVideoTrack.Close(video_track_);
            IsEncoding = false;
            encoded_frames_ = 0;
        }

        void StopTrackFrames()
        {
            is_receiving_track_frames_ = false;
            if (!currentTrackFrame.IsEmpty)
            {
                PPBMediaStreamVideoTrack.RecycleFrame(video_track_, currentTrackFrame);
                currentTrackFrame.Detach();
            }
        }

        void ConfigureTrack()
        {
            if (encoder_size_ == PPSize.Zero)
                frame_size_ = requested_size_;
            else
                frame_size_ = encoder_size_;
            
            int[] attrib_list = {(int)PPMediaStreamVideoTrackAttrib.Format,
                               (int)frame_format_,
                               (int)PPMediaStreamVideoTrackAttrib.Width,
                               frame_size_.Width,
                               (int)PPMediaStreamVideoTrackAttrib.Height,
                               frame_size_.Height,
                               (int)PPMediaStreamVideoTrackAttrib.None};

            PPBMediaStreamVideoTrack.Configure(video_track_, attrib_list, new CompletionCallback(OnConfiguredTrack));

        }

        void OnConfiguredTrack(PPError result)
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
            is_receiving_track_frames_ = true;
            var OnTrackFrameCallback = new CompletionCallbackWithOutput<PPResource>(OnTrackFrame);
            PPBMediaStreamVideoTrack.GetFrame(video_track_, out OnTrackFrameCallback.OutputAdapter.output, OnTrackFrameCallback.Callback );

        }

        static double Clamp(double min, double max, double value)
        {
            return Math.Max(Math.Min(value, max), min);
        }

        void ScheduleNextEncode()
        {
            
            // Avoid scheduling more than once at a time.
            if (is_encode_ticking_)
                return;
            var now = Core.Time;
            double tick = 1.0 / 30;
            // If the callback was triggered late, we need to account for that
            // delay for the next tick.
            
            var delta = tick - Clamp(0, tick, now - last_encode_tick_ - tick);
            Core.CallOnMainThread(GetEncoderFrameTick, (int)(delta * 1000));

            last_encode_tick_ = now;
            is_encode_ticking_ = true;
        }

        private void GetEncoderFrameTick(PPError result)
        {
            is_encode_ticking_ = false;
            //Console.WriteLine("GetEncoderFrameTick");
            if (IsEncoding)
            {
                //Console.WriteLine("GetEncoderFrameTick 2");
                if (currentTrackFrame != null && !currentTrackFrame.IsEmpty)
                {
                    //Console.WriteLine("GetEncoderFrameTick 3");
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

        private void HandleEncoderFrame(object sender, VideoEncoder.VideoFrameInfo videoFrameInfo)
        {
            var result = videoFrameInfo.Result;
            if (result == PPError.Aborted)
            {
                PPBMediaStreamVideoTrack.RecycleFrame(video_track_, encoderTrackFrame);
                return;
            }
            if (result != PPError.Ok)
            {
                PPBMediaStreamVideoTrack.RecycleFrame(video_track_, encoderTrackFrame);
                LogError(result, $"Cannot get video frame from video encoder {videoFrameInfo.VideoFrame} / {encoderTrackFrame}");
                return;
            }

            encoderTrackFrame.GetSize(out frame_size_);

            if (frame_size_ != encoder_size_)
            {
                PPBMediaStreamVideoTrack.RecycleFrame(video_track_, encoderTrackFrame);
                LogError(PPError.Failed, "MediaStreamVideoTrack frame size incorrect");
                return;
            }

            if (CopyVideoFrame(videoFrameInfo.VideoFrame, encoderTrackFrame) == PPError.Ok)
                EncodeFrame(videoFrameInfo.VideoFrame);
            PPBMediaStreamVideoTrack.RecycleFrame(video_track_, encoderTrackFrame);

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

        void OnTrackFrame(PPError result, PPResource frame)
        {
            if (result == PPError.Aborted)
            {
                return;
            }

            if (currentTrackFrame != null && !currentTrackFrame.IsEmpty)
            {
                PPBMediaStreamVideoTrack.RecycleFrame(video_track_, currentTrackFrame);
                currentTrackFrame.Detach();
            }

            if (result != PPError.Ok)
            {
                LogError(result, "Cannot get video frame from video track");
                return;
            }

            var videoFrame = new VideoFrame(PassRef.PassRef, frame);
            currentTrackFrame = new VideoFrame(videoFrame);
            if (is_receiving_track_frames_)
            {
                var OnTrackFrameCallback = new CompletionCallbackWithOutput<PPResource>(OnTrackFrame);
                PPBMediaStreamVideoTrack.GetFrame(video_track_, out OnTrackFrameCallback.OutputAdapter.output,
                    OnTrackFrameCallback.Callback);
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
                frame_format_, frame_size_, videoProfile, 2000000,
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

            if (videoEncoder.GetFrameCodedSize(out encoder_size_) != PPError.Ok)
            {
                LogError(result, "Cannot get encoder coded frame size");
                return;
            }

            var bitResult = videoEncoder.GetBitstreamBuffer();
            if (encoder_size_ != frame_size_)
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

            encoded_frames_++;

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
            byte[] data_ptr = null;
            uint data_offset = 0;
            if (videoProfile == VideoProfile.Vp8Any ||
                videoProfile == VideoProfile.Vp9Any)
            {
                uint frame_offset = 0;
                if (encoded_frames_ == 1)
                {
                    array_buffer = new VarArrayBuffer(size + ivf_writer_.GetFileHeaderSize() + ivf_writer_.GetFrameHeaderSize());
                    data_ptr = array_buffer.Map();
                    frame_offset = ivf_writer_.WriteFileHeader(data_ptr, VideoProfileToString(videoProfile).ToUpper(),
                        frame_size_.Width, frame_size_.Height);
                }
                else
                {
                    array_buffer = new VarArrayBuffer(size + ivf_writer_.GetFrameHeaderSize());
                    data_ptr = array_buffer.Map();
                }
                var timestamp = frameTimeStampQueue.Peek();
                frameTimeStampQueue.Dequeue();
                data_offset =
                    frame_offset +
                    ivf_writer_.WriteFrameHeader(data_ptr, frame_offset, timestamp, size);
            }

            Array.Copy(buffer, 0, data_ptr, data_offset, size);

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
            profile_to_string_.Add(profile, profile_str);
            profile_from_string_.Add(profile_str, profile);
        }

        void InitializeVideoProfiles()
        {
            profile_from_string_ = new Dictionary<string, VideoProfile>();
            profile_to_string_ = new Dictionary<VideoProfile, string>();

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
            if (!profile_from_string_.ContainsKey(str))
                return VideoProfile.Vp8Any;

            return profile_from_string_[str];

        }

        string VideoProfileToString(VideoProfile profile)
        {
            if (!profile_to_string_.ContainsKey(profile))
                return "unknown";

            return profile_to_string_[profile];
        }

    }
}
