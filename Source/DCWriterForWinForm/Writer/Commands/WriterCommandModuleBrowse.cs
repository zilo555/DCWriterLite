using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DCSoft.Common;
using DCSoft.Printing;
using DCSoft.Writer.Controls;
using DCSoft.Writer.Dom;

namespace DCSoft.Writer.Commands
{
    /// <summary>
    /// 只读的浏览文档内容的功能模块
    /// </summary>
    /// <remarks>
    /// 该模块中的功能用于滚动浏览文档内容，不会修改文档内容。编制，袁永福。
    /// </remarks>
    internal sealed class WriterCommandModuleBrowse : WriterCommandModule
    {
        /// <summary>
        /// 初始化对象
        /// </summary>
        public WriterCommandModuleBrowse()
        {
        }

        // DCSoft.Writer.Commands.WriterCommandModuleBrowse
        protected override WriterCommandList CreateCommands()
        {
            var list = new WriterCommandList();
            AddCommandToList(list, StandardCommandNames.DebugMode, this.DebugMode);
            AddCommandToList(list, StandardCommandNames.MoveDown, this.MoveDown, System.Windows.Forms.Keys.Down);
            AddCommandToList(list, StandardCommandNames.MoveEnd, this.MoveEnd, System.Windows.Forms.Keys.End);
            AddCommandToList(list, StandardCommandNames.MoveHome, this.MoveHome, System.Windows.Forms.Keys.Home);
            AddCommandToList(list, StandardCommandNames.MoveLeft, this.MoveLeft, System.Windows.Forms.Keys.Left);
            AddCommandToList(list, StandardCommandNames.MovePageDown, this.MovePageDown, System.Windows.Forms.Keys.Next);
            AddCommandToList(list, StandardCommandNames.MovePageUp, this.MovePageUp, System.Windows.Forms.Keys.PageUp);
            AddCommandToList(list, StandardCommandNames.MoveRight, this.MoveRight, System.Windows.Forms.Keys.Right);
            AddCommandToList(list, StandardCommandNames.MoveTo, this.MoveTo);
            AddCommandToList(list, StandardCommandNames.MoveToPage, this.MoveToPage);
            AddCommandToList(list, StandardCommandNames.MoveToPosition, this.MoveToPosition);
            AddCommandToList(list, StandardCommandNames.MoveUp, this.MoveUp, System.Windows.Forms.Keys.Up);
            AddCommandToList(list, StandardCommandNames.RefreshDocument, this.RefreshDocument);
            AddCommandToList(list, StandardCommandNames.RuleVisible, this.RuleVisible);
            AddCommandToList(list, StandardCommandNames.SelectAll, this.SelectAll, System.Windows.Forms.Keys.A | System.Windows.Forms.Keys.Control);
            AddCommandToList(list, StandardCommandNames.SelectLine, this.SelectLine);
            AddCommandToList(list, StandardCommandNames.ShiftMoveDown, this.ShiftMoveDown, System.Windows.Forms.Keys.Down | System.Windows.Forms.Keys.Shift);
            AddCommandToList(list, StandardCommandNames.ShiftMoveEnd, this.ShiftMoveEnd, System.Windows.Forms.Keys.End | System.Windows.Forms.Keys.Shift);
            AddCommandToList(list, StandardCommandNames.ShiftMoveHome, this.ShiftMoveHome, System.Windows.Forms.Keys.Home | System.Windows.Forms.Keys.Shift);
            AddCommandToList(list, StandardCommandNames.ShiftMoveLeft, this.ShiftMoveLeft, System.Windows.Forms.Keys.Left | System.Windows.Forms.Keys.Shift);
            AddCommandToList(list, StandardCommandNames.ShiftMovePageDown, this.ShiftMovePageDown, System.Windows.Forms.Keys.Next | System.Windows.Forms.Keys.Shift);
            AddCommandToList(list, StandardCommandNames.ShiftMovePageUp, this.ShiftMovePageUp, System.Windows.Forms.Keys.PageUp | System.Windows.Forms.Keys.Shift);
            AddCommandToList(list, StandardCommandNames.ShiftMoveRight, this.ShiftMoveRight, System.Windows.Forms.Keys.Right | System.Windows.Forms.Keys.Shift);
            AddCommandToList(list, StandardCommandNames.ShiftMoveUp, this.ShiftMoveUp, System.Windows.Forms.Keys.Up | System.Windows.Forms.Keys.Shift);
            return list;
        }

