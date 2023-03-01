using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Drawing;
using System.IO;
using MiMFa.Service;
using System.Xml;
using System.Globalization;

namespace MiMFa.Controls.WinForm.Editor.Model.Syntax
{
    public class HighlightingPattern
    {
        public static readonly Platform _PlatformType = PlatformType.GetOperationSystemPlatform();
        public static RegexOptions RegexCompiledOption
        {
            get
            {
                if (_PlatformType == Platform.X86)
                    return RegexOptions.Compiled;
                else
                    return RegexOptions.None;
            }
        }

        public string Name { get; set; } = "";
        public Regex Selector { get; set; } = null;
        public TextStyle Style { get; set; } = null;


        //styles
        public static TextStyle StatementStyle { get; set; } = new TextStyle(Brushes.RoyalBlue, null, FontStyle.Regular);
        public static TextStyle CommentStyle { get; set; } = new TextStyle(Brushes.SeaGreen, null, FontStyle.Regular);
        public static TextStyle DescriptionStyle { get; set; } = new TextStyle(Brushes.ForestGreen, null, FontStyle.Regular);
        public static TextStyle NameSpaceStyle { get; set; } = new TextStyle(Brushes.Gray, null, FontStyle.Regular);
        public static TextStyle AttributeStyle { get; set; } = new TextStyle(Brushes.MediumSpringGreen, null, FontStyle.Regular);
        public static TextStyle ClassStyle { get; set; } = new TextStyle(Brushes.MediumTurquoise, null, FontStyle.Regular);
        public static TextStyle FunctionStyle { get; set; } = new TextStyle(Brushes.Goldenrod, null, FontStyle.Regular);
        public static TextStyle VariableStyle { get; set; } = new TextStyle(Brushes.PaleVioletRed, null, FontStyle.Regular);
        public static TextStyle StringStyle { get; set; } = new TextStyle(Brushes.Brown, null, FontStyle.Regular);
        public static TextStyle NumberStyle { get; set; } = new TextStyle(Brushes.RosyBrown, null, FontStyle.Regular);
        public static TextStyle RegionStyle { get; set; } = new TextStyle(Brushes.Gray, null, FontStyle.Regular);
        public static TextStyle KeywordStyle { get; set; } = new TextStyle(Brushes.Orchid, null, FontStyle.Regular);
        public static TextStyle NormalStyle { get; set; } = new TextStyle(null, null, FontStyle.Regular);
        public static TextStyle ErrorStyle { get; set; } = new TextStyle(Brushes.Red,null, FontStyle.Strikeout);
        public static TextStyle GetStyle(string name)
        {
            switch (name.ToLower().Trim())
            {
                case "err":
                case "error":
                    return ErrorStyle;
                case "stmnt":
                case "statement":
                    return StatementStyle;// new TextStyle(Brushes.RoyalBlue, null, FontStyle.Regular);
                case "cmnt":
                case "comment":
                    return CommentStyle;// new TextStyle(Brushes.SeaGreen, null, FontStyle.Regular);
                case "description":
                    return DescriptionStyle;// new TextStyle(Brushes.ForestGreen, null, FontStyle.Regular);
                case "ns":
                case "namespace":
                    return NameSpaceStyle;// new TextStyle(null, null, FontStyle.Regular);
                case "attr":
                case "attribute":
                    return AttributeStyle;// new TextStyle(Brushes.MediumSeaGreen, null, FontStyle.Regular);
                case "cls":
                case "class":
                    return ClassStyle;// new TextStyle(Brushes.LightSeaGreen, null, FontStyle.Regular);
                case "func":
                case "function":
                case "method":
                    return FunctionStyle;// new TextStyle(Brushes.Goldenrod, null, FontStyle.Regular);
                case "var":
                case "variable":
                case "obj":
                case "object":
                    return VariableStyle;// new TextStyle(Brushes.PaleVioletRed, null, FontStyle.Regular);
                case "str":
                case "string":
                    return StringStyle;// new TextStyle(Brushes.Brown, null, FontStyle.Regular);
                case "num":
                case "number":
                    return NumberStyle;// new TextStyle(Brushes.RosyBrown, null, FontStyle.Regular);
                case "reg":
                case "region":
                    return RegionStyle;// new TextStyle(Brushes.Gray, null, FontStyle.Regular);
                case "kw":
                case "keyword":
                    return KeywordStyle;// new TextStyle(Brushes.Orchid, null, FontStyle.Regular)
                default:
                    return NormalStyle;
            }
        }

