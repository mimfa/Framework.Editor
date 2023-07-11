using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Text.RegularExpressions;
using MiMFa.Controls.WinForm.Editor.Model;
using MiMFa.Controls.WinForm.Editor.Model.AutoComplete;
using System.Linq;
using MiMFa.General;

namespace MiMFa.Controls.WinForm.Editor.Tools
{
    public enum SortResult
    {
        ByTitle,
        ByText,
        None
    }

    /// <summary>
    /// Popup menu for autocomplete
    /// </summary>
    [Browsable(true)]
    public class AutoCompleteMenu : ToolStripDropDown, IDisposable
    {
        public AutoCompleteListView ListView = new AutoCompleteListView();
        public ToolStripControlHost Host;
        public Range Fragment { get; internal set; }

        [Description("Target FastColoredTextBox")]
        public EditBox Target
        {
            get { return _Target; }
            set
            {
                _Target = value;
                if (value != null)
                {
                    Init(_Target);
                }
            }
        }
        private EditBox _Target=null;

        /// <summary>
        /// Regex pattern for serach fragment around caret
        /// </summary>
        public string SearchPattern { get; set; } = @"[\w\.\s]+";
        /// <summary>
        /// Regex pattern for replacing in fragment
        /// </summary>
        public string ReplacePointPattern { get; set; } = @"[\w\.]";
        /// <summary>
        /// Regex pattern for normalization fragment
        /// </summary>
        public string NormalizationPattern { get; set; } = @"[\s]+";
        public char[] SilentChars = new char[] { ' ', '\t', '\r', '\n' };
        public char[] ActorChars = new char[] { '.' };
        /// <summary>
        /// Minimum fragment length for popup
        /// </summary>
        public int MinFragmentLength { get; set; } 
        /// <summary>
        /// User selects item
        /// </summary>
        public event EventHandler<SelectingEventArgs> Selecting;
        /// <summary>
        /// It fires after item inserting
        /// </summary>
        public event EventHandler<SelectedEventArgs> Selected;
        /// <summary>
        /// User selects item
        /// </summary>
        public event EventHandler<SelectingEventArgs> Injecting;
        /// <summary>
        /// It fires after item inserting
        /// </summary>
        public event EventHandler<SelectedEventArgs> Injected;
        /// <summary>
        /// Occurs when popup menu is opening
        /// </summary>
        public new event EventHandler<CancelEventArgs> Opening;
        /// <summary>
        /// Allow TAB for select menu item
        /// </summary>
        public bool AllowTabKey { get { return ListView.AllowTabKey; } set { ListView.AllowTabKey = value; } }
        /// <summary>
        /// Interval of menu appear (ms)
        /// </summary>
        public int AppearInterval { get { return ListView.AppearInterval; } set { ListView.AppearInterval = value; } }

        /// <summary>
        /// Tooltip
        /// </summary>
        public ToolTip ToolTip
        {
            get { return Items.toolTip; }
            set { Items.toolTip = value; }
        }
        /// <summary>
        /// Tooltip will perm show and duration will be ignored
        /// </summary>
        public bool ToolTipRealTimeShow { get { return ListView.ToolTipRealTimeShow; } set { ListView.ToolTipRealTimeShow = value; } }
        public bool ToolTipShowAlways { get { return ListView.toolTip.ShowAlways; } set { ListView.toolTip.ShowAlways = value; } }
        /// <summary>
        /// Tooltip duration (ms)
        /// </summary>
        public int ToolTipDuration
        {
            get { return Items.toolTip.AutoPopDelay; }
            set { Items.toolTip.AutoPopDelay = value; }
        }
        public int ToolTipInitialDelay
        {
            get { return Items.toolTip.InitialDelay; }
            set { Items.toolTip.InitialDelay = value; }
        }
        /// <summary>
        /// Sets the max tooltip window size
        /// </summary>
        public Size ToolTipMaxSize { get { return ListView.ToolTipMaxSize; } set { ListView.ToolTipMaxSize = value; } }


