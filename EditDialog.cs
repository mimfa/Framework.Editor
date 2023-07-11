using MiMFa;
using MiMFa.Controls.WinForm.Editor;
using MiMFa.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiMFa.UIL.Editor
{
    public partial class EditDialog : Form
    {
        public string Path = null;

        public event EventHandler<TextChangedEventArgs> TextChanged;
        public event EventHandler SavedAs = (s, o) => { };
        public event EventHandler Saved = (s, o) => { };
        public event EventHandler Opened = (s, o) => { };
        public event EventHandler Newed = (s, o) => { };

        public EditDialog()
        {
            InitializeComponent();
            Default.Normalize(this);
            MainStripMenu.Renderer = new MiMFa.Controls.WinForm.Menu.ToolStripRender();
            TextChanged = EditBoxTextChanged;
            Editor.EditBox.TextChanged += (s,o)=>TextChanged(s, o);
        }
        public EditDialog(string path) : this()
        {
            if(!(
                tsb_SaveAs.Visible =
                tsb_New.Visible =
                tsb_Open.Visible =
                saveAsToolStripMenuItem.Visible = 
                openToolStripMenuItem.Visible =
                newToolStripMenuItem.Visible =
                string.IsNullOrWhiteSpace(path)))
                Open(path);
        }

        public void New()
        {
            Editor.EditBox.Clear();
            Path = null;
            Text = "MiMFa Editor";
            Newed(this, EventArgs.Empty);
        }
        public string Open()
        {
            string path = Open(DialogService.OpenFile(Path));
            return path;
        }
        public string Open(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;
            Editor.EditBox.OpenFile(path);
            Path = path;
            Text = "MiMFa Editor - " + System.IO.Path.GetFileName(Path);
            EditBoxTextChanged(this, EventArgs.Empty);
            Opened(this, EventArgs.Empty);
            return path;
        }
        public string SaveAs()
        {
            string path = Save(DialogService.SaveFile(Path));
            if(!string.IsNullOrWhiteSpace(path)) SavedAs(this, EventArgs.Empty);
            return path;
        }
        public string Save()
        {
            if (string.IsNullOrWhiteSpace(Path)) return SaveAs();
            return Save(Path);
        }
        public string Save(string path)
        {
            if (string.IsNullOrWhiteSpace(path)) return null;
            Editor.EditBox.SaveToFile(path, Encoding.UTF8);
            Path = path;
            Text = "MiMFa Editor - " + System.IO.Path.GetFileName(Path);
            EditBoxTextChanged(this, EventArgs.Empty);
            Saved(this, EventArgs.Empty);
            return path;
        }

        private void EditBoxTextChanged(object sender, EventArgs e)
        {
            undoToolStripMenuItem.Enabled =
            tsb_Undo.Enabled =
            Editor.EditBox.UndoEnabled;
            redoToolStripMenuItem.Enabled =
            tsb_Redo.Enabled =
            Editor.EditBox.RedoEnabled;
            tsb_Save.Enabled =
            tsb_Reload.Enabled =
            !string.IsNullOrWhiteSpace(Path) &&
            Editor.EditBox.UndoEnabled;
        }
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogService.Warning("Are you sure you want to clear everything?") == true)
                New();
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Open();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Path))
                Save(Path);
            else saveAsToolStripMenuItem_Click(sender, e);
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveAs();
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void tsb_Refresh_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(Path))
                Open(Path);
        }
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.EditBox.Undo();
        }
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.EditBox.Redo();
        }
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.EditBox.Copy();
        }
        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.EditBox.Cut();
        }
        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.EditBox.Paste();
        }

        private void mapViewToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Editor.MapBox.Visible = mapViewToolStripMenuItem.Checked;
        }
        private void rullerViewToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Editor.RulerBox.Visible = rullerViewToolStripMenuItem.Checked;
        }
        private void intelliCodeToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Editor.IntelliCode.Enabled = intelliCodeToolStripMenuItem.Checked;
        }
        private void wrapModeToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Editor.EditBox.WordWrap = wrapModeToolStripMenuItem.Checked;
        }
        private void scrollBarToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            Editor.EditBox.ShowScrollBars = scrollBarToolStripMenuItem.Checked;
        }
        private void showIntelliCodeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.IntelliCode.Open();
        }

        private void commentUnCommentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.EditBox.ToggleComment();
        }
        private void upperCaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.EditBox.ToggleCase();
        }

        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.EditBox.ShowFindDialog();
        }
        private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.EditBox.ShowReplaceDialog();
        }

        private void goToLineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.EditBox.ShowGoToDialog();
        }
        private void addHintToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.EditBox.AddHint(DialogService.Prompt("Type your hint!"));
        }
        private void clearHintsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.EditBox.ClearHints();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor.EditBox.Print();
        }

        private void zoomInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(Editor.EditBox.Zoom < 1000) Editor.EditBox.Zoom *= 2;
        }
        private void zoomOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Editor.EditBox.Zoom > 1) Editor.EditBox.Zoom /= 2 ;
        }

        private void EditDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Editor.EditBox.UndoEnabled)
            {
                var b = DialogService.Warning("There you have some unsaved changes," + Environment.NewLine + "Do you want to save them before?");
                if (b == true) Save();
                else if (!b.HasValue) e.Cancel = true;
            }
        }

    }
}
