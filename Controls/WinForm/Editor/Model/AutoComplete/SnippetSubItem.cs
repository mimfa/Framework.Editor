using MiMFa.Controls.WinForm.Editor.Tools;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text.RegularExpressions;

namespace MiMFa.Controls.WinForm.Editor.Model.AutoComplete
{
    /// <summary>
    /// Autocomplete item for code snippets
    /// </summary>
    /// <remarks>Snippet can contain special char ^ for caret position.</remarks>
    public class SnippetSubItem : SubItem
    {
        public string Snippet = "";
        public SnippetSubItem(string text, string snippet, int imageIndex = -1, string menuText = null, string toolTipTitle = null, string toolTipText = null, char splitChar = '.')
        : base(text, imageIndex, menuText ?? Regex.Split(snippet,"\\W").First(), toolTipTitle ?? "Code snippet:", toolTipText ?? snippet,splitChar)
        { Snippet = (string.IsNullOrWhiteSpace(snippet) ? Text : snippet.Replace("@", Text)).Replace("\r", ""); }
        public SnippetSubItem(string snippet, int imageIndex = -1, string menuText = null, string toolTipTitle = null, string toolTipText = null, char splitChar = '.')
        : this(menuText, (string.IsNullOrWhiteSpace(snippet) ? menuText : snippet), imageIndex, menuText ?? Regex.Split("\\W", snippet).First(), toolTipTitle ?? "Code snippet:", toolTipText ?? snippet, splitChar)
        { }

        public override string ToString()
        {
            return Title ?? Text.Replace("\r\n", " ").Replace("\n", " ").Replace("^", "");
        }

        public override string GetTextForInject()
        {
            return Snippet;
        }
    }


}
