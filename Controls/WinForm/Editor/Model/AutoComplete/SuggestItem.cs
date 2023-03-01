using MiMFa.Controls.WinForm.Editor.Tools;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text.RegularExpressions;

namespace MiMFa.Controls.WinForm.Editor.Model.AutoComplete
{
    /// <summary>
    /// This Item does not check correspondence to current text fragment.
    /// SuggestItem is intended for dynamic menus.
    /// </summary>
    public class SuggestItem : Item
    {
        public SuggestItem(string text, int imageIndex = -1, string menuText = null, string toolTipTitle = null, string toolTipText = null)
        : base(text, imageIndex, menuText, toolTipTitle, toolTipText) { }

        public override CompareResult Compare(string fragmentText, string normaledText)
        {
            return CompareResult.Visible;
        }
    }

}
