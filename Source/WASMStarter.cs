using DCSoft.Writer.Dom;
using DCSoft.Writer;
using DCSoft.Writer.Controls;
using DCSoft.Common;
using DCSoft.Writer.Commands;
using System.Collections.Generic;
using DCSoft.Drawing;
using DCSoft.Writer.Serialization;
using DCSoft.Writer.NewSerializationNoStringEncrypt;

namespace DCSoft.WASM
{
    public static class WASMStarter
    {
        private static char[] _RepalceCharsForLoad_Old = null;
        private static char[] _RepalceCharsForLoad_New = null;
        /// <summary>
        /// 添加在加载文档时的替换字符
        /// </summary>
        /// <param name="oldChar">旧字符编码</param>
        /// <param name="newChar">新字符编码</param>
        [JSInvokable]
        public static void AddReplaceCharsForLoad(int oldChar, int newChar)
        {
            if (oldChar != newChar)
            {
                if (_RepalceCharsForLoad_Old == null)
                {
                    _RepalceCharsForLoad_Old = new char[] { (char)oldChar };
                    _RepalceCharsForLoad_New = new char[] { (char)newChar };
                }
                else
                {
                    var temp = new char[_RepalceCharsForLoad_New.Length + 1];
                    Array.Copy(_RepalceCharsForLoad_Old, temp, _RepalceCharsForLoad_Old.Length);
                    temp[temp.Length - 1] = (char)oldChar;
                    _RepalceCharsForLoad_Old = temp;

                    temp = new char[_RepalceCharsForLoad_New.Length + 1];
                    Array.Copy(_RepalceCharsForLoad_New, temp, _RepalceCharsForLoad_New.Length);
                    temp[ temp.Length -1] = (char)newChar;
                    _RepalceCharsForLoad_New = temp;
                }
            }
        }
        /// <summary>
        /// 为加载文档而修复字符值
        /// </summary>
        /// <param name="c">字符值</param>
        /// <returns>新的字符值</returns>
        public static char FixCharValueForLoad(char c)
        {
            if (c > 127 && _RepalceCharsForLoad_Old != null)
            {
                for (var len = _RepalceCharsForLoad_Old.Length - 1; len >= 0; len--)
                {
                    if (_RepalceCharsForLoad_Old[len] == c)
                    {
                        return _RepalceCharsForLoad_New[len];
                    }
                }
            }
            return c;
        }

