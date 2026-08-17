using System.Collections.Generic;
using System.Text.RegularExpressions;
using System;
using System.Xml;
using System.IO;
using MiMFa.Service;

namespace MiMFa.Controls.WinForm.Editor.Model.Syntax
{
    public class HighlightingMap
    {
        public static string FirstBreaker = "(?<=[^\\.]\\b|^)";
        public static string LastBreaker = "\\b";

        public string CommentPrefix = "//";
        public char LeftBracket1 = '(';
        public char RightBracket1 = ')';
        public char LeftBracket2 = '{';
        public char RightBracket2 = '}';
        public BlockStrategy BlockingStrategy = BlockStrategy.Modern;
        public string IndentPatterns = @"
^\s*[\w\.]+(\s\w+)?\s*(?<range>=)\s*(?<range>[^;=]+);
^\s*(case|default)\s*[^:]*(?<range>:)\s*(?<range>[^;]+);
";
        public List<FoldingPattern> FoldingPatterns = new List<FoldingPattern>();
        public List<HighlightingPattern> HighlightingPatterns = new List<HighlightingPattern>();
        public Func<Range, bool> StartHighlighting = r => true;
        public Func<Range, bool> EndHighlighting = r => true;

        public Language Language = Language.Custom;

        public HighlightingMap() { }
        public HighlightingMap(HighlightingMap map)
        {
            From(map);
        }
        public HighlightingMap(Language language, string path = null)
        {
            From(language,path);
        }
        public HighlightingMap(string path)
        {
            From(path);
        }

        public void From(HighlightingMap map)
        {
            CommentPrefix = map.CommentPrefix;
            LeftBracket1 = map.LeftBracket1;
            RightBracket1 = map.RightBracket1;
            LeftBracket2 = map.LeftBracket2;
            RightBracket2 = map.RightBracket2;
            BlockingStrategy = map.BlockingStrategy;
            IndentPatterns = map.IndentPatterns;
            FoldingPatterns = map.FoldingPatterns;
            HighlightingPatterns = map.HighlightingPatterns;
            StartHighlighting = map.StartHighlighting;
            EndHighlighting = map.EndHighlighting;
        }
        public void From(Language language, string path = null) 
        {
            From(GetSyntax(Language = language, path));
        }
        public void From(string path)
        {
            if (!File.Exists(path)) return;
            var doc = new XmlDocument();
            doc.Load(path);
            From(doc);
        }
        public void From(XmlDocument doc)
        {
            XmlNode child;
            child = doc.SelectSingleNode("DOC/BASE");
            if (child != null && child.Attributes["NAME"] != null)
                From(GetSyntax(child.Attributes["NAME"].Value));
            child = doc.SelectSingleNode("DOC/INDENTING");
            if (child != null)
            {
                if (child.Attributes["LEFT"] != null)
                    LeftBracket1 = child.Attributes["LEFT"].Value[0];
                if (child.Attributes["LEFT1"] != null)
                    LeftBracket1 = child.Attributes["LEFT1"].Value[0];
                if (child.Attributes["RIGHT"] != null)
                    RightBracket1 = child.Attributes["RIGHT"].Value[0];
                if (child.Attributes["RIGHT1"] != null)
                    RightBracket1 = child.Attributes["RIGHT1"].Value[0];

                if (child.Attributes["LEFT2"] != null)
                    LeftBracket2 = child.Attributes["LEFT2"].Value[0];
                if (child.Attributes["RIGHT2"] != null)
                    RightBracket2 = child.Attributes["RIGHT2"].Value[0];

                if (child.Attributes["STRATEGY"] != null)
                    BlockingStrategy = (BlockStrategy)Enum.Parse(typeof(BlockStrategy), child.Attributes["strategy"].Value);

                IndentPatterns = child.Value;
            }
            child = doc.SelectSingleNode("DOC/HIGHLIGHTING");
            if(child != null)
                foreach (XmlNode rule in child.ChildNodes)
                    HighlightingPatterns.Insert(GetOrder(rule, HighlightingPatterns.Count),new HighlightingPattern(rule));
            child = doc.SelectSingleNode("DOC/FOLDING");
            if(child != null)
                foreach (XmlNode folding in child.ChildNodes)
                    FoldingPatterns.Insert(GetOrder(folding, FoldingPatterns.Count), new FoldingPattern(folding));
        }

        public static T ParseEnum<T>(string value, T dafVal) where T : Enum
        {
            T result = dafVal;
            bool started = false;
            foreach (var item in value.Split(' ', ',', '|', ';', '.', '\\', '/'))
                if (string.IsNullOrWhiteSpace(item)) continue;
                else if (started) result |= (dynamic)Enum.Parse(typeof(T), item, true);
                else
                {
                    result = (T)Enum.Parse(typeof(T), item, true);
                    started = true;
                }
            return result;
        }


        private int GetOrder(XmlNode rule, int max)
        {
            if(rule.Attributes["ORDER"] == null) return max;
            int num = ConvertService.TryToInt(rule.Attributes["ORDER"].Value,int.MinValue);
            if (num < 0 && max + num > -1) num = max + num;
            return Math.Max(num, max);
        }
        public HighlightingMap GetSyntax(string name, string customPath = null)
        {
            return GetSyntax(ParseEnum(name, customPath == null? Language.JS: Language.Custom), customPath); 
        }
        public HighlightingMap GetSyntax(Language language, string customPath = null)
        {
            switch (Language = language)
            {
                case Language.CS:
                   return CSharpSyntax();
                case Language.VB:
                    return VBSyntax();
                case Language.HTML:
                    return HTMLSyntax();
                case Language.XML:
                    return XMLSyntax();
                case Language.XPath:
                    return XPathSyntax();
                case Language.CSS:
                    return CSSSyntax();
                case Language.JS:
                    return JSSyntax();
                case Language.LUA:
                    return LUASyntax();
                case Language.PHP:
                    return PHPSyntax();
                case Language.SQL:
                    return SQLSyntax();
                case Language.JSON:
                    return LUASyntax();
                default:
                    return CustomSyntax(customPath);
            }
        }

