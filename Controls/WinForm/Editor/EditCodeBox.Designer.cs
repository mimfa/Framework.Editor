
namespace MiMFa.Controls.WinForm.Editor
{
    partial class EditCodeBox
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditCodeBox));
            this.EditBox = new MiMFa.Controls.WinForm.Editor.EditBox();
            this.IntelliCode = new MiMFa.Controls.WinForm.Editor.Tools.AutoCompleteMenu();
            this.MapBox = new MiMFa.Controls.WinForm.Editor.Tools.EditMap();
            this.RulerBox = new MiMFa.Controls.WinForm.Editor.Tools.Ruler();
            ((System.ComponentModel.ISupportInitialize)(this.EditBox)).BeginInit();
            this.SuspendLayout();
            // 
            // EditBox
            // 
            this.EditBox.AutoCompleteBracketsList = new char[] {
        '(',
        ')',
        '{',
        '}',
        '[',
        ']',
        '\"',
        '\"',
        '\'',
        '\''};
            this.EditBox.AutoDetectLanguage = true;
            this.EditBox.AutoIndentPattern = "\r\n^\\s*[\\w\\.]+(\\s\\w+)?\\s*(?<range>=)\\s*(?<range>[^;=]+);\r\n^\\s*(case|default)\\s*[^:" +
    "]*(?<range>:)\\s*(?<range>[^;]+);\r\n";
            this.EditBox.AutoScrollMinSize = new System.Drawing.Size(31, 18);
            this.EditBox.BackBrush = null;
            this.EditBox.BracketsHighlightStrategy = MiMFa.Controls.WinForm.Editor.BlockStrategy.Modern;
            this.EditBox.CharHeight = 18;
            this.EditBox.CharWidth = 10;
            this.EditBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.EditBox.DelayedEventsInterval = 500;
            this.EditBox.DelayedTextChangedInterval = 500;
            this.EditBox.DetectHyperLink = true;
            this.EditBox.DetectSameWords = true;
            this.EditBox.DetectSecondLanguage = true;
            this.EditBox.DisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.EditBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EditBox.HoveredWordPattern = "\\w|\\.";
            this.EditBox.IndentBackColor = System.Drawing.Color.Transparent;
            this.EditBox.IsReplaceMode = false;
            this.EditBox.Language = MiMFa.Controls.WinForm.Editor.Model.Syntax.Language.Custom;
            this.EditBox.LeftBracket1 = '(';
            this.EditBox.LeftBracket2 = '{';
            this.EditBox.Location = new System.Drawing.Point(125, 38);
            this.EditBox.Margin = new System.Windows.Forms.Padding(4);
            this.EditBox.Name = "EditBox";
            this.EditBox.Paddings = new System.Windows.Forms.Padding(0);
            this.EditBox.ProfessionalBehavior = true;
            this.EditBox.RightBracket1 = ')';
            this.EditBox.RightBracket2 = '}';
            this.EditBox.SecondLanguage = MiMFa.Controls.WinForm.Editor.Model.Syntax.Language.HTML;
            this.EditBox.SecondLanguagePattern = "``";
            this.EditBox.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(255)))));
            this.EditBox.ServiceColors = ((MiMFa.Controls.WinForm.Editor.ServiceColors)(resources.GetObject("EditBox.ServiceColors")));
            this.EditBox.Size = new System.Drawing.Size(509, 366);
            this.EditBox.SyntaxMapXMLFile = "";
            this.EditBox.TabIndex = 0;
            this.EditBox.Zoom = 100;
            // 
            // IntelliCode
            // 
            this.IntelliCode.AllowTabKey = true;
            this.IntelliCode.AppearInterval = 500;
            this.IntelliCode.HoveredBackColor = System.Drawing.Color.DimGray;
            this.IntelliCode.SelectedBackColor = System.Drawing.Color.MidnightBlue;
            this.IntelliCode.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.IntelliCode.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.IntelliCode.MinFragmentLength = 2;
            this.IntelliCode.Name = "IntelliCode";
            this.IntelliCode.NormalizationPattern = "[\\s]+";
            this.IntelliCode.Padding = new System.Windows.Forms.Padding(0);
            this.IntelliCode.ReplacePointPattern = "[\\w\\.]";
            this.IntelliCode.SearchPattern = "[\\w\\.\\s]+";
            this.IntelliCode.Size = new System.Drawing.Size(154, 154);
            this.IntelliCode.Target = this.EditBox;
            this.IntelliCode.ToolTipDuration = 5000;
            this.IntelliCode.ToolTipInitialDelay = 500;
            this.IntelliCode.ToolTipMaxSize = new System.Drawing.Size(0, 0);
            this.IntelliCode.ToolTipRealTimeShow = true;
            this.IntelliCode.ToolTipShowAlways = false;
            // 
            // MapBox
            // 
            this.MapBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.MapBox.ForeColor = System.Drawing.Color.Maroon;
            this.MapBox.Location = new System.Drawing.Point(0, 38);
            this.MapBox.Margin = new System.Windows.Forms.Padding(4);
            this.MapBox.Name = "MapBox";
            this.MapBox.Size = new System.Drawing.Size(100, 366);
            this.MapBox.TabIndex = 2;
            this.MapBox.Target = this.EditBox;
            // 
            // RulerBox
            // 
            this.RulerBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.RulerBox.Location = new System.Drawing.Point(0, 0);
            this.RulerBox.Margin = new System.Windows.Forms.Padding(4);
            this.RulerBox.MaximumSize = new System.Drawing.Size(1431655808, 30);
            this.RulerBox.MinimumSize = new System.Drawing.Size(0, 30);
            this.RulerBox.Name = "RulerBox";
            this.RulerBox.Size = new System.Drawing.Size(609, 30);
            this.RulerBox.TabIndex = 3;
            this.RulerBox.Target = this.EditBox;
            this.RulerBox.Visible = false;
            // 
            // EditCodeBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.EditBox);
            this.Controls.Add(this.MapBox);
            this.Controls.Add(this.RulerBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "EditCodeBox";
            this.Size = new System.Drawing.Size(609, 396);
            ((System.ComponentModel.ISupportInitialize)(this.EditBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public EditBox EditBox;
        public Tools.AutoCompleteMenu IntelliCode;
        public Tools.EditMap MapBox;
        public Tools.Ruler RulerBox;
    }
}
