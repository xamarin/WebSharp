using System;
using System.Runtime.InteropServices;

namespace PepperSharp
{
    public class Audio : Resource
    {
        IntPtr meBePinned = IntPtr.Zero;
        IntPtr userDataHandle = IntPtr.Zero;
        Action<byte[], uint, double, object> callbackDelegate;
        AudioConfig config;

        /// <summary>
        /// A constructor that creates an Audio resource. No sound will be heard
        /// until StartPlayback() is called. The Action<byte[], uint, double, object>
        /// is called with the byte[] buffer and user data whenever the buffer needs to be filled.
        /// From within the delegate, you should not call <code>Audio</code>
        /// functions. The action delegate will be called on a different thread than the one
        /// which created the interface. For performance-critical applications (such
        /// as low-latency audio), the action delegate should avoid blocking or calling
        /// functions that can obtain locks, such as memory allocates. The layout and the size
        /// of the buffer passed to the audio callback will be determined by
        /// the device configuration and is specified in the <code>AudioConfig</code>
        /// documentation.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="config"></param>
        /// <param name="callbackAction"></param>
        /// <param name="userData"></param>
        public Audio(Instance instance,
                    AudioConfig config,
                    Action<byte[], uint, double, object> callbackAction,
                    object userData = null)
        {
            this.config = config;
            callbackDelegate = callbackAction;
            meBePinned = (IntPtr)(GCHandle.Alloc(this, GCHandleType.Pinned));
            if (userData != null)
                userDataHandle = (IntPtr)(GCHandle.Alloc(userData, GCHandleType.Pinned));
            handle = PPBAudio.Create(instance, config, audioCallback, meBePinned);
        }

        byte[] samplesBytes = null;
        void audioCallback(IntPtr samples,
                       uint buffer_size,
                       double latency,
                       IntPtr data)
        {
            var instance = (Audio)((GCHandle)data).Target;

            if (samplesBytes == null || buffer_size != samplesBytes.Length)
                samplesBytes = new byte[buffer_size];

            object userData = null;
            if (userDataHandle != IntPtr.Zero)
                userData = ((GCHandle)userDataHandle).Target;

            if (samplesBytes.Length > 0)
                Marshal.Copy(samples, samplesBytes, 0, (int)buffer_size);

            callbackDelegate?.Invoke(samplesBytes, buffer_size, latency, userData);

            if (samplesBytes != null && samplesBytes.Length > 0)
            {
                var len = (int)Math.Min(buffer_size, samplesBytes.Length);
                Marshal.Copy(samplesBytes, 0, samples, len);
            }

        }

        /// <summary>
        /// Getter function for returning the internal <code>AudioConfig</code>
        /// struct.
        /// </summary>
        public AudioConfig Config
            => config;

        /// <summary>
        /// StartPlayback() starts playback of audio.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        public bool StartPlayback()
            => PPBAudio.StartPlayback(this) == PPBool.True;

        /// <summary>
        /// StopPlayback stops playback of audio.
        /// </summary>
        /// <returns>true if successful, otherwise false.</returns>
        public bool StopPlayback()
            => PPBAudio.StopPlayback(this) == PPBool.True;

        #region Implement IDisposable.

        protected override void Dispose(bool disposing)
        {
            if (!IsEmpty)
            {
                if (disposing)
                {
                    if (userDataHandle != IntPtr.Zero)
                    {
                        ((GCHandle)userDataHandle).Free();
                        userDataHandle = IntPtr.Zero;
                    }
                    if (meBePinned != IntPtr.Zero)
                    {
                        ((GCHandle)meBePinned).Free();
                        meBePinned = IntPtr.Zero;
                    }
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}
