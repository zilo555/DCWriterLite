using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using DCSoft.Writer.Dom;
using DCSoft.Common;
using DCSoft.Writer.Serialization;
using DCSoft.Drawing;
using DCSoft.Writer.Controls;
using DCSoft.WASM;

namespace DCSoft.Writer.NewSerialization
{
    public static partial class NewSerializer20220801
    {
        private static void UnknownNode(object instance, string nodeName) { }


        private static string[] allSupportFontNames = null;//获取所有的前端支持的字体

        public static DCSoft.Writer.Dom.DomDocument Read_XTextDocument(
            DCSystemXml.XmlReader reader,
            DCSoft.Writer.Dom.DomDocument instance)
        {
            //wyc20241015:添加抛出为空异常
            if (reader == null)
            {
                throw new NullReferenceException("NewSerializer20220801.Read_XTextDocument的参数reader为空");
            }
            //////////////////////////////
            if ( reader is DCSoft.Writer.Serialization.ArrayXmlReader)
            {
                ((DCSoft.Writer.Serialization.ArrayXmlReader)reader).ApplyNameTable();
            }
            reader.MoveToContent();
            if (reader.NodeType == DCSystemXml.XmlNodeType.Element)
            {
                try
                {
                    if (reader.LocalName == "XTextDocument")
                    {
                        var ser = new DCSoft.Writer.NewSerializationNoStringEncrypt.XTextDocumentSerializer();
                        var doc = (DCSoft.Writer.Dom.DomDocument)ser.Deserialize(reader);

                        //wyc20241015:添加抛出为空异常
                        if (doc == null)
                        {
                            throw new NullReferenceException("NewSerializer20220801.Read_XTextDocument的ser.Deserialize(reader)返回为空");
                        }
                        //////////////////////////////
                        
                        //以下代码对字体过滤，屏蔽不支持的字体                        
                        if (allSupportFontNames == null)
                        {//前端获取所有的前端支持的字体
                            allSupportFontNames = JavaScriptMethods.GetFontNames();
                        }
                        if(allSupportFontNames!=null&&allSupportFontNames.Length>0)
                        {
                            var contStyles = doc.ContentStyles;
                            if (contStyles != null)
                            {
                                if (contStyles.Default != null)
                                {
                                    if(Array.IndexOf(allSupportFontNames, contStyles.Default.FontName)==-1)
                                    //if(allSupportFontNames.GetValue().Contains(contStyles.Default.FontName)==false)
                                    {//默认字体不在支持的字体列表中
                                        contStyles.Default.FontName = "宋体";
                                    }
                                }

                                var list = contStyles.Styles;
                                if (list != null && list.Count > 0)
                                {
                                    for (int i = 0; i < list.Count; i++)
                                    {
                                        if (Array.IndexOf(allSupportFontNames, list[i].FontName) == -1)
                                        //if (allSupportFontNames.Contains(list[i].FontName) == false)
                                        {//字体不在支持的字体列表中
                                            list[i].FontName = "宋体";
                                        }
                                    }
                                }
                            }
                        }
                        //字体过滤处理结束


                        if (instance == null)
                        {
                            instance = doc;
                        }
                        else
                        {
                            instance.CopyContent(doc, true, true);
                            doc.Dispose();
                        }
                        return instance;
                    }
                    else { UnknownNode(null, reader.LocalName); }
                }
                finally
                {
                    DomElementList._InnerDeserializing = false;
                }
            }
            return null;
        }
    }
}
