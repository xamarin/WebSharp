using System;
using System.Runtime.InteropServices;

namespace PepperSharp
{
    /// <summary>
    /// The data for all gamepads connected to the system.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct GamepadsSampleData
    {
        internal PPGamepadsSampleData gamepadsSampelData;
        /// <summary>
        /// Number of valid elements in the |items| array.
        /// </summary>
        public uint Length { get; private set; }
        /**
         * Data for an individual gamepad device connected to the system.
         */
        public GamepadSampleData[] Items { get; private set;  }

        public GamepadSampleData this[int index]
        {
            get
            {
                if (index > Length)
                    throw new ArgumentOutOfRangeException("index out of range");

                return Items[index];
            }
        }

        internal GamepadsSampleData(PPGamepadsSampleData samplesData)
        {
            gamepadsSampelData = samplesData;
            Length = gamepadsSampelData.Length;
            Items = new GamepadSampleData[gamepadsSampelData.Length];
            for (int x = 0; x < Length; x++)
                Items[x] = new GamepadSampleData(gamepadsSampelData[x]);

        }
    }

    /// <summary>
    /// The data for one gamepad device.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct GamepadSampleData
    {
        PPGamepadSampleData gamepadSampleData;

        public uint AxesCount
        {
            get { return gamepadSampleData.AxesCount; }
        }

        public float Axes(int index)
        {
            if (index > AxesCount)
                throw new ArgumentOutOfRangeException("index out of range");

            return gamepadSampleData.Axes(index);

        }

        public uint ButtonCount
        {
            get { return gamepadSampleData.ButtonCount; }
        }

        public float Button(int index)
        {
            if (index > ButtonCount)
                throw new ArgumentOutOfRangeException("index out of range");

            return gamepadSampleData.Button(index);
        }

        public double TimeStamp
        {
            get { return gamepadSampleData.TimeStamp; }
        }

        public bool IsConnected
        {
            get { return gamepadSampleData.IsConnected; }
        }

        public ushort[] Id
        {

            get
            {
                return gamepadSampleData.Id;
            }

        }

        internal GamepadSampleData(PPGamepadSampleData gamepadSampleData)
        {
            this.gamepadSampleData = gamepadSampleData;
        }
    }
}
