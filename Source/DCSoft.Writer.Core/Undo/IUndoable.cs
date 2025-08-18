using System;

namespace DCSoft.Writer.Undo
{
	/// <summary>
	/// 可撤销和重做的接口类型
	/// </summary>
    public interface IUndoable:IDisposable
	{
		/// <summary>
		/// 撤销操作
		/// </summary>
        /// <param name="args">事件参数</param>
        void Undo(XUndoEventArgs args);
		/// <summary>
		/// 重新操作
		/// </summary>
        /// <param name="args">事件参数</param>
		void Redo( XUndoEventArgs args );
        /// <summary>
		/// 对象是否在一个批处理中
		/// </summary>
		bool InGroup { get ; set ; }
	}
}