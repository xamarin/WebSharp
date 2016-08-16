using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

using PepperSharp;

namespace VideoEncode
{
    public class VideoEncode : Instance
    {


        bool is_encoding_;
        bool is_encode_ticking_;
        bool is_receiving_track_frames_;

        PPResource video_encoder_;

        PPResource video_track_;

        PPVideoProfile video_profile_;
        PPVideoFrameFormat frame_format_;

        PPSize requested_size_;
        PPSize frame_size_;
        PPSize encoder_size_;
        int encoded_frames_;

        Queue<long> frames_timestamps_ = new Queue<long>();

        PPResource current_track_frame_;

        Dictionary<string, PPVideoProfile> profile_from_string_;
        Dictionary<PPVideoProfile, string> profile_to_string_;
        IVFWriter ivf_writer_ = new IVFWriter();

        double last_encode_tick_;

        public VideoEncode(IntPtr handle) : base(handle)
        {
#if USE_VP8_INSTEAD_OF_H264
            video_profile_ = PPVideoProfile.Vp8Any;
#else
            video_profile_ = PPVideoProfile.H264main;
#endif
            frame_format_ = PPVideoFrameFormat.I420;
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
                    video_encoder_ = PPBVideoEncoder.Create(this);
                    video_profile_ = VideoProfileFromString(dict_message.Get("profile").AsString());
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
            PPBVideoEncoder.Close(video_encoder_);
            StopTrackFrames();
            PPBMediaStreamVideoTrack.Close(video_track_);
            is_encoding_ = false;
            encoded_frames_ = 0;
        }

