// https://github.com/electron/electron/blob/master/atom/common/keyboard_util.cc

using System;

namespace WebSharpJs.Electron
{
    internal static class KeyboardUtilities
    {
        public static (string, bool) KeyboardCodeFromCharCode(char c)
        {

            c = Char.ToLower(c);
            bool shifted = false;

            switch (c)
            {
                case (char)0x08: // backspace
                    return (AcceleratorKeys.Backspace.ToString(), false);
                case (char)0x7F: // delete
                    return (AcceleratorKeys.Delete.ToString(), false);
                case (char)0x09: // tab
                    return (AcceleratorKeys.Tab.ToString(), false);
                case (char)0x0D: // return
                    return (AcceleratorKeys.Return.ToString(), false);
                case (char)0x1B: // escape
                    return (AcceleratorKeys.Escape.ToString(), false);
                case ' ':  // space
                    return (AcceleratorKeys.Space.ToString(), false);

                case 'a': return ("A", false);
                case 'b': return ("B", false);
                case 'c': return ("C", false);
                case 'd': return ("D", false);
                case 'e': return ("E", false);
                case 'f': return ("F", false);
                case 'g': return ("G", false);
                case 'h': return ("H", false);
                case 'i': return ("I", false);
                case 'j': return ("J", false);
                case 'k': return ("K", false);
                case 'l': return ("L", false);
                case 'm': return ("M", false);
                case 'n': return ("N", false);
                case 'o': return ("O", false);
                case 'p': return ("P", false);
                case 'q': return ("Q", false);
                case 'r': return ("R", false);
                case 's': return ("S", false);
                case 't': return ("T", false);
                case 'u': return ("U", false);
                case 'v': return ("V", false);
                case 'w': return ("W", false);
                case 'x': return ("X", false);
                case 'y': return ("Y", false);
                case 'z': return ("Z", false);

                case ')': return ("0", true);
                case '0': return ("0", false);
                case '!': return ("1", true);
                case '1': return ("1", false);
                case '@': return ("2", true);
                case '2': return ("2", false);
                case '#': return ("3", true);
                case '3': return ("3", false);
                case '$': return ("4", true);
                case '4': return ("4", false);
                case '%': return ("5", true);
                case '5': return ("5", false);
                case '^': return ("6", true);
                case '6': return ("6", false);
                case '&': return ("7", true);
                case '7': return ("7", false);
                case '*': return ("8", true);
                case '8': return ("8", false);
                case '(': return ("9", true);
                case '9': return ("9", false);


                case ':': return (";", true);
                case ';': return (";", false);
                case '+': return ("=", true);
                case '=': return ("=", false);
                case '<': return (",", true);
                case ',': return (",", false);
                case '_': return ("-", true);
                case '-': return ("-", false);
                case '>': return (".", true);
                case '.': return (".", false);
                case '?': return ("/", true);
                case '/': return ("/", false);
                case '~': return ("`", true);
                case '`': return ("`", false);
                case '{': return ("[", true);
                case '[': return ("[", false);
                case '|': return ("\\", true);
                case '\\': return ("\\", false);
                case '}': return ("]", true);
                case ']': return ("]", false);
                case '"': return ("\'", true);
                case '\'': return ("\'", false);

            }
            return (c.ToString().ToUpper(), shifted);
        }
    }
}
