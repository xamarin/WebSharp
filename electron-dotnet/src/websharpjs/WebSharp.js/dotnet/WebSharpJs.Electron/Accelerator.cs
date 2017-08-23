using System;
using System.Linq;
using System.Text;

namespace WebSharpJs.Electron
{
    public class Accelerator
    {

        string accelerator = string.Empty;

        public Accelerator(char key, params AcceleratorModifiers[] modifiers) :
            this(key.ToString(), modifiers)
        { }

        public Accelerator(AcceleratorKeys key, params AcceleratorModifiers[] modifiers) :
            this(key.ToString(), modifiers)
        { }

        private Accelerator(string key, params AcceleratorModifiers[] modifiers)
        {
            var code = key;
            var shift = false;

            if (key.Length == 1)
                (code, shift) = KeyboardUtilities.KeyboardCodeFromCharCode(key[0]);

            AcceleratorModifiers mods = (shift) ? AcceleratorModifiers.Shift : 0;
            foreach (var mod in modifiers)
            {
                mods |= mod;
            }
            var accel = new StringBuilder(string.Join("+", mods.ToString().Split(',').Select(t => t.Trim()).ToArray()));
            accel.Append($"+{code}");
            accelerator = accel.ToString();
        }

        private Accelerator() { }

        public static Accelerator Parse(string shortcut)
        {
            var accelerator = new Accelerator();
            accelerator.StringToAccelerator(shortcut);
            return accelerator;
        }

        public static bool TryParse(string shortcut, out Accelerator accelerator)
        {
            accelerator = new Accelerator();
            try
            {
                return accelerator.StringToAccelerator(shortcut);
            }
            catch
            {
                return false;
            }
        }

        // https://github.com/electron/electron/blob/master/atom/browser/ui/accelerator_util.cc
        private static readonly char[] separator = new char[] { '+' };
        bool StringToAccelerator(string shortcut)
        {
            bool parsed = true;

            if (!IsStringASCII(shortcut))
                throw new NotSupportedException("The accelerator string can only contain ASCII characters");

            AcceleratorModifiers modifiers = 0;
            string code = string.Empty;
            bool shifted = false;

            var tokens = shortcut.Split(separator, StringSplitOptions.RemoveEmptyEntries);
            foreach (var token in tokens)
            {
                AcceleratorModifiers modifier = 0;
                if (Enum.TryParse(token, true, out modifier))
                {
                    modifiers |= modifier;
                }
                else
                {
                    if (token.Length == 1)
                        (code, shifted) = KeyboardUtilities.KeyboardCodeFromCharCode(token[0]);
                    else
                    {
                        AcceleratorKeys c;
                        if (Enum.TryParse(token, true, out c))
                        {
                            code = token;
                        }
                    }
                }
            }

            if (code == string.Empty)
                throw new ArgumentException($"Argument: {nameof(shortcut)} does not contain a valid key.");

            accelerator = shortcut;

            return parsed;
        }

        private bool IsStringASCII(string ascii)
        {
            return ascii == Encoding.ASCII.GetString(Encoding.ASCII.GetBytes(ascii));
        }

        public override string ToString()
        {
            return accelerator;
        }
    }

    [Flags]
    public enum AcceleratorModifiers
    {
        Command = 0x1,
        Cmd = 0x2,
        Control = 0x4,
        Ctrl = 0x8,
        CommandOrControl = 0x20,
        CmdOrCtrl = 0x40,
        Alt = 0x80,
        Option = 0x100,
        AltGr = 0x200,
        Shift = 0x400,
        Super = 0x800
    }

    public enum AcceleratorKeys
    {
        F1,
        F2,
        F3,
        F4,
        F5,
        F6,
        F7,
        F8,
        F9,
        F10,
        F11,
        F12,
        F13,
        F14,
        F15,
        F16,
        F17,
        F18,
        F19,
        F20,
        F21,
        F22,
        F23,
        F24,
        Plus,
        Space,
        Tab,
        Backspace,
        Delete,
        Insert,
        Return,
        Enter,
        Up,
        Down,
        Left,
        Right,
        Home,
        End,
        PageUp,
        PageDown,
        Escape,
        Esc,
        VolumeUp,
        VolumeDown,
        VolumeMute,
        MediaNextTrack,
        MediaPreviousTrack,
        MediaStop,
        MediaPlayPause,
        PrintScreen
    }

}
