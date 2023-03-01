using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Drawing;
using System.IO;
using MiMFa.Service;
using System.Linq;
using System.Xml;

namespace MiMFa.Controls.WinForm.Editor.Model.Syntax
{
    public class FoldingPattern
    {
        public string StartPattern { get; set; } = null;
        public string EndPattern { get; set; } = null;
        public RegexOptions Options { get; set; } = RegexOptions.IgnoreCase;

        public FoldingPattern() { }
        public FoldingPattern(XmlNode foldingNode) { From(foldingNode); }
        public FoldingPattern(string startPattern, string endPattern, RegexOptions options  = RegexOptions.IgnoreCase)
        {
            StartPattern = startPattern;
            EndPattern = endPattern;
            Options = options;
        }
        public FoldingPattern(RegexOptions options, string startPattern, string endPattern)
        : this(startPattern, endPattern, options)
        { }
        public FoldingPattern(RegexOptions options, string[] patterns)
        : this(patterns.First(), patterns.Last(), options)
        { }

        public override string ToString()
        {
            var dic = new Dictionary<string, string>();
            try
            {
                dic.Add("startpattern",StartPattern);
                dic.Add("endpattern", EndPattern);
                dic.Add("regexoptions", Options.ToString());
            }
            catch { }
            return ConvertService.ToString(dic, "=>");
        }

        public void From(XmlNode foldingNode)
        {
            if (foldingNode.Attributes["START"] != null)
                StartPattern = foldingNode.Attributes["START"].Value;
            if (foldingNode.Attributes["FINISH"] != null)
                EndPattern = foldingNode.Attributes["FINISH"].Value;
            if (foldingNode.Attributes["OPTIONS"] != null)
                Options = HighlightingMap.ParseEnum(foldingNode.Attributes["OPTIONS"].Value,RegexOptions.None);
        }


    }
}