        public HighlightingPattern() { }
        public HighlightingPattern(XmlNode ruleNode) { From(ruleNode); }
        public HighlightingPattern(string name, Regex selector, TextStyle style)
        {
            Name = name;
            Selector = selector;
            Style = style;
        }
        public HighlightingPattern(Regex selector, TextStyle style)
            : this(selector.ToString(), selector, style) { }
        public HighlightingPattern(string name, string selector, string style, RegexOptions options = RegexOptions.None)
            : this(name, new Regex( selector, options), GetStyle(style)) { }
        public HighlightingPattern(string name, Regex selector, Brush forebrush = null, Brush backbrush = null, FontStyle fontStyle = FontStyle.Regular)
           : this(name, selector, new TextStyle(forebrush, backbrush, fontStyle)) { }
        public HighlightingPattern(Regex selector, Brush forebrush = null, Brush backbrush = null, FontStyle fontStyle = FontStyle.Regular)
            : this(selector.ToString(), selector, forebrush, backbrush, fontStyle) { }
        public HighlightingPattern(string name, string selector, Brush forebrush = null, Brush backbrush = null, RegexOptions options = RegexOptions.None, FontStyle fontStyle = FontStyle.Regular)
            : this(name, new Regex(selector, options), forebrush, backbrush, fontStyle) { }
        public HighlightingPattern(string selector, Brush forebrush = null, Brush backbrush = null, RegexOptions options = RegexOptions.None, FontStyle fontStyle = FontStyle.Regular)
            : this(selector, new Regex(selector, options), forebrush, backbrush, fontStyle) { }

        public override string ToString()
        {
            var dic = new Dictionary<string, string>();
            try
            {
                dic.Add("name", Name);
                dic.Add("pattern",Selector.ToString());
                dic.Add("forebrush", Style.ForeBrush.ToString());
                dic.Add("backbrush", Style.BackgroundBrush.ToString());
                dic.Add("regexoptions", Selector.Options.ToString());
                dic.Add("fontstyle", Style.FontStyle.ToString());
            }
            catch { }
            return ConvertService.ToString(dic, "=>");
        }

        public void From(XmlNode ruleNode)
        {
            XmlAttribute name = ruleNode.Attributes["NAME"];
            XmlAttribute style = ruleNode.Attributes["STYLE"];
            XmlAttribute forebrush = ruleNode.Attributes["FOREBRUSH"];
            XmlAttribute backbrush = ruleNode.Attributes["BACKBRUSH"];
            XmlAttribute fontstyle = ruleNode.Attributes["FONTSTYLE"];
            XmlAttribute options = ruleNode.Attributes["OPTIONS"];

            //Style
            Style = NormalStyle;
            if (name != null) Name = name.Value;
            if (style != null) Style = GetStyle(style.Value);
            if (forebrush != null) Style.ForeBrush = ParseBrush(forebrush.Value);
            if (backbrush != null) Style.BackgroundBrush = ParseBrush(backbrush.Value);
            if (fontstyle != null) Style.FontStyle = HighlightingMap.ParseEnum(fontstyle.Value, FontStyle.Regular);
            //options
            if (options == null)
                Selector = new Regex(ruleNode.InnerText);
            else Selector = new Regex(ruleNode.InnerText, HighlightingMap.ParseEnum(options.Value,RegexOptions.IgnoreCase) | RegexCompiledOption);
        }
        public Style ParseStyle(XmlNode styleNode)
        {
            XmlAttribute typeA = styleNode.Attributes["type"];
            XmlAttribute colorA = styleNode.Attributes["color"];
            XmlAttribute backColorA = styleNode.Attributes["backColor"];
            XmlAttribute fontStyleA = styleNode.Attributes["fontStyle"];
            XmlAttribute nameA = styleNode.Attributes["name"];
            //colors
            SolidBrush foreBrush = null;
            if (colorA != null)
                foreBrush = new SolidBrush(ParseColor(colorA.Value));
            SolidBrush backBrush = null;
            if (backColorA != null)
                backBrush = new SolidBrush(ParseColor(backColorA.Value));
            //fontStyle
            FontStyle fontStyle = FontStyle.Regular;
            if (fontStyleA != null)
                fontStyle = (FontStyle)Enum.Parse(typeof(FontStyle), fontStyleA.Value);

            return new TextStyle(foreBrush, backBrush, fontStyle);
        }
        public Color ParseColor(string value)
        {
            if (value.StartsWith("#"))
            {
                if (value.Length <= 7)
                    return Color.FromArgb(255,
                                          Color.FromArgb(Int32.Parse(value.Substring(1), NumberStyles.AllowHexSpecifier)));
                else
                    return Color.FromArgb(Int32.Parse(value.Substring(1), NumberStyles.AllowHexSpecifier));
            }
            else
                return Color.FromName(value);
        }
        public Brush ParseBrush(string value)
        {
            Type t = typeof(Brushes);
            return (Brush)t.GetProperty(value).GetConstantValue();
        }
    }
}
