using MiMFa.Controls.WinForm.Editor.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace MiMFa.Controls.WinForm.Editor.Model.AutoComplete
{
    /// <summary>
    /// Item of autocomplete menu
    /// </summary>
    public class Item
    {
        public string Title { get; set; } = null;
        public string Text { get; set; } = null;
        public int ImageIndex { get; set; } = -1;
        /// <summary>
        /// Title for tooltip.
        /// </summary>
        /// <remarks>Return null for disable tooltip for this item</remarks>
        public string ToolTipTitle { get; set; } = null;
        /// <summary>
        /// Tooltip text.
        /// </summary>
        /// <remarks>For display tooltip text, ToolTipTitle must be not null</remarks>
        public string ToolTipText { 
            get => string.IsNullOrEmpty(_ToolTipText)?
                string.IsNullOrEmpty(ToolTipTextPath)&&File.Exists(ToolTipTextPath)?
                File.ReadAllText(ToolTipTextPath):null: _ToolTipText;
            set=> _ToolTipText = value;
        }
        string _ToolTipText = null;
        public string ToolTipTextPath = null;
        /// <summary>
        /// Menu text. This text is displayed in the drop-down menu.
        /// </summary>
        public object Tag = null;
        protected string compartionText = null;

        /// <summary>
        /// Fore color of text of item
        /// </summary>
        public virtual Color ForeColor
        {
            get { return _ForeColor; }
            set { _ForeColor = value; }
        }
        protected  Color _ForeColor = Color.Transparent;

        /// <summary>
        /// Back color of item
        /// </summary>
        public virtual Color BackColor
        {
            get { return _BackColor; }
            set { _BackColor = value; }
        }
        protected Color _BackColor = Color.Transparent;
        public AutoCompleteMenu Parent { get; internal set; }


        public Item(string text = null, int imageIndex = -1, string menuText = null, string toolTipTitle = null, string toolTipText = null)
        {
            this.Title = string.IsNullOrWhiteSpace(menuText) ? text : menuText;
            this.Text = string.IsNullOrWhiteSpace(text) ? menuText : text;
            compartionText = Text.ToLower();
            this.ImageIndex = imageIndex;
            this.ToolTipTitle = toolTipTitle;
            this.ToolTipText = toolTipText;
            if (Default.HasTemplator && Default.Templator.Palette != null)
            {
                _ForeColor = Default.Templator.Palette.MenuForeColor;
                _BackColor = Default.Templator.Palette.MenuBackColor;
            }
        }

        /// <summary>
        /// Returns text for inserting into Textbox
        /// </summary>
        public virtual string GetTextForReplace()
        {
            return Text;
        }
        public virtual string GetTextForInject()
        {
            return Text;
        }

        /// <summary>
        /// Compares fragment text with this item
        /// </summary>
        public virtual CompareResult Compare(string fragmentText, string normaledText)
        {
            if (Text == fragmentText)
                return CompareResult.ExactVisibleAndSelected;
            else if (Text.StartsWith(fragmentText) || compartionText == normaledText)
                return CompareResult.VisibleAndSelected;
            else if (compartionText.Contains(normaledText))
                return CompareResult.Visible;
            return CompareResult.Hidden;
        }

        /// <summary>
        /// Returns text for display into popup menu
        /// </summary>
        public override string ToString()
        {
            return Title ?? Text;
        }

        /// <summary>
        /// This method is called after item inserted into text
        /// </summary>
        public virtual void OnSelected(AutoCompleteMenu popupMenu, SelectedEventArgs e)
        {
        }
        public virtual void OnInjected(AutoCompleteMenu popupMenu, SelectedEventArgs e)
        {
            e.Tb.BeginUpdate();
            e.Tb.Selection.BeginUpdate();
            //remember places
            var ps = e.Tb.Selection.Start;
            //do auto indent
            if (e.Tb.AutoIndent)
            {
                var pfs = popupMenu.Fragment.Start;
                var pfe = popupMenu.Fragment.End;
                for (int iLine = pfs.LineIndex + 1; iLine <= pfe.LineIndex; iLine++)
                {
                    e.Tb.Selection.Start = new Place(0, iLine);
                    e.Tb.DoAutoIndent(iLine);
                }
            }

            if (popupMenu.Fragment.Text.IndexOf('^')>-1) 
            {
                e.Tb.Selection.Start = popupMenu.Fragment.Start;
                //move caret position right and find char ^
                while (e.Tb.Selection.CharBeforeStart != '^')
                    if (!e.Tb.Selection.GoRightThroughFolded())
                        break;
                //remove char ^
                e.Tb.Selection.GoLeft(true);
                e.Tb.InsertText("");
                //
            }
            else e.Tb.Selection.Start = ps;

            e.Tb.Selection.EndUpdate();
            e.Tb.EndUpdate();
        }

    }
    public enum CompareResult
    {
        /// <summary>
        /// Item do not appears
        /// </summary>
        Hidden = 0,
        /// <summary>
        /// Item appears
        /// </summary>
        Visible = 1,
        /// <summary>
        /// Item appears and will selected
        /// </summary>
        VisibleAndSelected = 2,
        /// <summary>
        /// Item appears and will selected
        /// </summary>
        ExactVisibleAndSelected = 3
    }

    public class ItemComparer<T> : IEqualityComparer<T> where T : Item
    {
        public bool Equals(T x, T y) => x.Text == y.Text;

        public int GetHashCode(T obj) => obj.Text.GetHashCode();
    }
}