        /// <summary>
        /// 是否显示标尺
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.RuleVisible)]
        private void RuleVisible(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.EditorControl != null;
                if (args.EditorControl != null)
                {
                    args.Checked = args.EditorControl.RuleVisible;
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                bool v = WriterUtilsInner.GetArgumentBooleanValue(args.Parameter, !args.EditorControl.RuleVisible);
                if (args.EditorControl.RuleVisible != v)
                {
                    args.EditorControl.RuleVisible = v;
                }
            }
        }
        [WriterCommandDescription( StandardCommandNames.RefreshDocument)]
        private void RefreshDocument(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.EditorControl != null;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                if (args.EditorControl != null)
                {
                    args.EditorControl.RefreshDocument();
                }
            }
        }



        [WriterCommandDescription(
            StandardCommandNames.MoveToPage,
            ReturnValueType = typeof(bool))]
        private void MoveToPage(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.EditorControl != null && args.Document != null;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                DomDocument document = args.Document;
                PrintPage page = null;
                if (args.Parameter is PrintPage)
                {
                    page = (PrintPage)args.Parameter;
                    if (document.Pages.Contains(page) == false)
                    {
                        page = null;
                    }
                }
                else if (
                    args.Parameter is int
                    || args.Parameter is short
                    || args.Parameter is long
                    || args.Parameter is float
                    || args.Parameter is double
                    || args.Parameter is byte)
                {
                    int index = (int)args.Parameter;
                    page = document.Pages.SafeGet(index - 1);
                }
                else if (args.Parameter is string)
                {
                    int index = 0;
                    if (int.TryParse((string)args.Parameter, out index))
                    {
                        page = document.Pages.SafeGet(index - 1);
                    }
                }
                if (page != null)
                {
                    foreach (DomLine line in document.Body.Lines)
                    {
                        if (line.OwnerPage == page)
                        {
                            DomElement element = line[0];
                            if (element is DomTableElement)
                            {
                                element = element.FirstContentElementInPublicContent;
                            }
                            int index = document.Body.Content.IndexOf(element);
                            if (element is DomParagraphListItemElement)
                            {
                                index++;
                            }
                            index = document.Body.Content.InnerFixIndex(
                                index,
                                FixIndexDirection.Back,
                                true);
                            document.Body.Content.MoveToPosition(index);
                            args.Result = true;
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 移动位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.MoveTo)]
        private void MoveTo(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.Document != null;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                MoveTarget target = MoveTarget.None;
                if (args.Parameter is MoveTarget)
                {
                    target = (MoveTarget)args.Parameter;
                }
                else if (args.Parameter is string)
                {
                    try
                    {
                        target = (MoveTarget)Enum.Parse(
                            typeof(MoveTarget),
                            (string)args.Parameter,
                            true);
                    }
                    catch
                    {
                    }
                }
                if (target != MoveTarget.None)
                {
                    args.Document.Content.AutoClearSelection = true;
                    args.Document.Content.MoveToTarget(target);
                }
            }
        }
        /// <summary>
        /// 移动位置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(
            StandardCommandNames.MoveToPosition,
            ReturnValueType = typeof(bool))]
        private void MoveToPosition(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = args.Document != null;
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Result = false;
                if (args.Parameter == null)
                {
                    // 未指定用户参数，不执行操作
                    return;
                }
                int position = WriterUtilsInner.GetArgumentIntValue(args.Parameter, -1);
                if (position >= 0 && position < args.Document.Content.Count)
                {
                    args.Document.Content.AutoClearSelection = true;
                    args.Result = args.Document.Content.MoveToPosition(position);
                }
            }
        }

        /// <summary>
        /// 向上移动一页
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(
            StandardCommandNames.MovePageDown,
            ShortcutKey = Keys.PageDown)]
        private void MovePageDown(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.Document != null && args.EditorControl != null);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Document.Content.AutoClearSelection = true;
                args.Document.Content.MoveStep(
                    args.Document.PageSettings.ViewClientHeight);
                args.Document.EditorControl.UpdateTextCaret();
            }
        }

        /// <summary>
        /// 向上移动一页
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.MovePageUp, ShortcutKey = Keys.PageUp)]
        private void MovePageUp(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.Document != null && args.EditorControl != null);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Document.Content.AutoClearSelection = true;
                args.Document.Content.MoveStep(
                    -args.Document.PageSettings.ViewPaperHeight);
                args.Document.EditorControl.UpdateTextCaret();
            }
        }

        /// <summary>
        /// 向上移动一列
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.MoveUp, ShortcutKey = Keys.Up)]
        private void MoveUp(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.Document != null && args.EditorControl != null);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Document.Content.AutoClearSelection = true;
                args.Document.Content.MoveUpOneLine();
                args.EditorControl.UpdateTextCaret();
                //args.RefreshLevel = UIStateRefreshLevel.All;
            }
        }

        /// <summary>
        /// 向右移动一列
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.MoveRight, ShortcutKey = Keys.Right)]
        private void MoveRight(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.Document != null && args.EditorControl != null);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Document.Content.AutoClearSelection = true;
                    args.Document.Content.MoveRight();
                args.EditorControl.UpdateTextCaret();
            }
        }


        /// <summary>
        /// 向左移动一列
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.MoveLeft, ShortcutKey = Keys.Left)]
        private void MoveLeft(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.Document != null && args.EditorControl != null);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Document.Content.AutoClearSelection = true;
                //if (XTextLine.__NewLayoutMode)
                //{
                    args.Document.Content.MoveLeft();
                //}
                //else
                //{
                //    if (args.Document.Content.CurrentLayoutDirection == ContentLayoutDirectionStyle.RightToLeft)
                //    {
                //        args.Document.Content.MoveRight();
                //    }
                //    else
                //    {
                //        args.Document.Content.MoveLeft();
                //    }
                //}
                args.EditorControl.UpdateTextCaret();
                //args.RefreshLevel = UIStateRefreshLevel.All;
            }
        }



        /// <summary>
        /// 向下移动一行
        /// </summary>
        /// <param name="send"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.MoveDown, ShortcutKey = Keys.Down)]
        private void MoveDown(object send, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.Document != null && args.EditorControl != null);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Document.Content.AutoClearSelection = true;
                args.Document.Content.MoveDownOneLine();
                args.EditorControl.UpdateTextCaret();
                //args.RefreshLevel = UIStateRefreshLevel.All;
            }
        }
        /// <summary>
        /// 移动到行首
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.MoveHome, ShortcutKey = Keys.Home)]
        private void MoveHome(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.Document != null && args.EditorControl != null);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Document.Content.AutoClearSelection = true;
                args.Document.Content.MoveHome();
                args.EditorControl.UpdateTextCaret();
                //args.RefreshLevel = UIStateRefreshLevel.All;
            }
        }

        /// <summary>
        /// 移动到行尾
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.MoveEnd, ShortcutKey = Keys.End)]
        private void MoveEnd(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.Document != null && args.EditorControl != null);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Document.Content.AutoClearSelection = true;
                args.Document.Content.MoveEnd();
                args.EditorControl.UpdateTextCaret();
                //args.RefreshLevel = UIStateRefreshLevel.All;
            }
        }


        /// <summary>
        /// 向上移动一页
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.ShiftMovePageDown,
            ShortcutKey = Keys.Shift | Keys.PageDown)]
        private void ShiftMovePageDown(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.Document != null && args.EditorControl != null);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Document.Content.AutoClearSelection = false;
                args.Document.Content.MoveStep(
                    args.Document.PageSettings.ViewClientHeight);
                args.EditorControl.UpdateTextCaret();
            }
        }

        /// <summary>
        /// 向上移动一页
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.ShiftMovePageUp,
            ShortcutKey = Keys.Shift | Keys.PageUp)]
        private void ShiftMovePageUp(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.Document != null && args.EditorControl != null);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Document.Content.AutoClearSelection = false;
                args.Document.Content.MoveStep(
                    -args.Document.PageSettings.ViewClientHeight);
                args.EditorControl.UpdateTextCaret();
            }
        }

        /// <summary>
        /// 向上移动一列
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.ShiftMoveUp, ShortcutKey = Keys.Shift | Keys.Up)]
        private void ShiftMoveUp(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.Document != null && args.EditorControl != null);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Document.Content.AutoClearSelection = false;
                args.Document.Content.MoveUpOneLine();
                args.EditorControl.UpdateTextCaret();
                //args.RefreshLevel = UIStateRefreshLevel.All;
            }
        }

        /// <summary>
        /// 向右移动一列
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.ShiftMoveRight,
            ShortcutKey = Keys.Shift | Keys.Right)]
        private void ShiftMoveRight(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.Document != null && args.EditorControl != null);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Document.Content.AutoClearSelection = false;
                    args.Document.Content.MoveRight();
                args.EditorControl.UpdateTextCaret();
            }
        }


        /// <summary>
        /// 向左移动一列
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(StandardCommandNames.ShiftMoveLeft,
            ShortcutKey = Keys.Shift | Keys.Left)]
        private void ShiftMoveLeft(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.Document != null && args.EditorControl != null);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Document.Content.AutoClearSelection = false;
                args.Document.Content.LineEndFlag = false;
                    args.Document.Content.MoveLeft();
                args.EditorControl.UpdateTextCaret();
            }
        }


        [WriterCommandDescription(StandardCommandNames.SelectLine)]
        private void SelectLine(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.Document != null && args.EditorControl != null);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                if (args.Document.CurrentElement.OwnerLine.IsEmptyLine() == false)
                {
                    args.EditorControl.ExecuteCommand(StandardCommandNames.MoveHome, false, null);
                    args.EditorControl.ExecuteCommand(StandardCommandNames.ShiftMoveEnd, false, null);
                }
                args.EditorControl.UpdateTextCaret();
            }
        }


        /// <summary>
        /// 向下移动一行
        /// </summary>
        /// <param name="send"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.ShiftMoveDown,
            ShortcutKey = Keys.Shift | Keys.Down)]
        private void ShiftMoveDown(object send, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.Document != null && args.EditorControl != null);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Document.Content.AutoClearSelection = false;
                args.Document.Content.MoveDownOneLine();
                args.EditorControl.UpdateTextCaret();
            }
        }
        /// <summary>
        /// 移动到行首
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.ShiftMoveHome,
            ShortcutKey = Keys.Shift | Keys.Home)]
        private void ShiftMoveHome(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.Document != null && args.EditorControl != null);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Document.Content.AutoClearSelection = false;
                args.Document.Content.MoveHome();
                args.EditorControl.UpdateTextCaret();
                //args.RefreshLevel = UIStateRefreshLevel.All;
            }
        }

        /// <summary>
        /// 移动到行尾
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.ShiftMoveEnd,
            ShortcutKey = Keys.Shift | Keys.End)]
        private void ShiftMoveEnd(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.Document != null && args.EditorControl != null);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Document.Content.AutoClearSelection = false;
                args.Document.Content.MoveEnd();
                args.EditorControl.UpdateTextCaret();
                //args.RefreshLevel = UIStateRefreshLevel.All;
            }
        }
        /// <summary>
        /// 全选文档内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        [WriterCommandDescription(StandardCommandNames.SelectAll,
            ShortcutKey = Keys.Control | Keys.A)]
        private void SelectAll(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                args.Enabled = (args.Document != null);
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                args.Document.Content.SelectAll();
                args.ViewControl.Invalidate();
                args.EditorControl.UpdateTextCaret();
                //args.RefreshLevel = UIStateRefreshLevel.All;
            }
        }

        /// <summary>
        /// 编辑器是否处于调试模式
        /// </summary>
        /// <param name="sender">参数</param>
        /// <param name="args">参数</param>
        [WriterCommandDescription(
            StandardCommandNames.DebugMode,
            ReturnValueType = typeof(bool))]
        private void DebugMode(object sender, WriterCommandEventArgs args)
        {
            if (args.Mode == WriterCommandEventMode.QueryState)
            {
                if (args.EditorControl == null)
                {
                    args.Enabled = false;
                }
                else
                {
                    args.Enabled = true;
                    args.Checked = args.EditorControl.DocumentOptions.BehaviorOptions.DebugMode;
                }
            }
            else if (args.Mode == WriterCommandEventMode.Invoke)
            {
                var bopts = args.EditorControl.DocumentOptions.BehaviorOptions;
                bool v = WriterUtilsInner.GetArgumentBooleanValue(
                    args.Parameter,
                    !bopts.DebugMode);
                bopts.DebugMode = v;
                args.RefreshLevel = UIStateRefreshLevel.Current;
                args.Result = v;
            }
        }

         
    }
}