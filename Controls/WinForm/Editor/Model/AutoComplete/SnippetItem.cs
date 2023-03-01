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
    public class SnippetItem : Item
    {
        public string Snippet = "";
        public SnippetItem(string text,string snippet, int imageIndex = -1, string menuText = null, string toolTipTitle = null, string toolTipText = null)
        : base(text, imageIndex,  menuText?? Regex.Split("\\W", snippet).First(),  toolTipTitle?? "Code snippet:",  toolTipText?? snippet)
        { Snippet = (string.IsNullOrWhiteSpace(snippet) ? Text : snippet.Replace("@", Text)).Replace("\r", ""); }
        public SnippetItem(string snippet, int imageIndex = -1, string menuText = null, string toolTipTitle = null, string toolTipText = null)
        : this(menuText, (string.IsNullOrWhiteSpace(snippet) ? menuText : snippet).Replace("\r", ""), imageIndex,  menuText?? Regex.Split("\\W", snippet).First(),  toolTipTitle?? "Code snippet:",  toolTipText?? snippet)
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