        public static HighlightingMap CustomSyntax(string path)
        {
            return new HighlightingMap(path);
        }
        /// <summary>
        /// Highlights C# code
        /// </summary>
        public static HighlightingMap CSharpSyntax()
        {
            HighlightingMap Syntax = new HighlightingMap();
            Syntax.CommentPrefix = "//";
            Syntax.LeftBracket1 = '(';
            Syntax.RightBracket1 = ')';
            Syntax.LeftBracket2 = '{';
            Syntax.RightBracket2 = '}';
            Syntax.BlockingStrategy = BlockStrategy.Modern;
            Syntax.IndentPatterns = @"
^\s*[\w\.]+(\s\w+)?\s*(?<range>=)\s*(?<range>[^;=]+);
^\s*(case|default)\s*[^:]*(?<range>:)\s*(?<range>[^;]+);
";

            //set folding markers
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, new string[] { "{", "}" }));
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, new string[] { @"#region\b", @"#endregion\b" }));
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, new string[] { @"/\*", @"\*/" }));
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.ExplicitCapture | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace, new string[] { "@\\\"", "\\\"" }));

            // Keywords
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpKeyword",
                new Regex(@"\b(abstract|add|alias|as|ascending|async|await|base|bool|break|by|byte|case|catch|char|checked|class|const|continue|decimal|default|delegate|descending|do|double|dynamic|else|enum|equals|event|explicit|extern|false|finally|fixed|float|for|foreach|from|get|global|goto|group|if|implicit|in|int|interface|internal|into|is|join|let|lock|long|nameof|namespace|new|null|object|on|operator|orderby|out|override|params|partial|private|protected|public|readonly|ref|remove|return|sbyte|sealed|select|set|short|sizeof|stackalloc|static|string|struct|switch|this|throw|true|try|typeof|uint|ulong|unchecked|unsafe|ushort|using|value|var|virtual|void|volatile|when|where|while|yield)\b",
                RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );

            // Strings
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpString",
                new Regex(@"@""[^""]*""|""(?:[^""\\]

|\\.)*""", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StringStyle)
            );

            // Numbers
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpNumber",
                new Regex(@"\b\d+(\.\d+)?([eE][\-+]?\d+)?\b|\b0x[a-fA-F\d]+\b", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NumberStyle)
            );

            // Single-line Comments
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpComment",
                new Regex(@"\/\/.*$", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );

            // Multi-line Comments
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpMultiLineComment",
                new Regex(@"/\*[\s\S]*?\*/", RegexOptions.Singleline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );

            // XML Documentation Comments
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpXmlComment",
                new Regex(@"^\s*///.*$", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.DescriptionStyle)
            );

            // Attributes
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpAttribute",
                new Regex(@"\n\[.*?\]\n", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.AttributeStyle)
            );

            // Classes, Structs, Enums, Interfaces
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpClass",
                new Regex(@"\b(class|struct|enum|interface)\s+(\w+)\b", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.ClassStyle)
            );

            // Methods
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpFunction",
                new Regex(@"\b(\w+)\s*(?=\()", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.FunctionStyle)
            );

            // Preprocessor Directives
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpPreprocessor",
                new Regex(@"^\s*#(?:define|undef|if|elif|else|endif|line|error|warning|region|endregion|pragma|include)\b.*$", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.KeywordStyle)
            );


            HTMLSyntax();
            return Syntax;
        }
        /// <summary>
        /// Highlights VisualBasic code
        /// </summary>
        public static HighlightingMap VBSyntax()
        {
            HighlightingMap Syntax = new HighlightingMap();
            Syntax.CommentPrefix = "'";
            Syntax.LeftBracket1 = '(';
            Syntax.RightBracket1 = ')';
            Syntax.LeftBracket2 = '\x0';
            Syntax.RightBracket2 = '\x0';
            Syntax.BlockingStrategy = BlockStrategy.Classic;
            Syntax.IndentPatterns = @"
^\s*[\w\.\(\)]+\s*(?<range>=)\s*(?<range>.+)
";

            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, new string[] { @"#Region\b", @"#End\s+Region\b" }));
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, new string[] { @"\b(Class|Property|Enum|Structure|Interface)[ \t]+\S+", @"\bEnd (Class|Property|Enum|Structure|Interface)\b" }));
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.Multiline | RegexOptions.IgnoreCase, new string[] { @"^\s*(?<range>While)[ \t]+\S+", @"^\s*(?<range>End While)\b" }));
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, new string[] { @"\b(Sub|Function)[ \t]+[^\s']+", @"\bEnd (Sub|Function)\b" }));
            //this declared separately because Sub and Function can be unclosed
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, new string[] { @"(\r|\n|^)[ \t]*(?<range>Get|Set)[ \t]*(\r|\n|$)", @"\bEnd (Get|Set)\b" }));
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.Multiline | RegexOptions.IgnoreCase, new string[] { @"^\s*(?<range>For|For\s+Each)\b", @"^\s*(?<range>Next)\b" }));
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.Multiline | RegexOptions.IgnoreCase, new string[] { @"^\s*(?<range>Do)\b", @"^\s*(?<range>Loop)\b" }));
            // Keywords
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("VBKeyword",
                new Regex(@"\b(AddHandler|AddressOf|Alias|And|AndAlso|As|Boolean|ByRef|Byte|ByVal|Call|Case|Catch|CBool|CByte|CChar|CDate|CDbl|CDec|Char|CInt|Class|CLng|CObj|Const|Continue|CSByte|CShort|CSng|CStr|CType|CUInt|CULng|CUShort|Date|Decimal|Declare|Default|Delegate|Dim|DirectCast|Do|Double|Each|Else|ElseIf|End|EndIf|Enum|Erase|Error|Event|Exit|False|Finally|For|Friend|Function|Get|GetType|GetXMLNamespace|Global|GoSub|GoTo|Handles|If|Implements|Imports|In|Inherits|Integer|Interface|Is|IsNot|Let|Lib|Like|Long|Loop|Me|Mod|Module|MustInherit|MustOverride|MyBase|MyClass|Namespace|Narrowing|New|Next|Not|Nothing|NotInheritable|NotOverridable|Object|Of|On|Operator|Option|Optional|Or|OrElse|Overloads|Overridable|Overrides|ParamArray|Partial|Private|Property|Protected|Public|RaiseEvent|ReadOnly|ReDim|REM|RemoveHandler|Resume|Return|SByte|Select|Set|Shadows|Shared|Short|Single|Static|Step|Stop|String|Structure|Sub|SyncLock|Then|Throw|To|True|Try|TryCast|TypeOf|UInteger|ULong|UShort|Using|Variant|Wend|When|While|Widening|With|WithEvents|WriteOnly|Xor|Region)\b",
                RegexOptions.IgnoreCase | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );

            // Strings
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("VBString",
                new Regex(@"(""([^""\\]*(\\.[^""\\]*)*)"")|(""([^""]*)"")", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StringStyle)
            );

            // Numbers
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("VBNumber",
                new Regex(@"\b\d+(\.\d+)?([eE][\-+]?\d+)?\b", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NumberStyle)
            );

            // Single-line Comments
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("VBComment",
                new Regex(@"'.*$", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );

            // Functions
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("VBFunction",
                new Regex(@"\b\w+\s*(?=\()", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.FunctionStyle)
            );

            // Classes, Structures, Enums, Interfaces
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("VBClass",
                new Regex(@"\b(Class|Structure|Enum|Interface)\s+(\w+)", RegexOptions.IgnoreCase | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.ClassStyle)
            );

            // Regions
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("VBRegion",
                new Regex(@"#(Const|Else|ElseIf|End|If|Region|End Region)\b", RegexOptions.IgnoreCase | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.RegionStyle)
            );

            // XML Documentation Comments
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("VBXmlComment",
                new Regex(@"^\s*'''.*$", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.DescriptionStyle)
            );

            return Syntax;
        }
        /// <summary>
        /// Highlights SQL code
        /// </summary>
        public static HighlightingMap SQLSyntax()
        {
            HighlightingMap Syntax = new HighlightingMap();
            Syntax.CommentPrefix = "--";
            Syntax.LeftBracket1 = '(';
            Syntax.RightBracket1 = ')';
            Syntax.LeftBracket2 = '\x0';
            Syntax.RightBracket2 = '\x0';
            Syntax.IndentPatterns = @"";
            Syntax.BlockingStrategy = BlockStrategy.Classic;
            // Numbers
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("SQLNumber",
                new Regex(@"\b\d+(\.\d+)?([eE][\-+]?\d+)?\b", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NumberStyle)
            );

            // Functions
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("SQLFunctions",
                new Regex(@"(\b@@\w+\b|\b(ABS|ACOS|APP_NAME|ASCII|ASIN|ASSEMBLYPROPERTY|ASYMKEY_ID|ASYMKEYPROPERTY|ATAN|ATN2|AVG|CASE|CAST|CEILING|CERTPROPERTY|CHAR|CHARINDEX|CHECKSUM_AGG|COALESCE|COL_LENGTH|COL_NAME|COLLATIONPROPERTY|COLUMNPROPERTY|CONTAINSTABLE|CONVERT|COS|COT|COUNT|COUNT_BIG|CURRENT_TIMESTAMP|CURRENT_USER|CURSOR_STATUS|DATABASE_PRINCIPAL_ID|DATABASEPROPERTYEX|DATALENGTH|DATEADD|DATEDIFF|DATENAME|DATEPART|DAY|DB_ID|DB_NAME|DECRYPTBYASYMKEY|DECRYPTBYCERT|DECRYPTBYKEY|DECRYPTBYPASSPHRASE|DEGREES|DENSE_RANK|DIFFERENCE|ENCRYPTBYASYMKEY|ENCRYPTBYCERT|ENCRYPTBYKEY|ENCRYPTBYPASSPHRASE|ERROR_LINE|ERROR_MESSAGE|ERROR_NUMBER|ERROR_PROCEDURE|ERROR_SEVERITY|ERROR_STATE|EXP|FILE_ID|FILEGROUP_ID|FILEGROUP_NAME|FILEPROPERTY|FLOOR|FORMATMESSAGE|FREETEXTTABLE|FULLTEXTCATALOGPROPERTY|FULLTEXTSERVICEPROPERTY|GETDATE|GETUTCDATE|GROUPING|HAS_PERMS_BY_NAME|HOST_ID|HOST_NAME|IDENT_CURRENT|IDENT_INCR|IDENT_SEED|IDENTITY\(|INDEX_COL|INDEXKEY_PROPERTY|INDEXPROPERTY|IS_MEMBER|ISDATE|ISNULL|ISNUMERIC|KEY_GUID|KEY_ID|KEY_NAME|LEFT|LEN|LOG|LOG10|LOWER|LTRIM|MAX|MIN|MONTH|NEWID|NTILE|NULLIF|OBJECT_DEFINITION|OBJECT_ID|OBJECT_NAME|OBJECT_SCHEMA_NAME|OBJECTPROPERTY|OBJECTPROPERTYEX|OPENDATASOURCE|OPENQUERY|OPENROWSET|OPENXML|PARSENAME|PATINDEX|PERMISSIONS|PI|POWER|RADIANS|RAND|RANK|REPLICATE|REVERSE|RIGHT|ROUND|ROW_NUMBER|RTRIM|SCHEMA_ID|SCHEMA_NAME|SCOPE_IDENTITY|SERVERPROPERTY|SESSION_USER|SIGN|SIN|SOUNDEX|SPACE|SQRT|STATS_DATE|STDEV|STDEVP|STR|STUFF|SUBSTRING|SUM|SUSER_ID|SUSER_NAME|SYSTEM_USER|TAN|TEXTPTR|TEXTVALID|TRIGGER_NESTLEVEL|TYPE_ID|TYPE_NAME|TYPEPROPERTY|UNICODE|UPPER|USER_ID|USER_NAME|VAR|VARP|YEAR)\b)",
                RegexOptions.IgnoreCase | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.FunctionStyle)
            );

            // Variables
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("SQLVar",
                new Regex(@"@\w+\b", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.VariableStyle)
            );

            // Statements
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("SQLStatements",
                new Regex(@"\b(ALTER|CREATE|DELETE|DISABLE|DROP|ENABLE|EXEC|EXECUTE|FROM|INSERT|MERGE|OPTION|OUTPUT|SELECT|TOP|TRUNCATE|UPDATE|WITH|SET|BEGIN|END|IF|ELSE|WHILE|RETURN|PRINT|GO|DECLARE|FETCH|GRANT|REVOKE|OPEN|CLOSE|DEALLOCATE|USE|ROLLBACK|COMMIT|SAVE|TRAN|TRANSACTION|WAITFOR|RAISERROR|TRY|CATCH)\b",
                RegexOptions.IgnoreCase | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );

            // Keywords
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("SQLKeywords",
                new Regex(@"\b(ADD|ALL|AND|ANY|AS|ASC|AUTHORIZATION|BACKUP|BETWEEN|BREAK|BROWSE|BY|CASCADE|CHECK|CHECKPOINT|CLOSE|CLUSTERED|COLLATE|COLUMN|COMMIT|COMPUTE|CONSTRAINT|CONTAINS|CONTINUE|CROSS|CURRENT|CURSOR|DATABASE|DBCC|DEALLOCATE|DEFAULT|DENY|DESC|DISK|DISTINCT|DISTRIBUTED|DOUBLE|DUMP|ELSE|ERRLVL|ESCAPE|EXCEPT|EXISTS|EXIT|EXTERNAL|FETCH|FILE|FILLFACTOR|FOR|FOREIGN|FREETEXT|FULL|FUNCTION|GOTO|GRANT|GROUP|HAVING|HOLDLOCK|IDENTITY|IDENTITY_INSERT|IDENTITYCOL|IF|INDEX|INNER|INTERSECT|JOIN|KEY|KILL|LIKE|LINENO|LOAD|NATIONAL|NOCHECK|NONCLUSTERED|NOT|NULL|OF|OFF|OFFSETS|ON|OPEN|OR|ORDER|OUTER|OVER|PERCENT|PIVOT|PLAN|PRECISION|PRIMARY|PRINT|PROC|PROCEDURE|PUBLIC|READ|READTEXT|RECONFIGURE|REFERENCES|REPLICATION|RESTORE|RESTRICT|RETURN|REVERT|REVOKE|ROLLBACK|ROWCOUNT|ROWGUIDCOL|RULE|SAVE|SCHEMA|SECURITYAUDIT|SHUTDOWN|SOME|STATISTICS|TABLE|TABLESAMPLE|TEXTSIZE|THEN|TO|TSEQUAL|UNION|UNIQUE|UNPIVOT|UPDATETEXT|USE|USER|VALUES|VARYING|VIEW|WAITFOR|WHEN|WRITETEXT|OFFSET)\b",
                RegexOptions.IgnoreCase | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );

            // Data Types
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("SQLTypes",
                new Regex(@"\b(BIGINT|NUMERIC|BIT|SMALLINT|DECIMAL|SMALLMONEY|INT|TINYINT|MONEY|FLOAT|REAL|DATE|DATETIMEOFFSET|DATETIME2|SMALLDATETIME|DATETIME|TIME|CHAR|VARCHAR|TEXT|NCHAR|NVARCHAR|NTEXT|BINARY|VARBINARY|IMAGE|TIMESTAMP|HIERARCHYID|TABLE|UNIQUEIDENTIFIER|SQL_VARIANT|XML)\b",
                RegexOptions.IgnoreCase | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.AttributeStyle)
            );

            // Single-line Comments
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("SQLComment0",
                new Regex(@"--.*$", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.DescriptionStyle)
            );

            // Multi-line Comments
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("SQLComment1",
                new Regex(@"/\*[\s\S]*?\*/", RegexOptions.Singleline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );

            // Hash Comments
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("SQLComment3",
                new Regex(@"#.*$", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.DescriptionStyle)
            );

            // Strings
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("SQLString",
                new Regex(@"(""[^""]*""|'[^']*')", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StringStyle)
            );

            //set folding markers
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, @"\bBEGIN\b", @"\bEND\b"));
            //allow to collapse BEGIN..END blocks
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, @"/\*", @"\*/")); //allow to collapse comment block
            return Syntax;
        }
        /// <summary>
        /// Highlights PHP code
        /// </summary>
        public static HighlightingMap PHPSyntax()
        {
            HighlightingMap Syntax = new HighlightingMap();
            Syntax.CommentPrefix = "//";
            Syntax.LeftBracket1 = '(';
            Syntax.RightBracket1 = ')';
            Syntax.LeftBracket2 = '{';
            Syntax.RightBracket2 = '}';
            Syntax.BlockingStrategy = BlockStrategy.Modern;
            Syntax.IndentPatterns = @"
^\s*[\w\.]+(\s\w+)?\s*(?<range>=)\s*(?<range>[^;=]+);
^\s*(case|default)\s*[^:]*(?<range>:)\s*(?<range>[^;]+);
";

            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, "{", "}")); //allow to collapse brackets block
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, @"/\*", @"\*/")); //allow to collapse comment block
                                                                                                     // Functions
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPFunction",
                new Regex(@"\b\w+\s*(?=\()", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.FunctionStyle)
            );

            // Classes
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPClass",
                new Regex(@"\b(class)\s+(\w+)", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.ClassStyle)
            );

            // Variables
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPVar",
                new Regex(@"\$\w+\b", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.VariableStyle)
            );

            // Keywords
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPKeyword",
                new Regex(@"\b(die|echo|empty|exit|eval|include|include_once|isset|list|require|require_once|return|print|unset)\b",
                HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.KeywordStyle)
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPConstant",
                new Regex(@"__CLASS__|__DIR__|__FILE__|__LINE__|__FUNCTION__|__METHOD__|__NAMESPACE__",
                HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.AttributeStyle)
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPStatement",
                new Regex(@"\b(abstract|and|array|as|break|case|catch|cfunction|class|clone|const|continue|declare|default|do|else|elseif|enddeclare|endfor|endforeach|endif|endswitch|endwhile|extends|final|for|foreach|function|global|goto|if|implements|instanceof|interface|namespace|new|or|private|protected|public|static|switch|throw|try|use|var|while|xor)\b",
                HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );

            // Numbers
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPNumber",
                new Regex(@"\b\d+(\.\d+)?\b", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NumberStyle)
            );

            // Strings
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPString",
                new Regex(@"(['""]).*?\1|`[^`]*`", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StringStyle)
            );

            // Single-line Comments
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPComment0",
                new Regex(@"(//|#).*?$", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );

            // Multi-line Comments
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPComment1",
                new Regex(@"/\*[\s\S]*?\*/", RegexOptions.Singleline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPComment2",
                new Regex(@"/\*[\s\S]*?\*/|.*\*/", RegexOptions.Singleline | RegexOptions.RightToLeft | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );

            return Syntax;
        }
        /// <summary>
        /// Highlights JavaScrip code
        /// </summary>
        public static HighlightingMap JSSyntax()
        {
            HighlightingMap Syntax = new HighlightingMap();
            Syntax.CommentPrefix = "//";
            Syntax.LeftBracket1 = '(';
            Syntax.RightBracket1 = ')';
            Syntax.LeftBracket2 = '{';
            Syntax.RightBracket2 = '}';
            Syntax.BlockingStrategy = BlockStrategy.Modern;
            Syntax.IndentPatterns = @"
^\s*[\w\.]+(\s\w+)?\s*(?<range>=)\s*(?<range>[^;=]+);
^\s*(case|default)\s*[^:]*(?<range>:)\s*(?<range>[^;]+);
";

            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.None, "{", "}")); //allow to collapse brackets block
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.None, @"/\*", @"\*/")); //allow to collapse comment block

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptKeyword",
                new Regex(@"\b(arguments|public|protected|private|var|debugger|static|true|false|break|case|catch|continue|class|default|delete|do|else|export|for|function|if|in|of|async|await|instanceof|import|new|null|return|switch|this|throw|try|finally|var|void|while|with|typeof|yield|let|const|NaN|undefined)\b",
                HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );

            // Numbers
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptNumber",
                new Regex(@"\b\d+(\.\d+)?([eE][\-+]?\d+)?\b", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NumberStyle)
            );

            // Functions
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptFunction",
                new Regex(@"\b\w+\s*(?=\()", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.FunctionStyle)
            );

            // Keywords
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptKeyword",
                new Regex(@"\b(arguments|public|protected|private|var|debugger|static|true|false|break|case|catch|continue|class|default|delete|do|else|export|for|function|if|in|of|async|await|instanceof|import|new|null|return|switch|this|throw|try|finally|var|void|while|with|typeof|yield|let|const|NaN|undefined)\b", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.KeywordStyle)
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptClass",
                new Regex(@"\b(class|interface|abstract)\s+(\w+)", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptNameSpace",
                new Regex(@"\b([\w\$]+\.)+(?=\w)", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NameSpaceStyle)
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptClass",
                new Regex(@"\b(Object|JSON|Date|Array|Math|RegExp)\b", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.ClassStyle)
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptNumber",
                new Regex(@"\b\d+(\.\d+)?([eE][\-+]?\d+)?\b|\b0x[a-fA-F\d]+\b", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NumberStyle)
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptString0",
                new Regex(@"(['""])((?:\\.|(?!\1)[^\\])*)\1|`((?:\\.|[^\\`])*)`", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StringStyle)
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptComment0",
                new Regex(@"\/\/.*$", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptComment1",
                new Regex(@"/\*[\s\S]*?\*/", RegexOptions.Singleline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptComment2",
                new Regex(@"/\*[\s\S]*?\*/|.*\*/", RegexOptions.Singleline | RegexOptions.RightToLeft | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );

            return Syntax;
        }
        /// <summary>
        /// Highlights Lua code
        /// </summary>
        public static HighlightingMap LUASyntax()
        {
            HighlightingMap Syntax = new HighlightingMap();
            Syntax.CommentPrefix = "--";
            Syntax.LeftBracket1 = '(';
            Syntax.RightBracket1 = ')';
            Syntax.LeftBracket2 = '{';
            Syntax.RightBracket2 = '}';
            Syntax.IndentPatterns = @"
^\s*[\w\.]+(\s\w+)?\s*(?<range>=)\s*(?<range>.+)
";
            Syntax.BlockingStrategy = BlockStrategy.Modern;

            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, "{", "}")); //allow to collapse brackets block
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, @"--\[\[", @"\]\]")); //allow to collapse comment block
                                                                                                         // Functions
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("LuaFunctions",
                new Regex(@"\b(assert|collectgarbage|dofile|error|getfenv|getmetatable|ipairs|load|loadfile|loadstring|module|next|pairs|pcall|print|rawequal|rawget|rawset|require|select|setfenv|setmetatable|tonumber|tostring|type|unpack|xpcall)\b",
                HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.FunctionStyle)
            );

            // Keywords
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("LuaKeyword",
                new Regex(@"\b(and|break|do|else|elseif|end|false|for|function|if|in|local|nil|not|or|repeat|return|then|true|until|while)\b",
                HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );

            // Numbers
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("LuaNumber",
                new Regex(@"\b\d+(\.\d+)?([eE][\-+]?\d+)?\b|\b0x[a-fA-F\d]+\b", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NumberStyle)
            );

            // Single-line Comments
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("LuaComment0",
                new Regex(@"--.*$", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );

            // Multi-line Comments
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("LuaComment1",
                new Regex(@"--

\[

\[.*?\]

\]

--", RegexOptions.Singleline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("LuaComment2",
                new Regex(@"--

\[

\[.*?\]

\]

|.*\]

\]

", RegexOptions.Singleline | RegexOptions.RightToLeft | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );

            // Strings
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("LuaString",
                new Regex(@"(['""]).*?\1|

\[

\[.*?\]

\]

", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StringStyle)
            );

            return Syntax;
        }
        /// <summary>
        /// Highlights JSON code
        /// </summary>
        public static HighlightingMap JSONSyntax()
        {
            HighlightingMap Syntax = new HighlightingMap();
            Syntax.LeftBracket1 = '[';
            Syntax.RightBracket1 = ']';
            Syntax.LeftBracket2 = '{';
            Syntax.RightBracket2 = '}';
            Syntax.BlockingStrategy = BlockStrategy.Modern;
            Syntax.IndentPatterns = @"
^\s*[\w\.]+(\s\w+)?\s*(?<range>=)\s*(?<range>[^;]+);
";

            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, "{", "}")); //allow to collapse brackets block
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, @"\[", @"\]")); //allow to collapse comment block
                                                                                                   // Keys
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JSONKeyword",
                new Regex(@"""([^""\\]*(?:\\.[^""\\]*)*)""\s*:", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.VariableStyle)
            );

            // Values (Numbers, true, false, null)
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JSONNumber",
                new Regex(@"\b(\d+(\.\d+)?|true|false|null)\b", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NumberStyle)
            );

            // Strings
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JSONString",
                new Regex(@"""([^""\\]*(?:\\.[^""\\]*)*)""", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StringStyle)
            );

            return Syntax;
        }
        public static HighlightingMap HTMLSyntax()
        {
            HighlightingMap Syntax = new HighlightingMap();
            Syntax.CommentPrefix = null;
            Syntax.LeftBracket1 = '<';
            Syntax.RightBracket1 = '>';
            Syntax.LeftBracket2 = '(';
            Syntax.RightBracket2 = ')';
            Syntax.IndentPatterns = @"";
            Syntax.BlockingStrategy = BlockStrategy.Classic;

            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, new string[] { "<head", "</head>" }));
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, new string[] { "<body", "</body>" }));
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, new string[] { "<table", "</table>" }));
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, new string[] { "<tr", "</tr>" }));
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, new string[] { "<td", "</td>" }));
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, new string[] { "<p", "</p>" }));
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, new string[] { "<form", "</form>" }));
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, new string[] { "<div", "</div>" }));
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, new string[] { "<style", "</style>" }));
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, new string[] { "<script", "</script>" }));
            // Tags
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("HTMLTag",
                new Regex(@"<|/>|</|>", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );

            // Tag Names
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("HTMLTagName",
                new Regex(@"<\s*(?<range>[!\w:]+)", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );

            // End Tags
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("HTMLEndTag",
                new Regex(@"</(?<range>[\w:]+)>", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );

            // Attribute Names
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("HTMLAttributeName",
                new Regex(@"(?<range>[\w\d\-]+)=['""][^'""]*['""]|(?<range>[\w\d\-]+)=[\w\d\-]+", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.KeywordStyle)
            );

            // Attribute Values
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("HTMLAttributeValue",
                new Regex(@"[\w\d\-]+=('[^']*'|""[^""]*"")", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StringStyle)
            );

            // Tag Content
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("HTMLTagContent",
                new Regex(@"<[^>]+>", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NumberStyle)
            );

            // Comments
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("HTMLComment0",
                new Regex(@"<!--.*?-->", RegexOptions.Singleline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("HTMLComment1",
                new Regex(@"<!--.*?-->|.*-->", RegexOptions.Singleline | RegexOptions.RightToLeft | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );

            // Entities
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("HTMLEntity",
                new Regex(@"\&(amp|gt|lt|nbsp|quot|apos|copy|reg|#[0-9]{1,8}|#x[0-9a-f]{1,8});", RegexOptions.IgnoreCase | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.VariableStyle)
            );

            return Syntax;
        }
        public static HighlightingMap CSSSyntax()
        {
            HighlightingMap Syntax = new HighlightingMap();
            Syntax.LeftBracket1 = '{';
            Syntax.RightBracket1 = '}';
            Syntax.LeftBracket2 = '[';
            Syntax.RightBracket2 = ']';
            Syntax.BlockingStrategy = BlockStrategy.Modern;
            Syntax.IndentPatterns = @"
^\s*[\w\.]+(\s\w+)?\s*(?<range>=)\s*(?<range>[^;]+);
";

            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, "{", "}")); // Allow collapsing brackets block
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, @"

\[", @"\]

")); // Allow collapsing comment block

            // Selectors
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSSSelector",
                new Regex(@"^[^\s\{\}]+(?=\s*\{)", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NormalStyle)
            );

            // Class Selectors
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSSClassSelector",
                new Regex(@"\.[a-zA-Z_\-][\w\-]*", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.ClassStyle)
            );

            // ID Selectors
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSSIdSelector",
                new Regex(@"\#[a-zA-Z_\-][\w\-]*", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.ClassStyle)
            );

            // Properties
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSSProperty",
                new Regex(@"(?<=\{|\s|;)\s*[a-zA-Z\-]+\s*(?=:)", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.KeywordStyle)
            );

            // Values
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSSValue",
                new Regex(@":\s*[^;]+(?=;|\})", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StringStyle)
            );

            // Numbers
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSSNumber",
                new Regex(@"\b\d+(\.\d+)?(px|em|rem|%|vh|vw|s|ms)?\b", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NumberStyle)
            );

            // Colors
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSSColor",
                new Regex(@"#([0-9a-fA-F]{3}|[0-9a-fA-F]{6})\b|\brgba?\([^\)]+\)|\bhsl\([^\)]+\)", RegexOptions.IgnoreCase | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.VariableStyle)
            );

            // Comments
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSSComment",
                new Regex(@"/\*.*?\*/", RegexOptions.Singleline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );

            // At-Rules
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSSAtRule",
                new Regex(@"@[\w\-]+", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );

            // Pseudo-classes and Pseudo-elements
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSSPseudo",
                new Regex(@":{1,2}[\w\-]+", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.FunctionStyle)
            );

            // Operators (excluding hyphen)
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSSOperator",
                new Regex(@"[<>+/~*!]", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );

            return Syntax;
        }
        public static HighlightingMap XPathSyntax()
        {
            HighlightingMap Syntax = new HighlightingMap();
            Syntax.LeftBracket1 = '(';
            Syntax.RightBracket1 = ')';
            Syntax.LeftBracket2 = '[';
            Syntax.RightBracket2 = ']';
            Syntax.BlockingStrategy = BlockStrategy.Modern;
            Syntax.IndentPatterns = @"
^\s*[a-zA-Z_\d]+\s*(?<range>=|<=|>=|!=|<|>)\s*(?<range>[^,;]+);
";

            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, "{", "}")); // Allow collapsing brackets block
            Syntax.FoldingPatterns.Add(new FoldingPattern(RegexOptions.IgnoreCase, @"

\[", @"\]

")); // Allow collapsing bracketed block

            // Functions
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XPathFunction",
                new Regex(@"\b(ancestor|ancestor-or-self|attribute|child|descendant|descendant-or-self|following|following-sibling|namespace|parent|preceding|preceding-sibling|self|current|last|position|count|id|local-name|namespace-uri|name|string|concat|starts-with|contains|substring-before|substring-after|substring|string-length|normalize-space|translate|boolean|not|true|false|lang|number|sum|floor|ceiling|round)\b",
                HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.FunctionStyle)
            );

            // Axis
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XPathAxis",
                new Regex(@"\b(ancestor|ancestor-or-self|attribute|child|descendant|descendant-or-self|following|following-sibling|namespace|parent|preceding|preceding-sibling|self)\b", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.KeywordStyle)
            );

            // Operators
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XPathOperator",
                new Regex(@"\b(and|or|mod|div)\b|[=<>!+*/|]", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );

            // Numbers
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XPathNumber",
                new Regex(@"\b\d+(\.\d+)?\b", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NumberStyle)
            );

            // Strings
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XPathString",
                new Regex(@"(['""]).*?\1", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StringStyle)
            );

            // Node Tests
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XPathNodeTest",
                new Regex(@"\b(comment|text|processing-instruction|node)\(\)", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.KeywordStyle)
            );

            return Syntax;
        }
        public static HighlightingMap XMLSyntax()
        {
            HighlightingMap Syntax = new HighlightingMap();
            Syntax.CommentPrefix = null;
            Syntax.LeftBracket1 = '<';
            Syntax.RightBracket1 = '>';
            Syntax.LeftBracket2 = '(';
            Syntax.RightBracket2 = ')';
            Syntax.IndentPatterns = @"";
            Syntax.BlockingStrategy = BlockStrategy.Classic;
            // Tags
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XMLTag",
                new Regex(@"<\?|<|/>|</|>|\?>", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );

            // Tag Names
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XMLTagName",
                new Regex(@"<\?(?<range1>xml)\b|<(?<range>[!\w:]+)", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );

            // End Tags
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XMLEndTag",
                new Regex(@"</(?<range>[\w:]+)>", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );

            // Attribute Names
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XMLAttributeName",
                new Regex(@"(?<range>[\w\d\-\:]+)\s*=\s*'[^']*'|(?<range>[\w\d\-\:]+)\s*=\s*""[^""]*""|(?<range>[\w\d\-\:]+)\s*=\s*[\w\d\-\:]+", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.KeywordStyle)
            );

            // Attribute Values
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XMLAttributeValue",
                new Regex(@"[\w\d\-]+?=(?<range>'[^']*')|[\w\d\-]+\s*=\s*(?<range>""[^""]*"")|[\w\d\-]+\s*=\s*(?<range>[\w\d\-]+)", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StringStyle)
            );

            // Tag Content
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XMLTagContent",
                new Regex(@"<[^>]+>", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NumberStyle)
            );

            // CDATA Sections
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XMLCData",
                new Regex(@"<!

\[CDATA

\[(?<text>(?>[^]]+|](?!]>))*)\]

