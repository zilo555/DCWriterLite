using System.Collections.Generic;
using System;

namespace DCSoft.WASM
{
    /// <summary>
    /// HTML中的键盘事件中的按键名称
    /// </summary>
    public static class HtmlKeyNames
    {
        /// <summary>
        /// 按键名称列表，数据源自 https://developer.mozilla.org/en-US/docs/Web/API/UI_Events/Keyboard_event_key_values
        /// </summary>
        private static readonly Dictionary<string, System.Windows.Forms.Keys> _KeyNames = null;
        static HtmlKeyNames()
        {
            // https://developer.mozilla.org/en-US/docs/Web/API/UI_Events/Keyboard_event_key_values
            _KeyNames = new Dictionary<string, System.Windows.Forms.Keys>();
            
            //_KeyNames["Unidentified"] = System.Windows.Forms.Keys.Unidentified;//0x0000
            _KeyNames["Escape"] = System.Windows.Forms.Keys.Escape;//0x0001
            _KeyNames["Digit1"] = System.Windows.Forms.Keys.D1;//0x0002
            _KeyNames["Digit2"] = System.Windows.Forms.Keys.D2;//0x0003
            _KeyNames["Digit3"] = System.Windows.Forms.Keys.D3;//0x0004
            _KeyNames["Digit4"] = System.Windows.Forms.Keys.D4;//0x0005
            _KeyNames["Digit5"] = System.Windows.Forms.Keys.D5;//0x0006
            _KeyNames["Digit6"] = System.Windows.Forms.Keys.D6;//0x0007
            _KeyNames["Digit7"] = System.Windows.Forms.Keys.D7;//0x0008
            _KeyNames["Digit8"] = System.Windows.Forms.Keys.D8;//0x0009
            _KeyNames["Digit9"] = System.Windows.Forms.Keys.D9;//0x000A
            _KeyNames["Digit0"] = System.Windows.Forms.Keys.D0;//0x000B
            _KeyNames["Minus"] = System.Windows.Forms.Keys.Subtract;//0x000C
            //_KeyNames["Equal"] = System.Windows.Forms.Keys.Equal;//0x000D 等于号
            _KeyNames["Backspace"] = System.Windows.Forms.Keys.Back;//0x000E
            _KeyNames["Tab"] = System.Windows.Forms.Keys.Tab;//0x000F
            _KeyNames["KeyQ"] = System.Windows.Forms.Keys.Q;//0x0010
            _KeyNames["KeyW"] = System.Windows.Forms.Keys.W;//0x0011
            _KeyNames["KeyE"] = System.Windows.Forms.Keys.E;//0x0012
            _KeyNames["KeyR"] = System.Windows.Forms.Keys.R;//0x0013
            _KeyNames["KeyT"] = System.Windows.Forms.Keys.T;//0x0014
            _KeyNames["KeyY"] = System.Windows.Forms.Keys.Y;//0x0015
            _KeyNames["KeyU"] = System.Windows.Forms.Keys.U;//0x0016
            _KeyNames["KeyI"] = System.Windows.Forms.Keys.I;//0x0017
            _KeyNames["KeyO"] = System.Windows.Forms.Keys.O;//0x0018
            _KeyNames["KeyP"] = System.Windows.Forms.Keys.P;//0x0019
            _KeyNames["BracketLeft"] = System.Windows.Forms.Keys.OemOpenBrackets;//0x001A 
            _KeyNames["BracketRight"] = System.Windows.Forms.Keys.OemCloseBrackets;//0x001B 
            _KeyNames["Enter"] = System.Windows.Forms.Keys.Enter;//0x001C
            _KeyNames["ControlLeft"] = System.Windows.Forms.Keys.LControlKey;//0x001D
            _KeyNames["KeyA"] = System.Windows.Forms.Keys.A;//0x001E
            _KeyNames["KeyS"] = System.Windows.Forms.Keys.S;//0x001F
            _KeyNames["KeyD"] = System.Windows.Forms.Keys.D;//0x0020
            _KeyNames["KeyF"] = System.Windows.Forms.Keys.F;//0x0021
            _KeyNames["KeyG"] = System.Windows.Forms.Keys.G;//0x0022
            _KeyNames["KeyH"] = System.Windows.Forms.Keys.H;//0x0023
            _KeyNames["KeyJ"] = System.Windows.Forms.Keys.J;//0x0024
            _KeyNames["KeyK"] = System.Windows.Forms.Keys.K;//0x0025
            _KeyNames["KeyL"] = System.Windows.Forms.Keys.L;//0x0026
            _KeyNames["Semicolon"] = System.Windows.Forms.Keys.OemSemicolon;//0x0027
            _KeyNames["Quote"] = System.Windows.Forms.Keys.OemQuotes;//0x0028
            //_KeyNames["Backquote"] = System.Windows.Forms.Keys.Backquote;//0x0029
            _KeyNames["ShiftLeft"] = System.Windows.Forms.Keys.LShiftKey;//0x002A
            _KeyNames["Backslash"] = System.Windows.Forms.Keys.OemBackslash;//0x002B
            _KeyNames["KeyZ"] = System.Windows.Forms.Keys.Z;//0x002C
            _KeyNames["KeyX"] = System.Windows.Forms.Keys.X;//0x002D
            _KeyNames["KeyC"] = System.Windows.Forms.Keys.C;//0x002E
            _KeyNames["KeyV"] = System.Windows.Forms.Keys.V;//0x002F
            _KeyNames["KeyB"] = System.Windows.Forms.Keys.B;//0x0030
            _KeyNames["KeyN"] = System.Windows.Forms.Keys.N;//0x0031
            _KeyNames["KeyM"] = System.Windows.Forms.Keys.M;//0x0032
            _KeyNames["Comma"] = System.Windows.Forms.Keys.Oemcomma;//0x0033
            _KeyNames["Period"] = System.Windows.Forms.Keys.OemPeriod;//0x0034
            //_KeyNames["Slash"] = System.Windows.Forms.Keys.Slash;//0x0035
            _KeyNames["ShiftRight"] = System.Windows.Forms.Keys.RShiftKey;//0x0036
            _KeyNames["NumpadMultiply"] = System.Windows.Forms.Keys.Multiply;//0x0037
            _KeyNames["AltLeft"] = System.Windows.Forms.Keys.LMenu;//0x0038
            _KeyNames["Space"] = System.Windows.Forms.Keys.Space;//0x0039
            _KeyNames["CapsLock"] = System.Windows.Forms.Keys.CapsLock;//0x003A
            _KeyNames["F1"] = System.Windows.Forms.Keys.F1;//0x003B
            _KeyNames["F2"] = System.Windows.Forms.Keys.F2;//0x003C
            _KeyNames["F3"] = System.Windows.Forms.Keys.F3;//0x003D
            _KeyNames["F4"] = System.Windows.Forms.Keys.F4;//0x003E
            _KeyNames["F5"] = System.Windows.Forms.Keys.F5;//0x003F
            _KeyNames["F6"] = System.Windows.Forms.Keys.F6;//0x0040
            _KeyNames["F7"] = System.Windows.Forms.Keys.F7;//0x0041
            _KeyNames["F8"] = System.Windows.Forms.Keys.F8;//0x0042
            _KeyNames["F9"] = System.Windows.Forms.Keys.F9;//0x0043
            _KeyNames["F10"] = System.Windows.Forms.Keys.F10;//0x0044
            _KeyNames["Pause"] = System.Windows.Forms.Keys.Pause;//0x0045
            _KeyNames["ScrollLock"] = System.Windows.Forms.Keys.Scroll;//0x0046
            _KeyNames["Numpad7"] = System.Windows.Forms.Keys.NumPad7;//0x0047
            _KeyNames["Numpad8"] = System.Windows.Forms.Keys.NumPad8;//0x0048
            _KeyNames["Numpad9"] = System.Windows.Forms.Keys.NumPad9;//0x0049
            _KeyNames["NumpadSubtract"] = System.Windows.Forms.Keys.Subtract;//0x004A
            _KeyNames["Numpad4"] = System.Windows.Forms.Keys.NumPad4;//0x004B
            _KeyNames["Numpad5"] = System.Windows.Forms.Keys.NumPad5;//0x004C
            _KeyNames["Numpad6"] = System.Windows.Forms.Keys.NumPad6;//0x004D
            _KeyNames["NumpadAdd"] = System.Windows.Forms.Keys.Add;//0x004E
            _KeyNames["Numpad1"] = System.Windows.Forms.Keys.NumPad1;//0x004F
            _KeyNames["Numpad2"] = System.Windows.Forms.Keys.NumPad2;//0x0050
            _KeyNames["Numpad3"] = System.Windows.Forms.Keys.NumPad3;//0x0051
            _KeyNames["Numpad0"] = System.Windows.Forms.Keys.NumPad0;//0x0052
            _KeyNames["NumpadDecimal"] = System.Windows.Forms.Keys.Decimal;//0x0053
            _KeyNames["PrintScreen"] = System.Windows.Forms.Keys.PrintScreen;//0x0054 
            //_KeyNames["IntlBackslash"] = System.Windows.Forms.Keys.IntlBackslash;//0x0056
            _KeyNames["F11"] = System.Windows.Forms.Keys.F11;//0x0057
            _KeyNames["F12"] = System.Windows.Forms.Keys.F12;//0x0058
            //_KeyNames["NumpadEqual"] = System.Windows.Forms.Keys.NumpadEqual;//0x0059
            _KeyNames["F13"] = System.Windows.Forms.Keys.F13;//0x0064
            _KeyNames["F14"] = System.Windows.Forms.Keys.F14;//0x0065
            _KeyNames["F15"] = System.Windows.Forms.Keys.F15;//0x0066
            _KeyNames["F16"] = System.Windows.Forms.Keys.F16;//0x0067
            _KeyNames["F17"] = System.Windows.Forms.Keys.F17;//0x0068
            _KeyNames["F18"] = System.Windows.Forms.Keys.F18;//0x0069
            _KeyNames["F19"] = System.Windows.Forms.Keys.F19;//0x006A
            _KeyNames["F20"] = System.Windows.Forms.Keys.F20;//0x006B
            _KeyNames["F21"] = System.Windows.Forms.Keys.F21;//0x006C
            _KeyNames["F22"] = System.Windows.Forms.Keys.F22;//0x006D
            _KeyNames["F23"] = System.Windows.Forms.Keys.F23;//0x006E
            _KeyNames["KanaMode"] = System.Windows.Forms.Keys.KanaMode;//0x0070
            _KeyNames["Lang2"] = System.Windows.Forms.Keys.LaunchApplication2;//0x0071
            _KeyNames["Lang1"] = System.Windows.Forms.Keys.LaunchApplication1;//0x0072
            //_KeyNames["IntlRo"] = System.Windows.Forms.Keys.IntlRo;//0x0073
            _KeyNames["F24"] = System.Windows.Forms.Keys.F24;//0x0076
            _KeyNames["Convert"] = System.Windows.Forms.Keys.IMEConvert;//0x0079
            _KeyNames["NonConvert"] = System.Windows.Forms.Keys.IMENonconvert;//0x007B
            _KeyNames["IntlYen"] = System.Windows.Forms.Keys.Insert;//0x007D
            //_KeyNames["NumpadComma"] = System.Windows.Forms.Keys.NumpadComma;//0x007E
            _KeyNames["MediaTrackPrevious"] = System.Windows.Forms.Keys.MediaPreviousTrack;//0xE010
            _KeyNames["MediaTrackNext"] = System.Windows.Forms.Keys.MediaNextTrack;//0xE019
            _KeyNames["NumpadEnter"] = System.Windows.Forms.Keys.Enter;//0xE01C
            _KeyNames["ControlRight"] = System.Windows.Forms.Keys.RControlKey;//0xE01D
            _KeyNames["AudioVolumeMute"] = System.Windows.Forms.Keys.VolumeMute;//0xE020
            _KeyNames["LaunchApp2"] = System.Windows.Forms.Keys.Apps;//0xE021
            _KeyNames["MediaPlayPause"] = System.Windows.Forms.Keys.MediaPlayPause;//0xE022
            _KeyNames["MediaStop"] = System.Windows.Forms.Keys.MediaStop;//0xE024
            _KeyNames["AudioVolumeDown"] = System.Windows.Forms.Keys.VolumeDown;//0xE02E
            _KeyNames["AudioVolumeUp"] = System.Windows.Forms.Keys.VolumeUp;//0xE030
            _KeyNames["BrowserHome"] = System.Windows.Forms.Keys.BrowserHome;//0xE032
            _KeyNames["NumpadDivide"] = System.Windows.Forms.Keys.Divide;//0xE035
            _KeyNames["PrintScreen"] = System.Windows.Forms.Keys.PrintScreen;//0xE037
            _KeyNames["AltRight"] = System.Windows.Forms.Keys.Alt;//0xE038
            _KeyNames["AltLeft"] = System.Windows.Forms.Keys.Alt;//0xE038
            _KeyNames["Help"] = System.Windows.Forms.Keys.Help;//0xE03B
            _KeyNames["NumLock"] = System.Windows.Forms.Keys.NumLock;//0xE045
            _KeyNames["Pause"] = System.Windows.Forms.Keys.Pause;//0xE046
            _KeyNames["Home"] = System.Windows.Forms.Keys.Home;//0xE047
            _KeyNames["ArrowUp"] = System.Windows.Forms.Keys.Up;//0xE048
            _KeyNames["PageUp"] = System.Windows.Forms.Keys.PageUp;//0xE049
            _KeyNames["ArrowLeft"] = System.Windows.Forms.Keys.Left;//0xE04B
            _KeyNames["ArrowRight"] = System.Windows.Forms.Keys.Right;//0xE04D
            _KeyNames["End"] = System.Windows.Forms.Keys.End;//0xE04F
            _KeyNames["ArrowDown"] = System.Windows.Forms.Keys.Down;//0xE050
            _KeyNames["PageDown"] = System.Windows.Forms.Keys.PageDown;//0xE051
            _KeyNames["Insert"] = System.Windows.Forms.Keys.Insert;//0xE052
            _KeyNames["Delete"] = System.Windows.Forms.Keys.Delete;//0xE053
            _KeyNames["MetaLeft"] = System.Windows.Forms.Keys.LWin;//0xE05B
            _KeyNames["MetaRight"] = System.Windows.Forms.Keys.RWin;//0xE05C
                                                                    //_KeyNames["ContextMenu"] = System.Windows.Forms.Keys.ContextMenu;//0xE05D
                                                                    // _KeyNames["Power"] = System.Windows.Forms.Keys.Power;//0xE05E
            _KeyNames["Sleep"] = System.Windows.Forms.Keys.Sleep;//0xE05F
            //_KeyNames["WakeUp"] = System.Windows.Forms.Keys.WakeUp;//0xE063
            _KeyNames["BrowserSearch"] = System.Windows.Forms.Keys.BrowserSearch;//0xE065
            _KeyNames["BrowserFavorites"] = System.Windows.Forms.Keys.BrowserFavorites;//0xE066
            _KeyNames["BrowserRefresh"] = System.Windows.Forms.Keys.BrowserRefresh;//0xE067
            _KeyNames["BrowserStop"] = System.Windows.Forms.Keys.BrowserStop;//0xE068
            _KeyNames["BrowserForward"] = System.Windows.Forms.Keys.BrowserForward;//0xE069
            _KeyNames["BrowserBack"] = System.Windows.Forms.Keys.BrowserBack;//0xE06A
            //_KeyNames["LaunchApp1"] = System.Windows.Forms.Keys.LaunchApp1;//0xE06B
            _KeyNames["LaunchMail"] = System.Windows.Forms.Keys.LaunchMail;//0xE06C
            _KeyNames["MediaSelect"] = System.Windows.Forms.Keys.SelectMedia;//0xE06D

        }
        /// <summary>
        /// 将按键名称转换为按键值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>值</returns>
        public static System.Windows.Forms.Keys ParseKeys( string strName,string strCode )
        {
            if( strName != null 
                && strName.Length > 0  
                &&  _KeyNames.ContainsKey( strName))
            {
                return _KeyNames[strName];
            }
            if( strCode != null 
                && strCode.Length > 0
                && _KeyNames.ContainsKey( strCode))
            {
                return _KeyNames[strCode];
            }
            return System.Windows.Forms.Keys.None;
        }
    }
}
