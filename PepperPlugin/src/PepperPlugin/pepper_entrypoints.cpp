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
#include "ppapi/c/ppb_fullscreen.h"
#include "ppapi/c/ppb_graphics_2d.h"
#include "ppapi/c/ppb_image_data.h"
#include "ppapi/c/ppb_input_event.h"
#include "ppapi/c/ppb_instance.h"
#include "ppapi/c/ppb_media_stream_audio_track.h"
#include "ppapi/c/ppb_media_stream_video_track.h"
#include "ppapi/c/ppb_message_loop.h"
#include "ppapi/c/ppb_messaging.h"
#include "ppapi/c/ppb_mouse_cursor.h"
#include "ppapi/c/ppb_mouse_lock.h"
#include "ppapi/c/ppb_opengles2.h"
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
		template <> const char*	interface_name<PPB_Fullscreen_1_0>() {
			return PPB_FULLSCREEN_INTERFACE_1_0;
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
		template <> const char*	interface_name<PPB_OpenGLES2_1_0>() {
			return PPB_OPENGLES2_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_OpenGLES2InstancedArrays_1_0>() {
			return PPB_OPENGLES2_INSTANCEDARRAYS_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_OpenGLES2FramebufferBlit_1_0>() {
			return PPB_OPENGLES2_FRAMEBUFFERBLIT_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_OpenGLES2FramebufferMultisample_1_0>() {
			return PPB_OPENGLES2_FRAMEBUFFERMULTISAMPLE_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_OpenGLES2ChromiumEnableFeature_1_0>() {
			return PPB_OPENGLES2_CHROMIUMENABLEFEATURE_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_OpenGLES2ChromiumMapSub_1_0>() {
			return PPB_OPENGLES2_CHROMIUMMAPSUB_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_OpenGLES2Query_1_0>() {
			return PPB_OPENGLES2_QUERY_INTERFACE_1_0;
		}
		template <> const char*	interface_name<PPB_OpenGLES2VertexArrayObject_1_0>() {
			return PPB_OPENGLES2_VERTEXARRAYOBJECT_INTERFACE_1_0;
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

		#pragma region /* Begin entry point methods for PPB_OpenGLES2 */

		PEPPER_EXPORT void PPB_OpenGLES2_ActiveTexture(PP_Resource context, GLenum texture) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->ActiveTexture(context, texture);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_AttachShader(PP_Resource context, GLuint program, GLuint shader) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->AttachShader(context, program, shader);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_BindAttribLocation(PP_Resource context, GLuint program, GLuint index, const char* name) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->BindAttribLocation(context, program, index, name);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_BindBuffer(PP_Resource context, GLenum target, GLuint buffer) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->BindBuffer(context, target, buffer);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_BindFramebuffer(PP_Resource context, GLenum target, GLuint framebuffer) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->BindFramebuffer(context, target, framebuffer);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_BindRenderbuffer(PP_Resource context, GLenum target, GLuint renderbuffer) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->BindRenderbuffer(context, target, renderbuffer);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_BindTexture(PP_Resource context, GLenum target, GLuint texture) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->BindTexture(context, target, texture);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_BlendColor(PP_Resource context, GLclampf red, GLclampf green, GLclampf blue, GLclampf alpha) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->BlendColor(context, red, green, blue, alpha);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_BlendEquation(PP_Resource context, GLenum mode) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->BlendEquation(context, mode);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_BlendEquationSeparate(PP_Resource context, GLenum modeRGB, GLenum modeAlpha) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->BlendEquationSeparate(context, modeRGB, modeAlpha);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_BlendFunc(PP_Resource context, GLenum sfactor, GLenum dfactor) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->BlendFunc(context, sfactor, dfactor);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_BlendFuncSeparate(PP_Resource context, GLenum srcRGB, GLenum dstRGB, GLenum srcAlpha, GLenum dstAlpha) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->BlendFuncSeparate(context, srcRGB, dstRGB, srcAlpha, dstAlpha);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_BufferData(PP_Resource context, GLenum target, GLsizeiptr size, const void* data, GLenum usage) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->BufferData(context, target, size, data, usage);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_BufferSubData(PP_Resource context, GLenum target, GLintptr offset, GLsizeiptr size, const void* data) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->BufferSubData(context, target, offset, size, data);
			}
			return ;
		}

		PEPPER_EXPORT GLenum PPB_OpenGLES2_CheckFramebufferStatus(PP_Resource context, GLenum target) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				return get_interface<PPB_OpenGLES2_1_0>()->CheckFramebufferStatus(context, target);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Clear(PP_Resource context, GLbitfield mask) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Clear(context, mask);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_ClearColor(PP_Resource context, GLclampf red, GLclampf green, GLclampf blue, GLclampf alpha) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->ClearColor(context, red, green, blue, alpha);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_ClearDepthf(PP_Resource context, GLclampf depth) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->ClearDepthf(context, depth);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_ClearStencil(PP_Resource context, GLint s) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->ClearStencil(context, s);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_ColorMask(PP_Resource context, GLboolean red, GLboolean green, GLboolean blue, GLboolean alpha) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->ColorMask(context, red, green, blue, alpha);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_CompileShader(PP_Resource context, GLuint shader) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->CompileShader(context, shader);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_CompressedTexImage2D(PP_Resource context, GLenum target, GLint level, GLenum internalformat, GLsizei width, GLsizei height, GLint border, GLsizei imageSize, const void* data) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->CompressedTexImage2D(context, target, level, internalformat, width, height, border, imageSize, data);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_CompressedTexSubImage2D(PP_Resource context, GLenum target, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLsizei imageSize, const void* data) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->CompressedTexSubImage2D(context, target, level, xoffset, yoffset, width, height, format, imageSize, data);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_CopyTexImage2D(PP_Resource context, GLenum target, GLint level, GLenum internalformat, GLint x, GLint y, GLsizei width, GLsizei height, GLint border) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->CopyTexImage2D(context, target, level, internalformat, x, y, width, height, border);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_CopyTexSubImage2D(PP_Resource context, GLenum target, GLint level, GLint xoffset, GLint yoffset, GLint x, GLint y, GLsizei width, GLsizei height) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->CopyTexSubImage2D(context, target, level, xoffset, yoffset, x, y, width, height);
			}
			return ;
		}

		PEPPER_EXPORT GLuint PPB_OpenGLES2_CreateProgram(PP_Resource context) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				return get_interface<PPB_OpenGLES2_1_0>()->CreateProgram(context);
			}
			return NULL;
		}

		PEPPER_EXPORT GLuint PPB_OpenGLES2_CreateShader(PP_Resource context, GLenum type) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				return get_interface<PPB_OpenGLES2_1_0>()->CreateShader(context, type);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_CullFace(PP_Resource context, GLenum mode) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->CullFace(context, mode);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_DeleteBuffers(PP_Resource context, GLsizei n, const GLuint* buffers) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->DeleteBuffers(context, n, buffers);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_DeleteFramebuffers(PP_Resource context, GLsizei n, const GLuint* framebuffers) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->DeleteFramebuffers(context, n, framebuffers);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_DeleteProgram(PP_Resource context, GLuint program) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->DeleteProgram(context, program);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_DeleteRenderbuffers(PP_Resource context, GLsizei n, const GLuint* renderbuffers) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->DeleteRenderbuffers(context, n, renderbuffers);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_DeleteShader(PP_Resource context, GLuint shader) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->DeleteShader(context, shader);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_DeleteTextures(PP_Resource context, GLsizei n, const GLuint* textures) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->DeleteTextures(context, n, textures);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_DepthFunc(PP_Resource context, GLenum func) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->DepthFunc(context, func);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_DepthMask(PP_Resource context, GLboolean flag) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->DepthMask(context, flag);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_DepthRangef(PP_Resource context, GLclampf zNear, GLclampf zFar) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->DepthRangef(context, zNear, zFar);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_DetachShader(PP_Resource context, GLuint program, GLuint shader) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->DetachShader(context, program, shader);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Disable(PP_Resource context, GLenum cap) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Disable(context, cap);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_DisableVertexAttribArray(PP_Resource context, GLuint index) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->DisableVertexAttribArray(context, index);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_DrawArrays(PP_Resource context, GLenum mode, GLint first, GLsizei count) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->DrawArrays(context, mode, first, count);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_DrawElements(PP_Resource context, GLenum mode, GLsizei count, GLenum type, const void* indices) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->DrawElements(context, mode, count, type, indices);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Enable(PP_Resource context, GLenum cap) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Enable(context, cap);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_EnableVertexAttribArray(PP_Resource context, GLuint index) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->EnableVertexAttribArray(context, index);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Finish(PP_Resource context) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Finish(context);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Flush(PP_Resource context) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Flush(context);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_FramebufferRenderbuffer(PP_Resource context, GLenum target, GLenum attachment, GLenum renderbuffertarget, GLuint renderbuffer) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->FramebufferRenderbuffer(context, target, attachment, renderbuffertarget, renderbuffer);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_FramebufferTexture2D(PP_Resource context, GLenum target, GLenum attachment, GLenum textarget, GLuint texture, GLint level) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->FramebufferTexture2D(context, target, attachment, textarget, texture, level);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_FrontFace(PP_Resource context, GLenum mode) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->FrontFace(context, mode);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GenBuffers(PP_Resource context, GLsizei n, GLuint* buffers) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GenBuffers(context, n, buffers);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GenerateMipmap(PP_Resource context, GLenum target) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GenerateMipmap(context, target);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GenFramebuffers(PP_Resource context, GLsizei n, GLuint* framebuffers) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GenFramebuffers(context, n, framebuffers);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GenRenderbuffers(PP_Resource context, GLsizei n, GLuint* renderbuffers) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GenRenderbuffers(context, n, renderbuffers);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GenTextures(PP_Resource context, GLsizei n, GLuint* textures) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GenTextures(context, n, textures);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetActiveAttrib(PP_Resource context, GLuint program, GLuint index, GLsizei bufsize, GLsizei* length, GLint* size, GLenum* type, char* name) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetActiveAttrib(context, program, index, bufsize, length, size, type, name);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetActiveUniform(PP_Resource context, GLuint program, GLuint index, GLsizei bufsize, GLsizei* length, GLint* size, GLenum* type, char* name) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetActiveUniform(context, program, index, bufsize, length, size, type, name);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetAttachedShaders(PP_Resource context, GLuint program, GLsizei maxcount, GLsizei* count, GLuint* shaders) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetAttachedShaders(context, program, maxcount, count, shaders);
			}
			return ;
		}

		PEPPER_EXPORT GLint PPB_OpenGLES2_GetAttribLocation(PP_Resource context, GLuint program, const char* name) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				return get_interface<PPB_OpenGLES2_1_0>()->GetAttribLocation(context, program, name);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetBooleanv(PP_Resource context, GLenum pname, GLboolean* params) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetBooleanv(context, pname, params);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetBufferParameteriv(PP_Resource context, GLenum target, GLenum pname, GLint* params) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetBufferParameteriv(context, target, pname, params);
			}
			return ;
		}

		PEPPER_EXPORT GLenum PPB_OpenGLES2_GetError(PP_Resource context) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				return get_interface<PPB_OpenGLES2_1_0>()->GetError(context);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetFloatv(PP_Resource context, GLenum pname, GLfloat* params) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetFloatv(context, pname, params);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetFramebufferAttachmentParameteriv(PP_Resource context, GLenum target, GLenum attachment, GLenum pname, GLint* params) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetFramebufferAttachmentParameteriv(context, target, attachment, pname, params);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetIntegerv(PP_Resource context, GLenum pname, GLint* params) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetIntegerv(context, pname, params);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetProgramiv(PP_Resource context, GLuint program, GLenum pname, GLint* params) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetProgramiv(context, program, pname, params);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetProgramInfoLog(PP_Resource context, GLuint program, GLsizei bufsize, GLsizei* length, char* infolog) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetProgramInfoLog(context, program, bufsize, length, infolog);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetRenderbufferParameteriv(PP_Resource context, GLenum target, GLenum pname, GLint* params) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetRenderbufferParameteriv(context, target, pname, params);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetShaderiv(PP_Resource context, GLuint shader, GLenum pname, GLint* params) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetShaderiv(context, shader, pname, params);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetShaderInfoLog(PP_Resource context, GLuint shader, GLsizei bufsize, GLsizei* length, char* infolog) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetShaderInfoLog(context, shader, bufsize, length, infolog);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetShaderPrecisionFormat(PP_Resource context, GLenum shadertype, GLenum precisiontype, GLint* range, GLint* precision) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetShaderPrecisionFormat(context, shadertype, precisiontype, range, precision);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetShaderSource(PP_Resource context, GLuint shader, GLsizei bufsize, GLsizei* length, char* source) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetShaderSource(context, shader, bufsize, length, source);
			}
			return ;
		}

		PEPPER_EXPORT const GLubyte* PPB_OpenGLES2_GetString(PP_Resource context, GLenum name) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				return get_interface<PPB_OpenGLES2_1_0>()->GetString(context, name);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetTexParameterfv(PP_Resource context, GLenum target, GLenum pname, GLfloat* params) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetTexParameterfv(context, target, pname, params);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetTexParameteriv(PP_Resource context, GLenum target, GLenum pname, GLint* params) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetTexParameteriv(context, target, pname, params);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetUniformfv(PP_Resource context, GLuint program, GLint location, GLfloat* params) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetUniformfv(context, program, location, params);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetUniformiv(PP_Resource context, GLuint program, GLint location, GLint* params) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetUniformiv(context, program, location, params);
			}
			return ;
		}

		PEPPER_EXPORT GLint PPB_OpenGLES2_GetUniformLocation(PP_Resource context, GLuint program, const char* name) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				return get_interface<PPB_OpenGLES2_1_0>()->GetUniformLocation(context, program, name);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetVertexAttribfv(PP_Resource context, GLuint index, GLenum pname, GLfloat* params) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetVertexAttribfv(context, index, pname, params);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetVertexAttribiv(PP_Resource context, GLuint index, GLenum pname, GLint* params) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetVertexAttribiv(context, index, pname, params);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_GetVertexAttribPointerv(PP_Resource context, GLuint index, GLenum pname, void** pointer) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->GetVertexAttribPointerv(context, index, pname, pointer);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Hint(PP_Resource context, GLenum target, GLenum mode) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Hint(context, target, mode);
			}
			return ;
		}

		PEPPER_EXPORT GLboolean PPB_OpenGLES2_IsBuffer(PP_Resource context, GLuint buffer) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				return get_interface<PPB_OpenGLES2_1_0>()->IsBuffer(context, buffer);
			}
			return NULL;
		}

		PEPPER_EXPORT GLboolean PPB_OpenGLES2_IsEnabled(PP_Resource context, GLenum cap) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				return get_interface<PPB_OpenGLES2_1_0>()->IsEnabled(context, cap);
			}
			return NULL;
		}

		PEPPER_EXPORT GLboolean PPB_OpenGLES2_IsFramebuffer(PP_Resource context, GLuint framebuffer) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				return get_interface<PPB_OpenGLES2_1_0>()->IsFramebuffer(context, framebuffer);
			}
			return NULL;
		}

		PEPPER_EXPORT GLboolean PPB_OpenGLES2_IsProgram(PP_Resource context, GLuint program) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				return get_interface<PPB_OpenGLES2_1_0>()->IsProgram(context, program);
			}
			return NULL;
		}

		PEPPER_EXPORT GLboolean PPB_OpenGLES2_IsRenderbuffer(PP_Resource context, GLuint renderbuffer) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				return get_interface<PPB_OpenGLES2_1_0>()->IsRenderbuffer(context, renderbuffer);
			}
			return NULL;
		}

		PEPPER_EXPORT GLboolean PPB_OpenGLES2_IsShader(PP_Resource context, GLuint shader) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				return get_interface<PPB_OpenGLES2_1_0>()->IsShader(context, shader);
			}
			return NULL;
		}

		PEPPER_EXPORT GLboolean PPB_OpenGLES2_IsTexture(PP_Resource context, GLuint texture) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				return get_interface<PPB_OpenGLES2_1_0>()->IsTexture(context, texture);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_LineWidth(PP_Resource context, GLfloat width) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->LineWidth(context, width);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_LinkProgram(PP_Resource context, GLuint program) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->LinkProgram(context, program);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_PixelStorei(PP_Resource context, GLenum pname, GLint param) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->PixelStorei(context, pname, param);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_PolygonOffset(PP_Resource context, GLfloat factor, GLfloat units) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->PolygonOffset(context, factor, units);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_ReadPixels(PP_Resource context, GLint x, GLint y, GLsizei width, GLsizei height, GLenum format, GLenum type, void* pixels) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->ReadPixels(context, x, y, width, height, format, type, pixels);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_ReleaseShaderCompiler(PP_Resource context) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->ReleaseShaderCompiler(context);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_RenderbufferStorage(PP_Resource context, GLenum target, GLenum internalformat, GLsizei width, GLsizei height) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->RenderbufferStorage(context, target, internalformat, width, height);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_SampleCoverage(PP_Resource context, GLclampf value, GLboolean invert) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->SampleCoverage(context, value, invert);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Scissor(PP_Resource context, GLint x, GLint y, GLsizei width, GLsizei height) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Scissor(context, x, y, width, height);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_ShaderBinary(PP_Resource context, GLsizei n, const GLuint* shaders, GLenum binaryformat, const void* binary, GLsizei length) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->ShaderBinary(context, n, shaders, binaryformat, binary, length);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_ShaderSource(PP_Resource context, GLuint shader, GLsizei count, const char** str, const GLint* length) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->ShaderSource(context, shader, count, str, length);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_StencilFunc(PP_Resource context, GLenum func, GLint ref, GLuint mask) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->StencilFunc(context, func, ref, mask);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_StencilFuncSeparate(PP_Resource context, GLenum face, GLenum func, GLint ref, GLuint mask) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->StencilFuncSeparate(context, face, func, ref, mask);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_StencilMask(PP_Resource context, GLuint mask) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->StencilMask(context, mask);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_StencilMaskSeparate(PP_Resource context, GLenum face, GLuint mask) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->StencilMaskSeparate(context, face, mask);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_StencilOp(PP_Resource context, GLenum fail, GLenum zfail, GLenum zpass) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->StencilOp(context, fail, zfail, zpass);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_StencilOpSeparate(PP_Resource context, GLenum face, GLenum fail, GLenum zfail, GLenum zpass) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->StencilOpSeparate(context, face, fail, zfail, zpass);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_TexImage2D(PP_Resource context, GLenum target, GLint level, GLint internalformat, GLsizei width, GLsizei height, GLint border, GLenum format, GLenum type, const void* pixels) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->TexImage2D(context, target, level, internalformat, width, height, border, format, type, pixels);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_TexParameterf(PP_Resource context, GLenum target, GLenum pname, GLfloat param) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->TexParameterf(context, target, pname, param);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_TexParameterfv(PP_Resource context, GLenum target, GLenum pname, const GLfloat* params) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->TexParameterfv(context, target, pname, params);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_TexParameteri(PP_Resource context, GLenum target, GLenum pname, GLint param) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->TexParameteri(context, target, pname, param);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_TexParameteriv(PP_Resource context, GLenum target, GLenum pname, const GLint* params) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->TexParameteriv(context, target, pname, params);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_TexSubImage2D(PP_Resource context, GLenum target, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLenum type, const void* pixels) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->TexSubImage2D(context, target, level, xoffset, yoffset, width, height, format, type, pixels);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Uniform1f(PP_Resource context, GLint location, GLfloat x) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Uniform1f(context, location, x);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Uniform1fv(PP_Resource context, GLint location, GLsizei count, const GLfloat* v) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Uniform1fv(context, location, count, v);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Uniform1i(PP_Resource context, GLint location, GLint x) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Uniform1i(context, location, x);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Uniform1iv(PP_Resource context, GLint location, GLsizei count, const GLint* v) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Uniform1iv(context, location, count, v);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Uniform2f(PP_Resource context, GLint location, GLfloat x, GLfloat y) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Uniform2f(context, location, x, y);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Uniform2fv(PP_Resource context, GLint location, GLsizei count, const GLfloat* v) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Uniform2fv(context, location, count, v);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Uniform2i(PP_Resource context, GLint location, GLint x, GLint y) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Uniform2i(context, location, x, y);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Uniform2iv(PP_Resource context, GLint location, GLsizei count, const GLint* v) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Uniform2iv(context, location, count, v);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Uniform3f(PP_Resource context, GLint location, GLfloat x, GLfloat y, GLfloat z) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Uniform3f(context, location, x, y, z);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Uniform3fv(PP_Resource context, GLint location, GLsizei count, const GLfloat* v) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Uniform3fv(context, location, count, v);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Uniform3i(PP_Resource context, GLint location, GLint x, GLint y, GLint z) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Uniform3i(context, location, x, y, z);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Uniform3iv(PP_Resource context, GLint location, GLsizei count, const GLint* v) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Uniform3iv(context, location, count, v);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Uniform4f(PP_Resource context, GLint location, GLfloat x, GLfloat y, GLfloat z, GLfloat w) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Uniform4f(context, location, x, y, z, w);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Uniform4fv(PP_Resource context, GLint location, GLsizei count, const GLfloat* v) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Uniform4fv(context, location, count, v);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Uniform4i(PP_Resource context, GLint location, GLint x, GLint y, GLint z, GLint w) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Uniform4i(context, location, x, y, z, w);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Uniform4iv(PP_Resource context, GLint location, GLsizei count, const GLint* v) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Uniform4iv(context, location, count, v);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_UniformMatrix2fv(PP_Resource context, GLint location, GLsizei count, GLboolean transpose, const GLfloat* value) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->UniformMatrix2fv(context, location, count, transpose, value);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_UniformMatrix3fv(PP_Resource context, GLint location, GLsizei count, GLboolean transpose, const GLfloat* value) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->UniformMatrix3fv(context, location, count, transpose, value);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_UniformMatrix4fv(PP_Resource context, GLint location, GLsizei count, GLboolean transpose, const GLfloat* value) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->UniformMatrix4fv(context, location, count, transpose, value);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_UseProgram(PP_Resource context, GLuint program) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->UseProgram(context, program);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_ValidateProgram(PP_Resource context, GLuint program) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->ValidateProgram(context, program);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_VertexAttrib1f(PP_Resource context, GLuint indx, GLfloat x) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->VertexAttrib1f(context, indx, x);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_VertexAttrib1fv(PP_Resource context, GLuint indx, const GLfloat* values) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->VertexAttrib1fv(context, indx, values);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_VertexAttrib2f(PP_Resource context, GLuint indx, GLfloat x, GLfloat y) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->VertexAttrib2f(context, indx, x, y);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_VertexAttrib2fv(PP_Resource context, GLuint indx, const GLfloat* values) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->VertexAttrib2fv(context, indx, values);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_VertexAttrib3f(PP_Resource context, GLuint indx, GLfloat x, GLfloat y, GLfloat z) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->VertexAttrib3f(context, indx, x, y, z);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_VertexAttrib3fv(PP_Resource context, GLuint indx, const GLfloat* values) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->VertexAttrib3fv(context, indx, values);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_VertexAttrib4f(PP_Resource context, GLuint indx, GLfloat x, GLfloat y, GLfloat z, GLfloat w) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->VertexAttrib4f(context, indx, x, y, z, w);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_VertexAttrib4fv(PP_Resource context, GLuint indx, const GLfloat* values) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->VertexAttrib4fv(context, indx, values);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_VertexAttribPointer(PP_Resource context, GLuint indx, GLint size, GLenum type, GLboolean normalized, GLsizei stride, const void* ptr) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->VertexAttribPointer(context, indx, size, type, normalized, stride, ptr);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2_Viewport(PP_Resource context, GLint x, GLint y, GLsizei width, GLsizei height) {
			if (has_interface<PPB_OpenGLES2_1_0>()) {
				get_interface<PPB_OpenGLES2_1_0>()->Viewport(context, x, y, width, height);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_OpenGLES2 */

		#pragma region /* Begin entry point methods for PPB_OpenGLES2InstancedArrays */

		PEPPER_EXPORT void PPB_OpenGLES2InstancedArrays_DrawArraysInstancedANGLE(PP_Resource context, GLenum mode, GLint first, GLsizei count, GLsizei primcount) {
			if (has_interface<PPB_OpenGLES2InstancedArrays_1_0>()) {
				get_interface<PPB_OpenGLES2InstancedArrays_1_0>()->DrawArraysInstancedANGLE(context, mode, first, count, primcount);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2InstancedArrays_DrawElementsInstancedANGLE(PP_Resource context, GLenum mode, GLsizei count, GLenum type, const void* indices, GLsizei primcount) {
			if (has_interface<PPB_OpenGLES2InstancedArrays_1_0>()) {
				get_interface<PPB_OpenGLES2InstancedArrays_1_0>()->DrawElementsInstancedANGLE(context, mode, count, type, indices, primcount);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2InstancedArrays_VertexAttribDivisorANGLE(PP_Resource context, GLuint index, GLuint divisor) {
			if (has_interface<PPB_OpenGLES2InstancedArrays_1_0>()) {
				get_interface<PPB_OpenGLES2InstancedArrays_1_0>()->VertexAttribDivisorANGLE(context, index, divisor);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_OpenGLES2InstancedArrays */

		#pragma region /* Begin entry point methods for PPB_OpenGLES2FramebufferBlit */

		PEPPER_EXPORT void PPB_OpenGLES2FramebufferBlit_BlitFramebufferEXT(PP_Resource context, GLint srcX0, GLint srcY0, GLint srcX1, GLint srcY1, GLint dstX0, GLint dstY0, GLint dstX1, GLint dstY1, GLbitfield mask, GLenum filter) {
			if (has_interface<PPB_OpenGLES2FramebufferBlit_1_0>()) {
				get_interface<PPB_OpenGLES2FramebufferBlit_1_0>()->BlitFramebufferEXT(context, srcX0, srcY0, srcX1, srcY1, dstX0, dstY0, dstX1, dstY1, mask, filter);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_OpenGLES2FramebufferBlit */

		#pragma region /* Begin entry point methods for PPB_OpenGLES2FramebufferMultisample */

		PEPPER_EXPORT void PPB_OpenGLES2FramebufferMultisample_RenderbufferStorageMultisampleEXT(PP_Resource context, GLenum target, GLsizei samples, GLenum internalformat, GLsizei width, GLsizei height) {
			if (has_interface<PPB_OpenGLES2FramebufferMultisample_1_0>()) {
				get_interface<PPB_OpenGLES2FramebufferMultisample_1_0>()->RenderbufferStorageMultisampleEXT(context, target, samples, internalformat, width, height);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_OpenGLES2FramebufferMultisample */

		#pragma region /* Begin entry point methods for PPB_OpenGLES2ChromiumEnableFeature */

		PEPPER_EXPORT GLboolean PPB_OpenGLES2ChromiumEnableFeature_EnableFeatureCHROMIUM(PP_Resource context, const char* feature) {
			if (has_interface<PPB_OpenGLES2ChromiumEnableFeature_1_0>()) {
				return get_interface<PPB_OpenGLES2ChromiumEnableFeature_1_0>()->EnableFeatureCHROMIUM(context, feature);
			}
			return NULL;
		}

		#pragma endregion /* End entry point generation for PPB_OpenGLES2ChromiumEnableFeature */

		#pragma region /* Begin entry point methods for PPB_OpenGLES2ChromiumMapSub */

		PEPPER_EXPORT void* PPB_OpenGLES2ChromiumMapSub_MapBufferSubDataCHROMIUM(PP_Resource context, GLuint target, GLintptr offset, GLsizeiptr size, GLenum access) {
			if (has_interface<PPB_OpenGLES2ChromiumMapSub_1_0>()) {
				return get_interface<PPB_OpenGLES2ChromiumMapSub_1_0>()->MapBufferSubDataCHROMIUM(context, target, offset, size, access);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_OpenGLES2ChromiumMapSub_UnmapBufferSubDataCHROMIUM(PP_Resource context, const void* mem) {
			if (has_interface<PPB_OpenGLES2ChromiumMapSub_1_0>()) {
				get_interface<PPB_OpenGLES2ChromiumMapSub_1_0>()->UnmapBufferSubDataCHROMIUM(context, mem);
			}
			return ;
		}

		PEPPER_EXPORT void* PPB_OpenGLES2ChromiumMapSub_MapTexSubImage2DCHROMIUM(PP_Resource context, GLenum target, GLint level, GLint xoffset, GLint yoffset, GLsizei width, GLsizei height, GLenum format, GLenum type, GLenum access) {
			if (has_interface<PPB_OpenGLES2ChromiumMapSub_1_0>()) {
				return get_interface<PPB_OpenGLES2ChromiumMapSub_1_0>()->MapTexSubImage2DCHROMIUM(context, target, level, xoffset, yoffset, width, height, format, type, access);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_OpenGLES2ChromiumMapSub_UnmapTexSubImage2DCHROMIUM(PP_Resource context, const void* mem) {
			if (has_interface<PPB_OpenGLES2ChromiumMapSub_1_0>()) {
				get_interface<PPB_OpenGLES2ChromiumMapSub_1_0>()->UnmapTexSubImage2DCHROMIUM(context, mem);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_OpenGLES2ChromiumMapSub */

		#pragma region /* Begin entry point methods for PPB_OpenGLES2Query */

		PEPPER_EXPORT void PPB_OpenGLES2Query_GenQueriesEXT(PP_Resource context, GLsizei n, GLuint* queries) {
			if (has_interface<PPB_OpenGLES2Query_1_0>()) {
				get_interface<PPB_OpenGLES2Query_1_0>()->GenQueriesEXT(context, n, queries);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2Query_DeleteQueriesEXT(PP_Resource context, GLsizei n, const GLuint* queries) {
			if (has_interface<PPB_OpenGLES2Query_1_0>()) {
				get_interface<PPB_OpenGLES2Query_1_0>()->DeleteQueriesEXT(context, n, queries);
			}
			return ;
		}

		PEPPER_EXPORT GLboolean PPB_OpenGLES2Query_IsQueryEXT(PP_Resource context, GLuint id) {
			if (has_interface<PPB_OpenGLES2Query_1_0>()) {
				return get_interface<PPB_OpenGLES2Query_1_0>()->IsQueryEXT(context, id);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_OpenGLES2Query_BeginQueryEXT(PP_Resource context, GLenum target, GLuint id) {
			if (has_interface<PPB_OpenGLES2Query_1_0>()) {
				get_interface<PPB_OpenGLES2Query_1_0>()->BeginQueryEXT(context, target, id);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2Query_EndQueryEXT(PP_Resource context, GLenum target) {
			if (has_interface<PPB_OpenGLES2Query_1_0>()) {
				get_interface<PPB_OpenGLES2Query_1_0>()->EndQueryEXT(context, target);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2Query_GetQueryivEXT(PP_Resource context, GLenum target, GLenum pname, GLint* params) {
			if (has_interface<PPB_OpenGLES2Query_1_0>()) {
				get_interface<PPB_OpenGLES2Query_1_0>()->GetQueryivEXT(context, target, pname, params);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2Query_GetQueryObjectuivEXT(PP_Resource context, GLuint id, GLenum pname, GLuint* params) {
			if (has_interface<PPB_OpenGLES2Query_1_0>()) {
				get_interface<PPB_OpenGLES2Query_1_0>()->GetQueryObjectuivEXT(context, id, pname, params);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_OpenGLES2Query */

		#pragma region /* Begin entry point methods for PPB_OpenGLES2VertexArrayObject */

		PEPPER_EXPORT void PPB_OpenGLES2VertexArrayObject_GenVertexArraysOES(PP_Resource context, GLsizei n, GLuint* arrays) {
			if (has_interface<PPB_OpenGLES2VertexArrayObject_1_0>()) {
				get_interface<PPB_OpenGLES2VertexArrayObject_1_0>()->GenVertexArraysOES(context, n, arrays);
			}
			return ;
		}

		PEPPER_EXPORT void PPB_OpenGLES2VertexArrayObject_DeleteVertexArraysOES(PP_Resource context, GLsizei n, const GLuint* arrays) {
			if (has_interface<PPB_OpenGLES2VertexArrayObject_1_0>()) {
				get_interface<PPB_OpenGLES2VertexArrayObject_1_0>()->DeleteVertexArraysOES(context, n, arrays);
			}
			return ;
		}

		PEPPER_EXPORT GLboolean PPB_OpenGLES2VertexArrayObject_IsVertexArrayOES(PP_Resource context, GLuint array) {
			if (has_interface<PPB_OpenGLES2VertexArrayObject_1_0>()) {
				return get_interface<PPB_OpenGLES2VertexArrayObject_1_0>()->IsVertexArrayOES(context, array);
			}
			return NULL;
		}

		PEPPER_EXPORT void PPB_OpenGLES2VertexArrayObject_BindVertexArrayOES(PP_Resource context, GLuint array) {
			if (has_interface<PPB_OpenGLES2VertexArrayObject_1_0>()) {
				get_interface<PPB_OpenGLES2VertexArrayObject_1_0>()->BindVertexArrayOES(context, array);
			}
			return ;
		}

		#pragma endregion /* End entry point generation for PPB_OpenGLES2VertexArrayObject */

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
