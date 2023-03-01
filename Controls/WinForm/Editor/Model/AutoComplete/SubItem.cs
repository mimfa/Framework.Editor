using MiMFa.Controls.WinForm.Editor.Tools;
using System;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text.RegularExpressions;

namespace MiMFa.Controls.WinForm.Editor.Model.AutoComplete
{
    /// <summary>
    /// This autocomplete item appears after dot
    /// </summary>
    public class SubItem : Item
    {
        public bool IsPartitioned = false;
        public char SplitChar = '.';
        protected string FirstPart = "";
        protected string LastPart = "";

        public SubItem(string text, int imageIndex = -1, string menuText = null, string toolTipTitle = null, string toolTipText = null, char splitChar = '.')
        : base(text, imageIndex, menuText, toolTipTitle, toolTipText)
        { 
            SplitChar = splitChar;
            int li = text.LastIndexOf(SplitChar) + 1;
            if (IsPartitioned = li > 0)
            {
                FirstPart = Text.Substring(0, li);
                LastPart = Text.Substring(li).ToLower();
            }
            else 
                FirstPart =
                LastPart = Text.ToLower();
        }

        public override CompareResult Compare(string fragmentText, string normaledText)
        {
            if (!fragmentText.StartsWith(FirstPart))
                if (IsPartitioned) return CompareResult.Hidden;
                else return base.Compare(fragmentText, normaledText);
            CompareResult cr = base.Compare(fragmentText, normaledText);
            if (cr == CompareResult.Hidden)
            {
                string firstPart,lastPart;
                int li = fragmentText.LastIndexOf(SplitChar) + 1;
                if (li > 0)
                {
                    firstPart = fragmentText.Substring(0, li);
                    lastPart = fragmentText.Substring(li).ToLower();
                }
                else
                    firstPart =
                    lastPart = fragmentText.ToLower();
                if (!string.IsNullOrWhiteSpace(lastPart) && firstPart == FirstPart)
                    if (LastPart.StartsWith(lastPart) || LastPart.EndsWith(lastPart))
                        return CompareResult.VisibleAndSelected;
                    else if (LastPart.Contains(lastPart))
                        return CompareResult.Visible;
            }
            return cr;
        }
    }

}
