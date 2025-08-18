using System;
using System.Collections.Generic;
using System.Text;
namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// DCWriter内部使用。文档控制状态快照对象
    /// </summary>
    public class DocumentControlerSnapshot
    {
        private DocumentControler _OwnerControler = null;
        /// <summary>
        /// 控制器对象
        /// </summary>
        internal DocumentControler OwnerControler
        {
            get { return _OwnerControler; }
            set { _OwnerControler = value; }
        }

        private int _DocumentContentVersion = 0;
        /// <summary>
        /// 文档内容版本号
        /// </summary>
        public int DocumentContentVersion
        {
            get { return _DocumentContentVersion; }
            set { _DocumentContentVersion = value; }
        }

        private int _SelectionVerion = 0;
        /// <summary>
        /// 选择区域版本号
        /// </summary>
        public int SelectionVerion
        {
            get { return _SelectionVerion; }
            set { _SelectionVerion = value; }
        }

        //private Dictionary<Type, bool> _CanInsertElements = new Dictionary<Type, bool>();

        //public bool CanInsertElementAtCurrentPosition(Type elementType)
        //{
        //    if (_CanInsertElements.ContainsKey(elementType))
        //    {
        //        return _CanInsertElements[elementType];
        //    }
        //    else
        //    {
        //        bool flag = this.OwnerControler.CanInsertElementAtCurrentPosition(elementType);
        //        _CanInsertElements[elementType] = flag;
        //        return flag;
        //    }
        //}

        //public bool CanInsertAtCurrentPosition
        //{
        //    get
        //    {
        //        return CanInsertElementAtCurrentPosition(typeof(XTextElement));
        //    }
        //}

        private bool _CanDeleteSelection = true;
        /// <summary>
        /// 能否删除被选中的内容
        /// </summary>
        public bool CanDeleteSelection
        {
            get { return _CanDeleteSelection; }
            set { _CanDeleteSelection = value; }
        }

        private bool _CanModifySelection = true;
        /// <summary>
        /// 能否修改被选中的内容
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true)]
        public bool CanModifySelection
        {
            get { return _CanModifySelection; }
            set { _CanModifySelection = value; }
        }

        private bool _CanModifyParagraphs = true;
        /// <summary>
        /// 能否修改被选中的段落
        /// </summary>
        [System.Reflection.Obfuscation(Exclude = true)]
        public bool CanModifyParagraphs
        {
            get { return _CanModifyParagraphs; }
            set { _CanModifyParagraphs = value; }
        }
    }
}
