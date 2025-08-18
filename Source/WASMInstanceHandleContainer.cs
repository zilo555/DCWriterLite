using System;
using System.Collections.Generic;

namespace DCSoft.WASM
{
    /// <summary>
    /// 对象对外句柄容器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WASMInstanceHandleContainer<T> : IDisposable where T : class
    {
        public WASMInstanceHandleContainer()
        {

        }

        private Dictionary<int, int> _HashCodes = new Dictionary<int, int>();
        private Dictionary<int, List<int>> _HashCodesExt = null;
        private List<System.WeakReference<T>> _Handles = new List<WeakReference<T>>();
        /// <summary>
        /// 获得对象对外的句柄
        /// </summary>
        /// <param name="instance">对象实例</param>
        /// <returns>获得的句柄值</returns>
        public int GetHandle(T instance)
        {
            if (instance == null)
            {
                return -1;
            }
            var hc = instance.GetHashCode();
            int index = 0;
            if (_HashCodes.TryGetValue(hc, out index))
            {
                //var item = this._Handles[index];
                T itemValue = null;
                if (this._Handles[index].TryGetTarget(out itemValue))
                {
                    if (itemValue == instance)
                    {
                        return index;
                    }
                }
                // 这里应该是出现HashCode重复,但是概率很低
                if (this._HashCodesExt == null)
                {
                    this._HashCodesExt = new Dictionary<int, List<int>>();
                }
                List<int> list2 = null;
                if (this._HashCodesExt.TryGetValue(hc, out list2) == false)
                {
                    list2 = new List<int>();
                    this._HashCodesExt[hc] = list2;
                }
                foreach (var idx2 in list2)
                {
                    if (this._Handles[idx2].TryGetTarget(out itemValue)
                        && itemValue == instance)
                    {
                        return idx2;
                    }
                }
                index = this._Handles.Count;
                this._Handles.Add(new WeakReference<T>(instance));
                list2.Add(index);
                return index;
            }
            else
            {
                // 添加新的项目
                index = this._Handles.Count;
                this._Handles.Add(new WeakReference<T>(instance));
                this._HashCodes[hc] = index;
                return index;
            }
        }
        /// <summary>
        /// 根据句柄获得对象实例
        /// </summary>
        /// <param name="handle">句柄值</param>
        /// <returns>对象实例</returns>
        public T GetInstance(int handle)
        {
            if (handle >= 0 && this._Handles != null && handle < this._Handles.Count)
            {
                T result = null;
                if (this._Handles[handle].TryGetTarget(out result))
                {
                    return result;
                }
            }
            return null;
        }
        /// <summary>
        /// 清空对象数据
        /// </summary>
        public void Clear()
        {
            if (this._Handles != null)
            {
                for(var iCount = this._Handles.Count -1;iCount >= 0;iCount --)
                {
                    this._Handles[iCount].SetTarget(null);
                }
                this._Handles.Clear();
                this._Handles = new List<WeakReference<T>>();
            }
            if( this._HashCodes != null )
            {
                this._HashCodes.Clear();
                this._HashCodes = new Dictionary<int, int>();
            }
            if(this._HashCodesExt != null )
            {
                this._HashCodesExt.Clear();
                this._HashCodesExt = new Dictionary<int, List<int>>();
            }
            //GC.Collect();
        }
        /// <summary>
        /// 销毁对象
        /// </summary>
        public void Dispose()
        {
            this.Clear();
            this._Handles = null;
            this._HashCodes = null;
            this._HashCodesExt = null;
        }
    }
}
