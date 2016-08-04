/* Copyright (c) 2016 Xamarin. */
#include "stdafx.h"
/* NOTE: this is auto-generated from IDL */
#include "pepper_entrypoints.h"

#include "ppapi/c/ppb.h"
#include "ppapi/c/ppb_audio.h"
#include "ppapi/c/ppb_audio_buffer.h"
#include "ppapi/c/ppb_audio_config.h"
#include "ppapi/c/ppb_audio_encoder.h"
#include "ppapi/c/ppb_console.h"
#include "ppapi/c/ppb_core.h"
#include "ppapi/c/ppb_file_io.h"
#include "ppapi/c/ppb_file_ref.h"
#include "ppapi/c/ppb_file_system.h"
#include "ppapi/c/ppb_fullscreen.h"
#include "ppapi/c/ppb_gamepad.h"
#include "ppapi/c/ppb_graphics_2d.h"
#include "ppapi/c/ppb_host_resolver.h"
#include "ppapi/c/ppb_image_data.h"
#include "ppapi/c/ppb_input_event.h"
#include "ppapi/c/ppb_instance.h"
#include "ppapi/c/ppb_media_stream_audio_track.h"
#include "ppapi/c/ppb_media_stream_video_track.h"
#include "ppapi/c/ppb_message_loop.h"
#include "ppapi/c/ppb_messaging.h"
#include "ppapi/c/ppb_mouse_cursor.h"
#include "ppapi/c/ppb_mouse_lock.h"
#include "ppapi/c/ppb_net_address.h"
#include "ppapi/c/ppb_network_list.h"
#include "ppapi/c/ppb_network_monitor.h"
#include "ppapi/c/ppb_network_proxy.h"
#include "ppapi/c/ppb_tcp_socket.h"
#include "ppapi/c/ppb_udp_socket.h"
#include "ppapi/c/ppb_url_loader.h"
#include "ppapi/c/ppb_url_request_info.h"
#include "ppapi/c/ppb_url_response_info.h"
#include "ppapi/c/ppb_var.h"
#include "ppapi/c/ppb_var_array.h"
#include "ppapi/c/ppb_var_array_buffer.h"
#include "ppapi/c/ppb_var_dictionary.h"
#include "ppapi/c/ppb_video_decoder.h"
#include "ppapi/c/ppb_video_encoder.h"
#include "ppapi/c/ppb_video_frame.h"
#include "ppapi/c/ppb_view.h"
#include "ppapi/c/ppb_websocket.h"
#include "ppapi/c/ppp_input_event.h"
#include "ppapi/c/ppp_messaging.h"
#include "ppapi/c/ppp_mouse_lock.h"

#ifndef PEPPER_EXPORT
#define PEPPER_EXPORT __declspec(dllexport)
#endif

using namespace pp;

namespace Pepper {

	namespace {
		// Specialize this function to return the interface string corresponding to the
		// PP?_XXX structure.
		template <typename T> const char* interface_name() {
			return NULL;
		}

		template <typename T> inline T const* get_interface() {
			static T const* funcs = reinterpret_cast<T const*>(
				pp::Module::Get()->GetBrowserInterface(interface_name<T>()));
			return funcs;
		}

		template <typename T> inline bool has_interface() {
			return get_interface<T>() != NULL;
		}

	}
}
/* BEGIN Declarations for all Interface Definitions. */

