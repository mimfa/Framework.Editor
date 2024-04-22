
namespace MiMFa.UIL.Editor
{
    partial class EditDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditDialog));
            this.Editor = new MiMFa.Controls.WinForm.Editor.EditCodeBox();
            this.ContextStripMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toggleCommentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleCaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton4 = new System.Windows.Forms.ToolStripDropDownButton();
            this.findToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.replaceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.goToLineToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addHintToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearHintsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.MainStripMenu = new System.Windows.Forms.ToolStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.redoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pasteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.showIntelliCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.zoomInToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoomOutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripDropDownButton3 = new System.Windows.Forms.ToolStripDropDownButton();
            this.mapViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rullerViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.intelliCodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wrapModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scrollBarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsb_Undo = new System.Windows.Forms.ToolStripButton();
            this.tsb_Redo = new System.Windows.Forms.ToolStripButton();
            this.tsb_Reload = new System.Windows.Forms.ToolStripButton();
            this.tsb_New = new System.Windows.Forms.ToolStripButton();
            this.tsb_Open = new System.Windows.Forms.ToolStripButton();
            this.tsb_Save = new System.Windows.Forms.ToolStripButton();
            this.tsb_SaveAs = new System.Windows.Forms.ToolStripButton();
            this.tsb_Print = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton10 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton9 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.commentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeCommentsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commentUnCommentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lettersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lowerCasesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.upperCaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.uPPERCASEToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.sentenceCaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.titleCaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.commentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unCommentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ContextStripMenu.SuspendLayout();
            this.MainStripMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // Editor
            // 
            this.Editor.ContextMenuStrip = this.ContextStripMenu;
            this.Editor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Editor.Location = new System.Drawing.Point(0, 34);
            this.Editor.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Editor.Name = "Editor";
            this.Editor.Size = new System.Drawing.Size(800, 416);
            this.Editor.TabIndex = 0;
            // 
            // ContextStripMenu
            // 
            this.ContextStripMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ContextStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem1,
            this.cutToolStripMenuItem1,
            this.pasteToolStripMenuItem1,
            this.toolStripSeparator5,
            this.toggleCommentToolStripMenuItem,
            this.toggleCaseToolStripMenuItem});
            this.ContextStripMenu.Name = "ContextStripMenu";
            this.ContextStripMenu.Size = new System.Drawing.Size(212, 120);
            // 
            // copyToolStripMenuItem1
            // 
            this.copyToolStripMenuItem1.Name = "copyToolStripMenuItem1";
            this.copyToolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl+C";
            this.copyToolStripMenuItem1.Size = new System.Drawing.Size(211, 22);
            this.copyToolStripMenuItem1.Text = "Copy";
            this.copyToolStripMenuItem1.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // cutToolStripMenuItem1
            // 
            this.cutToolStripMenuItem1.Name = "cutToolStripMenuItem1";
            this.cutToolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl+X";
            this.cutToolStripMenuItem1.Size = new System.Drawing.Size(211, 22);
            this.cutToolStripMenuItem1.Text = "Cut";
            this.cutToolStripMenuItem1.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem1
            // 
            this.pasteToolStripMenuItem1.Name = "pasteToolStripMenuItem1";
            this.pasteToolStripMenuItem1.ShortcutKeyDisplayString = "Ctrl+V";
            this.pasteToolStripMenuItem1.Size = new System.Drawing.Size(211, 22);
            this.pasteToolStripMenuItem1.Text = "Paste";
            this.pasteToolStripMenuItem1.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(208, 6);
            // 
            // toggleCommentToolStripMenuItem
            // 
            this.toggleCommentToolStripMenuItem.Name = "toggleCommentToolStripMenuItem";
            this.toggleCommentToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+/";
            this.toggleCommentToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.toggleCommentToolStripMenuItem.Text = "Toggle Comment";
            this.toggleCommentToolStripMenuItem.Click += new System.EventHandler(this.commentUnCommentToolStripMenuItem_Click);
            // 
            // toggleCaseToolStripMenuItem
            // 
            this.toggleCaseToolStripMenuItem.Name = "toggleCaseToolStripMenuItem";
            this.toggleCaseToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+Shift+U";
            this.toggleCaseToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.toggleCaseToolStripMenuItem.Text = "Toggle Case";
            this.toggleCaseToolStripMenuItem.Click += new System.EventHandler(this.upperCaseToolStripMenuItem_Click);
            // 
            // toolStripDropDownButton4
            // 
            this.toolStripDropDownButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findToolStripMenuItem,
            this.replaceToolStripMenuItem,
            this.goToLineToolStripMenuItem,
            this.addHintToolStripMenuItem,
            this.clearHintsToolStripMenuItem});
            this.toolStripDropDownButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton4.Name = "toolStripDropDownButton4";
            this.toolStripDropDownButton4.Size = new System.Drawing.Size(42, 31);
            this.toolStripDropDownButton4.Text = "Tool";
            // 
            // findToolStripMenuItem
            // 
            this.findToolStripMenuItem.Name = "findToolStripMenuItem";
            this.findToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.F)));
            this.findToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.findToolStripMenuItem.Text = "Find";
            this.findToolStripMenuItem.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // replaceToolStripMenuItem
            // 
            this.replaceToolStripMenuItem.Name = "replaceToolStripMenuItem";
            this.replaceToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.H)));
            this.replaceToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.replaceToolStripMenuItem.Text = "Replace";
            this.replaceToolStripMenuItem.Click += new System.EventHandler(this.replaceToolStripMenuItem_Click);
            // 
            // goToLineToolStripMenuItem
            // 
            this.goToLineToolStripMenuItem.Name = "goToLineToolStripMenuItem";
            this.goToLineToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.G)));
            this.goToLineToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.goToLineToolStripMenuItem.Text = "Go to";
            this.goToLineToolStripMenuItem.Click += new System.EventHandler(this.goToLineToolStripMenuItem_Click);
            // 
            // addHintToolStripMenuItem
            // 
            this.addHintToolStripMenuItem.Name = "addHintToolStripMenuItem";
            this.addHintToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.H)));
            this.addHintToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.addHintToolStripMenuItem.Text = "Add Hint";
            this.addHintToolStripMenuItem.Click += new System.EventHandler(this.addHintToolStripMenuItem_Click);
            // 
            // clearHintsToolStripMenuItem
            // 
            this.clearHintsToolStripMenuItem.Name = "clearHintsToolStripMenuItem";
            this.clearHintsToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.clearHintsToolStripMenuItem.Text = "Clear Hints";
            this.clearHintsToolStripMenuItem.Click += new System.EventHandler(this.clearHintsToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Padding = new System.Windows.Forms.Padding(5);
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 34);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Padding = new System.Windows.Forms.Padding(5);
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 34);
            // 
            // MainStripMenu
            // 
            this.MainStripMenu.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.MainStripMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripDropDownButton2,
            this.toolStripDropDownButton3,
            this.toolStripDropDownButton4,
            this.toolStripSeparator7,
            this.tsb_Undo,
            this.tsb_Redo,
            this.tsb_Reload,
            this.toolStripSeparator8,
            this.tsb_New,
            this.tsb_Open,
            this.tsb_Save,
            this.tsb_SaveAs,
            this.tsb_Print,
            this.toolStripButton10,
            this.toolStripButton9,
            this.toolStripSeparator9,
            this.toolStripButton3,
            this.toolStripButton2,
            this.toolStripButton1});
            this.MainStripMenu.Location = new System.Drawing.Point(0, 0);
            this.MainStripMenu.Name = "MainStripMenu";
            this.MainStripMenu.Size = new System.Drawing.Size(800, 34);
            this.MainStripMenu.TabIndex = 1;
            this.MainStripMenu.Text = "toolStrip1";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator3,
            this.printToolStripMenuItem,
            this.toolStripSeparator4,
            this.exitToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(38, 31);
            this.toolStripDropDownButton1.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.S)));
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(183, 6);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.printToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.printToolStripMenuItem.Text = "Print";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(183, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(186, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStripDropDownButton2
            // 
            this.toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.redoToolStripMenuItem,
            this.toolStripSeparator2,
            this.copyToolStripMenuItem,
            this.pasteToolStripMenuItem,
            this.cutToolStripMenuItem,
            this.toolStripSeparator1,
            this.commentsToolStripMenuItem,
            this.lettersToolStripMenuItem,
            this.showIntelliCodeToolStripMenuItem,
            this.toolStripSeparator6,
            this.zoomInToolStripMenuItem,
            this.zoomOutToolStripMenuItem});
            this.toolStripDropDownButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton2.Image")));
            this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
            this.toolStripDropDownButton2.Size = new System.Drawing.Size(40, 31);
            this.toolStripDropDownButton2.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            this.undoToolStripMenuItem.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // redoToolStripMenuItem
            // 
            this.redoToolStripMenuItem.Name = "redoToolStripMenuItem";
            this.redoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.Z)));
            this.redoToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.redoToolStripMenuItem.Text = "Redo";
            this.redoToolStripMenuItem.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(225, 6);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            this.copyToolStripMenuItem.Click += new System.EventHandler(this.copyToolStripMenuItem_Click);
            // 
            // pasteToolStripMenuItem
            // 
            this.pasteToolStripMenuItem.Name = "pasteToolStripMenuItem";
            this.pasteToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.V)));
            this.pasteToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.pasteToolStripMenuItem.Text = "Paste";
            this.pasteToolStripMenuItem.Click += new System.EventHandler(this.pasteToolStripMenuItem_Click);
            // 
            // cutToolStripMenuItem
            // 
            this.cutToolStripMenuItem.Name = "cutToolStripMenuItem";
            this.cutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.X)));
            this.cutToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.cutToolStripMenuItem.Text = "Cut";
            this.cutToolStripMenuItem.Click += new System.EventHandler(this.cutToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(225, 6);
            // 
            // showIntelliCodeToolStripMenuItem
            // 
            this.showIntelliCodeToolStripMenuItem.Name = "showIntelliCodeToolStripMenuItem";
            this.showIntelliCodeToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Space)));
            this.showIntelliCodeToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.showIntelliCodeToolStripMenuItem.Text = "Show IntelliCode";
            this.showIntelliCodeToolStripMenuItem.Visible = false;
            this.showIntelliCodeToolStripMenuItem.Click += new System.EventHandler(this.showIntelliCodeToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(225, 6);
            // 
            // zoomInToolStripMenuItem
            // 
            this.zoomInToolStripMenuItem.Name = "zoomInToolStripMenuItem";
            this.zoomInToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl++";
            this.zoomInToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Oemplus)));
            this.zoomInToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.zoomInToolStripMenuItem.Text = "Zoom In";
            this.zoomInToolStripMenuItem.Click += new System.EventHandler(this.zoomInToolStripMenuItem_Click);
            // 
            // zoomOutToolStripMenuItem
            // 
            this.zoomOutToolStripMenuItem.Name = "zoomOutToolStripMenuItem";
            this.zoomOutToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+-";
            this.zoomOutToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.OemMinus)));
            this.zoomOutToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.zoomOutToolStripMenuItem.Text = "Zoom Out";
            this.zoomOutToolStripMenuItem.Click += new System.EventHandler(this.zoomOutToolStripMenuItem_Click);
            // 
            // toolStripDropDownButton3
            // 
            this.toolStripDropDownButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton3.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mapViewToolStripMenuItem,
            this.rullerViewToolStripMenuItem,
            this.intelliCodeToolStripMenuItem,
            this.wrapModeToolStripMenuItem,
            this.scrollBarToolStripMenuItem});
            this.toolStripDropDownButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton3.Image")));
            this.toolStripDropDownButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton3.Name = "toolStripDropDownButton3";
            this.toolStripDropDownButton3.Size = new System.Drawing.Size(45, 31);
            this.toolStripDropDownButton3.Text = "View";
            // 
            // mapViewToolStripMenuItem
            // 
            this.mapViewToolStripMenuItem.Checked = true;
            this.mapViewToolStripMenuItem.CheckOnClick = true;
            this.mapViewToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.mapViewToolStripMenuItem.Name = "mapViewToolStripMenuItem";
            this.mapViewToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.mapViewToolStripMenuItem.Text = "Map";
            this.mapViewToolStripMenuItem.CheckedChanged += new System.EventHandler(this.mapViewToolStripMenuItem_CheckedChanged);
            // 
            // rullerViewToolStripMenuItem
            // 
            this.rullerViewToolStripMenuItem.CheckOnClick = true;
            this.rullerViewToolStripMenuItem.Name = "rullerViewToolStripMenuItem";
            this.rullerViewToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.rullerViewToolStripMenuItem.Text = "Ruller";
            this.rullerViewToolStripMenuItem.CheckedChanged += new System.EventHandler(this.rullerViewToolStripMenuItem_CheckedChanged);
            // 
            // intelliCodeToolStripMenuItem
            // 
            this.intelliCodeToolStripMenuItem.Checked = true;
            this.intelliCodeToolStripMenuItem.CheckOnClick = true;
            this.intelliCodeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.intelliCodeToolStripMenuItem.Name = "intelliCodeToolStripMenuItem";
            this.intelliCodeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.intelliCodeToolStripMenuItem.Text = "IntelliCode";
            this.intelliCodeToolStripMenuItem.CheckedChanged += new System.EventHandler(this.intelliCodeToolStripMenuItem_CheckedChanged);
            // 
            // wrapModeToolStripMenuItem
            // 
            this.wrapModeToolStripMenuItem.CheckOnClick = true;
            this.wrapModeToolStripMenuItem.Name = "wrapModeToolStripMenuItem";
            this.wrapModeToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.wrapModeToolStripMenuItem.Text = "Words Wrap";
            this.wrapModeToolStripMenuItem.CheckedChanged += new System.EventHandler(this.wrapModeToolStripMenuItem_CheckedChanged);
            // 
            // scrollBarToolStripMenuItem
            // 
            this.scrollBarToolStripMenuItem.Checked = true;
            this.scrollBarToolStripMenuItem.CheckOnClick = true;
            this.scrollBarToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.scrollBarToolStripMenuItem.Name = "scrollBarToolStripMenuItem";
            this.scrollBarToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.scrollBarToolStripMenuItem.Text = "ScrollBar";
            this.scrollBarToolStripMenuItem.CheckedChanged += new System.EventHandler(this.scrollBarToolStripMenuItem_CheckedChanged);
            // 
            // tsb_Undo
            // 
            this.tsb_Undo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_Undo.Enabled = false;
            this.tsb_Undo.Image = global::MiMFa.Properties.Resources.Undo;
            this.tsb_Undo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Undo.Margin = new System.Windows.Forms.Padding(0);
            this.tsb_Undo.Name = "tsb_Undo";
            this.tsb_Undo.Padding = new System.Windows.Forms.Padding(5);
            this.tsb_Undo.Size = new System.Drawing.Size(34, 34);
            this.tsb_Undo.Text = "Undo";
            this.tsb_Undo.Click += new System.EventHandler(this.undoToolStripMenuItem_Click);
            // 
            // tsb_Redo
            // 
            this.tsb_Redo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_Redo.Enabled = false;
            this.tsb_Redo.Image = global::MiMFa.Properties.Resources.Redo;
            this.tsb_Redo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Redo.Margin = new System.Windows.Forms.Padding(0);
            this.tsb_Redo.Name = "tsb_Redo";
            this.tsb_Redo.Padding = new System.Windows.Forms.Padding(5);
            this.tsb_Redo.Size = new System.Drawing.Size(34, 34);
            this.tsb_Redo.Text = "Redo";
            this.tsb_Redo.Click += new System.EventHandler(this.redoToolStripMenuItem_Click);
            // 
            // tsb_Reload
            // 
            this.tsb_Reload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_Reload.Enabled = false;
            this.tsb_Reload.Image = global::MiMFa.Properties.Resources.Refresh;
            this.tsb_Reload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Reload.Margin = new System.Windows.Forms.Padding(0);
            this.tsb_Reload.Name = "tsb_Reload";
            this.tsb_Reload.Padding = new System.Windows.Forms.Padding(5);
            this.tsb_Reload.Size = new System.Drawing.Size(34, 34);
            this.tsb_Reload.Text = "Return to first";
            this.tsb_Reload.Click += new System.EventHandler(this.tsb_Refresh_Click);
            // 
            // tsb_New
            // 
            this.tsb_New.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_New.Image = global::MiMFa.Properties.Resources.Document;
            this.tsb_New.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_New.Margin = new System.Windows.Forms.Padding(0);
            this.tsb_New.Name = "tsb_New";
            this.tsb_New.Padding = new System.Windows.Forms.Padding(5);
            this.tsb_New.Size = new System.Drawing.Size(34, 34);
            this.tsb_New.Text = "New";
            this.tsb_New.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // tsb_Open
            // 
            this.tsb_Open.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_Open.Image = global::MiMFa.Properties.Resources.Directory;
            this.tsb_Open.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Open.Margin = new System.Windows.Forms.Padding(0);
            this.tsb_Open.Name = "tsb_Open";
            this.tsb_Open.Padding = new System.Windows.Forms.Padding(5);
            this.tsb_Open.Size = new System.Drawing.Size(34, 34);
            this.tsb_Open.Text = "Open";
            this.tsb_Open.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // tsb_Save
            // 
            this.tsb_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_Save.Enabled = false;
            this.tsb_Save.Image = global::MiMFa.Properties.Resources.Save_Green;
            this.tsb_Save.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Save.Margin = new System.Windows.Forms.Padding(0);
            this.tsb_Save.Name = "tsb_Save";
            this.tsb_Save.Padding = new System.Windows.Forms.Padding(5);
            this.tsb_Save.Size = new System.Drawing.Size(34, 34);
            this.tsb_Save.Text = "Save";
            this.tsb_Save.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // tsb_SaveAs
            // 
            this.tsb_SaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_SaveAs.Image = global::MiMFa.Properties.Resources.SaveAs_Green;
            this.tsb_SaveAs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_SaveAs.Margin = new System.Windows.Forms.Padding(0);
            this.tsb_SaveAs.Name = "tsb_SaveAs";
            this.tsb_SaveAs.Padding = new System.Windows.Forms.Padding(5);
            this.tsb_SaveAs.Size = new System.Drawing.Size(34, 34);
            this.tsb_SaveAs.Text = "Save As";
            this.tsb_SaveAs.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // tsb_Print
            // 
            this.tsb_Print.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsb_Print.Image = global::MiMFa.Properties.Resources.Print;
            this.tsb_Print.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsb_Print.Margin = new System.Windows.Forms.Padding(0);
            this.tsb_Print.Name = "tsb_Print";
            this.tsb_Print.Padding = new System.Windows.Forms.Padding(5);
            this.tsb_Print.Size = new System.Drawing.Size(34, 34);
            this.tsb_Print.Text = "Print";
            this.tsb_Print.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
            // 
            // toolStripButton10
            // 
            this.toolStripButton10.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton10.Image = global::MiMFa.Properties.Resources.Zoom_In;
            this.toolStripButton10.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton10.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripButton10.Name = "toolStripButton10";
            this.toolStripButton10.Padding = new System.Windows.Forms.Padding(5);
            this.toolStripButton10.Size = new System.Drawing.Size(34, 34);
            this.toolStripButton10.Text = "Zoom In";
            this.toolStripButton10.Click += new System.EventHandler(this.zoomInToolStripMenuItem_Click);
            // 
            // toolStripButton9
            // 
            this.toolStripButton9.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButton9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton9.Image = global::MiMFa.Properties.Resources.Zoom_Out;
            this.toolStripButton9.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton9.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripButton9.Name = "toolStripButton9";
            this.toolStripButton9.Padding = new System.Windows.Forms.Padding(5);
            this.toolStripButton9.Size = new System.Drawing.Size(34, 34);
            this.toolStripButton9.Text = "Zoom Out";
            this.toolStripButton9.Click += new System.EventHandler(this.zoomOutToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Padding = new System.Windows.Forms.Padding(5);
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 34);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::MiMFa.Properties.Resources.Magnifier;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Padding = new System.Windows.Forms.Padding(5);
            this.toolStripButton3.Size = new System.Drawing.Size(34, 34);
            this.toolStripButton3.Text = "Find";
            this.toolStripButton3.Click += new System.EventHandler(this.findToolStripMenuItem_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::MiMFa.Properties.Resources.Repace;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Padding = new System.Windows.Forms.Padding(5);
            this.toolStripButton2.Size = new System.Drawing.Size(34, 34);
            this.toolStripButton2.Text = "Replace";
            this.toolStripButton2.Click += new System.EventHandler(this.replaceToolStripMenuItem_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::MiMFa.Properties.Resources.Marker;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Padding = new System.Windows.Forms.Padding(5);
            this.toolStripButton1.Size = new System.Drawing.Size(34, 34);
            this.toolStripButton1.Text = "Go to";
            this.toolStripButton1.Click += new System.EventHandler(this.goToLineToolStripMenuItem_Click);
            // 
            // commentsToolStripMenuItem
            // 
            this.commentsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.commentUnCommentToolStripMenuItem,
            this.commentToolStripMenuItem,
            this.unCommentToolStripMenuItem,
            this.removeCommentsToolStripMenuItem});
            this.commentsToolStripMenuItem.Name = "commentsToolStripMenuItem";
            this.commentsToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.commentsToolStripMenuItem.Text = "Comments";
            // 
            // removeCommentsToolStripMenuItem
            // 
            this.removeCommentsToolStripMenuItem.Name = "removeCommentsToolStripMenuItem";
            this.removeCommentsToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.removeCommentsToolStripMenuItem.Text = "Remove Comments";
            this.removeCommentsToolStripMenuItem.Click += new System.EventHandler(this.removeCommentsToolStripMenuItem_Click);
            // 
            // commentUnCommentToolStripMenuItem
            // 
            this.commentUnCommentToolStripMenuItem.Name = "commentUnCommentToolStripMenuItem";
            this.commentUnCommentToolStripMenuItem.ShortcutKeyDisplayString = "Ctrl+/";
            this.commentUnCommentToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.OemQuestion)));
            this.commentUnCommentToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.commentUnCommentToolStripMenuItem.Text = "Toggle Comments";
            this.commentUnCommentToolStripMenuItem.Click += new System.EventHandler(this.commentUnCommentToolStripMenuItem_Click);
            // 
            // lettersToolStripMenuItem
            // 
            this.lettersToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.upperCaseToolStripMenuItem,
            this.lowerCasesToolStripMenuItem,
            this.uPPERCASEToolStripMenuItem1,
            this.sentenceCaseToolStripMenuItem,
            this.titleCaseToolStripMenuItem});
            this.lettersToolStripMenuItem.Name = "lettersToolStripMenuItem";
            this.lettersToolStripMenuItem.Size = new System.Drawing.Size(228, 22);
            this.lettersToolStripMenuItem.Text = "Letters";
            // 
            // lowerCasesToolStripMenuItem
            // 
            this.lowerCasesToolStripMenuItem.Name = "lowerCasesToolStripMenuItem";
            this.lowerCasesToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.lowerCasesToolStripMenuItem.Text = "lower case";
            this.lowerCasesToolStripMenuItem.Click += new System.EventHandler(this.lowerCasesToolStripMenuItem_Click);
            // 
            // upperCaseToolStripMenuItem
            // 
            this.upperCaseToolStripMenuItem.Name = "upperCaseToolStripMenuItem";
            this.upperCaseToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Shift) 
            | System.Windows.Forms.Keys.U)));
            this.upperCaseToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.upperCaseToolStripMenuItem.Text = "Toggle Case";
            // 
            // uPPERCASEToolStripMenuItem1
            // 
            this.uPPERCASEToolStripMenuItem1.Name = "uPPERCASEToolStripMenuItem1";
            this.uPPERCASEToolStripMenuItem1.Size = new System.Drawing.Size(211, 22);
            this.uPPERCASEToolStripMenuItem1.Text = "UPPER CASE";
            this.uPPERCASEToolStripMenuItem1.Click += new System.EventHandler(this.uPPERCASEToolStripMenuItem1_Click);
            // 
            // sentenceCaseToolStripMenuItem
            // 
            this.sentenceCaseToolStripMenuItem.Name = "sentenceCaseToolStripMenuItem";
            this.sentenceCaseToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.sentenceCaseToolStripMenuItem.Text = "Sentence case";
            this.sentenceCaseToolStripMenuItem.Click += new System.EventHandler(this.sentenceCaseToolStripMenuItem_Click);
            // 
            // titleCaseToolStripMenuItem
            // 
            this.titleCaseToolStripMenuItem.Name = "titleCaseToolStripMenuItem";
            this.titleCaseToolStripMenuItem.Size = new System.Drawing.Size(211, 22);
            this.titleCaseToolStripMenuItem.Text = "Title Case";
            this.titleCaseToolStripMenuItem.Click += new System.EventHandler(this.titleCaseToolStripMenuItem_Click);
            // 
            // commentToolStripMenuItem
            // 
            this.commentToolStripMenuItem.Name = "commentToolStripMenuItem";
            this.commentToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.commentToolStripMenuItem.Text = "Comment";
            this.commentToolStripMenuItem.Click += new System.EventHandler(this.commentToolStripMenuItem_Click);
            // 
            // unCommentToolStripMenuItem
            // 
            this.unCommentToolStripMenuItem.Name = "unCommentToolStripMenuItem";
            this.unCommentToolStripMenuItem.Size = new System.Drawing.Size(210, 22);
            this.unCommentToolStripMenuItem.Text = "UnComment";
            this.unCommentToolStripMenuItem.Click += new System.EventHandler(this.unCommentToolStripMenuItem_Click);
            // 
            // EditDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Editor);
            this.Controls.Add(this.MainStripMenu);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "EditDialog";
            this.Text = "MiMFa Editor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditDialog_FormClosing);
            this.ContextStripMenu.ResumeLayout(false);
            this.MainStripMenu.ResumeLayout(false);
            this.MainStripMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public MiMFa.Controls.WinForm.Editor.EditCodeBox Editor;
        public System.Windows.Forms.ContextMenuStrip ContextStripMenu;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem toggleCommentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleCaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem redoToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pasteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem showIntelliCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem zoomInToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoomOutToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton3;
        private System.Windows.Forms.ToolStripMenuItem mapViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rullerViewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem intelliCodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wrapModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton4;
        private System.Windows.Forms.ToolStripMenuItem findToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem replaceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem goToLineToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addHintToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearHintsToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton tsb_Redo;
        private System.Windows.Forms.ToolStripButton tsb_Undo;
        private System.Windows.Forms.ToolStripButton tsb_Reload;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton tsb_New;
        private System.Windows.Forms.ToolStripButton tsb_Open;
        private System.Windows.Forms.ToolStripButton tsb_Save;
        private System.Windows.Forms.ToolStripButton tsb_SaveAs;
        private System.Windows.Forms.ToolStripButton tsb_Print;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        public System.Windows.Forms.ToolStrip MainStripMenu;
        private System.Windows.Forms.ToolStripButton toolStripButton10;
        private System.Windows.Forms.ToolStripButton toolStripButton9;
        private System.Windows.Forms.ToolStripMenuItem scrollBarToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem commentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem commentUnCommentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeCommentsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lettersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem upperCaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem lowerCasesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem uPPERCASEToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem sentenceCaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem titleCaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem commentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unCommentToolStripMenuItem;
    }
}

