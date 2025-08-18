using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace DCSoft.Common
{
    /// <summary>
    /// 列表的反向遍历器
    /// </summary>
    public class ListReverseEnumrable : IEnumerable
    {
        public ListReverseEnumrable( IList srcList )
        {
            if (srcList == null)
            {
                throw new ArgumentNullException("srcList");
            }
            this._srcList = srcList;
        }

        private readonly IList _srcList = null;

        [System.Reflection.Obfuscation(Exclude = true, ApplyToMembers = true )]
        public IEnumerator GetEnumerator()
        {
            return new MyEnumerator(this._srcList);
        }

        private class MyEnumerator : IEnumerator
        {
            public MyEnumerator(IList srcList)
            {
                this._srcList = srcList;
                this._Index = srcList.Count;
            }
            private int _Index = 0;

            private readonly IList _srcList = null;

            public object Current
            {
                get
                {
                    if (this._Index >= 0 && this._Index < this._srcList.Count)
                    {
                        return this._srcList[this._Index];
                    }
                    else
                    {
                        throw new IndexOutOfRangeException("index:" + this._Index + ",count:" + this._srcList.Count);
                    }
                }
            }

            public bool MoveNext()
            {
                this._Index--;
                return this._Index >= 0;
            }

            public void Reset()
            {
                this._Index = this._srcList.Count ;
            }
        }

    }
}