namespace Pepper {
	namespace {
		template <> const char*	interface_name<PPB_Audio_1_0>() {
			return PPB_AUDIO_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_Audio_1_1>() {
			return PPB_AUDIO_INTERFACE_1_1;
		}
		template <> const char*	interface_name<PPB_AudioBuffer_0_1>() {
			return PPB_AUDIOBUFFER_INTERFACE_0_1;
		}
		template <> const char*	interface_name<PPB_AudioConfig_1_0>() {
			return PPB_AUDIO_CONFIG_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_AudioConfig_1_1>() {
			return PPB_AUDIO_CONFIG_INTERFACE_1_1;
		}
		template <> const char*	interface_name<PPB_AudioEncoder_0_1>() {
			return PPB_AUDIOENCODER_INTERFACE_0_1;
		}
		template <> const char*	interface_name<PPB_Console_1_0>() {
			return PPB_CONSOLE_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_Core_1_0>() {
			return PPB_CORE_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_FileIO_1_0>() {
			return PPB_FILEIO_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_FileIO_1_1>() {
			return PPB_FILEIO_INTERFACE_1_1;
		}
		template <> const char*	interface_name<PPB_FileRef_1_0>() {
			return PPB_FILEREF_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_FileRef_1_1>() {
			return PPB_FILEREF_INTERFACE_1_1;
		}
		template <> const char*	interface_name<PPB_FileRef_1_2>() {
			return PPB_FILEREF_INTERFACE_1_2;
		}
		template <> const char*	interface_name<PPB_FileSystem_1_0>() {
			return PPB_FILESYSTEM_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_Fullscreen_1_0>() {
			return PPB_FULLSCREEN_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_Gamepad_1_0>() {
			return PPB_GAMEPAD_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_Graphics2D_1_0>() {
			return PPB_GRAPHICS_2D_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_Graphics2D_1_1>() {
			return PPB_GRAPHICS_2D_INTERFACE_1_1;
		}
		template <> const char*	interface_name<PPB_Graphics2D_1_2>() {
			return PPB_GRAPHICS_2D_INTERFACE_1_2;
		}
		template <> const char*	interface_name<PPB_HostResolver_1_0>() {
			return PPB_HOSTRESOLVER_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_ImageData_1_0>() {
			return PPB_IMAGEDATA_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_InputEvent_1_0>() {
			return PPB_INPUT_EVENT_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_MouseInputEvent_1_0>() {
			return PPB_MOUSE_INPUT_EVENT_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_MouseInputEvent_1_1>() {
			return PPB_MOUSE_INPUT_EVENT_INTERFACE_1_1;
		}
		template <> const char*	interface_name<PPB_WheelInputEvent_1_0>() {
			return PPB_WHEEL_INPUT_EVENT_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_KeyboardInputEvent_1_0>() {
			return PPB_KEYBOARD_INPUT_EVENT_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_KeyboardInputEvent_1_2>() {
			return PPB_KEYBOARD_INPUT_EVENT_INTERFACE_1_2;
		}
		template <> const char*	interface_name<PPB_TouchInputEvent_1_0>() {
			return PPB_TOUCH_INPUT_EVENT_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_IMEInputEvent_1_0>() {
			return PPB_IME_INPUT_EVENT_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_Instance_1_0>() {
			return PPB_INSTANCE_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_MediaStreamAudioTrack_0_1>() {
			return PPB_MEDIASTREAMAUDIOTRACK_INTERFACE_0_1;
		}
		template <> const char*	interface_name<PPB_MediaStreamVideoTrack_0_1>() {
			return PPB_MEDIASTREAMVIDEOTRACK_INTERFACE_0_1;
		}
		template <> const char*	interface_name<PPB_MediaStreamVideoTrack_1_0>() {
			return PPB_MEDIASTREAMVIDEOTRACK_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_MessageLoop_1_0>() {
			return PPB_MESSAGELOOP_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_Messaging_1_0>() {
			return PPB_MESSAGING_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_Messaging_1_2>() {
			return PPB_MESSAGING_INTERFACE_1_2;
		}
		template <> const char*	interface_name<PPB_MouseCursor_1_0>() {
			return PPB_MOUSECURSOR_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_MouseLock_1_0>() {
			return PPB_MOUSELOCK_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_NetAddress_1_0>() {
			return PPB_NETADDRESS_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_NetworkList_1_0>() {
			return PPB_NETWORKLIST_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_NetworkMonitor_1_0>() {
			return PPB_NETWORKMONITOR_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_NetworkProxy_1_0>() {
			return PPB_NETWORKPROXY_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_TCPSocket_1_0>() {
			return PPB_TCPSOCKET_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_TCPSocket_1_1>() {
			return PPB_TCPSOCKET_INTERFACE_1_1;
		}
		template <> const char*	interface_name<PPB_TCPSocket_1_2>() {
			return PPB_TCPSOCKET_INTERFACE_1_2;
		}
		template <> const char*	interface_name<PPB_UDPSocket_1_0>() {
			return PPB_UDPSOCKET_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_UDPSocket_1_1>() {
			return PPB_UDPSOCKET_INTERFACE_1_1;
		}
		template <> const char*	interface_name<PPB_UDPSocket_1_2>() {
			return PPB_UDPSOCKET_INTERFACE_1_2;
		}
		template <> const char*	interface_name<PPB_URLLoader_1_0>() {
			return PPB_URLLOADER_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_URLRequestInfo_1_0>() {
			return PPB_URLREQUESTINFO_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_URLResponseInfo_1_0>() {
			return PPB_URLRESPONSEINFO_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_Var_1_0>() {
			return PPB_VAR_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_Var_1_1>() {
			return PPB_VAR_INTERFACE_1_1;
		}
		template <> const char*	interface_name<PPB_Var_1_2>() {
			return PPB_VAR_INTERFACE_1_2;
		}
		template <> const char*	interface_name<PPB_VarArray_1_0>() {
			return PPB_VAR_ARRAY_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_VarArrayBuffer_1_0>() {
			return PPB_VAR_ARRAY_BUFFER_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_VarDictionary_1_0>() {
			return PPB_VAR_DICTIONARY_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_VideoDecoder_0_1>() {
			return PPB_VIDEODECODER_INTERFACE_0_1;
		}
		template <> const char*	interface_name<PPB_VideoDecoder_0_2>() {
			return PPB_VIDEODECODER_INTERFACE_0_2;
		}
		template <> const char*	interface_name<PPB_VideoDecoder_1_0>() {
			return PPB_VIDEODECODER_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_VideoDecoder_1_1>() {
			return PPB_VIDEODECODER_INTERFACE_1_1;
		}
		template <> const char*	interface_name<PPB_VideoEncoder_0_1>() {
			return PPB_VIDEOENCODER_INTERFACE_0_1;
		}
		template <> const char*	interface_name<PPB_VideoEncoder_0_2>() {
			return PPB_VIDEOENCODER_INTERFACE_0_2;
		}
		template <> const char*	interface_name<PPB_VideoFrame_0_1>() {
			return PPB_VIDEOFRAME_INTERFACE_0_1;
		}
		template <> const char*	interface_name<PPB_View_1_0>() {
			return PPB_VIEW_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_View_1_1>() {
			return PPB_VIEW_INTERFACE_1_1;
		}
		template <> const char*	interface_name<PPB_View_1_2>() {
			return PPB_VIEW_INTERFACE_1_2;
		}
		template <> const char*	interface_name<PPB_WebSocket_1_0>() {
			return PPB_WEBSOCKET_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPP_InputEvent_0_1>() {
			return PPP_INPUT_EVENT_INTERFACE_0_1;
		}
		template <> const char*	interface_name<PPP_Messaging_1_0>() {
			return PPP_MESSAGING_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPP_MouseLock_1_0>() {
			return PPP_MOUSELOCK_INTERFACE_1_0;
		}
	}
}
/* END Declarations for all Interface Definitions. */

namespace Pepper {


	/* We don't want name mangling for these external functions.  We only need
	* 'extern "C"' if we're compiling with a C++ compiler.
	*/
#ifdef __cplusplus
	extern "C" {
#endif
	namespace {

		#pragma region /* Begin entry point methods for PPB_Audio */

		PEPPER_EXPORT PP_Resource PPB_Audio_Create(PP_Instance instance, PP_Resource config, PPB_Audio_Callback audio_callback, void* user_data) {
			if (has_interface<PPB_Audio_1_1>()) {
				return get_interface<PPB_Audio_1_1>()->Create(instance, config, audio_callback, user_data);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_Audio_IsAudio(PP_Resource resource) {
			if (has_interface<PPB_Audio_1_1>()) {
				return get_interface<PPB_Audio_1_1>()->IsAudio(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Resource PPB_Audio_GetCurrentConfig(PP_Resource audio) {
			if (has_interface<PPB_Audio_1_1>()) {
				return get_interface<PPB_Audio_1_1>()->GetCurrentConfig(audio);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_Audio_StartPlayback(PP_Resource audio) {
			if (has_interface<PPB_Audio_1_1>()) {
				return get_interface<PPB_Audio_1_1>()->StartPlayback(audio);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_Audio_StopPlayback(PP_Resource audio) {
			if (has_interface<PPB_Audio_1_1>()) {
				return get_interface<PPB_Audio_1_1>()->StopPlayback(audio);
			}
			return PP_FromBool(FALSE);
		}

		#pragma endregion /* End entry point generation for PPB_Audio */

		#pragma region /* Begin entry point methods for PPB_AudioBuffer */

		PEPPER_EXPORT PP_Bool PPB_AudioBuffer_IsAudioBuffer(PP_Resource resource) {
			if (has_interface<PPB_AudioBuffer_0_1>()) {
				return get_interface<PPB_AudioBuffer_0_1>()->IsAudioBuffer(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_TimeDelta PPB_AudioBuffer_GetTimestamp(PP_Resource buffer) {
			if (has_interface<PPB_AudioBuffer_0_1>()) {
				return get_interface<PPB_AudioBuffer_0_1>()->GetTimestamp(buffer);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_AudioBuffer_SetTimestamp(PP_Resource buffer, PP_TimeDelta timestamp) {
			if (has_interface<PPB_AudioBuffer_0_1>()) {
				get_interface<PPB_AudioBuffer_0_1>()->SetTimestamp(buffer, timestamp);
			}
			return ;
		}

		PEPPER_EXPORT PP_AudioBuffer_SampleRate PPB_AudioBuffer_GetSampleRate(PP_Resource buffer) {
			if (has_interface<PPB_AudioBuffer_0_1>()) {
				return get_interface<PPB_AudioBuffer_0_1>()->GetSampleRate(buffer);
			}
			return PP_AUDIOBUFFER_SAMPLERATE_UNKNOWN;
		}

		PEPPER_EXPORT PP_AudioBuffer_SampleSize PPB_AudioBuffer_GetSampleSize(PP_Resource buffer) {
			if (has_interface<PPB_AudioBuffer_0_1>()) {
				return get_interface<PPB_AudioBuffer_0_1>()->GetSampleSize(buffer);
			}
			return PP_AUDIOBUFFER_SAMPLESIZE_UNKNOWN;
		}

		PEPPER_EXPORT uint32_t PPB_AudioBuffer_GetNumberOfChannels(PP_Resource buffer) {
			if (has_interface<PPB_AudioBuffer_0_1>()) {
				return get_interface<PPB_AudioBuffer_0_1>()->GetNumberOfChannels(buffer);
			}
			return NULL;
		}

		PEPPER_EXPORT uint32_t PPB_AudioBuffer_GetNumberOfSamples(PP_Resource buffer) {
			if (has_interface<PPB_AudioBuffer_0_1>()) {
				return get_interface<PPB_AudioBuffer_0_1>()->GetNumberOfSamples(buffer);
			}
			return NULL;
		}

		PEPPER_EXPORT void* PPB_AudioBuffer_GetDataBuffer(PP_Resource buffer) {
			if (has_interface<PPB_AudioBuffer_0_1>()) {
				return get_interface<PPB_AudioBuffer_0_1>()->GetDataBuffer(buffer);
			}
			return NULL;
		}

		PEPPER_EXPORT uint32_t PPB_AudioBuffer_GetDataBufferSize(PP_Resource buffer) {
			if (has_interface<PPB_AudioBuffer_0_1>()) {
				return get_interface<PPB_AudioBuffer_0_1>()->GetDataBufferSize(buffer);
			}
			return NULL;
		}

		#pragma endregion /* End entry point generation for PPB_AudioBuffer */

		#pragma region /* Begin entry point methods for PPB_AudioConfig */

		PEPPER_EXPORT PP_Resource PPB_AudioConfig_CreateStereo16Bit(PP_Instance instance, PP_AudioSampleRate sample_rate, uint32_t sample_frame_count) {
			if (has_interface<PPB_AudioConfig_1_1>()) {
				return get_interface<PPB_AudioConfig_1_1>()->CreateStereo16Bit(instance, sample_rate, sample_frame_count);
			}
			else if (has_interface<PPB_AudioConfig_1_0>()) {
				return get_interface<PPB_AudioConfig_1_0>()->CreateStereo16Bit(instance, sample_rate, sample_frame_count);
			}
			return NULL;
		}

		PEPPER_EXPORT uint32_t PPB_AudioConfig_RecommendSampleFrameCount(PP_Instance instance, PP_AudioSampleRate sample_rate, uint32_t requested_sample_frame_count) {
			if (has_interface<PPB_AudioConfig_1_1>()) {
				return get_interface<PPB_AudioConfig_1_1>()->RecommendSampleFrameCount(instance, sample_rate, requested_sample_frame_count);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_AudioConfig_IsAudioConfig(PP_Resource resource) {
			if (has_interface<PPB_AudioConfig_1_1>()) {
				return get_interface<PPB_AudioConfig_1_1>()->IsAudioConfig(resource);
			}
			else if (has_interface<PPB_AudioConfig_1_0>()) {
				return get_interface<PPB_AudioConfig_1_0>()->IsAudioConfig(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_AudioSampleRate PPB_AudioConfig_GetSampleRate(PP_Resource config) {
			if (has_interface<PPB_AudioConfig_1_1>()) {
				return get_interface<PPB_AudioConfig_1_1>()->GetSampleRate(config);
			}
			else if (has_interface<PPB_AudioConfig_1_0>()) {
				return get_interface<PPB_AudioConfig_1_0>()->GetSampleRate(config);
			}
			return PP_AUDIOSAMPLERATE_NONE;
		}

		PEPPER_EXPORT uint32_t PPB_AudioConfig_GetSampleFrameCount(PP_Resource config) {
			if (has_interface<PPB_AudioConfig_1_1>()) {
				return get_interface<PPB_AudioConfig_1_1>()->GetSampleFrameCount(config);
			}
			else if (has_interface<PPB_AudioConfig_1_0>()) {
				return get_interface<PPB_AudioConfig_1_0>()->GetSampleFrameCount(config);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_AudioSampleRate PPB_AudioConfig_RecommendSampleRate(PP_Instance instance) {
			if (has_interface<PPB_AudioConfig_1_1>()) {
				return get_interface<PPB_AudioConfig_1_1>()->RecommendSampleRate(instance);
			}
			return PP_AUDIOSAMPLERATE_NONE;
		}

		#pragma endregion /* End entry point generation for PPB_AudioConfig */

		#pragma region /* Begin entry point methods for PPB_AudioEncoder */

		PEPPER_EXPORT PP_Resource PPB_AudioEncoder_Create(PP_Instance instance) {
			if (has_interface<PPB_AudioEncoder_0_1>()) {
				return get_interface<PPB_AudioEncoder_0_1>()->Create(instance);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_AudioEncoder_IsAudioEncoder(PP_Resource resource) {
			if (has_interface<PPB_AudioEncoder_0_1>()) {
				return get_interface<PPB_AudioEncoder_0_1>()->IsAudioEncoder(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT int32_t PPB_AudioEncoder_GetSupportedProfiles(PP_Resource audio_encoder, struct PP_ArrayOutput output, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_AudioEncoder_0_1>()) {
				return get_interface<PPB_AudioEncoder_0_1>()->GetSupportedProfiles(audio_encoder, output, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_AudioEncoder_Initialize(PP_Resource audio_encoder, uint32_t channels, PP_AudioBuffer_SampleRate input_sample_rate, PP_AudioBuffer_SampleSize input_sample_size, PP_AudioProfile output_profile, uint32_t initial_bitrate, PP_HardwareAcceleration acceleration, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_AudioEncoder_0_1>()) {
				return get_interface<PPB_AudioEncoder_0_1>()->Initialize(audio_encoder, channels, input_sample_rate, input_sample_size, output_profile, initial_bitrate, acceleration, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_AudioEncoder_GetNumberOfSamples(PP_Resource audio_encoder) {
			if (has_interface<PPB_AudioEncoder_0_1>()) {
				return get_interface<PPB_AudioEncoder_0_1>()->GetNumberOfSamples(audio_encoder);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_AudioEncoder_GetBuffer(PP_Resource audio_encoder, PP_Resource* audio_buffer, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_AudioEncoder_0_1>()) {
				return get_interface<PPB_AudioEncoder_0_1>()->GetBuffer(audio_encoder, audio_buffer, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_AudioEncoder_Encode(PP_Resource audio_encoder, PP_Resource audio_buffer, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_AudioEncoder_0_1>()) {
				return get_interface<PPB_AudioEncoder_0_1>()->Encode(audio_encoder, audio_buffer, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_AudioEncoder_GetBitstreamBuffer(PP_Resource audio_encoder, struct PP_AudioBitstreamBuffer* bitstream_buffer, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_AudioEncoder_0_1>()) {
				return get_interface<PPB_AudioEncoder_0_1>()->GetBitstreamBuffer(audio_encoder, bitstream_buffer, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_AudioEncoder_RecycleBitstreamBuffer(PP_Resource audio_encoder, struct PP_AudioBitstreamBuffer bitstream_buffer) {
			if (has_interface<PPB_AudioEncoder_0_1>()) {
				get_interface<PPB_AudioEncoder_0_1>()->RecycleBitstreamBuffer(audio_encoder, &bitstream_buffer);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_AudioEncoder_RequestBitrateChange(PP_Resource audio_encoder, uint32_t bitrate) {
			if (has_interface<PPB_AudioEncoder_0_1>()) {
				get_interface<PPB_AudioEncoder_0_1>()->RequestBitrateChange(audio_encoder, bitrate);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_AudioEncoder_Close(PP_Resource audio_encoder) {
			if (has_interface<PPB_AudioEncoder_0_1>()) {
				get_interface<PPB_AudioEncoder_0_1>()->Close(audio_encoder);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_AudioEncoder */

		#pragma region /* Begin entry point methods for PPB_Console */

		PEPPER_EXPORT void PPB_Console_Log(PP_Instance instance, PP_LogLevel level, struct PP_Var value) {
			if (has_interface<PPB_Console_1_0>()) {
				get_interface<PPB_Console_1_0>()->Log(instance, level, value);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_Console_LogWithSource(PP_Instance instance, PP_LogLevel level, struct PP_Var source, struct PP_Var value) {
			if (has_interface<PPB_Console_1_0>()) {
				get_interface<PPB_Console_1_0>()->LogWithSource(instance, level, source, value);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_Console */

		#pragma region /* Begin entry point methods for PPB_Core */

		PEPPER_EXPORT void PPB_Core_AddRefResource(PP_Resource resource) {
			if (has_interface<PPB_Core_1_0>()) {
				get_interface<PPB_Core_1_0>()->AddRefResource(resource);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_Core_ReleaseResource(PP_Resource resource) {
			if (has_interface<PPB_Core_1_0>()) {
				get_interface<PPB_Core_1_0>()->ReleaseResource(resource);
			}
			return ;
		}

		PEPPER_EXPORT PP_Time PPB_Core_GetTime(void) {
			if (has_interface<PPB_Core_1_0>()) {
				return get_interface<PPB_Core_1_0>()->GetTime();
			}
			return NULL;
		}

		PEPPER_EXPORT PP_TimeTicks PPB_Core_GetTimeTicks(void) {
			if (has_interface<PPB_Core_1_0>()) {
				return get_interface<PPB_Core_1_0>()->GetTimeTicks();
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_Core_CallOnMainThread(int32_t delay_in_milliseconds, struct PP_CompletionCallback callback, int32_t result) {
			if (has_interface<PPB_Core_1_0>()) {
				get_interface<PPB_Core_1_0>()->CallOnMainThread(delay_in_milliseconds, callback, result);
			}
			return ;
		}

		PEPPER_EXPORT PP_Bool PPB_Core_IsMainThread(void) {
			if (has_interface<PPB_Core_1_0>()) {
				return get_interface<PPB_Core_1_0>()->IsMainThread();
			}
			return PP_FromBool(FALSE);
		}

		#pragma endregion /* End entry point generation for PPB_Core */

		#pragma region /* Begin entry point methods for PPB_FileIO */

		PEPPER_EXPORT PP_Resource PPB_FileIO_Create(PP_Instance instance) {
			if (has_interface<PPB_FileIO_1_1>()) {
				return get_interface<PPB_FileIO_1_1>()->Create(instance);
			}
			else if (has_interface<PPB_FileIO_1_0>()) {
				return get_interface<PPB_FileIO_1_0>()->Create(instance);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_FileIO_IsFileIO(PP_Resource resource) {
			if (has_interface<PPB_FileIO_1_1>()) {
				return get_interface<PPB_FileIO_1_1>()->IsFileIO(resource);
			}
			else if (has_interface<PPB_FileIO_1_0>()) {
				return get_interface<PPB_FileIO_1_0>()->IsFileIO(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT int32_t PPB_FileIO_Open(PP_Resource file_io, PP_Resource file_ref, int32_t open_flags, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_FileIO_1_1>()) {
				return get_interface<PPB_FileIO_1_1>()->Open(file_io, file_ref, open_flags, callback);
			}
			else if (has_interface<PPB_FileIO_1_0>()) {
				return get_interface<PPB_FileIO_1_0>()->Open(file_io, file_ref, open_flags, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_FileIO_Query(PP_Resource file_io, struct PP_FileInfo* info, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_FileIO_1_1>()) {
				return get_interface<PPB_FileIO_1_1>()->Query(file_io, info, callback);
			}
			else if (has_interface<PPB_FileIO_1_0>()) {
				return get_interface<PPB_FileIO_1_0>()->Query(file_io, info, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_FileIO_Touch(PP_Resource file_io, PP_Time last_access_time, PP_Time last_modified_time, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_FileIO_1_1>()) {
				return get_interface<PPB_FileIO_1_1>()->Touch(file_io, last_access_time, last_modified_time, callback);
			}
			else if (has_interface<PPB_FileIO_1_0>()) {
				return get_interface<PPB_FileIO_1_0>()->Touch(file_io, last_access_time, last_modified_time, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_FileIO_Read(PP_Resource file_io, int64_t offset, char* buffer, int32_t bytes_to_read, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_FileIO_1_1>()) {
				return get_interface<PPB_FileIO_1_1>()->Read(file_io, offset, buffer, bytes_to_read, callback);
			}
			else if (has_interface<PPB_FileIO_1_0>()) {
				return get_interface<PPB_FileIO_1_0>()->Read(file_io, offset, buffer, bytes_to_read, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_FileIO_Write(PP_Resource file_io, int64_t offset, const char* buffer, int32_t bytes_to_write, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_FileIO_1_1>()) {
				return get_interface<PPB_FileIO_1_1>()->Write(file_io, offset, buffer, bytes_to_write, callback);
			}
			else if (has_interface<PPB_FileIO_1_0>()) {
				return get_interface<PPB_FileIO_1_0>()->Write(file_io, offset, buffer, bytes_to_write, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_FileIO_SetLength(PP_Resource file_io, int64_t length, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_FileIO_1_1>()) {
				return get_interface<PPB_FileIO_1_1>()->SetLength(file_io, length, callback);
			}
			else if (has_interface<PPB_FileIO_1_0>()) {
				return get_interface<PPB_FileIO_1_0>()->SetLength(file_io, length, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_FileIO_Flush(PP_Resource file_io, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_FileIO_1_1>()) {
				return get_interface<PPB_FileIO_1_1>()->Flush(file_io, callback);
			}
			else if (has_interface<PPB_FileIO_1_0>()) {
				return get_interface<PPB_FileIO_1_0>()->Flush(file_io, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_FileIO_Close(PP_Resource file_io) {
			if (has_interface<PPB_FileIO_1_1>()) {
				get_interface<PPB_FileIO_1_1>()->Close(file_io);
			}
			else if (has_interface<PPB_FileIO_1_0>()) {
				get_interface<PPB_FileIO_1_0>()->Close(file_io);
			}
			return ;
		}

		PEPPER_EXPORT int32_t PPB_FileIO_ReadToArray(PP_Resource file_io, int64_t offset, int32_t max_read_length, struct PP_ArrayOutput* output, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_FileIO_1_1>()) {
				return get_interface<PPB_FileIO_1_1>()->ReadToArray(file_io, offset, max_read_length, output, callback);
			}
			return NULL;
		}

		#pragma endregion /* End entry point generation for PPB_FileIO */

		#pragma region /* Begin entry point methods for PPB_FileRef */

		PEPPER_EXPORT PP_Resource PPB_FileRef_Create(PP_Resource file_system, const char* path) {
			if (has_interface<PPB_FileRef_1_2>()) {
				return get_interface<PPB_FileRef_1_2>()->Create(file_system, path);
			}
			else if (has_interface<PPB_FileRef_1_1>()) {
				return get_interface<PPB_FileRef_1_1>()->Create(file_system, path);
			}
			else if (has_interface<PPB_FileRef_1_0>()) {
				return get_interface<PPB_FileRef_1_0>()->Create(file_system, path);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_FileRef_IsFileRef(PP_Resource resource) {
			if (has_interface<PPB_FileRef_1_2>()) {
				return get_interface<PPB_FileRef_1_2>()->IsFileRef(resource);
			}
			else if (has_interface<PPB_FileRef_1_1>()) {
				return get_interface<PPB_FileRef_1_1>()->IsFileRef(resource);
			}
			else if (has_interface<PPB_FileRef_1_0>()) {
				return get_interface<PPB_FileRef_1_0>()->IsFileRef(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_FileSystemType PPB_FileRef_GetFileSystemType(PP_Resource file_ref) {
			if (has_interface<PPB_FileRef_1_2>()) {
				return get_interface<PPB_FileRef_1_2>()->GetFileSystemType(file_ref);
			}
			else if (has_interface<PPB_FileRef_1_1>()) {
				return get_interface<PPB_FileRef_1_1>()->GetFileSystemType(file_ref);
			}
			else if (has_interface<PPB_FileRef_1_0>()) {
				return get_interface<PPB_FileRef_1_0>()->GetFileSystemType(file_ref);
			}
			return PP_FILESYSTEMTYPE_EXTERNAL;
		}

		PEPPER_EXPORT struct PP_Var PPB_FileRef_GetName(PP_Resource file_ref) {
			if (has_interface<PPB_FileRef_1_2>()) {
				return get_interface<PPB_FileRef_1_2>()->GetName(file_ref);
			}
			else if (has_interface<PPB_FileRef_1_1>()) {
				return get_interface<PPB_FileRef_1_1>()->GetName(file_ref);
			}
			else if (has_interface<PPB_FileRef_1_0>()) {
				return get_interface<PPB_FileRef_1_0>()->GetName(file_ref);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT struct PP_Var PPB_FileRef_GetPath(PP_Resource file_ref) {
			if (has_interface<PPB_FileRef_1_2>()) {
				return get_interface<PPB_FileRef_1_2>()->GetPath(file_ref);
			}
			else if (has_interface<PPB_FileRef_1_1>()) {
				return get_interface<PPB_FileRef_1_1>()->GetPath(file_ref);
			}
			else if (has_interface<PPB_FileRef_1_0>()) {
				return get_interface<PPB_FileRef_1_0>()->GetPath(file_ref);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT PP_Resource PPB_FileRef_GetParent(PP_Resource file_ref) {
			if (has_interface<PPB_FileRef_1_2>()) {
				return get_interface<PPB_FileRef_1_2>()->GetParent(file_ref);
			}
			else if (has_interface<PPB_FileRef_1_1>()) {
				return get_interface<PPB_FileRef_1_1>()->GetParent(file_ref);
			}
			else if (has_interface<PPB_FileRef_1_0>()) {
				return get_interface<PPB_FileRef_1_0>()->GetParent(file_ref);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_FileRef_MakeDirectory(PP_Resource directory_ref, int32_t make_directory_flags, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_FileRef_1_2>()) {
				return get_interface<PPB_FileRef_1_2>()->MakeDirectory(directory_ref, make_directory_flags, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_FileRef_Touch(PP_Resource file_ref, PP_Time last_access_time, PP_Time last_modified_time, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_FileRef_1_2>()) {
				return get_interface<PPB_FileRef_1_2>()->Touch(file_ref, last_access_time, last_modified_time, callback);
			}
			else if (has_interface<PPB_FileRef_1_1>()) {
				return get_interface<PPB_FileRef_1_1>()->Touch(file_ref, last_access_time, last_modified_time, callback);
			}
			else if (has_interface<PPB_FileRef_1_0>()) {
				return get_interface<PPB_FileRef_1_0>()->Touch(file_ref, last_access_time, last_modified_time, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_FileRef_Delete(PP_Resource file_ref, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_FileRef_1_2>()) {
				return get_interface<PPB_FileRef_1_2>()->Delete(file_ref, callback);
			}
			else if (has_interface<PPB_FileRef_1_1>()) {
				return get_interface<PPB_FileRef_1_1>()->Delete(file_ref, callback);
			}
			else if (has_interface<PPB_FileRef_1_0>()) {
				return get_interface<PPB_FileRef_1_0>()->Delete(file_ref, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_FileRef_Rename(PP_Resource file_ref, PP_Resource new_file_ref, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_FileRef_1_2>()) {
				return get_interface<PPB_FileRef_1_2>()->Rename(file_ref, new_file_ref, callback);
			}
			else if (has_interface<PPB_FileRef_1_1>()) {
				return get_interface<PPB_FileRef_1_1>()->Rename(file_ref, new_file_ref, callback);
			}
			else if (has_interface<PPB_FileRef_1_0>()) {
				return get_interface<PPB_FileRef_1_0>()->Rename(file_ref, new_file_ref, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_FileRef_Query(PP_Resource file_ref, struct PP_FileInfo* info, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_FileRef_1_2>()) {
				return get_interface<PPB_FileRef_1_2>()->Query(file_ref, info, callback);
			}
			else if (has_interface<PPB_FileRef_1_1>()) {
				return get_interface<PPB_FileRef_1_1>()->Query(file_ref, info, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_FileRef_ReadDirectoryEntries(PP_Resource file_ref, struct PP_ArrayOutput output, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_FileRef_1_2>()) {
				return get_interface<PPB_FileRef_1_2>()->ReadDirectoryEntries(file_ref, output, callback);
			}
			else if (has_interface<PPB_FileRef_1_1>()) {
				return get_interface<PPB_FileRef_1_1>()->ReadDirectoryEntries(file_ref, output, callback);
			}
			return NULL;
		}

		#pragma endregion /* End entry point generation for PPB_FileRef */

		#pragma region /* Begin entry point methods for PPB_FileSystem */

		PEPPER_EXPORT PP_Resource PPB_FileSystem_Create(PP_Instance instance, PP_FileSystemType type) {
			if (has_interface<PPB_FileSystem_1_0>()) {
				return get_interface<PPB_FileSystem_1_0>()->Create(instance, type);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_FileSystem_IsFileSystem(PP_Resource resource) {
			if (has_interface<PPB_FileSystem_1_0>()) {
				return get_interface<PPB_FileSystem_1_0>()->IsFileSystem(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT int32_t PPB_FileSystem_Open(PP_Resource file_system, int64_t expected_size, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_FileSystem_1_0>()) {
				return get_interface<PPB_FileSystem_1_0>()->Open(file_system, expected_size, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_FileSystemType PPB_FileSystem_GetType(PP_Resource file_system) {
			if (has_interface<PPB_FileSystem_1_0>()) {
				return get_interface<PPB_FileSystem_1_0>()->GetType(file_system);
			}
			return PP_FILESYSTEMTYPE_EXTERNAL;
		}

		#pragma endregion /* End entry point generation for PPB_FileSystem */

		#pragma region /* Begin entry point methods for PPB_Fullscreen */

		PEPPER_EXPORT PP_Bool PPB_Fullscreen_IsFullscreen(PP_Instance instance) {
			if (has_interface<PPB_Fullscreen_1_0>()) {
				return get_interface<PPB_Fullscreen_1_0>()->IsFullscreen(instance);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_Fullscreen_SetFullscreen(PP_Instance instance, PP_Bool fullscreen) {
			if (has_interface<PPB_Fullscreen_1_0>()) {
				return get_interface<PPB_Fullscreen_1_0>()->SetFullscreen(instance, fullscreen);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_Fullscreen_GetScreenSize(PP_Instance instance, struct PP_Size* size) {
			if (has_interface<PPB_Fullscreen_1_0>()) {
				return get_interface<PPB_Fullscreen_1_0>()->GetScreenSize(instance, size);
			}
			return PP_FromBool(FALSE);
		}

		#pragma endregion /* End entry point generation for PPB_Fullscreen */

		#pragma region /* Begin entry point methods for PPB_Gamepad */

		PEPPER_EXPORT void PPB_Gamepad_Sample(PP_Instance instance, struct PP_GamepadsSampleData* data) {
			if (has_interface<PPB_Gamepad_1_0>()) {
				get_interface<PPB_Gamepad_1_0>()->Sample(instance, data);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_Gamepad */

		#pragma region /* Begin entry point methods for PPB_Graphics2D */

		PEPPER_EXPORT PP_Resource PPB_Graphics2D_Create(PP_Instance instance, struct PP_Size size, PP_Bool is_always_opaque) {
			if (has_interface<PPB_Graphics2D_1_2>()) {
				return get_interface<PPB_Graphics2D_1_2>()->Create(instance, &size, is_always_opaque);
			}
			else if (has_interface<PPB_Graphics2D_1_1>()) {
				return get_interface<PPB_Graphics2D_1_1>()->Create(instance, &size, is_always_opaque);
			}
			else if (has_interface<PPB_Graphics2D_1_0>()) {
				return get_interface<PPB_Graphics2D_1_0>()->Create(instance, &size, is_always_opaque);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_Graphics2D_IsGraphics2D(PP_Resource resource) {
			if (has_interface<PPB_Graphics2D_1_2>()) {
				return get_interface<PPB_Graphics2D_1_2>()->IsGraphics2D(resource);
			}
			else if (has_interface<PPB_Graphics2D_1_1>()) {
				return get_interface<PPB_Graphics2D_1_1>()->IsGraphics2D(resource);
			}
			else if (has_interface<PPB_Graphics2D_1_0>()) {
				return get_interface<PPB_Graphics2D_1_0>()->IsGraphics2D(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_Graphics2D_Describe(PP_Resource graphics_2d, struct PP_Size* size, PP_Bool* is_always_opaque) {
			if (has_interface<PPB_Graphics2D_1_2>()) {
				return get_interface<PPB_Graphics2D_1_2>()->Describe(graphics_2d, size, is_always_opaque);
			}
			else if (has_interface<PPB_Graphics2D_1_1>()) {
				return get_interface<PPB_Graphics2D_1_1>()->Describe(graphics_2d, size, is_always_opaque);
			}
			else if (has_interface<PPB_Graphics2D_1_0>()) {
				return get_interface<PPB_Graphics2D_1_0>()->Describe(graphics_2d, size, is_always_opaque);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT void PPB_Graphics2D_PaintImageData(PP_Resource graphics_2d, PP_Resource image_data, struct PP_Point top_left, struct PP_Rect src_rect) {
			if (has_interface<PPB_Graphics2D_1_2>()) {
				get_interface<PPB_Graphics2D_1_2>()->PaintImageData(graphics_2d, image_data, &top_left, &src_rect);
			}
			else if (has_interface<PPB_Graphics2D_1_1>()) {
				get_interface<PPB_Graphics2D_1_1>()->PaintImageData(graphics_2d, image_data, &top_left, &src_rect);
			}
			else if (has_interface<PPB_Graphics2D_1_0>()) {
				get_interface<PPB_Graphics2D_1_0>()->PaintImageData(graphics_2d, image_data, &top_left, &src_rect);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_Graphics2D_Scroll(PP_Resource graphics_2d, struct PP_Rect clip_rect, struct PP_Point amount) {
			if (has_interface<PPB_Graphics2D_1_2>()) {
				get_interface<PPB_Graphics2D_1_2>()->Scroll(graphics_2d, &clip_rect, &amount);
			}
			else if (has_interface<PPB_Graphics2D_1_1>()) {
				get_interface<PPB_Graphics2D_1_1>()->Scroll(graphics_2d, &clip_rect, &amount);
			}
			else if (has_interface<PPB_Graphics2D_1_0>()) {
				get_interface<PPB_Graphics2D_1_0>()->Scroll(graphics_2d, &clip_rect, &amount);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_Graphics2D_ReplaceContents(PP_Resource graphics_2d, PP_Resource image_data) {
			if (has_interface<PPB_Graphics2D_1_2>()) {
				get_interface<PPB_Graphics2D_1_2>()->ReplaceContents(graphics_2d, image_data);
			}
			else if (has_interface<PPB_Graphics2D_1_1>()) {
				get_interface<PPB_Graphics2D_1_1>()->ReplaceContents(graphics_2d, image_data);
			}
			else if (has_interface<PPB_Graphics2D_1_0>()) {
				get_interface<PPB_Graphics2D_1_0>()->ReplaceContents(graphics_2d, image_data);
			}
			return ;
		}

		PEPPER_EXPORT int32_t PPB_Graphics2D_Flush(PP_Resource graphics_2d, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_Graphics2D_1_2>()) {
				return get_interface<PPB_Graphics2D_1_2>()->Flush(graphics_2d, callback);
			}
			else if (has_interface<PPB_Graphics2D_1_1>()) {
				return get_interface<PPB_Graphics2D_1_1>()->Flush(graphics_2d, callback);
			}
			else if (has_interface<PPB_Graphics2D_1_0>()) {
				return get_interface<PPB_Graphics2D_1_0>()->Flush(graphics_2d, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_Graphics2D_SetScale(PP_Resource resource, float scale) {
			if (has_interface<PPB_Graphics2D_1_2>()) {
				return get_interface<PPB_Graphics2D_1_2>()->SetScale(resource, scale);
			}
			else if (has_interface<PPB_Graphics2D_1_1>()) {
				return get_interface<PPB_Graphics2D_1_1>()->SetScale(resource, scale);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT float PPB_Graphics2D_GetScale(PP_Resource resource) {
			if (has_interface<PPB_Graphics2D_1_2>()) {
				return get_interface<PPB_Graphics2D_1_2>()->GetScale(resource);
			}
			else if (has_interface<PPB_Graphics2D_1_1>()) {
				return get_interface<PPB_Graphics2D_1_1>()->GetScale(resource);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_Graphics2D_SetLayerTransform(PP_Resource resource, float scale, struct PP_Point origin, struct PP_Point translate) {
			if (has_interface<PPB_Graphics2D_1_2>()) {
				return get_interface<PPB_Graphics2D_1_2>()->SetLayerTransform(resource, scale, &origin, &translate);
			}
			return PP_FromBool(FALSE);
		}

		#pragma endregion /* End entry point generation for PPB_Graphics2D */

		#pragma region /* Begin entry point methods for PPB_HostResolver */

		PEPPER_EXPORT PP_Resource PPB_HostResolver_Create(PP_Instance instance) {
			if (has_interface<PPB_HostResolver_1_0>()) {
				return get_interface<PPB_HostResolver_1_0>()->Create(instance);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_HostResolver_IsHostResolver(PP_Resource resource) {
			if (has_interface<PPB_HostResolver_1_0>()) {
				return get_interface<PPB_HostResolver_1_0>()->IsHostResolver(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT int32_t PPB_HostResolver_Resolve(PP_Resource host_resolver, const char* host, uint16_t port, struct PP_HostResolver_Hint hint, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_HostResolver_1_0>()) {
				return get_interface<PPB_HostResolver_1_0>()->Resolve(host_resolver, host, port, &hint, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT struct PP_Var PPB_HostResolver_GetCanonicalName(PP_Resource host_resolver) {
			if (has_interface<PPB_HostResolver_1_0>()) {
				return get_interface<PPB_HostResolver_1_0>()->GetCanonicalName(host_resolver);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT uint32_t PPB_HostResolver_GetNetAddressCount(PP_Resource host_resolver) {
			if (has_interface<PPB_HostResolver_1_0>()) {
				return get_interface<PPB_HostResolver_1_0>()->GetNetAddressCount(host_resolver);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Resource PPB_HostResolver_GetNetAddress(PP_Resource host_resolver, uint32_t index) {
			if (has_interface<PPB_HostResolver_1_0>()) {
				return get_interface<PPB_HostResolver_1_0>()->GetNetAddress(host_resolver, index);
			}
			return NULL;
		}

		#pragma endregion /* End entry point generation for PPB_HostResolver */

		#pragma region /* Begin entry point methods for PPB_ImageData */

		PEPPER_EXPORT PP_ImageDataFormat PPB_ImageData_GetNativeImageDataFormat(void) {
			if (has_interface<PPB_ImageData_1_0>()) {
				return get_interface<PPB_ImageData_1_0>()->GetNativeImageDataFormat();
			}
			return PP_IMAGEDATAFORMAT_BGRA_PREMUL;
		}

		PEPPER_EXPORT PP_Bool PPB_ImageData_IsImageDataFormatSupported(PP_ImageDataFormat format) {
			if (has_interface<PPB_ImageData_1_0>()) {
				return get_interface<PPB_ImageData_1_0>()->IsImageDataFormatSupported(format);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Resource PPB_ImageData_Create(PP_Instance instance, PP_ImageDataFormat format, struct PP_Size size, PP_Bool init_to_zero) {
			if (has_interface<PPB_ImageData_1_0>()) {
				return get_interface<PPB_ImageData_1_0>()->Create(instance, format, &size, init_to_zero);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_ImageData_IsImageData(PP_Resource image_data) {
			if (has_interface<PPB_ImageData_1_0>()) {
				return get_interface<PPB_ImageData_1_0>()->IsImageData(image_data);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_ImageData_Describe(PP_Resource image_data, struct PP_ImageDataDesc* desc) {
			if (has_interface<PPB_ImageData_1_0>()) {
				return get_interface<PPB_ImageData_1_0>()->Describe(image_data, desc);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT void* PPB_ImageData_Map(PP_Resource image_data) {
			if (has_interface<PPB_ImageData_1_0>()) {
				return get_interface<PPB_ImageData_1_0>()->Map(image_data);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_ImageData_Unmap(PP_Resource image_data) {
			if (has_interface<PPB_ImageData_1_0>()) {
				get_interface<PPB_ImageData_1_0>()->Unmap(image_data);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_ImageData */

		#pragma region /* Begin entry point methods for PPB_InputEvent */

		PEPPER_EXPORT int32_t PPB_InputEvent_RequestInputEvents(PP_Instance instance, uint32_t event_classes) {
			if (has_interface<PPB_InputEvent_1_0>()) {
				return get_interface<PPB_InputEvent_1_0>()->RequestInputEvents(instance, event_classes);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_InputEvent_RequestFilteringInputEvents(PP_Instance instance, uint32_t event_classes) {
			if (has_interface<PPB_InputEvent_1_0>()) {
				return get_interface<PPB_InputEvent_1_0>()->RequestFilteringInputEvents(instance, event_classes);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_InputEvent_ClearInputEventRequest(PP_Instance instance, uint32_t event_classes) {
			if (has_interface<PPB_InputEvent_1_0>()) {
				get_interface<PPB_InputEvent_1_0>()->ClearInputEventRequest(instance, event_classes);
			}
			return ;
		}

		PEPPER_EXPORT PP_Bool PPB_InputEvent_IsInputEvent(PP_Resource resource) {
			if (has_interface<PPB_InputEvent_1_0>()) {
				return get_interface<PPB_InputEvent_1_0>()->IsInputEvent(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_InputEvent_Type PPB_InputEvent_GetType(PP_Resource event) {
			if (has_interface<PPB_InputEvent_1_0>()) {
				return get_interface<PPB_InputEvent_1_0>()->GetType(event);
			}
			return PP_INPUTEVENT_TYPE_UNDEFINED;
		}

		PEPPER_EXPORT PP_TimeTicks PPB_InputEvent_GetTimeStamp(PP_Resource event) {
			if (has_interface<PPB_InputEvent_1_0>()) {
				return get_interface<PPB_InputEvent_1_0>()->GetTimeStamp(event);
			}
			return NULL;
		}

		PEPPER_EXPORT uint32_t PPB_InputEvent_GetModifiers(PP_Resource event) {
			if (has_interface<PPB_InputEvent_1_0>()) {
				return get_interface<PPB_InputEvent_1_0>()->GetModifiers(event);
			}
			return NULL;
		}

		#pragma endregion /* End entry point generation for PPB_InputEvent */

		#pragma region /* Begin entry point methods for PPB_MouseInputEvent */

		PEPPER_EXPORT PP_Resource PPB_MouseInputEvent_Create(PP_Instance instance, PP_InputEvent_Type type, PP_TimeTicks time_stamp, uint32_t modifiers, PP_InputEvent_MouseButton mouse_button, struct PP_Point mouse_position, int32_t click_count, struct PP_Point mouse_movement) {
			if (has_interface<PPB_MouseInputEvent_1_1>()) {
				return get_interface<PPB_MouseInputEvent_1_1>()->Create(instance, type, time_stamp, modifiers, mouse_button, &mouse_position, click_count, &mouse_movement);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_MouseInputEvent_IsMouseInputEvent(PP_Resource resource) {
			if (has_interface<PPB_MouseInputEvent_1_1>()) {
				return get_interface<PPB_MouseInputEvent_1_1>()->IsMouseInputEvent(resource);
			}
			else if (has_interface<PPB_MouseInputEvent_1_0>()) {
				return get_interface<PPB_MouseInputEvent_1_0>()->IsMouseInputEvent(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_InputEvent_MouseButton PPB_MouseInputEvent_GetButton(PP_Resource mouse_event) {
			if (has_interface<PPB_MouseInputEvent_1_1>()) {
				return get_interface<PPB_MouseInputEvent_1_1>()->GetButton(mouse_event);
			}
			else if (has_interface<PPB_MouseInputEvent_1_0>()) {
				return get_interface<PPB_MouseInputEvent_1_0>()->GetButton(mouse_event);
			}
			return PP_INPUTEVENT_MOUSEBUTTON_NONE;
		}

		PEPPER_EXPORT struct PP_Point PPB_MouseInputEvent_GetPosition(PP_Resource mouse_event) {
			if (has_interface<PPB_MouseInputEvent_1_1>()) {
				return get_interface<PPB_MouseInputEvent_1_1>()->GetPosition(mouse_event);
			}
			else if (has_interface<PPB_MouseInputEvent_1_0>()) {
				return get_interface<PPB_MouseInputEvent_1_0>()->GetPosition(mouse_event);
			}
			return PP_MakePoint(0,0);
		}

		PEPPER_EXPORT int32_t PPB_MouseInputEvent_GetClickCount(PP_Resource mouse_event) {
			if (has_interface<PPB_MouseInputEvent_1_1>()) {
				return get_interface<PPB_MouseInputEvent_1_1>()->GetClickCount(mouse_event);
			}
			else if (has_interface<PPB_MouseInputEvent_1_0>()) {
				return get_interface<PPB_MouseInputEvent_1_0>()->GetClickCount(mouse_event);
			}
			return NULL;
		}

		PEPPER_EXPORT struct PP_Point PPB_MouseInputEvent_GetMovement(PP_Resource mouse_event) {
			if (has_interface<PPB_MouseInputEvent_1_1>()) {
				return get_interface<PPB_MouseInputEvent_1_1>()->GetMovement(mouse_event);
			}
			return PP_MakePoint(0,0);
		}

		#pragma endregion /* End entry point generation for PPB_MouseInputEvent */

		#pragma region /* Begin entry point methods for PPB_WheelInputEvent */

		PEPPER_EXPORT PP_Resource PPB_WheelInputEvent_Create(PP_Instance instance, PP_TimeTicks time_stamp, uint32_t modifiers, struct PP_FloatPoint wheel_delta, struct PP_FloatPoint wheel_ticks, PP_Bool scroll_by_page) {
			if (has_interface<PPB_WheelInputEvent_1_0>()) {
				return get_interface<PPB_WheelInputEvent_1_0>()->Create(instance, time_stamp, modifiers, &wheel_delta, &wheel_ticks, scroll_by_page);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_WheelInputEvent_IsWheelInputEvent(PP_Resource resource) {
			if (has_interface<PPB_WheelInputEvent_1_0>()) {
				return get_interface<PPB_WheelInputEvent_1_0>()->IsWheelInputEvent(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT struct PP_FloatPoint PPB_WheelInputEvent_GetDelta(PP_Resource wheel_event) {
			if (has_interface<PPB_WheelInputEvent_1_0>()) {
				return get_interface<PPB_WheelInputEvent_1_0>()->GetDelta(wheel_event);
			}
			return PP_MakeFloatPoint(0,0);
		}

		PEPPER_EXPORT struct PP_FloatPoint PPB_WheelInputEvent_GetTicks(PP_Resource wheel_event) {
			if (has_interface<PPB_WheelInputEvent_1_0>()) {
				return get_interface<PPB_WheelInputEvent_1_0>()->GetTicks(wheel_event);
			}
			return PP_MakeFloatPoint(0,0);
		}

		PEPPER_EXPORT PP_Bool PPB_WheelInputEvent_GetScrollByPage(PP_Resource wheel_event) {
			if (has_interface<PPB_WheelInputEvent_1_0>()) {
				return get_interface<PPB_WheelInputEvent_1_0>()->GetScrollByPage(wheel_event);
			}
			return PP_FromBool(FALSE);
		}

		#pragma endregion /* End entry point generation for PPB_WheelInputEvent */

		#pragma region /* Begin entry point methods for PPB_KeyboardInputEvent */

		PEPPER_EXPORT PP_Resource PPB_KeyboardInputEvent_Create(PP_Instance instance, PP_InputEvent_Type type, PP_TimeTicks time_stamp, uint32_t modifiers, uint32_t key_code, struct PP_Var character_text, struct PP_Var code) {
			if (has_interface<PPB_KeyboardInputEvent_1_2>()) {
				return get_interface<PPB_KeyboardInputEvent_1_2>()->Create(instance, type, time_stamp, modifiers, key_code, character_text, code);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_KeyboardInputEvent_IsKeyboardInputEvent(PP_Resource resource) {
			if (has_interface<PPB_KeyboardInputEvent_1_2>()) {
				return get_interface<PPB_KeyboardInputEvent_1_2>()->IsKeyboardInputEvent(resource);
			}
			else if (has_interface<PPB_KeyboardInputEvent_1_0>()) {
				return get_interface<PPB_KeyboardInputEvent_1_0>()->IsKeyboardInputEvent(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT uint32_t PPB_KeyboardInputEvent_GetKeyCode(PP_Resource key_event) {
			if (has_interface<PPB_KeyboardInputEvent_1_2>()) {
				return get_interface<PPB_KeyboardInputEvent_1_2>()->GetKeyCode(key_event);
			}
			else if (has_interface<PPB_KeyboardInputEvent_1_0>()) {
				return get_interface<PPB_KeyboardInputEvent_1_0>()->GetKeyCode(key_event);
			}
			return NULL;
		}

		PEPPER_EXPORT struct PP_Var PPB_KeyboardInputEvent_GetCharacterText(PP_Resource character_event) {
			if (has_interface<PPB_KeyboardInputEvent_1_2>()) {
				return get_interface<PPB_KeyboardInputEvent_1_2>()->GetCharacterText(character_event);
			}
			else if (has_interface<PPB_KeyboardInputEvent_1_0>()) {
				return get_interface<PPB_KeyboardInputEvent_1_0>()->GetCharacterText(character_event);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT struct PP_Var PPB_KeyboardInputEvent_GetCode(PP_Resource key_event) {
			if (has_interface<PPB_KeyboardInputEvent_1_2>()) {
				return get_interface<PPB_KeyboardInputEvent_1_2>()->GetCode(key_event);
			}
			return PP_MakeNull();
		}

		#pragma endregion /* End entry point generation for PPB_KeyboardInputEvent */

		#pragma region /* Begin entry point methods for PPB_TouchInputEvent */

		PEPPER_EXPORT PP_Resource PPB_TouchInputEvent_Create(PP_Instance instance, PP_InputEvent_Type type, PP_TimeTicks time_stamp, uint32_t modifiers) {
			if (has_interface<PPB_TouchInputEvent_1_0>()) {
				return get_interface<PPB_TouchInputEvent_1_0>()->Create(instance, type, time_stamp, modifiers);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_TouchInputEvent_AddTouchPoint(PP_Resource touch_event, PP_TouchListType list, struct PP_TouchPoint point) {
			if (has_interface<PPB_TouchInputEvent_1_0>()) {
				get_interface<PPB_TouchInputEvent_1_0>()->AddTouchPoint(touch_event, list, &point);
			}
			return ;
		}

		PEPPER_EXPORT PP_Bool PPB_TouchInputEvent_IsTouchInputEvent(PP_Resource resource) {
			if (has_interface<PPB_TouchInputEvent_1_0>()) {
				return get_interface<PPB_TouchInputEvent_1_0>()->IsTouchInputEvent(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT uint32_t PPB_TouchInputEvent_GetTouchCount(PP_Resource resource, PP_TouchListType list) {
			if (has_interface<PPB_TouchInputEvent_1_0>()) {
				return get_interface<PPB_TouchInputEvent_1_0>()->GetTouchCount(resource, list);
			}
			return NULL;
		}

		PEPPER_EXPORT struct PP_TouchPoint PPB_TouchInputEvent_GetTouchByIndex(PP_Resource resource, PP_TouchListType list, uint32_t index) {
			if (has_interface<PPB_TouchInputEvent_1_0>()) {
				return get_interface<PPB_TouchInputEvent_1_0>()->GetTouchByIndex(resource, list, index);
			}
			return PP_MakeTouchPoint();
		}

		PEPPER_EXPORT struct PP_TouchPoint PPB_TouchInputEvent_GetTouchById(PP_Resource resource, PP_TouchListType list, uint32_t touch_id) {
			if (has_interface<PPB_TouchInputEvent_1_0>()) {
				return get_interface<PPB_TouchInputEvent_1_0>()->GetTouchById(resource, list, touch_id);
			}
			return PP_MakeTouchPoint();
		}

		#pragma endregion /* End entry point generation for PPB_TouchInputEvent */

		#pragma region /* Begin entry point methods for PPB_IMEInputEvent */

		PEPPER_EXPORT PP_Resource PPB_IMEInputEvent_Create(PP_Instance instance, PP_InputEvent_Type type, PP_TimeTicks time_stamp, struct PP_Var text, uint32_t segment_number, const uint32_t segment_offsets[], int32_t target_segment, uint32_t selection_start, uint32_t selection_end) {
			if (has_interface<PPB_IMEInputEvent_1_0>()) {
				return get_interface<PPB_IMEInputEvent_1_0>()->Create(instance, type, time_stamp, text, segment_number, segment_offsets, target_segment, selection_start, selection_end);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_IMEInputEvent_IsIMEInputEvent(PP_Resource resource) {
			if (has_interface<PPB_IMEInputEvent_1_0>()) {
				return get_interface<PPB_IMEInputEvent_1_0>()->IsIMEInputEvent(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT struct PP_Var PPB_IMEInputEvent_GetText(PP_Resource ime_event) {
			if (has_interface<PPB_IMEInputEvent_1_0>()) {
				return get_interface<PPB_IMEInputEvent_1_0>()->GetText(ime_event);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT uint32_t PPB_IMEInputEvent_GetSegmentNumber(PP_Resource ime_event) {
			if (has_interface<PPB_IMEInputEvent_1_0>()) {
				return get_interface<PPB_IMEInputEvent_1_0>()->GetSegmentNumber(ime_event);
			}
			return NULL;
		}

		PEPPER_EXPORT uint32_t PPB_IMEInputEvent_GetSegmentOffset(PP_Resource ime_event, uint32_t index) {
			if (has_interface<PPB_IMEInputEvent_1_0>()) {
				return get_interface<PPB_IMEInputEvent_1_0>()->GetSegmentOffset(ime_event, index);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_IMEInputEvent_GetTargetSegment(PP_Resource ime_event) {
			if (has_interface<PPB_IMEInputEvent_1_0>()) {
				return get_interface<PPB_IMEInputEvent_1_0>()->GetTargetSegment(ime_event);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_IMEInputEvent_GetSelection(PP_Resource ime_event, uint32_t* start, uint32_t* end) {
			if (has_interface<PPB_IMEInputEvent_1_0>()) {
				get_interface<PPB_IMEInputEvent_1_0>()->GetSelection(ime_event, start, end);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_IMEInputEvent */

		#pragma region /* Begin entry point methods for PPB_Instance */

		PEPPER_EXPORT PP_Bool PPB_Instance_BindGraphics(PP_Instance instance, PP_Resource device) {
			if (has_interface<PPB_Instance_1_0>()) {
				return get_interface<PPB_Instance_1_0>()->BindGraphics(instance, device);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_Instance_IsFullFrame(PP_Instance instance) {
			if (has_interface<PPB_Instance_1_0>()) {
				return get_interface<PPB_Instance_1_0>()->IsFullFrame(instance);
			}
			return PP_FromBool(FALSE);
		}

		#pragma endregion /* End entry point generation for PPB_Instance */

		#pragma region /* Begin entry point methods for PPB_MediaStreamAudioTrack */

		PEPPER_EXPORT PP_Bool PPB_MediaStreamAudioTrack_IsMediaStreamAudioTrack(PP_Resource resource) {
			if (has_interface<PPB_MediaStreamAudioTrack_0_1>()) {
				return get_interface<PPB_MediaStreamAudioTrack_0_1>()->IsMediaStreamAudioTrack(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT int32_t PPB_MediaStreamAudioTrack_Configure(PP_Resource audio_track, const int32_t attrib_list[], struct PP_CompletionCallback callback) {
			if (has_interface<PPB_MediaStreamAudioTrack_0_1>()) {
				return get_interface<PPB_MediaStreamAudioTrack_0_1>()->Configure(audio_track, attrib_list, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_MediaStreamAudioTrack_GetAttrib(PP_Resource audio_track, PP_MediaStreamAudioTrack_Attrib attrib, int32_t* value) {
			if (has_interface<PPB_MediaStreamAudioTrack_0_1>()) {
				return get_interface<PPB_MediaStreamAudioTrack_0_1>()->GetAttrib(audio_track, attrib, value);
			}
			return NULL;
		}

		PEPPER_EXPORT struct PP_Var PPB_MediaStreamAudioTrack_GetId(PP_Resource audio_track) {
			if (has_interface<PPB_MediaStreamAudioTrack_0_1>()) {
				return get_interface<PPB_MediaStreamAudioTrack_0_1>()->GetId(audio_track);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT PP_Bool PPB_MediaStreamAudioTrack_HasEnded(PP_Resource audio_track) {
			if (has_interface<PPB_MediaStreamAudioTrack_0_1>()) {
				return get_interface<PPB_MediaStreamAudioTrack_0_1>()->HasEnded(audio_track);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT int32_t PPB_MediaStreamAudioTrack_GetBuffer(PP_Resource audio_track, PP_Resource* buffer, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_MediaStreamAudioTrack_0_1>()) {
				return get_interface<PPB_MediaStreamAudioTrack_0_1>()->GetBuffer(audio_track, buffer, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_MediaStreamAudioTrack_RecycleBuffer(PP_Resource audio_track, PP_Resource buffer) {
			if (has_interface<PPB_MediaStreamAudioTrack_0_1>()) {
				return get_interface<PPB_MediaStreamAudioTrack_0_1>()->RecycleBuffer(audio_track, buffer);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_MediaStreamAudioTrack_Close(PP_Resource audio_track) {
			if (has_interface<PPB_MediaStreamAudioTrack_0_1>()) {
				get_interface<PPB_MediaStreamAudioTrack_0_1>()->Close(audio_track);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_MediaStreamAudioTrack */

		#pragma region /* Begin entry point methods for PPB_MediaStreamVideoTrack */

		PEPPER_EXPORT PP_Resource PPB_MediaStreamVideoTrack_Create(PP_Instance instance) {
			if (has_interface<PPB_MediaStreamVideoTrack_1_0>()) {
				return get_interface<PPB_MediaStreamVideoTrack_1_0>()->Create(instance);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_MediaStreamVideoTrack_IsMediaStreamVideoTrack(PP_Resource resource) {
			if (has_interface<PPB_MediaStreamVideoTrack_1_0>()) {
				return get_interface<PPB_MediaStreamVideoTrack_1_0>()->IsMediaStreamVideoTrack(resource);
			}
			else if (has_interface<PPB_MediaStreamVideoTrack_0_1>()) {
				return get_interface<PPB_MediaStreamVideoTrack_0_1>()->IsMediaStreamVideoTrack(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT int32_t PPB_MediaStreamVideoTrack_Configure(PP_Resource video_track, const int32_t attrib_list[], struct PP_CompletionCallback callback) {
			if (has_interface<PPB_MediaStreamVideoTrack_1_0>()) {
				return get_interface<PPB_MediaStreamVideoTrack_1_0>()->Configure(video_track, attrib_list, callback);
			}
			else if (has_interface<PPB_MediaStreamVideoTrack_0_1>()) {
				return get_interface<PPB_MediaStreamVideoTrack_0_1>()->Configure(video_track, attrib_list, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_MediaStreamVideoTrack_GetAttrib(PP_Resource video_track, PP_MediaStreamVideoTrack_Attrib attrib, int32_t* value) {
			if (has_interface<PPB_MediaStreamVideoTrack_1_0>()) {
				return get_interface<PPB_MediaStreamVideoTrack_1_0>()->GetAttrib(video_track, attrib, value);
			}
			else if (has_interface<PPB_MediaStreamVideoTrack_0_1>()) {
				return get_interface<PPB_MediaStreamVideoTrack_0_1>()->GetAttrib(video_track, attrib, value);
			}
			return NULL;
		}

		PEPPER_EXPORT struct PP_Var PPB_MediaStreamVideoTrack_GetId(PP_Resource video_track) {
			if (has_interface<PPB_MediaStreamVideoTrack_1_0>()) {
				return get_interface<PPB_MediaStreamVideoTrack_1_0>()->GetId(video_track);
			}
			else if (has_interface<PPB_MediaStreamVideoTrack_0_1>()) {
				return get_interface<PPB_MediaStreamVideoTrack_0_1>()->GetId(video_track);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT PP_Bool PPB_MediaStreamVideoTrack_HasEnded(PP_Resource video_track) {
			if (has_interface<PPB_MediaStreamVideoTrack_1_0>()) {
				return get_interface<PPB_MediaStreamVideoTrack_1_0>()->HasEnded(video_track);
			}
			else if (has_interface<PPB_MediaStreamVideoTrack_0_1>()) {
				return get_interface<PPB_MediaStreamVideoTrack_0_1>()->HasEnded(video_track);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT int32_t PPB_MediaStreamVideoTrack_GetFrame(PP_Resource video_track, PP_Resource* frame, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_MediaStreamVideoTrack_1_0>()) {
				return get_interface<PPB_MediaStreamVideoTrack_1_0>()->GetFrame(video_track, frame, callback);
			}
			else if (has_interface<PPB_MediaStreamVideoTrack_0_1>()) {
				return get_interface<PPB_MediaStreamVideoTrack_0_1>()->GetFrame(video_track, frame, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_MediaStreamVideoTrack_RecycleFrame(PP_Resource video_track, PP_Resource frame) {
			if (has_interface<PPB_MediaStreamVideoTrack_1_0>()) {
				return get_interface<PPB_MediaStreamVideoTrack_1_0>()->RecycleFrame(video_track, frame);
			}
			else if (has_interface<PPB_MediaStreamVideoTrack_0_1>()) {
				return get_interface<PPB_MediaStreamVideoTrack_0_1>()->RecycleFrame(video_track, frame);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_MediaStreamVideoTrack_Close(PP_Resource video_track) {
			if (has_interface<PPB_MediaStreamVideoTrack_1_0>()) {
				get_interface<PPB_MediaStreamVideoTrack_1_0>()->Close(video_track);
			}
			else if (has_interface<PPB_MediaStreamVideoTrack_0_1>()) {
				get_interface<PPB_MediaStreamVideoTrack_0_1>()->Close(video_track);
			}
			return ;
		}

		PEPPER_EXPORT int32_t PPB_MediaStreamVideoTrack_GetEmptyFrame(PP_Resource video_track, PP_Resource* frame, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_MediaStreamVideoTrack_1_0>()) {
				return get_interface<PPB_MediaStreamVideoTrack_1_0>()->GetEmptyFrame(video_track, frame, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_MediaStreamVideoTrack_PutFrame(PP_Resource video_track, PP_Resource frame) {
			if (has_interface<PPB_MediaStreamVideoTrack_1_0>()) {
				return get_interface<PPB_MediaStreamVideoTrack_1_0>()->PutFrame(video_track, frame);
			}
			return NULL;
		}

		#pragma endregion /* End entry point generation for PPB_MediaStreamVideoTrack */

		#pragma region /* Begin entry point methods for PPB_MessageLoop */

		PEPPER_EXPORT PP_Resource PPB_MessageLoop_Create(PP_Instance instance) {
			if (has_interface<PPB_MessageLoop_1_0>()) {
				return get_interface<PPB_MessageLoop_1_0>()->Create(instance);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Resource PPB_MessageLoop_GetForMainThread(void) {
			if (has_interface<PPB_MessageLoop_1_0>()) {
				return get_interface<PPB_MessageLoop_1_0>()->GetForMainThread();
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Resource PPB_MessageLoop_GetCurrent(void) {
			if (has_interface<PPB_MessageLoop_1_0>()) {
				return get_interface<PPB_MessageLoop_1_0>()->GetCurrent();
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_MessageLoop_AttachToCurrentThread(PP_Resource message_loop) {
			if (has_interface<PPB_MessageLoop_1_0>()) {
				return get_interface<PPB_MessageLoop_1_0>()->AttachToCurrentThread(message_loop);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_MessageLoop_Run(PP_Resource message_loop) {
			if (has_interface<PPB_MessageLoop_1_0>()) {
				return get_interface<PPB_MessageLoop_1_0>()->Run(message_loop);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_MessageLoop_PostWork(PP_Resource message_loop, struct PP_CompletionCallback callback, int64_t delay_ms) {
			if (has_interface<PPB_MessageLoop_1_0>()) {
				return get_interface<PPB_MessageLoop_1_0>()->PostWork(message_loop, callback, delay_ms);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_MessageLoop_PostQuit(PP_Resource message_loop, PP_Bool should_destroy) {
			if (has_interface<PPB_MessageLoop_1_0>()) {
				return get_interface<PPB_MessageLoop_1_0>()->PostQuit(message_loop, should_destroy);
			}
			return NULL;
		}

		#pragma endregion /* End entry point generation for PPB_MessageLoop */

		#pragma region /* Begin entry point methods for PPB_Messaging */

		PEPPER_EXPORT void PPB_Messaging_PostMessage(PP_Instance instance, struct PP_Var message) {
			if (has_interface<PPB_Messaging_1_2>()) {
				get_interface<PPB_Messaging_1_2>()->PostMessage(instance, message);
			}
			else if (has_interface<PPB_Messaging_1_0>()) {
				get_interface<PPB_Messaging_1_0>()->PostMessage(instance, message);
			}
			return ;
		}

		PEPPER_EXPORT int32_t PPB_Messaging_RegisterMessageHandler(PP_Instance instance, void* user_data, const struct PPP_MessageHandler_0_2* handler, PP_Resource message_loop) {
			if (has_interface<PPB_Messaging_1_2>()) {
				return get_interface<PPB_Messaging_1_2>()->RegisterMessageHandler(instance, user_data, handler, message_loop);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_Messaging_UnregisterMessageHandler(PP_Instance instance) {
			if (has_interface<PPB_Messaging_1_2>()) {
				get_interface<PPB_Messaging_1_2>()->UnregisterMessageHandler(instance);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_Messaging */

		#pragma region /* Begin entry point methods for PPB_MouseCursor */

		PEPPER_EXPORT PP_Bool PPB_MouseCursor_SetCursor(PP_Instance instance, enum PP_MouseCursor_Type type, PP_Resource image, struct PP_Point hot_spot) {
			if (has_interface<PPB_MouseCursor_1_0>()) {
				return get_interface<PPB_MouseCursor_1_0>()->SetCursor(instance, type, image, &hot_spot);
			}
			return PP_FromBool(FALSE);
		}

		#pragma endregion /* End entry point generation for PPB_MouseCursor */

		#pragma region /* Begin entry point methods for PPB_MouseLock */

		PEPPER_EXPORT int32_t PPB_MouseLock_LockMouse(PP_Instance instance, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_MouseLock_1_0>()) {
				return get_interface<PPB_MouseLock_1_0>()->LockMouse(instance, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_MouseLock_UnlockMouse(PP_Instance instance) {
			if (has_interface<PPB_MouseLock_1_0>()) {
				get_interface<PPB_MouseLock_1_0>()->UnlockMouse(instance);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_MouseLock */

		#pragma region /* Begin entry point methods for PPB_NetAddress */

		PEPPER_EXPORT PP_Resource PPB_NetAddress_CreateFromIPv4Address(PP_Instance instance, struct PP_NetAddress_IPv4 ipv4_addr) {
			if (has_interface<PPB_NetAddress_1_0>()) {
				return get_interface<PPB_NetAddress_1_0>()->CreateFromIPv4Address(instance, &ipv4_addr);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Resource PPB_NetAddress_CreateFromIPv6Address(PP_Instance instance, struct PP_NetAddress_IPv6 ipv6_addr) {
			if (has_interface<PPB_NetAddress_1_0>()) {
				return get_interface<PPB_NetAddress_1_0>()->CreateFromIPv6Address(instance, &ipv6_addr);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_NetAddress_IsNetAddress(PP_Resource resource) {
			if (has_interface<PPB_NetAddress_1_0>()) {
				return get_interface<PPB_NetAddress_1_0>()->IsNetAddress(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_NetAddress_Family PPB_NetAddress_GetFamily(PP_Resource addr) {
			if (has_interface<PPB_NetAddress_1_0>()) {
				return get_interface<PPB_NetAddress_1_0>()->GetFamily(addr);
			}
			return PP_NETADDRESS_FAMILY_UNSPECIFIED;
		}

		PEPPER_EXPORT struct PP_Var PPB_NetAddress_DescribeAsString(PP_Resource addr, PP_Bool include_port) {
			if (has_interface<PPB_NetAddress_1_0>()) {
				return get_interface<PPB_NetAddress_1_0>()->DescribeAsString(addr, include_port);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT PP_Bool PPB_NetAddress_DescribeAsIPv4Address(PP_Resource addr, struct PP_NetAddress_IPv4* ipv4_addr) {
			if (has_interface<PPB_NetAddress_1_0>()) {
				return get_interface<PPB_NetAddress_1_0>()->DescribeAsIPv4Address(addr, ipv4_addr);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_NetAddress_DescribeAsIPv6Address(PP_Resource addr, struct PP_NetAddress_IPv6* ipv6_addr) {
			if (has_interface<PPB_NetAddress_1_0>()) {
				return get_interface<PPB_NetAddress_1_0>()->DescribeAsIPv6Address(addr, ipv6_addr);
			}
			return PP_FromBool(FALSE);
		}

		#pragma endregion /* End entry point generation for PPB_NetAddress */

		#pragma region /* Begin entry point methods for PPB_NetworkList */

		PEPPER_EXPORT PP_Bool PPB_NetworkList_IsNetworkList(PP_Resource resource) {
			if (has_interface<PPB_NetworkList_1_0>()) {
				return get_interface<PPB_NetworkList_1_0>()->IsNetworkList(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT uint32_t PPB_NetworkList_GetCount(PP_Resource resource) {
			if (has_interface<PPB_NetworkList_1_0>()) {
				return get_interface<PPB_NetworkList_1_0>()->GetCount(resource);
			}
			return NULL;
		}

		PEPPER_EXPORT struct PP_Var PPB_NetworkList_GetName(PP_Resource resource, uint32_t index) {
			if (has_interface<PPB_NetworkList_1_0>()) {
				return get_interface<PPB_NetworkList_1_0>()->GetName(resource, index);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT PP_NetworkList_Type PPB_NetworkList_GetType(PP_Resource resource, uint32_t index) {
			if (has_interface<PPB_NetworkList_1_0>()) {
				return get_interface<PPB_NetworkList_1_0>()->GetType(resource, index);
			}
			return PP_NETWORKLIST_TYPE_ETHERNET;
		}

		PEPPER_EXPORT PP_NetworkList_State PPB_NetworkList_GetState(PP_Resource resource, uint32_t index) {
			if (has_interface<PPB_NetworkList_1_0>()) {
				return get_interface<PPB_NetworkList_1_0>()->GetState(resource, index);
			}
			return PP_NETWORKLIST_STATE_DOWN;
		}

		PEPPER_EXPORT int32_t PPB_NetworkList_GetIpAddresses(PP_Resource resource, uint32_t index, struct PP_ArrayOutput output) {
			if (has_interface<PPB_NetworkList_1_0>()) {
				return get_interface<PPB_NetworkList_1_0>()->GetIpAddresses(resource, index, output);
			}
			return NULL;
		}

		PEPPER_EXPORT struct PP_Var PPB_NetworkList_GetDisplayName(PP_Resource resource, uint32_t index) {
			if (has_interface<PPB_NetworkList_1_0>()) {
				return get_interface<PPB_NetworkList_1_0>()->GetDisplayName(resource, index);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT uint32_t PPB_NetworkList_GetMTU(PP_Resource resource, uint32_t index) {
			if (has_interface<PPB_NetworkList_1_0>()) {
				return get_interface<PPB_NetworkList_1_0>()->GetMTU(resource, index);
			}
			return NULL;
		}

		#pragma endregion /* End entry point generation for PPB_NetworkList */

		#pragma region /* Begin entry point methods for PPB_NetworkMonitor */

		PEPPER_EXPORT PP_Resource PPB_NetworkMonitor_Create(PP_Instance instance) {
			if (has_interface<PPB_NetworkMonitor_1_0>()) {
				return get_interface<PPB_NetworkMonitor_1_0>()->Create(instance);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_NetworkMonitor_UpdateNetworkList(PP_Resource network_monitor, PP_Resource* network_list, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_NetworkMonitor_1_0>()) {
				return get_interface<PPB_NetworkMonitor_1_0>()->UpdateNetworkList(network_monitor, network_list, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_NetworkMonitor_IsNetworkMonitor(PP_Resource resource) {
			if (has_interface<PPB_NetworkMonitor_1_0>()) {
				return get_interface<PPB_NetworkMonitor_1_0>()->IsNetworkMonitor(resource);
			}
			return PP_FromBool(FALSE);
		}

		#pragma endregion /* End entry point generation for PPB_NetworkMonitor */

		#pragma region /* Begin entry point methods for PPB_NetworkProxy */

		PEPPER_EXPORT int32_t PPB_NetworkProxy_GetProxyForURL(PP_Instance instance, struct PP_Var url, struct PP_Var* proxy_string, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_NetworkProxy_1_0>()) {
				return get_interface<PPB_NetworkProxy_1_0>()->GetProxyForURL(instance, url, proxy_string, callback);
			}
			return NULL;
		}

		#pragma endregion /* End entry point generation for PPB_NetworkProxy */

		#pragma region /* Begin entry point methods for PPB_TCPSocket */

		PEPPER_EXPORT PP_Resource PPB_TCPSocket_Create(PP_Instance instance) {
			if (has_interface<PPB_TCPSocket_1_2>()) {
				return get_interface<PPB_TCPSocket_1_2>()->Create(instance);
			}
			else if (has_interface<PPB_TCPSocket_1_1>()) {
				return get_interface<PPB_TCPSocket_1_1>()->Create(instance);
			}
			else if (has_interface<PPB_TCPSocket_1_0>()) {
				return get_interface<PPB_TCPSocket_1_0>()->Create(instance);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_TCPSocket_IsTCPSocket(PP_Resource resource) {
			if (has_interface<PPB_TCPSocket_1_2>()) {
				return get_interface<PPB_TCPSocket_1_2>()->IsTCPSocket(resource);
			}
			else if (has_interface<PPB_TCPSocket_1_1>()) {
				return get_interface<PPB_TCPSocket_1_1>()->IsTCPSocket(resource);
			}
			else if (has_interface<PPB_TCPSocket_1_0>()) {
				return get_interface<PPB_TCPSocket_1_0>()->IsTCPSocket(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT int32_t PPB_TCPSocket_Bind(PP_Resource tcp_socket, PP_Resource addr, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_TCPSocket_1_2>()) {
				return get_interface<PPB_TCPSocket_1_2>()->Bind(tcp_socket, addr, callback);
			}
			else if (has_interface<PPB_TCPSocket_1_1>()) {
				return get_interface<PPB_TCPSocket_1_1>()->Bind(tcp_socket, addr, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_TCPSocket_Connect(PP_Resource tcp_socket, PP_Resource addr, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_TCPSocket_1_2>()) {
				return get_interface<PPB_TCPSocket_1_2>()->Connect(tcp_socket, addr, callback);
			}
			else if (has_interface<PPB_TCPSocket_1_1>()) {
				return get_interface<PPB_TCPSocket_1_1>()->Connect(tcp_socket, addr, callback);
			}
			else if (has_interface<PPB_TCPSocket_1_0>()) {
				return get_interface<PPB_TCPSocket_1_0>()->Connect(tcp_socket, addr, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Resource PPB_TCPSocket_GetLocalAddress(PP_Resource tcp_socket) {
			if (has_interface<PPB_TCPSocket_1_2>()) {
				return get_interface<PPB_TCPSocket_1_2>()->GetLocalAddress(tcp_socket);
			}
			else if (has_interface<PPB_TCPSocket_1_1>()) {
				return get_interface<PPB_TCPSocket_1_1>()->GetLocalAddress(tcp_socket);
			}
			else if (has_interface<PPB_TCPSocket_1_0>()) {
				return get_interface<PPB_TCPSocket_1_0>()->GetLocalAddress(tcp_socket);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Resource PPB_TCPSocket_GetRemoteAddress(PP_Resource tcp_socket) {
			if (has_interface<PPB_TCPSocket_1_2>()) {
				return get_interface<PPB_TCPSocket_1_2>()->GetRemoteAddress(tcp_socket);
			}
			else if (has_interface<PPB_TCPSocket_1_1>()) {
				return get_interface<PPB_TCPSocket_1_1>()->GetRemoteAddress(tcp_socket);
			}
			else if (has_interface<PPB_TCPSocket_1_0>()) {
				return get_interface<PPB_TCPSocket_1_0>()->GetRemoteAddress(tcp_socket);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_TCPSocket_Read(PP_Resource tcp_socket, char* buffer, int32_t bytes_to_read, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_TCPSocket_1_2>()) {
				return get_interface<PPB_TCPSocket_1_2>()->Read(tcp_socket, buffer, bytes_to_read, callback);
			}
			else if (has_interface<PPB_TCPSocket_1_1>()) {
				return get_interface<PPB_TCPSocket_1_1>()->Read(tcp_socket, buffer, bytes_to_read, callback);
			}
			else if (has_interface<PPB_TCPSocket_1_0>()) {
				return get_interface<PPB_TCPSocket_1_0>()->Read(tcp_socket, buffer, bytes_to_read, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_TCPSocket_Write(PP_Resource tcp_socket, const char* buffer, int32_t bytes_to_write, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_TCPSocket_1_2>()) {
				return get_interface<PPB_TCPSocket_1_2>()->Write(tcp_socket, buffer, bytes_to_write, callback);
			}
			else if (has_interface<PPB_TCPSocket_1_1>()) {
				return get_interface<PPB_TCPSocket_1_1>()->Write(tcp_socket, buffer, bytes_to_write, callback);
			}
			else if (has_interface<PPB_TCPSocket_1_0>()) {
				return get_interface<PPB_TCPSocket_1_0>()->Write(tcp_socket, buffer, bytes_to_write, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_TCPSocket_Listen(PP_Resource tcp_socket, int32_t backlog, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_TCPSocket_1_2>()) {
				return get_interface<PPB_TCPSocket_1_2>()->Listen(tcp_socket, backlog, callback);
			}
			else if (has_interface<PPB_TCPSocket_1_1>()) {
				return get_interface<PPB_TCPSocket_1_1>()->Listen(tcp_socket, backlog, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_TCPSocket_Accept(PP_Resource tcp_socket, PP_Resource* accepted_tcp_socket, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_TCPSocket_1_2>()) {
				return get_interface<PPB_TCPSocket_1_2>()->Accept(tcp_socket, accepted_tcp_socket, callback);
			}
			else if (has_interface<PPB_TCPSocket_1_1>()) {
				return get_interface<PPB_TCPSocket_1_1>()->Accept(tcp_socket, accepted_tcp_socket, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_TCPSocket_Close(PP_Resource tcp_socket) {
			if (has_interface<PPB_TCPSocket_1_2>()) {
				get_interface<PPB_TCPSocket_1_2>()->Close(tcp_socket);
			}
			else if (has_interface<PPB_TCPSocket_1_1>()) {
				get_interface<PPB_TCPSocket_1_1>()->Close(tcp_socket);
			}
			else if (has_interface<PPB_TCPSocket_1_0>()) {
				get_interface<PPB_TCPSocket_1_0>()->Close(tcp_socket);
			}
			return ;
		}

		PEPPER_EXPORT int32_t PPB_TCPSocket_SetOption(PP_Resource tcp_socket, PP_TCPSocket_Option name, struct PP_Var value, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_TCPSocket_1_2>()) {
				return get_interface<PPB_TCPSocket_1_2>()->SetOption(tcp_socket, name, value, callback);
			}
			return NULL;
		}

		#pragma endregion /* End entry point generation for PPB_TCPSocket */

		#pragma region /* Begin entry point methods for PPB_UDPSocket */

		PEPPER_EXPORT PP_Resource PPB_UDPSocket_Create(PP_Instance instance) {
			if (has_interface<PPB_UDPSocket_1_2>()) {
				return get_interface<PPB_UDPSocket_1_2>()->Create(instance);
			}
			else if (has_interface<PPB_UDPSocket_1_1>()) {
				return get_interface<PPB_UDPSocket_1_1>()->Create(instance);
			}
			else if (has_interface<PPB_UDPSocket_1_0>()) {
				return get_interface<PPB_UDPSocket_1_0>()->Create(instance);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_UDPSocket_IsUDPSocket(PP_Resource resource) {
			if (has_interface<PPB_UDPSocket_1_2>()) {
				return get_interface<PPB_UDPSocket_1_2>()->IsUDPSocket(resource);
			}
			else if (has_interface<PPB_UDPSocket_1_1>()) {
				return get_interface<PPB_UDPSocket_1_1>()->IsUDPSocket(resource);
			}
			else if (has_interface<PPB_UDPSocket_1_0>()) {
				return get_interface<PPB_UDPSocket_1_0>()->IsUDPSocket(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT int32_t PPB_UDPSocket_Bind(PP_Resource udp_socket, PP_Resource addr, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_UDPSocket_1_2>()) {
				return get_interface<PPB_UDPSocket_1_2>()->Bind(udp_socket, addr, callback);
			}
			else if (has_interface<PPB_UDPSocket_1_1>()) {
				return get_interface<PPB_UDPSocket_1_1>()->Bind(udp_socket, addr, callback);
			}
			else if (has_interface<PPB_UDPSocket_1_0>()) {
				return get_interface<PPB_UDPSocket_1_0>()->Bind(udp_socket, addr, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Resource PPB_UDPSocket_GetBoundAddress(PP_Resource udp_socket) {
			if (has_interface<PPB_UDPSocket_1_2>()) {
				return get_interface<PPB_UDPSocket_1_2>()->GetBoundAddress(udp_socket);
			}
			else if (has_interface<PPB_UDPSocket_1_1>()) {
				return get_interface<PPB_UDPSocket_1_1>()->GetBoundAddress(udp_socket);
			}
			else if (has_interface<PPB_UDPSocket_1_0>()) {
				return get_interface<PPB_UDPSocket_1_0>()->GetBoundAddress(udp_socket);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_UDPSocket_RecvFrom(PP_Resource udp_socket, char* buffer, int32_t num_bytes, PP_Resource* addr, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_UDPSocket_1_2>()) {
				return get_interface<PPB_UDPSocket_1_2>()->RecvFrom(udp_socket, buffer, num_bytes, addr, callback);
			}
			else if (has_interface<PPB_UDPSocket_1_1>()) {
				return get_interface<PPB_UDPSocket_1_1>()->RecvFrom(udp_socket, buffer, num_bytes, addr, callback);
			}
			else if (has_interface<PPB_UDPSocket_1_0>()) {
				return get_interface<PPB_UDPSocket_1_0>()->RecvFrom(udp_socket, buffer, num_bytes, addr, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_UDPSocket_SendTo(PP_Resource udp_socket, const char* buffer, int32_t num_bytes, PP_Resource addr, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_UDPSocket_1_2>()) {
				return get_interface<PPB_UDPSocket_1_2>()->SendTo(udp_socket, buffer, num_bytes, addr, callback);
			}
			else if (has_interface<PPB_UDPSocket_1_1>()) {
				return get_interface<PPB_UDPSocket_1_1>()->SendTo(udp_socket, buffer, num_bytes, addr, callback);
			}
			else if (has_interface<PPB_UDPSocket_1_0>()) {
				return get_interface<PPB_UDPSocket_1_0>()->SendTo(udp_socket, buffer, num_bytes, addr, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_UDPSocket_Close(PP_Resource udp_socket) {
			if (has_interface<PPB_UDPSocket_1_2>()) {
				get_interface<PPB_UDPSocket_1_2>()->Close(udp_socket);
			}
			else if (has_interface<PPB_UDPSocket_1_1>()) {
				get_interface<PPB_UDPSocket_1_1>()->Close(udp_socket);
			}
			else if (has_interface<PPB_UDPSocket_1_0>()) {
				get_interface<PPB_UDPSocket_1_0>()->Close(udp_socket);
			}
			return ;
		}

		PEPPER_EXPORT int32_t PPB_UDPSocket_SetOption(PP_Resource udp_socket, PP_UDPSocket_Option name, struct PP_Var value, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_UDPSocket_1_2>()) {
				return get_interface<PPB_UDPSocket_1_2>()->SetOption(udp_socket, name, value, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_UDPSocket_JoinGroup(PP_Resource udp_socket, PP_Resource group, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_UDPSocket_1_2>()) {
				return get_interface<PPB_UDPSocket_1_2>()->JoinGroup(udp_socket, group, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_UDPSocket_LeaveGroup(PP_Resource udp_socket, PP_Resource group, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_UDPSocket_1_2>()) {
				return get_interface<PPB_UDPSocket_1_2>()->LeaveGroup(udp_socket, group, callback);
			}
			return NULL;
		}

		#pragma endregion /* End entry point generation for PPB_UDPSocket */

		#pragma region /* Begin entry point methods for PPB_URLLoader */

		PEPPER_EXPORT PP_Resource PPB_URLLoader_Create(PP_Instance instance) {
			if (has_interface<PPB_URLLoader_1_0>()) {
				return get_interface<PPB_URLLoader_1_0>()->Create(instance);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_URLLoader_IsURLLoader(PP_Resource resource) {
			if (has_interface<PPB_URLLoader_1_0>()) {
				return get_interface<PPB_URLLoader_1_0>()->IsURLLoader(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT int32_t PPB_URLLoader_Open(PP_Resource loader, PP_Resource request_info, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_URLLoader_1_0>()) {
				return get_interface<PPB_URLLoader_1_0>()->Open(loader, request_info, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_URLLoader_FollowRedirect(PP_Resource loader, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_URLLoader_1_0>()) {
				return get_interface<PPB_URLLoader_1_0>()->FollowRedirect(loader, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_URLLoader_GetUploadProgress(PP_Resource loader, int64_t* bytes_sent, int64_t* total_bytes_to_be_sent) {
			if (has_interface<PPB_URLLoader_1_0>()) {
				return get_interface<PPB_URLLoader_1_0>()->GetUploadProgress(loader, bytes_sent, total_bytes_to_be_sent);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_URLLoader_GetDownloadProgress(PP_Resource loader, int64_t* bytes_received, int64_t* total_bytes_to_be_received) {
			if (has_interface<PPB_URLLoader_1_0>()) {
				return get_interface<PPB_URLLoader_1_0>()->GetDownloadProgress(loader, bytes_received, total_bytes_to_be_received);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Resource PPB_URLLoader_GetResponseInfo(PP_Resource loader) {
			if (has_interface<PPB_URLLoader_1_0>()) {
				return get_interface<PPB_URLLoader_1_0>()->GetResponseInfo(loader);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_URLLoader_ReadResponseBody(PP_Resource loader, void* buffer, int32_t bytes_to_read, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_URLLoader_1_0>()) {
				return get_interface<PPB_URLLoader_1_0>()->ReadResponseBody(loader, buffer, bytes_to_read, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_URLLoader_FinishStreamingToFile(PP_Resource loader, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_URLLoader_1_0>()) {
				return get_interface<PPB_URLLoader_1_0>()->FinishStreamingToFile(loader, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_URLLoader_Close(PP_Resource loader) {
			if (has_interface<PPB_URLLoader_1_0>()) {
				get_interface<PPB_URLLoader_1_0>()->Close(loader);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_URLLoader */

		#pragma region /* Begin entry point methods for PPB_URLRequestInfo */

		PEPPER_EXPORT PP_Resource PPB_URLRequestInfo_Create(PP_Instance instance) {
			if (has_interface<PPB_URLRequestInfo_1_0>()) {
				return get_interface<PPB_URLRequestInfo_1_0>()->Create(instance);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_URLRequestInfo_IsURLRequestInfo(PP_Resource resource) {
			if (has_interface<PPB_URLRequestInfo_1_0>()) {
				return get_interface<PPB_URLRequestInfo_1_0>()->IsURLRequestInfo(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_URLRequestInfo_SetProperty(PP_Resource request, PP_URLRequestProperty property, struct PP_Var value) {
			if (has_interface<PPB_URLRequestInfo_1_0>()) {
				return get_interface<PPB_URLRequestInfo_1_0>()->SetProperty(request, property, value);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_URLRequestInfo_AppendDataToBody(PP_Resource request, const void* data, uint32_t len) {
			if (has_interface<PPB_URLRequestInfo_1_0>()) {
				return get_interface<PPB_URLRequestInfo_1_0>()->AppendDataToBody(request, data, len);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_URLRequestInfo_AppendFileToBody(PP_Resource request, PP_Resource file_ref, int64_t start_offset, int64_t number_of_bytes, PP_Time expected_last_modified_time) {
			if (has_interface<PPB_URLRequestInfo_1_0>()) {
				return get_interface<PPB_URLRequestInfo_1_0>()->AppendFileToBody(request, file_ref, start_offset, number_of_bytes, expected_last_modified_time);
			}
			return PP_FromBool(FALSE);
		}

		#pragma endregion /* End entry point generation for PPB_URLRequestInfo */

		#pragma region /* Begin entry point methods for PPB_URLResponseInfo */

		PEPPER_EXPORT PP_Bool PPB_URLResponseInfo_IsURLResponseInfo(PP_Resource resource) {
			if (has_interface<PPB_URLResponseInfo_1_0>()) {
				return get_interface<PPB_URLResponseInfo_1_0>()->IsURLResponseInfo(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT struct PP_Var PPB_URLResponseInfo_GetProperty(PP_Resource response, PP_URLResponseProperty property) {
			if (has_interface<PPB_URLResponseInfo_1_0>()) {
				return get_interface<PPB_URLResponseInfo_1_0>()->GetProperty(response, property);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT PP_Resource PPB_URLResponseInfo_GetBodyAsFileRef(PP_Resource response) {
			if (has_interface<PPB_URLResponseInfo_1_0>()) {
				return get_interface<PPB_URLResponseInfo_1_0>()->GetBodyAsFileRef(response);
			}
			return NULL;
		}

		#pragma endregion /* End entry point generation for PPB_URLResponseInfo */

		#pragma region /* Begin entry point methods for PPB_Var */

		PEPPER_EXPORT void PPB_Var_AddRef(struct PP_Var var) {
			if (has_interface<PPB_Var_1_2>()) {
				get_interface<PPB_Var_1_2>()->AddRef(var);
			}
			else if (has_interface<PPB_Var_1_1>()) {
				get_interface<PPB_Var_1_1>()->AddRef(var);
			}
			else if (has_interface<PPB_Var_1_0>()) {
				get_interface<PPB_Var_1_0>()->AddRef(var);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_Var_Release(struct PP_Var var) {
			if (has_interface<PPB_Var_1_2>()) {
				get_interface<PPB_Var_1_2>()->Release(var);
			}
			else if (has_interface<PPB_Var_1_1>()) {
				get_interface<PPB_Var_1_1>()->Release(var);
			}
			else if (has_interface<PPB_Var_1_0>()) {
				get_interface<PPB_Var_1_0>()->Release(var);
			}
			return ;
		}

		PEPPER_EXPORT struct PP_Var PPB_Var_VarFromUtf8(const char* data, uint32_t len) {
			if (has_interface<PPB_Var_1_2>()) {
				return get_interface<PPB_Var_1_2>()->VarFromUtf8(data, len);
			}
			else if (has_interface<PPB_Var_1_1>()) {
				return get_interface<PPB_Var_1_1>()->VarFromUtf8(data, len);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT const char* PPB_Var_VarToUtf8(struct PP_Var var, uint32_t* len) {
			if (has_interface<PPB_Var_1_2>()) {
				return get_interface<PPB_Var_1_2>()->VarToUtf8(var, len);
			}
			else if (has_interface<PPB_Var_1_1>()) {
				return get_interface<PPB_Var_1_1>()->VarToUtf8(var, len);
			}
			else if (has_interface<PPB_Var_1_0>()) {
				return get_interface<PPB_Var_1_0>()->VarToUtf8(var, len);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Resource PPB_Var_VarToResource(struct PP_Var var) {
			if (has_interface<PPB_Var_1_2>()) {
				return get_interface<PPB_Var_1_2>()->VarToResource(var);
			}
			return NULL;
		}

		PEPPER_EXPORT struct PP_Var PPB_Var_VarFromResource(PP_Resource resource) {
			if (has_interface<PPB_Var_1_2>()) {
				return get_interface<PPB_Var_1_2>()->VarFromResource(resource);
			}
			return PP_MakeNull();
		}

		#pragma endregion /* End entry point generation for PPB_Var */

		#pragma region /* Begin entry point methods for PPB_VarArray */

		PEPPER_EXPORT struct PP_Var PPB_VarArray_Create(void) {
			if (has_interface<PPB_VarArray_1_0>()) {
				return get_interface<PPB_VarArray_1_0>()->Create();
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT struct PP_Var PPB_VarArray_Get(struct PP_Var array, uint32_t index) {
			if (has_interface<PPB_VarArray_1_0>()) {
				return get_interface<PPB_VarArray_1_0>()->Get(array, index);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT PP_Bool PPB_VarArray_Set(struct PP_Var array, uint32_t index, struct PP_Var value) {
			if (has_interface<PPB_VarArray_1_0>()) {
				return get_interface<PPB_VarArray_1_0>()->Set(array, index, value);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT uint32_t PPB_VarArray_GetLength(struct PP_Var array) {
			if (has_interface<PPB_VarArray_1_0>()) {
				return get_interface<PPB_VarArray_1_0>()->GetLength(array);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_VarArray_SetLength(struct PP_Var array, uint32_t length) {
			if (has_interface<PPB_VarArray_1_0>()) {
				return get_interface<PPB_VarArray_1_0>()->SetLength(array, length);
			}
			return PP_FromBool(FALSE);
		}

		#pragma endregion /* End entry point generation for PPB_VarArray */

		#pragma region /* Begin entry point methods for PPB_VarArrayBuffer */

		PEPPER_EXPORT struct PP_Var PPB_VarArrayBuffer_Create(uint32_t size_in_bytes) {
			if (has_interface<PPB_VarArrayBuffer_1_0>()) {
				return get_interface<PPB_VarArrayBuffer_1_0>()->Create(size_in_bytes);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT PP_Bool PPB_VarArrayBuffer_ByteLength(struct PP_Var array, uint32_t* byte_length) {
			if (has_interface<PPB_VarArrayBuffer_1_0>()) {
				return get_interface<PPB_VarArrayBuffer_1_0>()->ByteLength(array, byte_length);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT void* PPB_VarArrayBuffer_Map(struct PP_Var array) {
			if (has_interface<PPB_VarArrayBuffer_1_0>()) {
				return get_interface<PPB_VarArrayBuffer_1_0>()->Map(array);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_VarArrayBuffer_Unmap(struct PP_Var array) {
			if (has_interface<PPB_VarArrayBuffer_1_0>()) {
				get_interface<PPB_VarArrayBuffer_1_0>()->Unmap(array);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_VarArrayBuffer */

		#pragma region /* Begin entry point methods for PPB_VarDictionary */

		PEPPER_EXPORT struct PP_Var PPB_VarDictionary_Create(void) {
			if (has_interface<PPB_VarDictionary_1_0>()) {
				return get_interface<PPB_VarDictionary_1_0>()->Create();
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT struct PP_Var PPB_VarDictionary_Get(struct PP_Var dict, struct PP_Var key) {
			if (has_interface<PPB_VarDictionary_1_0>()) {
				return get_interface<PPB_VarDictionary_1_0>()->Get(dict, key);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT PP_Bool PPB_VarDictionary_Set(struct PP_Var dict, struct PP_Var key, struct PP_Var value) {
			if (has_interface<PPB_VarDictionary_1_0>()) {
				return get_interface<PPB_VarDictionary_1_0>()->Set(dict, key, value);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT void PPB_VarDictionary_Delete(struct PP_Var dict, struct PP_Var key) {
			if (has_interface<PPB_VarDictionary_1_0>()) {
				get_interface<PPB_VarDictionary_1_0>()->Delete(dict, key);
			}
			return ;
		}

		PEPPER_EXPORT PP_Bool PPB_VarDictionary_HasKey(struct PP_Var dict, struct PP_Var key) {
			if (has_interface<PPB_VarDictionary_1_0>()) {
				return get_interface<PPB_VarDictionary_1_0>()->HasKey(dict, key);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT struct PP_Var PPB_VarDictionary_GetKeys(struct PP_Var dict) {
			if (has_interface<PPB_VarDictionary_1_0>()) {
				return get_interface<PPB_VarDictionary_1_0>()->GetKeys(dict);
			}
			return PP_MakeNull();
		}

		#pragma endregion /* End entry point generation for PPB_VarDictionary */

		#pragma region /* Begin entry point methods for PPB_VideoDecoder */

		PEPPER_EXPORT PP_Resource PPB_VideoDecoder_Create(PP_Instance instance) {
			if (has_interface<PPB_VideoDecoder_1_1>()) {
				return get_interface<PPB_VideoDecoder_1_1>()->Create(instance);
			}
			else if (has_interface<PPB_VideoDecoder_1_0>()) {
				return get_interface<PPB_VideoDecoder_1_0>()->Create(instance);
			}
			else if (has_interface<PPB_VideoDecoder_0_2>()) {
				return get_interface<PPB_VideoDecoder_0_2>()->Create(instance);
			}
			else if (has_interface<PPB_VideoDecoder_0_1>()) {
				return get_interface<PPB_VideoDecoder_0_1>()->Create(instance);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_VideoDecoder_IsVideoDecoder(PP_Resource resource) {
			if (has_interface<PPB_VideoDecoder_1_1>()) {
				return get_interface<PPB_VideoDecoder_1_1>()->IsVideoDecoder(resource);
			}
			else if (has_interface<PPB_VideoDecoder_1_0>()) {
				return get_interface<PPB_VideoDecoder_1_0>()->IsVideoDecoder(resource);
			}
			else if (has_interface<PPB_VideoDecoder_0_2>()) {
				return get_interface<PPB_VideoDecoder_0_2>()->IsVideoDecoder(resource);
			}
			else if (has_interface<PPB_VideoDecoder_0_1>()) {
				return get_interface<PPB_VideoDecoder_0_1>()->IsVideoDecoder(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT int32_t PPB_VideoDecoder_Initialize(PP_Resource video_decoder, PP_Resource graphics3d_context, PP_VideoProfile profile, PP_HardwareAcceleration acceleration, uint32_t min_picture_count, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_VideoDecoder_1_1>()) {
				return get_interface<PPB_VideoDecoder_1_1>()->Initialize(video_decoder, graphics3d_context, profile, acceleration, min_picture_count, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_VideoDecoder_Decode(PP_Resource video_decoder, uint32_t decode_id, uint32_t size, const void* buffer, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_VideoDecoder_1_1>()) {
				return get_interface<PPB_VideoDecoder_1_1>()->Decode(video_decoder, decode_id, size, buffer, callback);
			}
			else if (has_interface<PPB_VideoDecoder_1_0>()) {
				return get_interface<PPB_VideoDecoder_1_0>()->Decode(video_decoder, decode_id, size, buffer, callback);
			}
			else if (has_interface<PPB_VideoDecoder_0_2>()) {
				return get_interface<PPB_VideoDecoder_0_2>()->Decode(video_decoder, decode_id, size, buffer, callback);
			}
			else if (has_interface<PPB_VideoDecoder_0_1>()) {
				return get_interface<PPB_VideoDecoder_0_1>()->Decode(video_decoder, decode_id, size, buffer, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_VideoDecoder_GetPicture(PP_Resource video_decoder, struct PP_VideoPicture* picture, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_VideoDecoder_1_1>()) {
				return get_interface<PPB_VideoDecoder_1_1>()->GetPicture(video_decoder, picture, callback);
			}
			else if (has_interface<PPB_VideoDecoder_1_0>()) {
				return get_interface<PPB_VideoDecoder_1_0>()->GetPicture(video_decoder, picture, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_VideoDecoder_RecyclePicture(PP_Resource video_decoder, struct PP_VideoPicture picture) {
			if (has_interface<PPB_VideoDecoder_1_1>()) {
				get_interface<PPB_VideoDecoder_1_1>()->RecyclePicture(video_decoder, &picture);
			}
			else if (has_interface<PPB_VideoDecoder_1_0>()) {
				get_interface<PPB_VideoDecoder_1_0>()->RecyclePicture(video_decoder, &picture);
			}
			else if (has_interface<PPB_VideoDecoder_0_2>()) {
				get_interface<PPB_VideoDecoder_0_2>()->RecyclePicture(video_decoder, &picture);
			}
			else if (has_interface<PPB_VideoDecoder_0_1>()) {
				get_interface<PPB_VideoDecoder_0_1>()->RecyclePicture(video_decoder, &picture);
			}
			return ;
		}

		PEPPER_EXPORT int32_t PPB_VideoDecoder_Flush(PP_Resource video_decoder, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_VideoDecoder_1_1>()) {
				return get_interface<PPB_VideoDecoder_1_1>()->Flush(video_decoder, callback);
			}
			else if (has_interface<PPB_VideoDecoder_1_0>()) {
				return get_interface<PPB_VideoDecoder_1_0>()->Flush(video_decoder, callback);
			}
			else if (has_interface<PPB_VideoDecoder_0_2>()) {
				return get_interface<PPB_VideoDecoder_0_2>()->Flush(video_decoder, callback);
			}
			else if (has_interface<PPB_VideoDecoder_0_1>()) {
				return get_interface<PPB_VideoDecoder_0_1>()->Flush(video_decoder, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_VideoDecoder_Reset(PP_Resource video_decoder, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_VideoDecoder_1_1>()) {
				return get_interface<PPB_VideoDecoder_1_1>()->Reset(video_decoder, callback);
			}
			else if (has_interface<PPB_VideoDecoder_1_0>()) {
				return get_interface<PPB_VideoDecoder_1_0>()->Reset(video_decoder, callback);
			}
			else if (has_interface<PPB_VideoDecoder_0_2>()) {
				return get_interface<PPB_VideoDecoder_0_2>()->Reset(video_decoder, callback);
			}
			else if (has_interface<PPB_VideoDecoder_0_1>()) {
				return get_interface<PPB_VideoDecoder_0_1>()->Reset(video_decoder, callback);
			}
			return NULL;
		}

		#pragma endregion /* End entry point generation for PPB_VideoDecoder */

		#pragma region /* Begin entry point methods for PPB_VideoEncoder */

		PEPPER_EXPORT PP_Resource PPB_VideoEncoder_Create(PP_Instance instance) {
			if (has_interface<PPB_VideoEncoder_0_2>()) {
				return get_interface<PPB_VideoEncoder_0_2>()->Create(instance);
			}
			else if (has_interface<PPB_VideoEncoder_0_1>()) {
				return get_interface<PPB_VideoEncoder_0_1>()->Create(instance);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_VideoEncoder_IsVideoEncoder(PP_Resource resource) {
			if (has_interface<PPB_VideoEncoder_0_2>()) {
				return get_interface<PPB_VideoEncoder_0_2>()->IsVideoEncoder(resource);
			}
			else if (has_interface<PPB_VideoEncoder_0_1>()) {
				return get_interface<PPB_VideoEncoder_0_1>()->IsVideoEncoder(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT int32_t PPB_VideoEncoder_GetSupportedProfiles(PP_Resource video_encoder, struct PP_ArrayOutput output, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_VideoEncoder_0_2>()) {
				return get_interface<PPB_VideoEncoder_0_2>()->GetSupportedProfiles(video_encoder, output, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_VideoEncoder_Initialize(PP_Resource video_encoder, PP_VideoFrame_Format input_format, struct PP_Size input_visible_size, PP_VideoProfile output_profile, uint32_t initial_bitrate, PP_HardwareAcceleration acceleration, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_VideoEncoder_0_2>()) {
				return get_interface<PPB_VideoEncoder_0_2>()->Initialize(video_encoder, input_format, &input_visible_size, output_profile, initial_bitrate, acceleration, callback);
			}
			else if (has_interface<PPB_VideoEncoder_0_1>()) {
				return get_interface<PPB_VideoEncoder_0_1>()->Initialize(video_encoder, input_format, &input_visible_size, output_profile, initial_bitrate, acceleration, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_VideoEncoder_GetFramesRequired(PP_Resource video_encoder) {
			if (has_interface<PPB_VideoEncoder_0_2>()) {
				return get_interface<PPB_VideoEncoder_0_2>()->GetFramesRequired(video_encoder);
			}
			else if (has_interface<PPB_VideoEncoder_0_1>()) {
				return get_interface<PPB_VideoEncoder_0_1>()->GetFramesRequired(video_encoder);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_VideoEncoder_GetFrameCodedSize(PP_Resource video_encoder, struct PP_Size* coded_size) {
			if (has_interface<PPB_VideoEncoder_0_2>()) {
				return get_interface<PPB_VideoEncoder_0_2>()->GetFrameCodedSize(video_encoder, coded_size);
			}
			else if (has_interface<PPB_VideoEncoder_0_1>()) {
				return get_interface<PPB_VideoEncoder_0_1>()->GetFrameCodedSize(video_encoder, coded_size);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_VideoEncoder_GetVideoFrame(PP_Resource video_encoder, PP_Resource* video_frame, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_VideoEncoder_0_2>()) {
				return get_interface<PPB_VideoEncoder_0_2>()->GetVideoFrame(video_encoder, video_frame, callback);
			}
			else if (has_interface<PPB_VideoEncoder_0_1>()) {
				return get_interface<PPB_VideoEncoder_0_1>()->GetVideoFrame(video_encoder, video_frame, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_VideoEncoder_Encode(PP_Resource video_encoder, PP_Resource video_frame, PP_Bool force_keyframe, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_VideoEncoder_0_2>()) {
				return get_interface<PPB_VideoEncoder_0_2>()->Encode(video_encoder, video_frame, force_keyframe, callback);
			}
			else if (has_interface<PPB_VideoEncoder_0_1>()) {
				return get_interface<PPB_VideoEncoder_0_1>()->Encode(video_encoder, video_frame, force_keyframe, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_VideoEncoder_GetBitstreamBuffer(PP_Resource video_encoder, struct PP_BitstreamBuffer* bitstream_buffer, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_VideoEncoder_0_2>()) {
				return get_interface<PPB_VideoEncoder_0_2>()->GetBitstreamBuffer(video_encoder, bitstream_buffer, callback);
			}
			else if (has_interface<PPB_VideoEncoder_0_1>()) {
				return get_interface<PPB_VideoEncoder_0_1>()->GetBitstreamBuffer(video_encoder, bitstream_buffer, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_VideoEncoder_RecycleBitstreamBuffer(PP_Resource video_encoder, struct PP_BitstreamBuffer bitstream_buffer) {
			if (has_interface<PPB_VideoEncoder_0_2>()) {
				get_interface<PPB_VideoEncoder_0_2>()->RecycleBitstreamBuffer(video_encoder, &bitstream_buffer);
			}
			else if (has_interface<PPB_VideoEncoder_0_1>()) {
				get_interface<PPB_VideoEncoder_0_1>()->RecycleBitstreamBuffer(video_encoder, &bitstream_buffer);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_VideoEncoder_RequestEncodingParametersChange(PP_Resource video_encoder, uint32_t bitrate, uint32_t framerate) {
			if (has_interface<PPB_VideoEncoder_0_2>()) {
				get_interface<PPB_VideoEncoder_0_2>()->RequestEncodingParametersChange(video_encoder, bitrate, framerate);
			}
			else if (has_interface<PPB_VideoEncoder_0_1>()) {
				get_interface<PPB_VideoEncoder_0_1>()->RequestEncodingParametersChange(video_encoder, bitrate, framerate);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_VideoEncoder_Close(PP_Resource video_encoder) {
			if (has_interface<PPB_VideoEncoder_0_2>()) {
				get_interface<PPB_VideoEncoder_0_2>()->Close(video_encoder);
			}
			else if (has_interface<PPB_VideoEncoder_0_1>()) {
				get_interface<PPB_VideoEncoder_0_1>()->Close(video_encoder);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_VideoEncoder */

		#pragma region /* Begin entry point methods for PPB_VideoFrame */

		PEPPER_EXPORT PP_Bool PPB_VideoFrame_IsVideoFrame(PP_Resource resource) {
			if (has_interface<PPB_VideoFrame_0_1>()) {
				return get_interface<PPB_VideoFrame_0_1>()->IsVideoFrame(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_TimeDelta PPB_VideoFrame_GetTimestamp(PP_Resource frame) {
			if (has_interface<PPB_VideoFrame_0_1>()) {
				return get_interface<PPB_VideoFrame_0_1>()->GetTimestamp(frame);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_VideoFrame_SetTimestamp(PP_Resource frame, PP_TimeDelta timestamp) {
			if (has_interface<PPB_VideoFrame_0_1>()) {
				get_interface<PPB_VideoFrame_0_1>()->SetTimestamp(frame, timestamp);
			}
			return ;
		}

		PEPPER_EXPORT PP_VideoFrame_Format PPB_VideoFrame_GetFormat(PP_Resource frame) {
			if (has_interface<PPB_VideoFrame_0_1>()) {
				return get_interface<PPB_VideoFrame_0_1>()->GetFormat(frame);
			}
			return PP_VIDEOFRAME_FORMAT_UNKNOWN;
		}

		PEPPER_EXPORT PP_Bool PPB_VideoFrame_GetSize(PP_Resource frame, struct PP_Size* size) {
			if (has_interface<PPB_VideoFrame_0_1>()) {
				return get_interface<PPB_VideoFrame_0_1>()->GetSize(frame, size);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT void* PPB_VideoFrame_GetDataBuffer(PP_Resource frame) {
			if (has_interface<PPB_VideoFrame_0_1>()) {
				return get_interface<PPB_VideoFrame_0_1>()->GetDataBuffer(frame);
			}
			return NULL;
		}

		PEPPER_EXPORT uint32_t PPB_VideoFrame_GetDataBufferSize(PP_Resource frame) {
			if (has_interface<PPB_VideoFrame_0_1>()) {
				return get_interface<PPB_VideoFrame_0_1>()->GetDataBufferSize(frame);
			}
			return NULL;
		}

		#pragma endregion /* End entry point generation for PPB_VideoFrame */

		#pragma region /* Begin entry point methods for PPB_View */

		PEPPER_EXPORT PP_Bool PPB_View_IsView(PP_Resource resource) {
			if (has_interface<PPB_View_1_2>()) {
				return get_interface<PPB_View_1_2>()->IsView(resource);
			}
			else if (has_interface<PPB_View_1_1>()) {
				return get_interface<PPB_View_1_1>()->IsView(resource);
			}
			else if (has_interface<PPB_View_1_0>()) {
				return get_interface<PPB_View_1_0>()->IsView(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_View_GetRect(PP_Resource resource, struct PP_Rect* rect) {
			if (has_interface<PPB_View_1_2>()) {
				return get_interface<PPB_View_1_2>()->GetRect(resource, rect);
			}
			else if (has_interface<PPB_View_1_1>()) {
				return get_interface<PPB_View_1_1>()->GetRect(resource, rect);
			}
			else if (has_interface<PPB_View_1_0>()) {
				return get_interface<PPB_View_1_0>()->GetRect(resource, rect);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_View_IsFullscreen(PP_Resource resource) {
			if (has_interface<PPB_View_1_2>()) {
				return get_interface<PPB_View_1_2>()->IsFullscreen(resource);
			}
			else if (has_interface<PPB_View_1_1>()) {
				return get_interface<PPB_View_1_1>()->IsFullscreen(resource);
			}
			else if (has_interface<PPB_View_1_0>()) {
				return get_interface<PPB_View_1_0>()->IsFullscreen(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_View_IsVisible(PP_Resource resource) {
			if (has_interface<PPB_View_1_2>()) {
				return get_interface<PPB_View_1_2>()->IsVisible(resource);
			}
			else if (has_interface<PPB_View_1_1>()) {
				return get_interface<PPB_View_1_1>()->IsVisible(resource);
			}
			else if (has_interface<PPB_View_1_0>()) {
				return get_interface<PPB_View_1_0>()->IsVisible(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_View_IsPageVisible(PP_Resource resource) {
			if (has_interface<PPB_View_1_2>()) {
				return get_interface<PPB_View_1_2>()->IsPageVisible(resource);
			}
			else if (has_interface<PPB_View_1_1>()) {
				return get_interface<PPB_View_1_1>()->IsPageVisible(resource);
			}
			else if (has_interface<PPB_View_1_0>()) {
				return get_interface<PPB_View_1_0>()->IsPageVisible(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT PP_Bool PPB_View_GetClipRect(PP_Resource resource, struct PP_Rect* clip) {
			if (has_interface<PPB_View_1_2>()) {
				return get_interface<PPB_View_1_2>()->GetClipRect(resource, clip);
			}
			else if (has_interface<PPB_View_1_1>()) {
				return get_interface<PPB_View_1_1>()->GetClipRect(resource, clip);
			}
			else if (has_interface<PPB_View_1_0>()) {
				return get_interface<PPB_View_1_0>()->GetClipRect(resource, clip);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT float PPB_View_GetDeviceScale(PP_Resource resource) {
			if (has_interface<PPB_View_1_2>()) {
				return get_interface<PPB_View_1_2>()->GetDeviceScale(resource);
			}
			else if (has_interface<PPB_View_1_1>()) {
				return get_interface<PPB_View_1_1>()->GetDeviceScale(resource);
			}
			return NULL;
		}

		PEPPER_EXPORT float PPB_View_GetCSSScale(PP_Resource resource) {
			if (has_interface<PPB_View_1_2>()) {
				return get_interface<PPB_View_1_2>()->GetCSSScale(resource);
			}
			else if (has_interface<PPB_View_1_1>()) {
				return get_interface<PPB_View_1_1>()->GetCSSScale(resource);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_View_GetScrollOffset(PP_Resource resource, struct PP_Point* offset) {
			if (has_interface<PPB_View_1_2>()) {
				return get_interface<PPB_View_1_2>()->GetScrollOffset(resource, offset);
			}
			return PP_FromBool(FALSE);
		}

		#pragma endregion /* End entry point generation for PPB_View */

		#pragma region /* Begin entry point methods for PPB_WebSocket */

		PEPPER_EXPORT PP_Resource PPB_WebSocket_Create(PP_Instance instance) {
			if (has_interface<PPB_WebSocket_1_0>()) {
				return get_interface<PPB_WebSocket_1_0>()->Create(instance);
			}
			return NULL;
		}

		PEPPER_EXPORT PP_Bool PPB_WebSocket_IsWebSocket(PP_Resource resource) {
			if (has_interface<PPB_WebSocket_1_0>()) {
				return get_interface<PPB_WebSocket_1_0>()->IsWebSocket(resource);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT int32_t PPB_WebSocket_Connect(PP_Resource web_socket, struct PP_Var url, const struct PP_Var protocols[], uint32_t protocol_count, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_WebSocket_1_0>()) {
				return get_interface<PPB_WebSocket_1_0>()->Connect(web_socket, url, protocols, protocol_count, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_WebSocket_Close(PP_Resource web_socket, uint16_t code, struct PP_Var reason, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_WebSocket_1_0>()) {
				return get_interface<PPB_WebSocket_1_0>()->Close(web_socket, code, reason, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_WebSocket_ReceiveMessage(PP_Resource web_socket, struct PP_Var* message, struct PP_CompletionCallback callback) {
			if (has_interface<PPB_WebSocket_1_0>()) {
				return get_interface<PPB_WebSocket_1_0>()->ReceiveMessage(web_socket, message, callback);
			}
			return NULL;
		}

		PEPPER_EXPORT int32_t PPB_WebSocket_SendMessage(PP_Resource web_socket, struct PP_Var message) {
			if (has_interface<PPB_WebSocket_1_0>()) {
				return get_interface<PPB_WebSocket_1_0>()->SendMessage(web_socket, message);
			}
			return NULL;
		}

		PEPPER_EXPORT uint64_t PPB_WebSocket_GetBufferedAmount(PP_Resource web_socket) {
			if (has_interface<PPB_WebSocket_1_0>()) {
				return get_interface<PPB_WebSocket_1_0>()->GetBufferedAmount(web_socket);
			}
			return NULL;
		}

		PEPPER_EXPORT uint16_t PPB_WebSocket_GetCloseCode(PP_Resource web_socket) {
			if (has_interface<PPB_WebSocket_1_0>()) {
				return get_interface<PPB_WebSocket_1_0>()->GetCloseCode(web_socket);
			}
			return NULL;
		}

		PEPPER_EXPORT struct PP_Var PPB_WebSocket_GetCloseReason(PP_Resource web_socket) {
			if (has_interface<PPB_WebSocket_1_0>()) {
				return get_interface<PPB_WebSocket_1_0>()->GetCloseReason(web_socket);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT PP_Bool PPB_WebSocket_GetCloseWasClean(PP_Resource web_socket) {
			if (has_interface<PPB_WebSocket_1_0>()) {
				return get_interface<PPB_WebSocket_1_0>()->GetCloseWasClean(web_socket);
			}
			return PP_FromBool(FALSE);
		}

		PEPPER_EXPORT struct PP_Var PPB_WebSocket_GetExtensions(PP_Resource web_socket) {
			if (has_interface<PPB_WebSocket_1_0>()) {
				return get_interface<PPB_WebSocket_1_0>()->GetExtensions(web_socket);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT struct PP_Var PPB_WebSocket_GetProtocol(PP_Resource web_socket) {
			if (has_interface<PPB_WebSocket_1_0>()) {
				return get_interface<PPB_WebSocket_1_0>()->GetProtocol(web_socket);
			}
			return PP_MakeNull();
		}

		PEPPER_EXPORT PP_WebSocketReadyState PPB_WebSocket_GetReadyState(PP_Resource web_socket) {
			if (has_interface<PPB_WebSocket_1_0>()) {
				return get_interface<PPB_WebSocket_1_0>()->GetReadyState(web_socket);
			}
			return PP_WEBSOCKETREADYSTATE_INVALID;
		}

		PEPPER_EXPORT struct PP_Var PPB_WebSocket_GetURL(PP_Resource web_socket) {
			if (has_interface<PPB_WebSocket_1_0>()) {
				return get_interface<PPB_WebSocket_1_0>()->GetURL(web_socket);
			}
			return PP_MakeNull();
		}

		#pragma endregion /* End entry point generation for PPB_WebSocket */

		#pragma region /* Begin entry point methods for PPP_InputEvent */

		PEPPER_EXPORT PP_Bool PPP_InputEvent_HandleInputEvent(PP_Instance instance, PP_Resource input_event) {
			if (has_interface<PPP_InputEvent_0_1>()) {
				return get_interface<PPP_InputEvent_0_1>()->HandleInputEvent(instance, input_event);
			}
			return PP_FromBool(FALSE);
		}

		#pragma endregion /* End entry point generation for PPP_InputEvent */

/* Not generating entry point methods for PPP_MessageHandler_0_2 */

		#pragma region /* Begin entry point methods for PPP_Messaging */

		PEPPER_EXPORT void PPP_Messaging_HandleMessage(PP_Instance instance, struct PP_Var message) {
			if (has_interface<PPP_Messaging_1_0>()) {
				get_interface<PPP_Messaging_1_0>()->HandleMessage(instance, message);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPP_Messaging */

		#pragma region /* Begin entry point methods for PPP_MouseLock */

		PEPPER_EXPORT void PPP_MouseLock_MouseLockLost(PP_Instance instance) {
			if (has_interface<PPP_MouseLock_1_0>()) {
				get_interface<PPP_MouseLock_1_0>()->MouseLockLost(instance);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPP_MouseLock */

	}
#ifdef __cplusplus
	}  /* extern "C" */
#endif
}
