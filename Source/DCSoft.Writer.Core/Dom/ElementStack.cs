using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace DCSoft.Writer.Dom
{
    /// <summary>
    /// 快速的文档元素堆栈
    /// </summary>
    internal class ElementStack
    {
        ///// <summary>
        ///// 表示空的元素堆栈
        ///// </summary>
        //public static readonly ElementStack Empty = new ElementStack();

        //private ElementStack()
        //{
        //    this._Elements = new XTextElement[0];
        //    this._Position = -1;
        //}

        private static DomElement[] _LastArray = null;
        public static void ClearLastCache()
        {
            _LastArray = null;
        }
        /// <summary>
        /// 关闭对象，试图保存缓存池
        /// </summary>
        public void Close()
        {
            Array.Clear(this._Elements, 0, this._ElementsLength);
            if( _LastArray == null || _LastArray.Length < this._Elements.Length )
            {
                _LastArray = this._Elements;
            }
            this._Elements = null;
        }
        public unsafe ElementStack(DomElementList elements, int index, int length)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException("index:" + index);
            }
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException("length:" + length);
            }
            if (index + length > elements.Count)
            {
                throw new ArgumentOutOfRangeException("index+length:" + Convert.ToString(index + length));
            }
            //var tick = Environment.TickCount;
            DomElement[] locElements = _LastArray;
            if (_LastArray != null && _LastArray.Length >= length)
            {
                locElements = _LastArray;
            }
            else
            {
                locElements = new DomElement[length];
            }
            _LastArray = null;
            this._ElementsLength = length;
            DomElement[] srcArray = elements.InnerGetArrayRaw();
            //fixed( XTextElement *locP = locElements)
            //{
            //    fixed( XTextElement *srcP = elements.InnerGetArrayRaw())
            //    {
            //        for (int iCount = length - 1; iCount >= 0; iCount--)
            //        {
            //            locP[length - iCount - 1] = srcP[index + iCount];
            //        }
            //    }
            //}
            for (int iCount = length - 1; iCount >= 0; iCount--)
            {
                locElements[length - iCount - 1] = srcArray[index + iCount];
            }
            this._Elements = locElements;
            //elements.CopyTo(index, _Elements, 0, length);
            //Array.Reverse(_Elements);
            this._Position = this._ElementsLength - 1;
            //_TotalTick += Math.Abs(Environment.TickCount - tick);
            //_TotalElementCount += length;
            //if((DateTime.Now - _dtm2).TotalSeconds > 10 )
            //{
            //    Console.WriteLine(_TotalTick + " and " + _TotalElementCount);
            //    _dtm2 = DateTime.Now;
            //}
        }
        //private static int _TotalTick = 0;
        //private static int _TotalElementCount = 0;
        //private static DateTime _dtm2 = DateTime.Now;
        public void Push(DomElement element)
        {
            var e2 = this._Elements[this._Position + 1];
            if( e2 != element )
            {

            }
            this._Elements[++this._Position] = element;
            //_Position++;
            //_Elements[_Position] = element;
        }

        /// <summary>
        /// 上一次弹出的文档元素
        /// </summary>
        public DomElement LastPopElement
        {
            get
            {
                if (this._Position < this._ElementsLength - 1 && this._Position >= 0 )
                {
                    return this._Elements[this._Position + 1];
                }
                return null;
            }
        }

        public DomElement Pop()
        {
            if (this._Position >= 0)
            {
                return this._Elements[this._Position--];
                //XTextElement result = _Elements[_Position];
                //_Position--;
                //return result;
            }
            else
            {
                return null;
            }
        }

        public int Count
        {
            get
            {
                return this._Position + 1;
            }
        }
         

        public DomElement Peek()
        {
            if (this._Position >= 0)
            {
                return this._Elements[this._Position];
            }
            else
            {
                return null;
            }
        }

        private DomElement[] _Elements ;
        private int _ElementsLength = 0;
        private int _Position ;
    }
}