\]

>", RegexOptions.IgnoreCase | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.RegionStyle)
            );

            // Comments
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XMLComment1",
                new Regex(@"<!--.*?-->|<!--.*", RegexOptions.Singleline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XMLComment2",
                new Regex(@"<!--.*?-->|.*-->", RegexOptions.Singleline | RegexOptions.RightToLeft | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );

            // Entities
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XMLEntity",
                new Regex(@"\&(amp|gt|lt|nbsp|quot|apos|copy|reg|#[0-9]{1,8}|#x[0-9a-f]{1,8});", RegexOptions.IgnoreCase | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.VariableStyle)
            );

            // End Highlighting Callback for Folding Tags
            Syntax.EndHighlighting = (range) =>
            {
                if (XMLFoldingRegex == null)
                    XMLFoldingRegex = new Regex(@"<(?<range>/?\w+)\s[^>]*?[^/]>|<(?<range>/?\w+)\s*>", RegexOptions.Singleline | HighlightingPattern.RegexCompiledOption);

                var stack = new Stack<XmlFoldingTag>();
                var id = 0;
                var fctb = range.tb;

                // Extract opening and closing tags (excluding self-closing tags: <TAG/>)
                foreach (var r in range.GetRanges(XMLFoldingRegex))
                {
                    var tagName = r.Text;
                    var iLine = r.Start.LineIndex;

                    // If it is an opening tag...
                    if (tagName[0] != '/')
                    {
                        // Push into stack
                        var tag = new XmlFoldingTag { Name = tagName, ID = id++, StartLine = r.Start.LineIndex };
                        stack.Push(tag);

                        // Set marker if this line has no markers
                        if (string.IsNullOrEmpty(fctb[iLine].FoldingStartMarker))
                            fctb[iLine].FoldingStartMarker = tag.Marker;
                    }
                    else
                    {
                        // If it is a closing tag, pop from stack
                        if (stack.Count > 0)
                        {
                            var tag = stack.Pop();

                            // Compare line number
                            if (iLine == tag.StartLine)
                            {
                                // Remove marker since the same line cannot be folded
                                if (fctb[iLine].FoldingStartMarker == tag.Marker) // Was it our marker?
                                    fctb[iLine].FoldingStartMarker = null;
                            }
                            else
                            {
                                // Set end folding marker
                                if (string.IsNullOrEmpty(fctb[iLine].FoldingEndMarker))
                                    fctb[iLine].FoldingEndMarker = tag.Marker;
                            }
                        }
                    }
                }
                return true;
            };

            return Syntax;
        }
        static Regex XMLFoldingRegex = null;
        class XmlFoldingTag
        {
            public string Name;
            public int ID;
            public int StartLine;
            public string Marker { get { return Name + ID; } }
        }


        public static void PHPAutoIndentNeeded(object sender, AutoIndentEventArgs args)
        {
            /*
            FastColoredTextBox tb = sender as FastColoredTextBox;
            tb.CalcAutoIndentShiftByCodeFolding(sender, args);*/
            //block {}
            if (Regex.IsMatch(args.LineText, @"^[^""']*\{.*\}[^""']*$"))
                return;
            //start of block {}
            if (Regex.IsMatch(args.LineText, @"^[^""']*\{"))
            {
                args.ShiftNextLines = args.TabLength;
                return;
            }
            //end of block {}
            if (Regex.IsMatch(args.LineText, @"}[^""']*$"))
            {
                args.Shift = -args.TabLength;
                args.ShiftNextLines = -args.TabLength;
                return;
            }
            //is unclosed operator in previous line ?
            if (Regex.IsMatch(args.PrevLineText, @"^\s*(if|for|foreach|while|[\}\s]*else)\b[^{]*$"))
                if (!Regex.IsMatch(args.PrevLineText, @"(;\s*$)|(;\s*//)")) //operator is unclosed
                {
                    args.Shift = args.TabLength;
                    return;
                }
        }
        public static void SQLAutoIndentNeeded(object sender, AutoIndentEventArgs args)
        {
            var tb = sender as EditBox;
            tb.CalcAutoIndentShiftByCodeFolding(sender, args);
        }
        public static void HTMLAutoIndentNeeded(object sender, AutoIndentEventArgs args)
        {
            var tb = sender as EditBox;
            tb.CalcAutoIndentShiftByCodeFolding(sender, args);
        }
        public static void XMLAutoIndentNeeded(object sender, AutoIndentEventArgs args)
        {
            var tb = sender as EditBox;
            tb.CalcAutoIndentShiftByCodeFolding(sender, args);
        }
        public static void VBAutoIndentNeeded(object sender, AutoIndentEventArgs args)
        {
            //end of block
            if (Regex.IsMatch(args.LineText, @"^\s*(End|EndIf|Next|Loop)\b", RegexOptions.IgnoreCase))
            {
                args.Shift = -args.TabLength;
                args.ShiftNextLines = -args.TabLength;
                return;
            }
            //start of declaration
            if (Regex.IsMatch(args.LineText,
                              @"\b(Class|Property|Enum|Structure|Sub|Function|Namespace|Interface|Get)\b|(Set\s*\()",
                              RegexOptions.IgnoreCase))
            {
                args.ShiftNextLines = args.TabLength;
                return;
            }
            // then ...
            if (Regex.IsMatch(args.LineText, @"\b(Then)\s*\S+", RegexOptions.IgnoreCase))
                return;
            //start of operator block
            if (Regex.IsMatch(args.LineText, @"^\s*(If|While|For|Do|Try|With|Using|Select)\b", RegexOptions.IgnoreCase))
            {
                args.ShiftNextLines = args.TabLength;
                return;
            }

            //Statements else, elseif, case etc
            if (Regex.IsMatch(args.LineText, @"^\s*(Else|ElseIf|Case|Catch|Finally)\b", RegexOptions.IgnoreCase))
            {
                args.Shift = -args.TabLength;
                return;
            }

            //Char _
            if (args.PrevLineText.TrimEnd().EndsWith("_"))
            {
                args.Shift = args.TabLength;
                return;
            }
        }
        public static void CSharpAutoIndentNeeded(object sender, AutoIndentEventArgs args)
        {
            //block {}
            if (Regex.IsMatch(args.LineText, @"^[^""']*\{.*\}[^""']*$"))
                return;
            //start of block {}
            if (Regex.IsMatch(args.LineText, @"^[^""']*\{"))
            {
                args.ShiftNextLines = args.TabLength;
                return;
            }
            //end of block {}
            if (Regex.IsMatch(args.LineText, @"}[^""']*$"))
            {
                args.Shift = -args.TabLength;
                args.ShiftNextLines = -args.TabLength;
                return;
            }
            //label
            if (Regex.IsMatch(args.LineText, @"^\s*\w+\s*:\s*($|//)") &&
                !Regex.IsMatch(args.LineText, @"^\s*default\s*:"))
            {
                args.Shift = -args.TabLength;
                return;
            }
            //some statements: case, default
            if (Regex.IsMatch(args.LineText, @"^\s*(case|default)\b.*:\s*($|//)"))
            {
                args.Shift = -args.TabLength / 2;
                return;
            }
            //is unclosed operator in previous line ?
            if (Regex.IsMatch(args.PrevLineText, @"^\s*(if|for|foreach|while|[\}\s]*else)\b[^{]*$"))
                if (!Regex.IsMatch(args.PrevLineText, @"(;\s*$)|(;\s*//)")) //operator is unclosed
                {
                    args.Shift = args.TabLength;
                    return;
                }
        }
        public static void LuaAutoIndentNeeded(object sender, AutoIndentEventArgs args)
        {
            //end of block
            if (Regex.IsMatch(args.LineText, @"^\s*(end|until)\b"))
            {
                args.Shift = -args.TabLength;
                args.ShiftNextLines = -args.TabLength;
                return;
            }
            // then ...
            if (Regex.IsMatch(args.LineText, @"\b(then)\s*\S+"))
                return;
            //start of operator block
            if (Regex.IsMatch(args.LineText, @"^\s*(function|do|for|while|repeat|if)\b"))
            {
                args.ShiftNextLines = args.TabLength;
                return;
            }

            //Statements else, elseif, case etc
            if (Regex.IsMatch(args.LineText, @"^\s*(else|elseif)\b", RegexOptions.IgnoreCase))
            {
                args.Shift = -args.TabLength;
                return;
            }
        }

    }
}