        void StopTrackFrames()
        {
            is_receiving_track_frames_ = false;
            if (!current_track_frame_.IsEmpty)
            {
                PPBMediaStreamVideoTrack.RecycleFrame(video_track_, current_track_frame_);
                current_track_frame_.Detach();
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

            if (is_encoding_)
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
            Core.CallOnMainThread(new CompletionCallback(GetEncoderFrameTick), (int)(delta * 1000));

            last_encode_tick_ = now;
            is_encode_ticking_ = true;
        }

        private void GetEncoderFrameTick(PPError result)
        {
            is_encode_ticking_ = false;
            if (is_encoding_)
            {
                if (current_track_frame_ != null && !current_track_frame_.IsEmpty)
                {
                    var frame = new PPResource(current_track_frame_);
                    current_track_frame_.Detach();
                    GetEncoderFrame(frame);
                }
                ScheduleNextEncode();
            }
        }

        void GetEncoderFrame(PPResource trackframe)
        {
            var OnEncoderFrameCallback = new CompletionCallbackWithOutput<PPResource, PPResource>(OnEncoderFrame, trackframe);
            PPBVideoEncoder.GetVideoFrame(video_encoder_, out OnEncoderFrameCallback.OutputAdapter.output, OnEncoderFrameCallback.Callback);

        }

        private void OnEncoderFrame(PPError result, PPResource encoder_frame, PPResource trackFrame)
        {
            if (result == PPError.Aborted)
            {
                PPBMediaStreamVideoTrack.RecycleFrame(video_track_, trackFrame);
                return;
            }
            if (result != PPError.Ok)
            {
                PPBMediaStreamVideoTrack.RecycleFrame(video_track_, trackFrame);
                LogError(result, $"Cannot get video frame from video encoder {encoder_frame} / {trackFrame}");
                return;
            }

            PPBVideoFrame.GetSize(trackFrame, out frame_size_);

            if (frame_size_ != encoder_size_)
            {
                PPBMediaStreamVideoTrack.RecycleFrame(video_track_, trackFrame);
                LogError(PPError.Failed, "MediaStreamVideoTrack frame size incorrect");
                return;
            }

            if (CopyVideoFrame(encoder_frame, trackFrame) == PPError.Ok)
                EncodeFrame(encoder_frame);
            PPBMediaStreamVideoTrack.RecycleFrame(video_track_, trackFrame);
        }

        PPError CopyVideoFrame(PPResource dest, PPResource src)
        {

            var destSize = PPBVideoFrame.GetDataBufferSize(dest);
            var srcSize = PPBVideoFrame.GetDataBufferSize(src);
            if (PPBVideoFrame.GetDataBufferSize(dest) < PPBVideoFrame.GetDataBufferSize(src))
            {
                LogError(PPError.Failed, $"Incorrect destination video frame buffer size : {destSize} < {srcSize}");
                return PPError.Failed;
            }
            PPBVideoFrame.SetTimestamp(dest, PPBVideoFrame.GetTimestamp(src));
            var destBuffer = PPBVideoFrame.GetDataBuffer(dest);
            var srcBuffer = PPBVideoFrame.GetDataBuffer(src);
            CopyMemory(destBuffer, srcBuffer, srcSize);
            return PPError.Ok;
        }

        [DllImport("kernel32.dll")]
        static extern void CopyMemory(IntPtr destination, IntPtr source, uint length);

        void EncodeFrame(PPResource frame) {
            frames_timestamps_.Enqueue((long)(PPBVideoFrame.GetTimestamp(frame) * 1000));
            PPBVideoEncoder.Encode(video_encoder_, frame, PPBool.False, new CompletionCallback(OnEncodeDone));

        }

        private void OnEncodeDone(PPError result)
        {
            if ((PPError)result == PPError.Aborted)
                return;
            if ((PPError)result != PPError.Ok)
                LogError(result, "Encode failed");
        }

        void OnTrackFrame(PPError result, PPResource frame)
        {
            if (result == PPError.Aborted)
            {
                return;
            }
            if (!current_track_frame_.IsEmpty)
            {
                PPBMediaStreamVideoTrack.RecycleFrame(video_track_, current_track_frame_);
                current_track_frame_.Detach();
            }

            if (result != PPError.Ok)
            {
                LogError(result, "Cannot get video frame from video track");
                return;
            }

            current_track_frame_ = new PPResource(frame);
            if (is_receiving_track_frames_)
            {
                var OnTrackFrameCallback = new CompletionCallbackWithOutput<PPResource>(OnTrackFrame);
                PPBMediaStreamVideoTrack.GetFrame(video_track_, out OnTrackFrameCallback.OutputAdapter.output,
                    OnTrackFrameCallback.Callback);
            }
        }

        void StartEncoder()
        {
            if (video_encoder_.ppresource != 0)
                video_encoder_.Dispose();

            video_encoder_ = PPBVideoEncoder.Create(this);
            frames_timestamps_.Clear();

            var error = (PPError)PPBVideoEncoder.Initialize(video_encoder_,
                frame_format_, frame_size_, video_profile_, 2000000,
                PPHardwareAcceleration.Withfallback,
                new CompletionCallback(OnInitializedEncoder));

            if (error != PPError.OkCompletionpending)
            {
                LogError(error, "Cannot initialize encoder");
                return;
            }
        }

        void OnInitializedEncoder(PPError result)
        {
            if ((PPError)result != PPError.Ok)
            {
                LogError(result, "Encoder initialization failed");
                return;
            }

            is_encoding_ = true;
            Log("started");

            if ((PPError)PPBVideoEncoder.GetFrameCodedSize(video_encoder_, out encoder_size_) != PPError.Ok)
            {
                LogError(result, "Cannot get encoder coded frame size");
                return;
            }

            var OnGetBitstreamBufferCallback = new CompletionCallbackWithOutput<PPBitstreamBuffer>(OnGetBitstreamBuffer);

            var bitResult = (PPError)PPBVideoEncoder.GetBitstreamBuffer(video_encoder_, out OnGetBitstreamBufferCallback.OutputAdapter.output, OnGetBitstreamBufferCallback.Callback);
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
        void OnGetBitstreamBuffer(PPError result, PPBitstreamBuffer bitstreamBuffer)
        {

            if (result == PPError.Aborted)
                return;

            if (result != PPError.Ok)
            {
                LogError(result, "Cannot get bitstream buffer");
                return;
            }

            encoded_frames_++;
            var numBytes = bitstreamBuffer.size;
            var bufferMap = new byte[numBytes];
            Marshal.Copy(bitstreamBuffer.buffer, bufferMap, 0, bufferMap.Length);

            PostDataMessage(bufferMap, numBytes);
            PPBVideoEncoder.RecycleBitstreamBuffer(video_encoder_, bitstreamBuffer);
            var OnGetBitstreamBufferCallback = new CompletionCallbackWithOutput<PPBitstreamBuffer>(OnGetBitstreamBuffer);
            var bitResult = (PPError)PPBVideoEncoder.GetBitstreamBuffer(video_encoder_, out OnGetBitstreamBufferCallback.OutputAdapter.output, OnGetBitstreamBufferCallback.Callback);
        }

        void ProbeEncoder()
        {
            video_encoder_ = PPBVideoEncoder.Create(this);
            var encoderProbedCallback = new CompletionCallbackWithOutput<PPVideoProfileDescription[]>(OnEncoderProbed);
            PPBVideoEncoder.GetSupportedProfiles(video_encoder_, (PPArrayOutput)encoderProbedCallback.OutputAdapter.Adapter, encoderProbedCallback.Callback);
        }

        void OnEncoderProbed(PPError result, PPVideoProfileDescription[] profiles)
        {

            var dict = new VarDictionary();
            dict.Set("name", "supportedProfiles");
            VarArray js_profiles = new VarArray();
            dict.Set("profiles", js_profiles);

            if (result < 0)
            {
                LogError(result, "Cannot get supported profiles");
                PostMessage(dict);
            }

            if (profiles != null)
            {
                var idx = 0u;
                for (var i = 0; i < profiles.Length; i++)
                {
                    var profile = profiles[i];
                    js_profiles.Set(idx++, VideoProfileToString(profile.profile));
                }
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
            if (video_profile_ == PPVideoProfile.Vp8Any ||
                video_profile_ == PPVideoProfile.Vp9Any)
            {
                uint frame_offset = 0;
                if (encoded_frames_ == 1)
                {
                    array_buffer = new VarArrayBuffer(size + ivf_writer_.GetFileHeaderSize() + ivf_writer_.GetFrameHeaderSize());
                    data_ptr = array_buffer.Map();
                    frame_offset = ivf_writer_.WriteFileHeader(data_ptr, VideoProfileToString(video_profile_).ToUpper(),
                        frame_size_.Width, frame_size_.Height);
                }
                else
                {
                    array_buffer = new VarArrayBuffer(size + ivf_writer_.GetFrameHeaderSize());
                    data_ptr = array_buffer.Map();
                }
                var timestamp = frames_timestamps_.Peek();
                frames_timestamps_.Dequeue();
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

        void AddVideoProfile(PPVideoProfile profile,
                                           string profile_str)
        {
            profile_to_string_.Add(profile, profile_str);
            profile_from_string_.Add(profile_str, profile);
        }

        void InitializeVideoProfiles()
        {
            profile_from_string_ = new Dictionary<string, PPVideoProfile>();
            profile_to_string_ = new Dictionary<PPVideoProfile, string>();

            AddVideoProfile(PPVideoProfile.H264baseline, "h264baseline");
            AddVideoProfile(PPVideoProfile.H264main, "h264main");
            AddVideoProfile(PPVideoProfile.H264extended, "h264extended");
            AddVideoProfile(PPVideoProfile.H264high, "h264high");
            AddVideoProfile(PPVideoProfile.H264high10profile, "h264high10");
            AddVideoProfile(PPVideoProfile.H264high422profile, "h264high422");
            AddVideoProfile(PPVideoProfile.H264high444predictiveprofile,
                            "h264high444predictive");
            AddVideoProfile(PPVideoProfile.H264scalablebaseline, "h264scalablebaseline");
            AddVideoProfile(PPVideoProfile.H264scalablehigh, "h264scalablehigh");
            AddVideoProfile(PPVideoProfile.H264stereohigh, "h264stereohigh");
            AddVideoProfile(PPVideoProfile.H264multiviewhigh, "h264multiviewhigh");
            AddVideoProfile(PPVideoProfile.Vp8Any, "vp8");
            AddVideoProfile(PPVideoProfile.Vp9Any, "vp9");
        }

        PPVideoProfile VideoProfileFromString(string str)
        {
            if (!profile_from_string_.ContainsKey(str))
                return PPVideoProfile.Vp8Any;

            return profile_from_string_[str];

        }

        string VideoProfileToString(PPVideoProfile profile)
        {
            if (!profile_to_string_.ContainsKey(profile))
                return "unknown";

            return profile_to_string_[profile];
        }

    }
}
