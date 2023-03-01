using MiMFa.General;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace MiMFa.Controls.WinForm.Editor.Model.Syntax
{
    public class Highlighter : IDisposable
    {
        private Language _Language { get; set; } = Language.Custom;
        public Language Language
        {
            get => _Language;
            set
            {
                _Language = value;
                Init();
            }
        }
        private string _SyntaxXMLFile = "";
        public string MapXMLFile
        {
            get => _SyntaxXMLFile; set
            {
                _SyntaxXMLFile = value;
                Init();
            }
        }

        public HighlightingMap Map = new HighlightingMap();

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion

        public Highlighter():base() { }
        public Highlighter(Language language) :base() { Language =language; }

        public void Init()
        {
            Map = new HighlightingMap(Language,MapXMLFile);
        }

        /// <summary>
        /// Highlights codes
        /// </summary>
        /// <param name="range"></param>
        public virtual void Highlight(Range range)
        {
            if (range==null || !Map.StartHighlighting(range)) return;

            range.tb.CommentPrefix = Map.CommentPrefix;
            range.tb.LeftBracket1 = Map.LeftBracket1;
            range.tb.RightBracket1 = Map.RightBracket1;
            range.tb.LeftBracket2 = Map.LeftBracket2;
            range.tb.RightBracket2 = Map.RightBracket2;
            range.tb.BracketsHighlightStrategy = Map.BlockingStrategy;
            range.tb.AutoIndentPattern = Map.IndentPatterns;

            //clear style of changed range
            range.ClearStyle((from v in Map.HighlightingPatterns select v.Style).ToArray());

            // highlighting
            foreach (var item in Map.HighlightingPatterns)
                range.SetStyle(item.Style, item.Selector);

            //clear folding markers
            range.ClearFoldingMarkers();

            //set folding markers
            foreach (var item in Map.FoldingPatterns)
                range.SetFoldingMarkers(item.StartPattern,item.EndPattern, item.Options);

            Map.EndHighlighting(range);
        }

        public virtual void AutoIndentNeeded(object sender, AutoIndentEventArgs args)
        {
            //var tb = (sender as EditBox);
            //Language language = tb.Language;
            switch (Map.Language)
            {
                case Language.CS:
                    HighlightingMap.CSharpAutoIndentNeeded(sender, args);
                    break;
                case Language.VB:
                    HighlightingMap.VBAutoIndentNeeded(sender, args);
                    break;
                case Language.HTML:
                    HighlightingMap.HTMLAutoIndentNeeded(sender, args);
                    break;
                case Language.XML:
                    HighlightingMap.XMLAutoIndentNeeded(sender, args);
                    break;
                case Language.SQL:
                    HighlightingMap.SQLAutoIndentNeeded(sender, args);
                    break;
                case Language.PHP:
                    HighlightingMap.PHPAutoIndentNeeded(sender, args);
                    break;
                case Language.JS:
                    HighlightingMap.CSharpAutoIndentNeeded(sender, args);
                    break; //JS like C#
                case Language.Lua:
                    HighlightingMap.LuaAutoIndentNeeded(sender, args);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Language
    /// </summary>
    public enum Language
    {
        Custom,
        CS,
        VB,
        HTML,
        XML,
        SQL,
        PHP,
        JS,
        Lua,
        JSON
    }
}