        /// <summary>
        /// Back color of selected item
        /// </summary>
        [DefaultValue(typeof(SortResult), "ByTitle")]
        public SortResult SortingMode
        {
            get { return ListView.SortingMode; }
            set { ListView.SortingMode = value; }
        }

        /// <summary>
        /// Back color of selected item
        /// </summary>
        [DefaultValue(typeof(Color), "Orange")]
        public Color SelectedBackColor
        {
            get { return ListView.SelectedColor; }
            set { ListView.SelectedColor = value; }
        }

        /// <summary>
        /// Border color of hovered item
        /// </summary>
        [DefaultValue(typeof(Color), "Red")]
        public Color HoveredBackColor
        {
            get { return ListView.HoveredColor; }
            set { ListView.HoveredColor = value; }
        }
        public new Font Font
        {
            get { return ListView.Font; }
            set { ListView.Font = value; }
        }
        public new AutoCompleteListView Items
        {
            get { return ListView; }
        }

        /// <summary>
        /// Minimal size of menu
        /// </summary>
        public new Size MinimumSize
        {
            get { return Items.MinimumSize; }
            set { Items.MinimumSize = value; }
        }

        /// <summary>
        /// Image list of menu
        /// </summary>
        [Browsable(true)]
        [Description("Image list of menu")]
        public new ImageList ImageList
        {
            get { return Items.ImageList; }
            set { Items.ImageList = value; }
        }



        public AutoCompleteMenu()
        {
            // create a new popup and add the list view to it 
            AutoClose = false;
            AutoSize = false;
            Margin = Padding.Empty;
            Padding = Padding.Empty;
            BackColor = Color.White;
            MinFragmentLength = 2;
        }
        public AutoCompleteMenu(EditBox tb) : this()
        {
            Init(tb);
        }
        
        public void Init(EditBox tb)
        {
            ListView.Target = tb;
            Host = new ToolStripControlHost(ListView);
            Host.Margin = new Padding(2, 2, 2, 2);
            Host.Padding = Padding.Empty;
            Host.AutoSize = false;
            Host.AutoToolTip = false;
            CalcSize();
            base.Items.Add(Host);
            ListView.Parent = this;
        }

        new internal void OnOpening(CancelEventArgs args)
        {
            if (Opening != null) Opening(this, args);
        }

        public void Open()
        {
            if(ListView.Target == null) base.Show();
            base.Show(ListView.Target, ListView.Target.PlaceToPoint(ListView.Target.SelectionStartPlace));
        }

        public new void Close()
        {
            ListView.toolTip.Hide(ListView);
            base.Close();
        }

        internal void CalcSize()
        {
            Host.Size = ListView.Size;
            Size = new System.Drawing.Size(ListView.Size.Width + 4, ListView.Size.Height + 4);
        }

        public virtual void OnSelecting()
        {
            ListView.OnSelecting();
        }
        public virtual void OnInjecting()
        {
            ListView.OnSelecting(false);
        }

        public void SelectNext(int shift)
        {
            ListView.SelectNext(shift);
        }

        internal void OnSelecting(SelectingEventArgs args)
        {
            if (Selecting != null)
                Selecting(this, args);
        }
        public void OnSelected(SelectedEventArgs args)
        {
            if (Selected != null)
                Selected(this, args);
        }

        internal void OnInjecting(SelectingEventArgs args)
        {
            if (Injecting != null)
                Injecting(this, args);
        }
        public void OnInjected(SelectedEventArgs args)
        {
            if (Injected != null)
                Injected(this, args);
        }


        /// <summary>
        /// Shows popup menu immediately
        /// </summary>
        /// <param name="forced">If True - MinFragmentLength will be ignored</param>
        public void Show(bool forced)
        {
            Items.DoAutoComplete(forced);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (ListView != null && !ListView.IsDisposed)
                ListView.Dispose();
        }
    }

    public class SelectingEventArgs : EventArgs
    {
        public Item Item { get; internal set; }
        public bool Cancel {get;set;}
        public int SelectedIndex{get;set;}
        public bool Handled { get; set; }
    }

    public class SelectedEventArgs : EventArgs
    {
        public Item Item { get; internal set; }
        public EditBox Tb { get; set; }
    }
}
