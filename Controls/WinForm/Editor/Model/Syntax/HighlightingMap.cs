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
                case Language.JS:
                    return JSSyntax();
                case Language.Lua:
                    return LuaSyntax();
                case Language.PHP:
                    return PHPSyntax();
                case Language.SQL:
                    return SQLSyntax();
                case Language.JSON:
                    return LuaSyntax();
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
   
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpFunction",
                    new Regex(@"(\w|\$)*(\s|\t|\r|\n)*(?=(\((\w|\W)*\)))",
                                          HighlightingPattern.RegexCompiledOption),
                    HighlightingPattern.FunctionStyle
                )
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpAttribute",
                    new Regex(@"^\s*(?<range>\[.+?\])\s*$", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                    HighlightingPattern.AttributeStyle
                )
            );

            //Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpNameSpace",
            //        new Regex(@"\b((\w|\$)+(\s|\t|\r|\n)*\.{1}(\s|\t|\r|\n)*)+\b", HighlightingPattern.RegexCompiledOption),
            //        HighlightingPattern.NameSpaceStyle
            //    )
            //);
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpClass",
                    new Regex(@"(class|struct|enum|interface)\s+(?<range>\w+?)\b", HighlightingPattern.RegexCompiledOption),
                    HighlightingPattern.ClassStyle
                )
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpKeyword",
                new Regex(
                    @"\b(abstract|add|alias|as|ascending|async|await|base|bool|break|by|byte|case|catch|char|checked|class|const|continue|decimal|default|delegate|descending|do|double|dynamic|else|enum|equals|event|explicit|extern|false|finally|fixed|float|for|foreach|from|get|global|goto|group|if|implicit|in|int|interface|internal|into|is|join|let|lock|long|nameof|namespace|new|null|object|on|operator|orderby|out|override|params|partial|private|protected|public|readonly|ref|remove|return|sbyte|sealed|select|set|short|sizeof|stackalloc|static|static|string|struct|switch|this|throw|true|try|typeof|uint|ulong|unchecked|unsafe|ushort|using|using|value|var|virtual|void|volatile|when|where|while|yield)\b",
                    HighlightingPattern.RegexCompiledOption),
                    HighlightingPattern.StatementStyle
                )
            );        
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpNumber",
                    new Regex(@"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\B0x[a-fA-F\d]+\b",
                                          HighlightingPattern.RegexCompiledOption),
                    HighlightingPattern.NumberStyle
                )
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpRegion",
                    new Regex(@"#region\b|#endregion\b", RegexOptions.Singleline | HighlightingPattern.RegexCompiledOption),
                    HighlightingPattern.RegionStyle
                )
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpComment",
                    new Regex(@"//.*$", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                    HighlightingPattern.CommentStyle
                )
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpComment1",
                    new Regex(@"(/\*.*?\*/)|(/\*.*)", RegexOptions.Singleline | HighlightingPattern.RegexCompiledOption),
                    HighlightingPattern.CommentStyle
                )
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpComment2",
                    new Regex(@"(/\*.*?\*/)|(.*\*/)", RegexOptions.Singleline | RegexOptions.RightToLeft | HighlightingPattern.RegexCompiledOption),
                    HighlightingPattern.CommentStyle
                )
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpComment3",
                new Regex(@"^\s*///.*$",
                RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                    HighlightingPattern.DescriptionStyle
                )
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("CSharpString",
                    new Regex(
                        @"
                                # Character definitions:
                                '
                                (?> # disable backtracking
                                  (?:
                                    \\[^\r\n]|    # escaped meta char
                                    [^'\r\n]      # any character except '
                                  )*
                                )
                                '?
                                |
                                # Normal string & verbatim strings definitions:
                                (?<verbatimIdentifier>@)?         # this group matches if it is an verbatim string
                                ""
                                (?> # disable backtracking
                                  (?:
                                    # match and consume an escaped character including escaped double quote ("") char
                                    (?(verbatimIdentifier)        # if it is a verbatim string ...
                                      """"|                         #   then: only match an escaped double quote ("") char
                                      \\.                         #   else: match an escaped sequence
                                    )
                                    | # OR
            
                                    # match any char except double quote char ("")
                                    [^""]
                                  )*
                                )
                                ""
                            ",
                        RegexOptions.ExplicitCapture | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace |
                        HighlightingPattern.RegexCompiledOption
                        ),
                    HighlightingPattern.StringStyle
                )); //thanks to rittergig for this regex


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

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("VBFunction",
                    new Regex(@"\B(\w)*(\s|\t|\r|\n)*\((\w|\W)*\)\B",
                                          HighlightingPattern.RegexCompiledOption),
                    HighlightingPattern.FunctionStyle
                )
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("VBClass", new Regex(@"\b(Class|Structure|Enum|Interface)[ ]+(?<range>\w+?)\b",
                                         RegexOptions.IgnoreCase | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.ClassStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("VBKeyword",
                new Regex(
                    @"\b(AddHandler|AddressOf|Alias|And|AndAlso|As|Boolean|ByRef|Byte|ByVal|Call|Case|Catch|CBool|CByte|CChar|CDate|CDbl|CDec|Char|CInt|Class|CLng|CObj|Const|Continue|CSByte|CShort|CSng|CStr|CType|CUInt|CULng|CUShort|Date|Decimal|Declare|Default|Delegate|Dim|DirectCast|Do|Double|Each|Else|ElseIf|End|EndIf|Enum|Erase|Error|Event|Exit|False|Finally|For|Friend|Function|Get|GetType|GetXMLNamespace|Global|GoSub|GoTo|Handles|If|Implements|Imports|In|Inherits|Integer|Interface|Is|IsNot|Let|Lib|Like|Long|Loop|Me|Mod|Module|MustInherit|MustOverride|MyBase|MyClass|Namespace|Narrowing|New|Next|Not|Nothing|NotInheritable|NotOverridable|Object|Of|On|Operator|Option|Optional|Or|OrElse|Overloads|Overridable|Overrides|ParamArray|Partial|Private|Property|Protected|Public|RaiseEvent|ReadOnly|ReDim|REM|RemoveHandler|Resume|Return|SByte|Select|Set|Shadows|Shared|Short|Single|Static|Step|Stop|String|Structure|Sub|SyncLock|Then|Throw|To|True|Try|TryCast|TypeOf|UInteger|ULong|UShort|Using|Variant|Wend|When|While|Widening|With|WithEvents|WriteOnly|Xor|Region)\b",
                    RegexOptions.IgnoreCase | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("VBNumber", new Regex(@"\b\d+[\.]?\d*([eE]\-?\d+)?\b", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NumberStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("VBRegion", new Regex(@"(#Const|#Else|#ElseIf|#End|#If|#Region)\b",
                RegexOptions.IgnoreCase | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.RegionStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("VBComment", new Regex(@"'.*$", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("VBString",
                new Regex(@"""""|"".*?[^\\]""", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StringStyle)
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

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("SQLNumber",
                new Regex(@"\b\d+[\.]?\d*([eE]\-?\d+)?\b", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NumberStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("SQLFunctions",
                new Regex(@"(@@CONNECTIONS|@@CPU_BUSY|@@CURSOR_ROWS|@@DATEFIRST|@@DATEFIRST|@@DBTS|@@ERROR|@@FETCH_STATUS|@@IDENTITY|@@IDLE|@@IO_BUSY|@@LANGID|@@LANGUAGE|@@LOCK_TIMEOUT|@@MAX_CONNECTIONS|@@MAX_PRECISION|@@NESTLEVEL|@@OPTIONS|@@PACKET_ERRORS|@@PROCID|@@REMSERVER|@@ROWCOUNT|@@SERVERNAME|@@SERVICENAME|@@SPID|@@TEXTSIZE|@@TRANCOUNT|@@VERSION)\b|\b(ABS|ACOS|APP_NAME|ASCII|ASIN|ASSEMBLYPROPERTY|AsymKey_ID|ASYMKEY_ID|asymkeyproperty|ASYMKEYPROPERTY|ATAN|ATN2|AVG|CASE|CAST|CEILING|Cert_ID|Cert_ID|CertProperty|CHAR|CHARINDEX|CHECKSUM_AGG|COALESCE|COL_LENGTH|COL_NAME|COLLATIONPROPERTY|COLLATIONPROPERTY|COLUMNPROPERTY|COLUMNS_UPDATED|COLUMNS_UPDATED|CONTAINSTABLE|CONVERT|COS|COT|COUNT|COUNT_BIG|CRYPT_GEN_RANDOM|CURRENT_TIMESTAMP|CURRENT_TIMESTAMP|CURRENT_USER|CURRENT_USER|CURSOR_STATUS|DATABASE_PRINCIPAL_ID|DATABASE_PRINCIPAL_ID|DATABASEPROPERTY|DATABASEPROPERTYEX|DATALENGTH|DATALENGTH|DATEADD|DATEDIFF|DATENAME|DATEPART|DAY|DB_ID|DB_NAME|DECRYPTBYASYMKEY|DECRYPTBYCERT|DECRYPTBYKEY|DECRYPTBYKEYAUTOASYMKEY|DECRYPTBYKEYAUTOCERT|DECRYPTBYPASSPHRASE|DEGREES|DENSE_RANK|DIFFERENCE|ENCRYPTBYASYMKEY|ENCRYPTBYCERT|ENCRYPTBYKEY|ENCRYPTBYPASSPHRASE|ERROR_LINE|ERROR_MESSAGE|ERROR_NUMBER|ERROR_PROCEDURE|ERROR_SEVERITY|ERROR_STATE|EVENTDATA|EXP|FILE_ID|FILE_IDEX|FILE_NAME|FILEGROUP_ID|FILEGROUP_NAME|FILEGROUPPROPERTY|FILEPROPERTY|FLOOR|fn_helpcollations|fn_listextendedproperty|fn_servershareddrives|fn_virtualfilestats|fn_virtualfilestats|FORMATMESSAGE|FREETEXTTABLE|FULLTEXTCATALOGPROPERTY|FULLTEXTSERVICEPROPERTY|GETANSINULL|GETDATE|GETUTCDATE|GROUPING|HAS_PERMS_BY_NAME|HOST_ID|HOST_NAME|IDENT_CURRENT|IDENT_CURRENT|IDENT_INCR|IDENT_INCR|IDENT_SEED|IDENTITY\(|INDEX_COL|INDEXKEY_PROPERTY|INDEXPROPERTY|IS_MEMBER|IS_OBJECTSIGNED|IS_SRVROLEMEMBER|ISDATE|ISDATE|ISNULL|ISNUMERIC|Key_GUID|Key_GUID|Key_ID|Key_ID|KEY_NAME|KEY_NAME|LEFT|LEN|LOG|LOG10|LOWER|LTRIM|MAX|MIN|MONTH|NCHAR|NEWID|NTILE|NULLIF|OBJECT_DEFINITION|OBJECT_ID|OBJECT_NAME|OBJECT_SCHEMA_NAME|OBJECTPROPERTY|OBJECTPROPERTYEX|OPENDATASOURCE|OPENQUERY|OPENROWSET|OPENXML|ORIGINAL_LOGIN|ORIGINAL_LOGIN|PARSENAME|PATINDEX|PATINDEX|PERMISSIONS|PI|POWER|PUBLISHINGSERVERNAME|PWDCOMPARE|PWDENCRYPT|QUOTENAME|RADIANS|RAND|RANK|REPLICATE|REVERSE|RIGHT|ROUND|ROW_NUMBER|ROWCOUNT_BIG|RTRIM|SCHEMA_ID|SCHEMA_ID|SCHEMA_NAME|SCHEMA_NAME|SCOPE_IDENTITY|SERVERPROPERTY|SESSION_USER|SESSION_USER|SESSIONPROPERTY|SETUSER|SIGN|SignByAsymKey|SignByCert|SIN|SOUNDEX|SPACE|SQL_VARIANT_PROPERTY|SQRT|SQUARE|STATS_DATE|STDEV|STDEVP|STR|STUFF|SUBSTRING|SUM|SUSER_ID|SUSER_NAME|SUSER_SID|SUSER_SNAME|SWITCHOFFSET|SYMKEYPROPERTY|symkeyproperty|sys\.dm_db_index_physical_stats|sys\.fn_builtin_permissions|sys\.fn_my_permissions|SYSDATETIME|SYSDATETIMEOFFSET|SYSTEM_USER|SYSTEM_USER|SYSUTCDATETIME|TAN|TERTIARY_WEIGHTS|TEXTPTR|TODATETIMEOFFSET|TRIGGER_NESTLEVEL|TYPE_ID|TYPE_NAME|TYPEPROPERTY|UNICODE|UPDATE\(|UPPER|USER_ID|USER_NAME|USER_NAME|VAR|VARP|VerifySignedByAsymKey|VerifySignedByCert|XACT_STATE|YEAR)\b", RegexOptions.IgnoreCase | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.FunctionStyle)
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("SQLVar",
                new Regex(@"@[a-zA-Z_\d]*\b", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.VariableStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("SQLStatements",
                new Regex(@"\b(ALTER APPLICATION ROLE|ALTER ASSEMBLY|ALTER ASYMMETRIC KEY|ALTER AUTHORIZATION|ALTER BROKER PRIORITY|ALTER CERTIFICATE|ALTER CREDENTIAL|ALTER CRYPTOGRAPHIC PROVIDER|ALTER DATABASE|ALTER DATABASE AUDIT SPECIFICATION|ALTER DATABASE ENCRYPTION KEY|ALTER ENDPOINT|ALTER EVENT SESSION|ALTER FULLTEXT CATALOG|ALTER FULLTEXT INDEX|ALTER FULLTEXT STOPLIST|ALTER FUNCTION|ALTER INDEX|ALTER LOGIN|ALTER MASTER KEY|ALTER MESSAGE TYPE|ALTER PARTITION FUNCTION|ALTER PARTITION SCHEME|ALTER PROCEDURE|ALTER QUEUE|ALTER REMOTE SERVICE BINDING|ALTER RESOURCE GOVERNOR|ALTER RESOURCE POOL|ALTER ROLE|ALTER ROUTE|ALTER SCHEMA|ALTER SERVER AUDIT|ALTER SERVER AUDIT SPECIFICATION|ALTER SERVICE|ALTER SERVICE MASTER KEY|ALTER SYMMETRIC KEY|ALTER TABLE|ALTER TRIGGER|ALTER USER|ALTER VIEW|ALTER WORKLOAD GROUP|ALTER XML SCHEMA COLLECTION|BULK INSERT|CREATE AGGREGATE|CREATE APPLICATION ROLE|CREATE ASSEMBLY|CREATE ASYMMETRIC KEY|CREATE BROKER PRIORITY|CREATE CERTIFICATE|CREATE CONTRACT|CREATE CREDENTIAL|CREATE CRYPTOGRAPHIC PROVIDER|CREATE DATABASE|CREATE DATABASE AUDIT SPECIFICATION|CREATE DATABASE ENCRYPTION KEY|CREATE DEFAULT|CREATE ENDPOINT|CREATE EVENT NOTIFICATION|CREATE EVENT SESSION|CREATE FULLTEXT CATALOG|CREATE FULLTEXT INDEX|CREATE FULLTEXT STOPLIST|CREATE FUNCTION|CREATE INDEX|CREATE LOGIN|CREATE MASTER KEY|CREATE MESSAGE TYPE|CREATE PARTITION FUNCTION|CREATE PARTITION SCHEME|CREATE PROCEDURE|CREATE QUEUE|CREATE REMOTE SERVICE BINDING|CREATE RESOURCE POOL|CREATE ROLE|CREATE ROUTE|CREATE RULE|CREATE SCHEMA|CREATE SERVER AUDIT|CREATE SERVER AUDIT SPECIFICATION|CREATE SERVICE|CREATE SPATIAL INDEX|CREATE STATISTICS|CREATE SYMMETRIC KEY|CREATE SYNONYM|CREATE TABLE|CREATE TRIGGER|CREATE TYPE|CREATE USER|CREATE VIEW|CREATE WORKLOAD GROUP|CREATE XML INDEX|CREATE XML SCHEMA COLLECTION|DELETE|DISABLE TRIGGER|DROP AGGREGATE|DROP APPLICATION ROLE|DROP ASSEMBLY|DROP ASYMMETRIC KEY|DROP BROKER PRIORITY|DROP CERTIFICATE|DROP CONTRACT|DROP CREDENTIAL|DROP CRYPTOGRAPHIC PROVIDER|DROP DATABASE|DROP DATABASE AUDIT SPECIFICATION|DROP DATABASE ENCRYPTION KEY|DROP DEFAULT|DROP ENDPOINT|DROP EVENT NOTIFICATION|DROP EVENT SESSION|DROP FULLTEXT CATALOG|DROP FULLTEXT INDEX|DROP FULLTEXT STOPLIST|DROP FUNCTION|DROP INDEX|DROP LOGIN|DROP MASTER KEY|DROP MESSAGE TYPE|DROP PARTITION FUNCTION|DROP PARTITION SCHEME|DROP PROCEDURE|DROP QUEUE|DROP REMOTE SERVICE BINDING|DROP RESOURCE POOL|DROP ROLE|DROP ROUTE|DROP RULE|DROP SCHEMA|DROP SERVER AUDIT|DROP SERVER AUDIT SPECIFICATION|DROP SERVICE|DROP SIGNATURE|DROP STATISTICS|DROP SYMMETRIC KEY|DROP SYNONYM|DROP TABLE|DROP TRIGGER|DROP TYPE|DROP USER|DROP VIEW|DROP WORKLOAD GROUP|DROP XML SCHEMA COLLECTION|ENABLE TRIGGER|EXEC|EXECUTE|REPLACE|FROM|INSERT|MERGE|OPTION|OUTPUT|SELECT|TOP|TRUNCATE TABLE|UPDATE|UPDATE STATISTICS|WHERE|WITH|INTO|IN|SET)\b", RegexOptions.IgnoreCase | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("SQLKeywords",
                new Regex(@"\b(ADD|ALL|AND|ANY|AS|ASC|AUTHORIZATION|BACKUP|BEGIN|BETWEEN|BREAK|BROWSE|BY|CASCADE|CHECK|CHECKPOINT|CLOSE|CLUSTERED|COLLATE|COLUMN|COMMIT|COMPUTE|CONSTRAINT|CONTAINS|CONTINUE|CROSS|CURRENT|CURRENT_DATE|CURRENT_TIME|CURSOR|DATABASE|DBCC|DEALLOCATE|DECLARE|DEFAULT|DENY|DESC|DISK|DISTINCT|DISTRIBUTED|DOUBLE|DUMP|ELSE|END|ERRLVL|ESCAPE|EXCEPT|EXISTS|EXIT|EXTERNAL|FETCH|FILE|FILLFACTOR|FOR|FOREIGN|FREETEXT|FULL|FUNCTION|GOTO|GRANT|GROUP|HAVING|HOLDLOCK|IDENTITY|IDENTITY_INSERT|IDENTITYCOL|IF|INDEX|INNER|INTERSECT|IS|JOIN|KEY|KILL|LIKE|LINENO|LOAD|NATIONAL|NOCHECK|NONCLUSTERED|NOT|NULL|OF|OFF|OFFSETS|ON|OPEN|OR|ORDER|OUTER|OVER|PERCENT|PIVOT|PLAN|PRECISION|PRIMARY|PRINT|PROC|PROCEDURE|PUBLIC|RAISERROR|READ|READTEXT|RECONFIGURE|REFERENCES|REPLICATION|RESTORE|RESTRICT|RETURN|REVERT|REVOKE|ROLLBACK|ROWCOUNT|ROWGUIDCOL|RULE|SAVE|SCHEMA|SECURITYAUDIT|SHUTDOWN|SOME|STATISTICS|TABLE|TABLESAMPLE|TEXTSIZE|THEN|TO|TRAN|TRANSACTION|TRIGGER|TSEQUAL|UNION|UNIQUE|UNPIVOT|UPDATETEXT|USE|USER|VALUES|VARYING|VIEW|WAITFOR|WHEN|WHILE|WRITETEXT)\b", RegexOptions.IgnoreCase | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("SQLTypes",
                new Regex(
                    @"\b(BIGINT|NUMERIC|BIT|SMALLINT|DECIMAL|SMALLMONEY|INT|TINYINT|MONEY|FLOAT|REAL|DATE|DATETIMEOFFSET|DATETIME2|SMALLDATETIME|DATETIME|TIME|CHAR|VARCHAR|TEXT|NCHAR|NVARCHAR|NTEXT|BINARY|VARBINARY|IMAGE|TIMESTAMP|HIERARCHYID|TABLE|UNIQUEIDENTIFIER|SQL_VARIANT|XML)\b",
                    RegexOptions.IgnoreCase | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.AttributeStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("SQLComment0",
                new Regex(@"--.*$", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.DescriptionStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("SQLComment1",
                new Regex(@"(/\*.*?\*/)|(/\*.*)", RegexOptions.Singleline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("SQLComment2",
                 new Regex(@"(/\*.*?\*/)|(.*\*/)", RegexOptions.Singleline | RegexOptions.RightToLeft | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("SQLComment3",
                 new Regex(@"#.*$", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.DescriptionStyle)
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("SQLString",
                new Regex(@"""""|''|"".*?[^\\]""|'.*?[^\\]'", HighlightingPattern.RegexCompiledOption),
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

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPFunction",
                    new Regex(@"(\w|\$)*(\s|\t|\r|\n)*(?=(\((\w|\W)*\)))",
                                          HighlightingPattern.RegexCompiledOption),
                    HighlightingPattern.FunctionStyle
                )
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPClass",
                    new Regex(@"\b(class)\s+(?<range>\w+?)\b", HighlightingPattern.RegexCompiledOption),
                    HighlightingPattern.ClassStyle
                )
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPVar",
                 new Regex(@"\$[a-zA-Z_\d]*\b", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.VariableStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPKeyword",
               new Regex(
                    @"\b(die|echo|empty|exit|eval|include|include_once|isset|list|require|require_once|return|print|unset)\b",
                    HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.KeywordStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPKeyword",
                    new Regex(@"__CLASS__ |__DIR__|__FILE__|__LINE__|__FUNCTION__|__METHOD__|__NAMESPACE__",
                     HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.AttributeStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPKeyword",
               new Regex(
                    @"\b(abstract|and|array|as|break|case|catch|cfunction|class|clone|const|continue|declare|default|do|else|elseif|enddeclare|endfor|endforeach|endif|endswitch|endwhile|extends|final|for|foreach|function|global|goto|if|implements|instanceof|interface|namespace|new|or|private|protected|public|static|switch|throw|try|use|var|while|xor)\b",
                    HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPNumber",
                new Regex(@"\b\d+[\.]?\d*\b", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NumberStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPString0",
                new Regex(@"(?:(['""]).*[^\\]\1)|(?:`[^`]*[^\\]`)|(`|""|')\2", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StringStyle)
            );
            return Syntax;
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPComment0",
                new Regex(@"(//|#).*$", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPComment1",
                 new Regex(@"(/\*.*?\*/)|(/\*.*)", RegexOptions.Singleline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("PHPComment2",
                 new Regex(@"(/\*.*?\*/)|(.*\*/)",
                                         RegexOptions.Singleline | RegexOptions.RightToLeft | HighlightingPattern.RegexCompiledOption),
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

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptFunction",
                    new Regex(@"(\w|\$)*(\s|\t|\r|\n)*(?=(\([\s|\S]*\)))",
                                          HighlightingPattern.RegexCompiledOption),
                    HighlightingPattern.FunctionStyle
                )
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptClass",
                    new Regex(FirstBreaker + @"(class|interface|abstract)[\s\r\n\t]+(?<range>\w+?)" + LastBreaker, HighlightingPattern.RegexCompiledOption),
                    HighlightingPattern.ClassStyle
                )
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptNameSpace",
                    new Regex(FirstBreaker + "([\\w\\$]+\\.)+(?<=([\\w\\$]+\\.[\\w\\$]+))" + LastBreaker, HighlightingPattern.RegexCompiledOption),
                    HighlightingPattern.NameSpaceStyle
                )
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptKeyword",
                new Regex(
                    FirstBreaker + @"(arguments|public|protected|private|var|debugger|static|true|false|break|case|catch|continue|class|default|delete|do|else|export|for|function|if|in|of|async|await|instanceof|import|new|null|return|switch|this|throw|try|finally|var|void|while|with|typeof|yield|let|const|NaN|undefined)" + LastBreaker,
                    HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptClass",
                new Regex(
                    FirstBreaker + @"(Object|JSON|Date|Array|Math|RegExp)\b" + LastBreaker,
                    HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.ClassStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptNumber",
                 new Regex(@"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\b0x[a-fA-F\d]+\b",
                                           HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NumberStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptString0",
                new Regex(@"(?:(['""]).*[^\\]\1)|(?:`[^`]*[^\\]`)|(`|""|')\2", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StringStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptComment",
                 new Regex(@"\/\/.*$", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptComment",
                 new Regex(@"(\/\*.*?\*\/)|(\/\*.*)", RegexOptions.Singleline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptComment",
                 new Regex(@"(\/\*.*?\*\/)|(.*\*\/)",
                                             RegexOptions.Singleline | RegexOptions.RightToLeft | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );
            return Syntax;
        }
        /// <summary>
        /// Highlights Lua code
        /// </summary>
        public static HighlightingMap LuaSyntax()
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

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("LuaFunctions",
                new Regex(
                    @"\b(assert|collectgarbage|dofile|error|getfenv|getmetatable|ipairs|load|loadfile|loadstring|module|next|pairs|pcall|print|rawequal|rawget|rawset|require|select|setfenv|setmetatable|tonumber|tostring|type|unpack|xpcall)\b",
                    HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.FunctionStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("LuaKeyword",
                new Regex(
                    @"\b(and|break|do|else|elseif|end|false|for|function|if|in|local|nil|not|or|repeat|return|then|true|until|while)\b",
                    HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("LuaNumber",
                new Regex(@"\b\d+[\.]?\d*([eE]\-?\d+)?[lLdDfF]?\b|\b0x[a-fA-F\d]+\b",
                HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NumberStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("LuaComment0",
                new Regex(@"--.*$", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("LuaComment1",
                new Regex(@"(--\[\[.*?\]\])|(--\[\[.*)", RegexOptions.Singleline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("LuaComment2",
                new Regex(@"(--\[\[.*?\]\])|(.*\]\])",
                                             RegexOptions.Singleline | RegexOptions.RightToLeft | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("LuaString",
                new Regex(@"""""|''|"".*?[^\\]""|'.*?[^\\]'", HighlightingPattern.RegexCompiledOption),
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

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JSONKeyword",
                 new Regex(@"(?<range>""([^\\""]|\\"")*"")\s*:", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.VariableStyle)
            );

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JSONNumber",
                 new Regex(@"\b(\d+[\.]?\d*|true|false|null)\b", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NumberStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("JScriptString0",
                new Regex(@"(?:(['""]).*[^\\]\1)|(?:`[^`]*[^\\]`)|(`|""|')\2", RegexOptions.Multiline | HighlightingPattern.RegexCompiledOption),
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

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("HTMLTag",
                 new Regex(@" <|/>|</|>", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            ); ;
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("HTMLTagName",
                 new Regex(@" < (?<range>[!\w:]+)", HighlightingPattern.RegexCompiledOption),
               HighlightingPattern.StatementStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("HTMLEndTag",
                 new Regex(@" </(?<range>[\w:]+)>", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("HTMLAttributeName",
                new Regex(
                    @"(?<range>[\w\d\-]{1,20}?)='[^']*'|(?<range>[\w\d\-]{1,20})=""[^""]*""|(?<range>[\w\d\-]{1,20})=[\w\d\-]{1,20}",
                    HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.KeywordStyle
                )
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("HTMLTagContent",
                 new Regex(@" <[^>]+>", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NumberStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("HTMLComment0",
                new Regex(@"(<!--.*?-->)|(<!--.*)", RegexOptions.Singleline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("HTMLComment1",
                 new Regex(@"(<!--.*?-->)|(.*-->)",
                           RegexOptions.Singleline | RegexOptions.RightToLeft | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("HTMLAttributeValue",
                new Regex(
                    @"[\w\d\-]{1,20}?=(?<range>'[^']*')|[\w\d\-]{1,20}=(?<range>""[^""]*"")|[\w\d\-]{1,20}=(?<range>[\w\d\-]{1,20})",
                    HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StringStyle
                )
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("HTMLEntity",
                 new Regex(@"\&(amp|gt|lt|nbsp|quot|apos|copy|reg|#[0-9]{1,8}|#x[0-9a-f]{1,8});",
                                        HighlightingPattern.RegexCompiledOption | RegexOptions.IgnoreCase),
                HighlightingPattern.VariableStyle
                )
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

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XMLTag",
                 new Regex(@" <\?|<|/>|</|>|\?>", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XMLTagName",
                 new Regex(@" <[?](?<range1>[x][m][l]{1})|<(?<range>[!\w:]+)", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XMLEndTag",
                 new Regex(@" </(?<range>[\w:]+)>", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StatementStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XMLAttributeName",
                new Regex(
                    @"(?<range>[\w\d\-\:]+)[ ]*=[ ]*'[^']*'|(?<range>[\w\d\-\:]+)[ ]*=[ ]*""[^""]*""|(?<range>[\w\d\-\:]+)[ ]*=[ ]*[\w\d\-\:]+",
                    HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.KeywordStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XMLTagContent",
                 new Regex(@" <[^>]+>", HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.NumberStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XMLCData",
                 new Regex(@" < !\s*\[CDATA\s*\[(?<text>(?>[^]]+|](?!]>))*)]]>", HighlightingPattern.RegexCompiledOption | RegexOptions.IgnoreCase),
                HighlightingPattern.RegionStyle)
            ); // http://stackoverflow.com/questions/21681861/i-need-a-regex-that-matches-cdata-elements-in-html

            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XMLComment1",
                new Regex(@"(<!--.*?-->)|(<!--.*)", RegexOptions.Singleline | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XMLComment2",
                 new Regex(@"(<!--.*?-->)|(.*-->)",
                                          RegexOptions.Singleline | RegexOptions.RightToLeft | HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.CommentStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XMLAttributeValue",
                new Regex(
                    @"[\w\d\-]+?=(?<range>'[^']*')|[\w\d\-]+[ ]*=[ ]*(?<range>""[^""]*"")|[\w\d\-]+[ ]*=[ ]*(?<range>[\w\d\-]+)",
                    HighlightingPattern.RegexCompiledOption),
                HighlightingPattern.StringStyle)
            );
            Syntax.HighlightingPatterns.Add(new HighlightingPattern("XMLEntity",
                 new Regex(@"\&(amp|gt|lt|nbsp|quot|apos|copy|reg|#[0-9]{1,8}|#x[0-9a-f]{1,8});",
                                        HighlightingPattern.RegexCompiledOption | RegexOptions.IgnoreCase),
                HighlightingPattern.VariableStyle)
            );

            Syntax.EndHighlighting = (range)=>
                {
                    if (XMLFoldingRegex == null)
                        XMLFoldingRegex = new Regex(@" < (?<range>/?\w+)\s[^>]*?[^/]>|<(?<range>/?\w+)\s*>", RegexOptions.Singleline | HighlightingPattern.RegexCompiledOption);
                    var stack = new Stack<XmlFoldingTag>();
                    var id = 0;
                    var fctb = range.tb;
                    //extract opening and closing tags (exclude open-close tags: <TAG/>)
                    foreach (var r in range.GetRanges(XMLFoldingRegex))
                    {
                        var tagName = r.Text;
                        var iLine = r.Start.LineIndex;
                        //if it is opening tag...
                        if (tagName[0] != '/')
                        {
                            // ...push into stack
                            var tag = new XmlFoldingTag { Name = tagName, ID = id++, StartLine = r.Start.LineIndex };
                            stack.Push(tag);
                            // if this line has no markers - set marker
                            if (string.IsNullOrEmpty(fctb[iLine].FoldingStartMarker))
                                fctb[iLine].FoldingStartMarker = tag.Marker;
                        }
                        else
                        {
                            //if it is closing tag - pop from stack
                            if (stack.Count > 0)
                            {
                                var tag = stack.Pop();
                                //compare line number
                                if (iLine == tag.StartLine)
                                {
                                    //remove marker, because same line can not be folding
                                    if (fctb[iLine].FoldingStartMarker == tag.Marker) //was it our marker?
                                        fctb[iLine].FoldingStartMarker = null;
                                }
                                else
                                {
                                    //set end folding marker
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