        [System.Reflection.Obfuscation(Exclude = false , ApplyToMembers = false , Feature = "JIEJIE.NET.SWITCH:+strings")]
        public static void StartModules()
        {
            var startTick99 = DateTime.Now;
            
            LoaderListBuffer<DomElement[]>.Instance.SetCreator(delegate () { return new DomElement[100]; });
            LoaderListBuffer<DomElementList>.Instance.SetCreator(delegate (){ return new DomElementList(); });
            LoaderListBuffer<DomLine[]>.Instance.SetCreator(delegate () { return new DomLine[100]; });
            LoaderListBuffer<DomLineList>.Instance.SetCreator(delegate () { return new DomLineList(); });
            LoaderListBuffer<List<DomElement>>.Instance.SetCreator(delegate () { return new List<DomElement>(); });

            DCObjectPool<DomLine>.ResetHandler = delegate (DomLine line) { line.ResetFieldValues(); line.ClearAndEmpty(); };
            DCSoft.Data.ValueFormater.EventExecute = WASMUtils.ExecuteValueFormat;

            DCSystemXml.XmlConvert.EventParseDateTime = WASMUtils.ParseXmlDateTime;
            // DUWRITER5_0-3671，遇到宋体不支持的特殊字符，无法显示，则在CS编辑器中使用“宋体10”来输入文字，
            // 获得字符元素的以Document为单位的宽度值，然后挨个在此特别输入。
            DCSoft.Writer.Dom.CharacterMeasurer.SetSpecifyCharWidth((char)8321, 16.65f);
            DCSoft.Writer.Dom.CharacterMeasurer.SetSpecifyCharWidth((char)8322, 16.65f);
            DCSoft.Writer.Dom.CharacterMeasurer.SetSpecifyCharWidth((char)8323, 16.65f);
            DCSoft.Writer.Dom.CharacterMeasurer.SetSpecifyCharWidth((char)189, 20.83f);
            DCSoft.Writer.Dom.CharacterMeasurer.SetSpecifyCharWidth((char)8531, 41.7f);
            DCSoft.Writer.Dom.CharacterMeasurer.SetSpecifyCharWidth((char)8532, 41.7f);
            DCSoft.Writer.Dom.CharacterMeasurer.SetSpecifyCharWidth((char)188, 20.83f);

            DCSoft.Writer.Dom.CharacterMeasurer.SetSpecifyCharWidth((char)9322, 50f);
            DCSoft.Writer.Dom.CharacterMeasurer.SetSpecifyCharWidth((char)9323, 50f);
            DCSoft.Writer.Dom.CharacterMeasurer.SetSpecifyCharWidth((char)9324, 50f);
            DCSoft.Writer.Dom.CharacterMeasurer.SetSpecifyCharWidth((char)9325, 50f);
            DCSoft.Writer.Dom.CharacterMeasurer.SetSpecifyCharWidth((char)9326, 50f);
            DCSoft.Writer.Dom.CharacterMeasurer.SetSpecifyCharWidth((char)9327, 50f);
            DCSoft.Writer.Dom.CharacterMeasurer.SetSpecifyCharWidth((char)9328, 50f);
            DCSoft.Writer.Dom.CharacterMeasurer.SetSpecifyCharWidth((char)9329, 50f);
            DCSoft.Writer.Dom.CharacterMeasurer.SetSpecifyCharWidth((char)9330, 50f);
            DCSoft.Writer.Dom.CharacterMeasurer.SetSpecifyCharWidth((char)9331, 50f);

            XFontValue.DefaultFontName = "宋体";
            XFontValue.EventIsSupportFontName = JavaScriptMethods.Tools_IsSupportFontName;

            //Matrix.EventGetMatrixElements = JavaScriptMethods.Paint_GetMatrixElements;
            Graphics.EventIsStandardImageListReady = JavaScriptMethods.Paint_IsStandardImageListReady;            
            // 软件发布时间
            PublishDateTime.SetPublishDateTime();
            TTFontFileHelper.Start();
            JavaScriptMethods.InitDefaultResourceStrings();
            // XML序列化相关的初始化代码
            DCSoft.Writer.Serialization.Xml.XMLContentSerializer.ReadXTextDocumentHandler = DCSoft.Writer.NewSerialization.NewSerializer20220801.Read_XTextDocument;
            WriterToolsBase.Instance = new WriterTools();
            DCSoft.Writer.Serialization.Xml.MyXmlSerializeHelper.AddSerializerType(
                typeof(DomDocument),
                typeof(DCSoft.Writer.NewSerializationNoStringEncrypt.XTextDocumentSerializer));
            var S2 = DCSR.By;
            var s22 = new DocumentOptions();
            var str = DCSoft.Drawing.XFontValue.DefaultFontName;
            var v2 = DCSoft.WinForms.XCursors.Hand;
            v2 = DCSoft.WinForms.XCursors.RightArrow;
            var v9 = DCSoft.Writer.Dom.DocumentContentStyle.DefaultFontName;
            var zzz = DCSoft.Drawing.StandardPaperSizeInfo.GetStandardPaperSize( PaperKind.A4);
            var startTick = Environment.TickCount;
            DCSoft.Drawing.ParagraphListHelper.IsBulletedList(DCSoft.Drawing.ParagraphListStyle.BulletedList);
            var tick1 = (DateTime.Now.Ticks - startTick) / 1000;
            DCSoft.Writer.DocumentEditOptions.VoidMethod();
            var tick6 = (DateTime.Now.Ticks - startTick) / 1000;
            DCSoft.Drawing.XFontValue.VoidMethod();
            var tick5 = (DateTime.Now.Ticks - startTick) / 1000;
            DCSoft.Drawing.ContentStyle.VoidMethod();
            var tick3 = (DateTime.Now.Ticks - startTick) / 1000;
            DCSoft.Writer.Dom.DocumentContentStyle.VoidMethod2();
            DCSoft.Writer.Dom.DCContentRender.VoidMethod();
            var tick4 = (DateTime.Now.Ticks - startTick) / 1000;
            // 添加标准编辑器命令功能模块
            DCSoft.Writer.Commands.WriterActionModuleList.Default.AddModule(new DCSoft.Writer.Commands.WriterCommandModuleBrowse());
            DCSoft.Writer.Commands.WriterActionModuleList.Default.AddModule(new DCSoft.Writer.Commands.WriterCommandModuleEdit());
            DCSoft.Writer.Commands.WriterActionModuleList.Default.AddModule(new DCSoft.Writer.Commands.WriterCommandModuleFile());
            DCSoft.Writer.Commands.WriterActionModuleList.Default.AddModule(new DCSoft.Writer.Commands.WriterCommandModuleFormat());
            DCSoft.Writer.Commands.WriterActionModuleList.Default.AddModule(new DCSoft.Writer.Commands.WriterCommandModuleInsert());
            DCSoft.Writer.Commands.WriterActionModuleList.Default.AddModule(new DCSoft.Writer.Commands.WriterCommandModuleTable());
            DCSoft.Writer.Commands.WriterActionModuleList.Default.AddModule(new DCSoft.Writer.Commands.WriterCommandModuleTools());
            DCSoft.Writer.Commands.WriterActionModuleList.Default.AddModule(new DCSoft.Writer.Commands.WriterCommandModuleData());
            DCSoft.Writer.Commands.WriterActionModuleList.Default.AddModule(new DCSoft.Writer.Extension.WriterCommandModuleExtension());

            var args = new DCSoft.Writer.Commands.WriterCommandEventArgs(null, null, DCSoft.Writer.Commands.WriterCommandEventMode.InnerNop, null);
            foreach (DCSoft.Writer.Commands.WriterCommandModule m in DCSoft.Writer.Commands.WriterActionModuleList.Default)
            {
                foreach (var m2 in m.Commands())
                {
                    m2.Execute(args);
                }
            }

            DCSoft.Writer.Dom.CharacterMeasurer.StdSize10FontInfo.VoidMethod();
            DCSoft.Writer.Controls.DCDocumentRuleControl.CheckResourceImage();
            DCConsole.Default.WriteLine("DCWriter5 loaded,span " + (DateTime.Now - startTick99).TotalMilliseconds + " milliseconds,version:" +  DCSystemInfo.PublishDateString );
        }
    }
}
