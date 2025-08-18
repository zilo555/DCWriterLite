using System;
using System.Reflection;
using System.ComponentModel;

namespace DCSoft.Writer.Dom.Undo
{
	/// <summary>
	/// 撤销设置对象属性操作
	/// </summary>
    public class XTextUndoNameProperty : XTextUndoBase
    {
        public XTextUndoNameProperty(object instance, PropertyInfo property, object oldValue, object newValue)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }
            if (property == null)
            {
                throw new ArgumentNullException("property");
            }
            this._ObjectInstance = instance;
            this._Property = property;
            this._OldValue = oldValue;
            this._NewValue = newValue;
        }

        public override void Dispose()
        {
            base.Dispose();
            this._Property = null;
            this._ObjectInstance = null;
            this._OldValue = null;
            this._NewValue = null;
        }

        /// <summary>
        /// 属性信息
        /// </summary>
        private System.Reflection.PropertyInfo _Property = null;

        /// <summary>
        /// 对象实例
        /// </summary>
        private object _ObjectInstance = null;

        /// <summary>
        /// 旧数据
        /// </summary>
        private object _OldValue = null;


        /// <summary>
        /// 新数据
        /// </summary>
        private object _NewValue = null;

        private string PropertyName
        {
            get
            {
                if (this._Property != null)
                {
                    return this._Property.Name;
                }
                return null;
            }
        }

        /// <summary>
        /// 重复操作
        /// </summary>
        public override void Redo(DCSoft.Writer.Undo.XUndoEventArgs args)
        {
                _Property.SetValue(_ObjectInstance, _NewValue, null);
            if (_ObjectInstance is DomElement)
            {
                DomElement element = (DomElement)_ObjectInstance;
                element.SizeInvalid = true;
                this.OwnerList.RefreshElements.Add(element);
                if (element is DomFieldElementBase)
                {
                    DomFieldElementBase field = (DomFieldElementBase)element;
                    if (field.StartElement != null)
                    {
                        this.OwnerList.RefreshElements.Add(field.StartElement);
                    }
                    if (field.EndElement != null)
                    {
                        this.OwnerList.RefreshElements.Add(field.EndElement);
                    }
                }
            }
            if (_ObjectInstance is DomTableRowElement)
            {
                if (this.PropertyName == "SpecifyHeight"
                    || this.PropertyName == "HeaderStyle")
                {
                    this.OwnerList.NeedInvokeMethods
                        = this.OwnerList.NeedInvokeMethods | UndoMethodTypes.RefreshDocument;
                }
            }
            if (_ObjectInstance is DomDocument)
            {
                if (this.PropertyName == "PageSettings"
                    || this.PropertyName == "DefaultStyle")
                {
                    this.OwnerList.NeedInvokeMethods
                        = this.OwnerList.NeedInvokeMethods | UndoMethodTypes.RefreshDocument;
                }
            }
        }

        /// <summary>
        /// 撤销操作
        /// </summary>
        public override void Undo(DCSoft.Writer.Undo.XUndoEventArgs args)
        {
            if (_Property != null)
            {
                _Property.SetValue(_ObjectInstance, _OldValue, null);
            }
            if (_ObjectInstance is DomElement)
            {
                DomElement element = (DomElement)_ObjectInstance;
                element.UpdateContentVersion();
                element.SizeInvalid = true;
                this.OwnerList.RefreshElements.Add(element);
                if (element is DomFieldElementBase)
                {
                    DomFieldElementBase field = (DomFieldElementBase)element;
                    if (field.StartElement != null)
                    {
                        this.OwnerList.RefreshElements.Add(field.StartElement);
                    }
                    if (field.EndElement != null)
                    {
                        this.OwnerList.RefreshElements.Add(field.EndElement);
                    }
                }
            }
            if (_ObjectInstance is DomTableRowElement)
            {
                if (this.PropertyName == "SpecifyHeight"
                    || this.PropertyName == "HeaderStyle")
                {
                    this.OwnerList.NeedInvokeMethods
                        = this.OwnerList.NeedInvokeMethods | UndoMethodTypes.RefreshDocument;
                }
            }
            if (_ObjectInstance is DomDocument)
            {
                if (this.PropertyName == "PageSettings"
                    || this.PropertyName == "DefaultStyle")
                {
                    this.OwnerList.NeedInvokeMethods
                        = this.OwnerList.NeedInvokeMethods | UndoMethodTypes.RefreshDocument;
                }
            }
        }
    }
}