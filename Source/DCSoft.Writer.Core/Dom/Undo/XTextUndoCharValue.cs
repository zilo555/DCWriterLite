//using System;
//using System.Collections.Generic;
//using System.Text;
//using DCSoft.Writer.Dom;

//namespace DCSoft.Writer.Dom.Undo
//{
//    internal class XTextUndoCharValue : XTextUndoBase
//    {
//        public XTextUndoCharValue()
//        {
//        }

        
//        public void AddInfo(XTextCharElement element, char oldValue, char newValue, bool sizeInvalidate)
//        {
//            if (element == null)
//            {
//                throw new ArgumentNullException("element");
//            }
//            CharValueInfo info = new CharValueInfo();
//            info.Element = element;
//            info.OldValue = oldValue;
//            info.NewValue = newValue;
//            info.SizeInvalidate = sizeInvalidate;
//            this._Infos.Add(info);
//        }

//        private readonly List<CharValueInfo> _Infos = new List<CharValueInfo>();
//        private class CharValueInfo
//        {
//            public XTextCharElement Element = null;
//            public char OldValue = char.MinValue;
//            public char NewValue = char.MinValue;
//            public bool SizeInvalidate = false;
//        }

//        public int Count
//        {
//            get
//            {
//                return _Infos.Count;
//            }
//        }
//        public override void Undo(Writer.Undo.XUndoEventArgs args)
//        {
//            Execute(true);
//        }
//        public override void Redo(Writer.Undo.XUndoEventArgs args)
//        {
//            Execute(false);
//        }
//        private void Execute( bool undo )
//        {
//            //bool hasSizeInvalidate = false ;
//            foreach (CharValueInfo info in this._Infos)
//            {
//                if (undo)
//                {
//                    info.Element.CharValue = info.OldValue;
//                }
//                else
//                {
//                    info.Element.CharValue = info.NewValue;
//                }
//                info.Element.SizeInvalid = info.SizeInvalidate;
//                if( info.SizeInvalidate )
//                {
//                    //hasSizeInvalidate = true ;
//                }
//            }
//            if (this._Infos.Count > 0)
//            {
//                XTextCharElement element = this._Infos[0].Element;
//                if (element.OwnerDocument != null && element.InnerViewControl != null)
//                {
//                    element.OwnerDocument.InnerViewControl.Invalidate();
//                }
//            }
            
//        }
         
//    }
//}
