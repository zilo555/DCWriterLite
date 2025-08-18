using System;
using System.Collections.Generic;
using System.Text;
using DCSoft.Writer.Dom;
using DCSoft.Writer.Commands;
using DCSoft.Writer.Controls;
using System.ComponentModel;
using System.ComponentModel.Design;
using DCSoft.Writer.Data;
using DCSoft.Writer.Serialization;
// // 
using DCSoft.WinForms;
using DCSoft.Drawing;
using DCSoft.Common;

namespace DCSoft.Writer
{
    /// <summary>
    /// 文本编辑器宿主对象
    /// </summary>
    public class WriterAppHost
    {
        static WriterAppHost()
        {
        }

        private static WriterAppHost _Default = null;
        public static WriterAppHost Default
        {
            get
            {
                //lock (typeof(WriterAppHost))
                {
                    if (_Default == null)
                    {
                        _Default = new WriterAppHost();
                        //WriterToolsBase.Instance.StartAllModule(_Default);// ModuleStarter.StartAllModule(_Default);
                        ////_Default.CheckStartAllModule();
                        //if (_Default.CommandContainer.Modules.Count == 0)
                        //{
                        //    System.Windows.Forms.MessageBox.Show("aaa");
                        //}
                    }
                    return _Default;
                }
            }
            set
            {
                _Default = value;
            }
        }

        /// <summary>
        /// 注册自定义文档元素类型
        /// </summary>
        /// <param name="elementType">自定义的文档元素对象</param>
        public static void RegisterElementType(Type elementType, string typeDisplayName)
        {
            if (elementType == null)
            {
                throw new ArgumentNullException("elementType");
            }
            DCSoft.Writer.Serialization.Xml.MyXmlSerializeHelper.AddElementType(elementType);
            WriterUtilsInner.SetTypeDisplayName(elementType, typeDisplayName);
            //XTextElement element = (XTextElement)System.Activator.CreateInstance(elementType);
        }



        /// <summary>
        /// 调用文档元素编辑器
        /// </summary>
        /// <param name="args">编辑器命令参数对象</param>
        /// <param name="element">文档元素对象</param>
        /// <param name="method">编辑使用的方法</param>
        /// <returns>操作是否成功</returns>
         
        public static bool CallElementEdtior(
            WriterCommandEventArgs args,
            DomElement element,
            ElementPropertiesEditMethod method)
        {
            return true;
        }

            /// <summary>
            /// 初始化对象
            /// </summary>
        public WriterAppHost()
        {
        }

        private WriterCommandContainer _CommandContainer = new WriterCommandContainer();
        public WriterCommandContainer CommandContainer
        {
            get
            {
                if (_CommandContainer == null)
                {
                    _CommandContainer = new WriterCommandContainer();
                }
                return _CommandContainer;
            }
            set
            {
                _CommandContainer = value;
            }
        }
    }
}
