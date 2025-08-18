
using System;
using System.Collections.Generic;
using System.ComponentModel;
//using System.Data;
using System.Text;

namespace System.Windows.Forms
{
    public static class Clipboard
    {
        public static void Clear()
        { }
        public static System.Windows.Forms.IDataObject GetDataObject()
        { return default(System.Windows.Forms.IDataObject); }
        public static void SetDataObject(object data, bool copy, int retryTimes, int retryDelay)
        { }
    }
    public static class DataFormats
    {
        public static readonly string Bitmap = "Bitmap";
        public static readonly string FileDrop = "FileDrop";
        public static readonly string Text = "Text";
        public static readonly string UnicodeText = "UnicodeText";
        public static readonly string Html = "Html";
    }
    public class DataObject : IDataObject
    {
        public DataObject()
        { }
        public object GetData(string format)
        { return null; }
        public bool GetDataPresent(string format)
        { return false; }
        public string[] GetFormats()
        { return null; }
        public void SetData(object data)
        { }
        public void SetData(string format, object data)
        { }
        public void SetTitle( string strTitle )
        {

        }
    }
    [Flags]
    public enum DragDropEffects
    {
        None = 0,
        Copy = 1,
        Move = 2,
        Link = 4,
        Scroll = -2147483648,
        All = -2147483645
    }

    public class DragEventArgs : EventArgs
    {

        public DragEventArgs(
            System.Windows.Forms.IDataObject data,
            int keyState,
            int x,
            int y,
            System.Windows.Forms.DragDropEffects allowedEffect,
            System.Windows.Forms.DragDropEffects effect)
        {
            this._Data = data;
            this._AllowedEffect = allowedEffect;
            this._Effect = effect;
            this._KeyState = keyState;
            this._X = x;
            this._Y = y;
        }


        private System.Windows.Forms.DragDropEffects _AllowedEffect = DragDropEffects.Copy;
        public System.Windows.Forms.DragDropEffects AllowedEffect
        {
            get
            {
                return this._AllowedEffect;
            }
            set
            {
                this._AllowedEffect = value;
            }
        }

        private readonly System.Windows.Forms.IDataObject _Data = null;
        public System.Windows.Forms.IDataObject Data
        {
            get
            {
                return this._Data;
            }
        }
        private System.Windows.Forms.DragDropEffects _Effect = DragDropEffects.Copy;
        public System.Windows.Forms.DragDropEffects Effect
        {
            get
            {
                return this._Effect;
            }
            set
            {
                this._Effect = value;
            }
        }
        private readonly int _KeyState = 0;
        public int KeyState
        {
            get { return this._KeyState; }
        }
        private readonly int _X = 0;
        public int X
        {
            get { return this._X; }
        }
        private readonly int _Y = 0;
        public int Y
        {
            get { return this._Y; }
        }
    }

    public interface IDataObject
    {
        object GetData(string format);
        bool GetDataPresent(string format);
        string[] GetFormats();
        void SetData(object data);
        void SetData(string format, object data);
        void SetTitle(string strTitle);
    }
    public class KeyEventArgs
    {
        private readonly Keys keyData;

        private bool handled;


        public virtual bool Alt
        {
            get
            {
                return (this.keyData & Keys.Alt) == Keys.Alt;
            }
        }

        public bool Control
        {
            get
            {
                return (this.keyData & Keys.Control) == Keys.Control;
            }
        }

        public bool Handled
        {
            get
            {
                return handled;
            }
            set
            {
                handled = value;
            }
        }

        public Keys KeyCode
        {
            get
            {
                Keys keys = this.keyData & Keys.KeyCode;
                if (!Enum.IsDefined(typeof(Keys), (int)keys))
                {
                    return Keys.None;
                }
                return keys;
            }
        }


        public Keys KeyData
        {
            get
            {
                return this.keyData;
            }
        }

        public Keys Modifiers
        {
            get
            {
                return this.keyData & Keys.Modifiers;
            }
        }

        public virtual bool Shift
        {
            get
            {
                return (this.keyData & Keys.Shift) == Keys.Shift;
            }
        }

        public KeyEventArgs(Keys keyData)
        {
            this.keyData = keyData;
        }
    }
    public class KeyPressEventArgs
    {
        private char keyChar;

        private bool handled;

        public char KeyChar
        {
            get
            {
                return keyChar;
            }
            set
            {
                keyChar = value;
            }
        }

        public bool Handled
        {
            get
            {
                return handled;
            }
            set
            {
                handled = value;
            }
        }

