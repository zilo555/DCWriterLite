using System;
using System.Collections.Generic ;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 文本行集合
    /// </summary>
#if !RELEASE
    [System.Diagnostics.DebuggerDisplay("Count={ Count }")]
    [System.Diagnostics.DebuggerTypeProxy(typeof(DCSoft.Common.ListDebugView))]
#endif
    public partial class DomLineList : DCSoft.Common.DCList<DomLine>
	{
        public DomLineList()
        {

        }
        public DomLineList( int capacity) :base(capacity)
        {

        }
        public bool ContainsByIndexOfDocumentContentLines(DomLine line)
        {
            return line != null
                && line.IndexOfDocumentContentLines >= 0
                && line.IndexOfDocumentContentLines < this.Count
                && this[line.IndexOfDocumentContentLines] == line;
        }
       
        public int FastIndexOf(DomLine line)
        {
            if (line == null)
            {
                throw new ArgumentNullException("line");
            }
            int index = line.IndexOfDocumentContentLines;
            if (index >= 0 && index < this.Count && this[index] == line)
            {
                return index;
            }
            return base.IndexOf(line);
        }


        /// <summary>
        /// 第一个文本行
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
        public DomLine FirstLine
        {
            get
            {
                if (this.Count > 0)
                {
                    return this[0];
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// 最后一个文本行
        /// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
        public DomLine LastLine
        {
            get
            {
                if (this.Count > 0)
                {
                    return this[this.Count - 1];
                }
                else
                {
                    return null;
                }
            }
        }

		/// <summary>
		/// 所有文本行的高度和
		/// </summary>
        //[System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
        public float Height
		{
			get
			{
				float h = 0 ;
				//XTextLine LastLine = null;
				foreach( DomLine line in this )
				{
					//LastLine = line ;
                    h = h + line.Height;// + line.Spacing ;
				}
				//if( LastLine != null )
				//	h -= LastLine.Spacing ;
				return h ;
			}
		}
        ///// <summary>
        ///// 添加文本行
        ///// </summary>
        ///// <param name="line">要添加的文本行对象</param>
        ///// <returns>新文本行在列表中的序号</returns>
        //public int Add( XTextLine line )
        //{
        //    if( line != null )
        //    {
        //        //line.OwnerList = this ;
        //        return this.List.Add( line );
        //    }
        //    return -1 ;
        //}

		/// <summary>
		/// 将旧行对象替换成新的行对象
		/// </summary>
		/// <param name="index">序号</param>
		/// <param name="NewLine">新的行对象</param>
		public void Replace( int index , DomLine NewLine )
		{
			//NewLine.OwnerList = this ;
            this[index] = NewLine;
			//this.List[ index ] = NewLine ;
		}

//		public void Replace( XTextLine OldLine , XTextLine NewLine )
//		{
//
//		}
        ///// <summary>
        ///// 插入文本行对象
        ///// </summary>
        ///// <param name="index">插入的位置序号</param>
        ///// <param name="line">要插入的文本行对象</param>
        //public void Insert( int index , XTextLine line )
        //{
        //    //line.OwnerList = this ;
        //    this.List.Insert( index , line );
        //}
        ///// <summary>
        ///// 删除文本行对象
        ///// </summary>
        ///// <param name="line">要删除的文本行对象</param>
        //public void Remove( XTextLine line )
        //{
        //    this.List.Remove( line );
        //}

        ///// <summary>
        ///// 获得后面一行
        ///// </summary>
        ///// <param name="line"></param>
        ///// <returns></returns>
        //internal XTextLine GetNextLine(XTextLine line)
        //{
        //    int ic = this.Count;
        //    int index = line.ContentLineIndex;
        //    if (index >= 0 && index < ic && this[index] == line)
        //    {
        //        if (index < ic - 1 )
        //        {
        //            return this[index + 1];
        //        }
        //        else
        //        {
        //            return null;
        //        }
        //    }
        //    index = this.IndexOf(line);
        //    if (index >= 0 && index < ic - 1)
        //    {
        //        return this[index + 1];
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        /// <summary>
        /// 获得前面一行
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        internal DomLine GetPreLine(DomLine line)
        {
            if (line == null)
            {
                return null;
            }
            //int index = line.ContentLineIndex;
            //if (index >= 0 && index < this.Count && this[index] == line)
            //{
            //    if (index > 0)
            //    {
            //        return this[index - 1];
            //    }
            //    else
            //    {
            //        return null;
            //    }
            //}
            int index = this.IndexOf(line);
            if (index > 0)
            {
                return this[index - 1];
            }
            else
            {
                return null;
            }
        }

        internal void InnerDispose()
        {
            var arr = this.InnerGetArrayRaw();
            for(var iCount = this.Count -1;iCount >=0;iCount --)
            {
                arr[iCount].InnerDispose();
            }
            //foreach (XTextLine line in this.FastForEach())
            //{
            //    line.InnerDispose();
            //}
            this.ClearAndEmpty();
        }
	}//public class XTextLineList : System.Collections.CollectionBase
}