        public KeyPressEventArgs(char keyChar)
        {
            this.keyChar = keyChar;
        }
    }
    [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true)]
    [Flags]
    public enum Keys
    {
        KeyCode = 65535,
        Modifiers = -65536,
        None = 0,
        LButton = 1,
        RButton = 2,
        Cancel = 3,
        MButton = 4,
        XButton1 = 5,
        XButton2 = 6,
        Back = 8,
        Tab = 9,
        LineFeed = 10,
        Clear = 12,
        Return = 13,
        Enter = 13,
        ShiftKey = 16,
        ControlKey = 17,
        Menu = 18,
        Pause = 19,
        Capital = 20,
        CapsLock = 20,
        KanaMode = 21,
        HanguelMode = 21,
        HangulMode = 21,
        JunjaMode = 23,
        FinalMode = 24,
        HanjaMode = 25,
        KanjiMode = 25,
        Escape = 27,
        IMEConvert = 28,
        IMENonconvert = 29,
        IMEAccept = 30,
        IMEAceept = 30,
        IMEModeChange = 31,
        Space = 32,
        Prior = 33,
        PageUp = 33,
        Next = 34,
        PageDown = 34,
        End = 35,
        Home = 36,
        Left = 37,
        Up = 38,
        Right = 39,
        Down = 40,
        Select = 41,
        Print = 42,
        Execute = 43,
        Snapshot = 44,
        PrintScreen = 44,
        Insert = 45,
        Delete = 46,
        Help = 47,
        D0 = 48,
        D1 = 49,
        D2 = 50,
        D3 = 51,
        D4 = 52,
        D5 = 53,
        D6 = 54,
        D7 = 55,
        D8 = 56,
        D9 = 57,
        A = 65,
        B = 66,
        C = 67,
        D = 68,
        E = 69,
        F = 70,
        G = 71,
        H = 72,
        I = 73,
        J = 74,
        K = 75,
        L = 76,
        M = 77,
        N = 78,
        O = 79,
        P = 80,
        Q = 81,
        R = 82,
        S = 83,
        T = 84,
        U = 85,
        V = 86,
        W = 87,
        X = 88,
        Y = 89,
        Z = 90,
        LWin = 91,
        RWin = 92,
        Apps = 93,
        Sleep = 95,
        NumPad0 = 96,
        NumPad1 = 97,
        NumPad2 = 98,
        NumPad3 = 99,
        NumPad4 = 100,
        NumPad5 = 101,
        NumPad6 = 102,
        NumPad7 = 103,
        NumPad8 = 104,
        NumPad9 = 105,
        Multiply = 106,
        Add = 107,
        Separator = 108,
        Subtract = 109,
        Decimal = 110,
        Divide = 111,
        F1 = 112,
        F2 = 113,
        F3 = 114,
        F4 = 115,
        F5 = 116,
        F6 = 117,
        F7 = 118,
        F8 = 119,
        F9 = 120,
        F10 = 121,
        F11 = 122,
        F12 = 123,
        F13 = 124,
        F14 = 125,
        F15 = 126,
        F16 = 127,
        F17 = 128,
        F18 = 129,
        F19 = 130,
        F20 = 131,
        F21 = 132,
        F22 = 133,
        F23 = 134,
        F24 = 135,
        NumLock = 144,
        Scroll = 145,
        LShiftKey = 160,
        RShiftKey = 161,
        LControlKey = 162,
        RControlKey = 163,
        LMenu = 164,
        RMenu = 165,
        BrowserBack = 166,
        BrowserForward = 167,
        BrowserRefresh = 168,
        BrowserStop = 169,
        BrowserSearch = 170,
        BrowserFavorites = 171,
        BrowserHome = 172,
        VolumeMute = 173,
        VolumeDown = 174,
        VolumeUp = 175,
        MediaNextTrack = 176,
        MediaPreviousTrack = 177,
        MediaStop = 178,
        MediaPlayPause = 179,
        LaunchMail = 180,
        SelectMedia = 181,
        LaunchApplication1 = 182,
        LaunchApplication2 = 183,
        OemSemicolon = 186,
        Oem1 = 186,
        Oemplus = 187,
        Oemcomma = 188,
        OemMinus = 189,
        OemPeriod = 190,
        OemQuestion = 191,
        Oem2 = 191,
        Oemtilde = 192,
        Oem3 = 192,
        OemOpenBrackets = 219,
        Oem4 = 219,
        OemPipe = 220,
        Oem5 = 220,
        OemCloseBrackets = 221,
        Oem6 = 221,
        OemQuotes = 222,
        Oem7 = 222,
        Oem8 = 223,
        OemBackslash = 226,
        Oem102 = 226,
        ProcessKey = 229,
        Packet = 231,
        Attn = 246,
        Crsel = 247,
        Exsel = 248,
        EraseEof = 249,
        Play = 250,
        Zoom = 251,
        NoName = 252,
        Pa1 = 253,
        OemClear = 254,
        Shift = 65536,
        Control = 131072,
        Alt = 262144
    }
    
}
