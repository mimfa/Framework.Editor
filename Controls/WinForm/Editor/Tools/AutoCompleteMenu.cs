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
            if (Opening != null)
                Opening(this, args);
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

    [ToolboxItem(false)]
    public class AutoCompleteListView : UserControl, IDisposable
    {
        public event EventHandler FocussedItemIndexChanged;

        internal List<Item> visibleItems;
        public GenericEventHandler<AutoCompleteListView, string, IEnumerable<Item>> SourceItemsHandler = (s,e)=>new Item[0];
        int focussedItemIndex = 0;
        int hoveredItemIndex = -1;

        private int ItemHeight
        {
            get { return Font.Height + 2; }
        }

        AutoCompleteMenu Menu { get { return Parent as AutoCompleteMenu; } }
        int oldItemCount = 0;
        [Description("Target FastColoredTextBox")]
        public EditBox Target
        {
            get { return _Target; }
            set
            {
                _Target = value;
                if (value != null)
                {
                    _Target.KeyDown += new KeyEventHandler(tb_KeyDown);
                    _Target.SelectionChanged += new EventHandler(tb_SelectionChanged);
                    _Target.KeyPressed += new KeyPressEventHandler(tb_KeyPressed);

                    Form form = _Target.FindForm();
                    if (form != null)
                    {
                        form.LocationChanged += delegate { SafetyClose(); };
                        form.ResizeBegin += delegate { SafetyClose(); };
                        form.FormClosing += delegate { SafetyClose(); };
                        form.LostFocus += delegate { SafetyClose(); };
                    }

                    _Target.LostFocus += (o, e) =>
                    {
                        if (Menu != null && !Menu.IsDisposed)
                            if (!Menu.Focused)
                                SafetyClose();
                    };

                    _Target.Scroll += delegate { SafetyClose(); };

                    this.VisibleChanged += (o, e) =>
                    {
                        if (this.Visible)
                            DoSelectedVisible();
                    };
                }
            }
        }
        private EditBox _Target = null;

        internal ToolTip toolTip = new ToolTip();
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();

        internal bool AllowTabKey { get; set; }
        public ImageList ImageList { get; set; }
        internal int AppearInterval { get { return timer.Interval; } set { timer.Interval = value; } }
        public int Count
        {
            get { return visibleItems.Count; }
        }
        public SortResult SortingMode { get; set; } = SortResult.ByTitle;
        public bool ToolTipRealTimeShow { get; set; }
        public Size ToolTipMaxSize { get; set; }
        public Color SelectedColor { get; set; }
        public Color HoveredColor { get; set; }
        public int FocussedItemIndex
        {
            get { return focussedItemIndex; }
            set
            {
                if (focussedItemIndex != value)
                {
                    focussedItemIndex = value;
                    if (FocussedItemIndexChanged != null)
                        FocussedItemIndexChanged(this, EventArgs.Empty);
                }
            }
        }

        public Item FocussedItem
        {
            get
            {
                if (FocussedItemIndex >= 0 && focussedItemIndex < visibleItems.Count)
                    return visibleItems[focussedItemIndex];
                return null;
            }
            set
            {
                FocussedItemIndex = visibleItems.IndexOf(value);
            }
        }

        internal AutoCompleteListView(EditBox tb=null)
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            base.Font = new Font(FontFamily.GenericSansSerif, 9);
            visibleItems = new List<Item>();
            VerticalScroll.SmallChange = ItemHeight;
            MaximumSize = new Size(Size.Width, 180);
            AppearInterval = 500;
            timer.Tick += new EventHandler(timer_Tick);
            SelectedColor = Color.Orange;
            HoveredColor = Color.Red;
            toolTip.Popup += ToolTip_Popup;

            Target = tb;
        }

        private void ToolTip_Popup(object sender, PopupEventArgs e)
        {
            if (ToolTipMaxSize.Height > 0 && ToolTipMaxSize.Width > 0)
                e.ToolTipSize = ToolTipMaxSize;
        }

        protected override void Dispose(bool disposing)
        {
            if (toolTip != null)
            {
                toolTip.Popup -= ToolTip_Popup;
                toolTip.Dispose();
            }
            if (_Target != null)
            {
                _Target.KeyDown -= tb_KeyDown;
                _Target.KeyPressed -= tb_KeyPressed;
                _Target.SelectionChanged -= tb_SelectionChanged;
            }

            if (timer != null)
            {
                timer.Stop();
                timer.Tick -= timer_Tick;
                timer.Dispose();
            }

            base.Dispose(disposing);
        }

        void SafetyClose()
        {
            if (Menu != null && !Menu.IsDisposed)
                Menu.Close();
        }

        void tb_KeyPressed(object sender, KeyPressEventArgs e)
        {
            bool backspaceORdel = e.KeyChar == '\b' || e.KeyChar == 0xff;

            /*
            if (backspaceORdel)
                prevSelection = tb.Selection.Start;*/

            if (Menu.Visible && !backspaceORdel)
                DoAutoComplete(false);
            else
                ResetTimer(timer);
        }

        void timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            DoAutoComplete(false);
        }

        void ResetTimer(System.Windows.Forms.Timer timer)
        {
            timer.Stop();
            timer.Start();
        }

        internal void DoAutoComplete()
        {
            DoAutoComplete(false);
        }

        internal void DoAutoComplete(bool forced)
        {
            if (!Menu.Enabled)
            {
                Menu.Close();
                return;
            }

            visibleItems.Clear();
            FocussedItemIndex = -1;
            VerticalScroll.Value = 0;
            //some magic for update scrolls
            AutoScrollMinSize -= new Size(1, 0);
            AutoScrollMinSize += new Size(1, 0);
            //get fragment around caret
            bool rtl = RightToLeft == RightToLeft.Yes ? true : false;
            Range fragment = _Target.Selection.GetFragment(Menu.SearchPattern, !rtl, rtl);
            //if (!string.IsNullOrEmpty(fragment.Text) && fragment.Text.LastIndexOfAny(Menu.SilentChars) == fragment.Text.Length -1)
            //{
            //    string s = fragment.Text.TrimEnd(Menu.SilentChars);
            //    if (s.LastIndexOfAny(Menu.ActorChars) < s.Length - 1)
            //        return;
            //}
            string text = Regex.Replace(fragment.Text,Menu.NormalizationPattern, "");
            string normaledtext = text.ToLower();
            //calc screen point for popup menu
            Point point = _Target.PlaceToPoint(fragment.End);
            point.Offset(2, _Target.CharHeight);
            //
            if (forced || (text.Length >= Menu.MinFragmentLength 
                && _Target.Selection.IsEmpty /*pops up only if selected range is empty*/
                && (_Target.Selection.Start > fragment.Start || text.Length == 0/*pops up only if caret is after first letter*/)))
            {
                Menu.Fragment = fragment;
                bool importantfoundSelected = false;
                bool foundSelected = false;
                Item aci = null;

                //build popup menu
                foreach (var item in SourceItemsHandler(this, text).Distinct(new ItemComparer<Item>()))
                {
                    item.Parent = Menu;
                    CompareResult res = item.Compare(text, normaledtext);
                    if(res != CompareResult.Hidden)
                        visibleItems.Add(item);
                    if (!importantfoundSelected)
                        if (res == CompareResult.VisibleAndSelected && !foundSelected)
                        {
                            foundSelected = true;
                            FocussedItemIndex = visibleItems.Count - 1;
                            aci = item;
                        }
                        else if (res == CompareResult.ExactVisibleAndSelected)
                        {
                            importantfoundSelected = true;
                            FocussedItemIndex = visibleItems.Count - 1;
                            aci = item;
                        }
                }
                //if (foundSelected)
                //{
                //    AdjustScroll();
                //    DoSelectedVisible();
                //}
                switch (SortingMode)
                {
                    case SortResult.ByTitle:
                        visibleItems.Sort((v1, v2) => v1.Title.CompareTo(v2.Title));
                        break;
                    case SortResult.ByText:
                        visibleItems.Sort((v1, v2) => v1.Text.CompareTo(v2.Text));
                        break;
                    default:
                        break;
                }
                FocussedItem = aci;
            }

            //show popup menu
            if (Count > 0)
            {
                if (!Menu.Visible)
                {
                    CancelEventArgs args = new CancelEventArgs();
                    Menu.OnOpening(args);
                    if(!args.Cancel)
                        Menu.Show(_Target, point);
                }

                DoSelectedVisible();
                Invalidate();
            }
            else
                Menu.Close();
        }

        void tb_SelectionChanged(object sender, EventArgs e)
        {
            /*
            FastColoredTextBox tb = sender as FastColoredTextBox;
            
            if (Math.Abs(prevSelection.iChar - tb.Selection.Start.iChar) > 1 ||
                        prevSelection.iLine != tb.Selection.Start.iLine)
                Menu.Close();
            prevSelection = tb.Selection.Start;*/
            if (Menu.Visible)
            {
                bool needClose = false;

                if (!_Target.Selection.IsEmpty)
                    needClose = true;
                else
                    if (!Menu.Fragment.Contains(_Target.Selection.Start))
                    {
                        if (_Target.Selection.Start.LineIndex == Menu.Fragment.End.LineIndex && _Target.Selection.Start.CharIndex == Menu.Fragment.End.CharIndex + 1)
                        {
                            //user press key at end of fragment
                            char c = _Target.Selection.CharBeforeStart;
                            if (!Regex.IsMatch(c.ToString(), Menu.SearchPattern))//check char
                                needClose = true;
                        }
                        else
                            needClose = true;
                    }

                if (needClose)
                    Menu.Close();
            }
            
        }

        void tb_KeyDown(object sender, KeyEventArgs e)
        {
            var tb = sender as EditBox;

            if (Menu.Visible)
                if (ProcessKey(e.KeyCode, e.Modifiers))
                    e.Handled = true;

            if (!Menu.Visible)
            {
                if (tb.HotkeysMapping.ContainsKey(e.KeyData) && tb.HotkeysMapping[e.KeyData] == FCTBAction.AutocompleteMenu)
                {
                    DoAutoComplete();
                    e.Handled = true;
                }
                else
                {
                    if (e.KeyCode == Keys.Escape && timer.Enabled)
                        timer.Stop();
                }
            }
        }

        void AdjustScroll()
        {
            if (oldItemCount == visibleItems.Count)
                return;

            int needHeight = ItemHeight * visibleItems.Count + 1;
            Height = Math.Min(needHeight, MaximumSize.Height);
            Menu.CalcSize();

            AutoScrollMinSize = new Size(0, needHeight);
            oldItemCount = visibleItems.Count;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            AdjustScroll();

            var itemHeight = ItemHeight;
            int startI = VerticalScroll.Value / itemHeight - 1;
            int finishI = (VerticalScroll.Value + ClientSize.Height) / itemHeight + 1;
            startI = Math.Max(startI, 0);
            finishI = Math.Min(finishI, visibleItems.Count);
            int y = 0;
            int leftPadding = 18;
            for (int i = startI; i < finishI; i++)
            {
                y = i * itemHeight - VerticalScroll.Value;

                var item = visibleItems[i];

                if(item.BackColor != Color.Transparent)
                using (var brush = new SolidBrush(item.BackColor))
                    e.Graphics.FillRectangle(brush, 1, y, ClientSize.Width - 1 - 1, itemHeight - 1);

                if (ImageList != null && visibleItems[i].ImageIndex >= 0)
                    e.Graphics.DrawImage(ImageList.Images[item.ImageIndex], 1, y);

                if (i == FocussedItemIndex)
                using (var selectedBrush = new LinearGradientBrush(new Point(0, y - 3), new Point(0, y + itemHeight), Color.Transparent, SelectedColor))
                using (var pen = new Pen(SelectedColor))
                {
                    e.Graphics.FillRectangle(selectedBrush, leftPadding, y, ClientSize.Width - 1 - leftPadding, itemHeight - 1);
                    e.Graphics.DrawRectangle(pen, leftPadding, y, ClientSize.Width - 1 - leftPadding, itemHeight - 1);
                }

                if (i == hoveredItemIndex)
                using(var pen = new Pen(HoveredColor))
                    e.Graphics.DrawRectangle(pen, leftPadding, y, ClientSize.Width - 1 - leftPadding, itemHeight - 1);

                using (var brush = new SolidBrush(item.ForeColor != Color.Transparent ? item.ForeColor : ForeColor))
                    e.Graphics.DrawString(item.ToString(), Font, brush, leftPadding, y);
            }
        }

        protected override void OnScroll(ScrollEventArgs se)
        {
            base.OnScroll(se);
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                FocussedItemIndex = PointToItemIndex(e.Location);
                DoSelectedVisible();
                if (!ToolTipRealTimeShow) SetToolTip();
                Invalidate();
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            FocussedItemIndex = PointToItemIndex(e.Location);
            Invalidate();
            OnSelecting();
        }

        internal virtual void OnSelecting(bool inject = false)
        {
            if (FocussedItemIndex < 0 || FocussedItemIndex >= visibleItems.Count)
                return;
            _Target.TextSource.Manager.BeginAutoUndoCommands();
            try
            {
                Item item = FocussedItem;
                SelectingEventArgs args = new SelectingEventArgs()
                {
                    Item = item,
                    SelectedIndex = FocussedItemIndex
                };

                if(inject) Menu.OnInjecting(args);
                else Menu.OnSelecting(args);

                if (args.Cancel)
                {
                    FocussedItemIndex = args.SelectedIndex;
                    Invalidate();
                    return;
                }

                if (!args.Handled)
                {
                    var fragment = Menu.Fragment;
                    DoAutoComplete(item, fragment, inject);
                }

                Menu.Close();
                //
                SelectedEventArgs args2 = new SelectedEventArgs()
                {
                    Item = item,
                    Tb = Menu.Fragment.tb
                };
                if (inject)
                {
                    item.OnInjected(Menu, args2);
                    Menu.OnInjected(args2);
                }
                else
                {
                    item.OnSelected(Menu, args2);
                    Menu.OnSelected(args2);
                }
            }
            finally
            {
                _Target.TextSource.Manager.EndAutoUndoCommands();
            }
        }

        private void DoAutoComplete(Item item, Range fragment, bool inject = false)
        {
            int index = Math.Max(0,Regex.Match(fragment.Text, Menu.ReplacePointPattern).Index);
            string newText = Regex.Replace(fragment.Text, Menu.ReplacePointPattern,"");
            string txt = inject ? item.GetTextForInject() : item.GetTextForReplace();
            newText = newText.Insert(index, txt);
            index += txt.Length - newText.Length;

            //replace text of fragment
            var tb = fragment.tb;

            tb.BeginAutoUndo();
            tb.TextSource.Manager.ExecuteCommand(new SelectCommand(tb.TextSource));
            if (tb.Selection.ColumnSelectionMode)
            {
                var start = tb.Selection.Start;
                var end = tb.Selection.End;
                start.CharIndex = fragment.Start.CharIndex;
                end.CharIndex = fragment.End.CharIndex;
                tb.Selection.Start = start;
                tb.Selection.End = end;
            }
            else
            {
                tb.Selection.Start = fragment.Start;
                tb.Selection.End = fragment.End;
            }
            tb.InsertText(newText);
            fragment.End = tb.Selection.End;
            tb.TextSource.Manager.ExecuteCommand(new SelectCommand(tb.TextSource));
            tb.EndAutoUndo();
            tb.SelectionStart += index;
            tb.Focus();
        }

        int PointToItemIndex(Point p)
        {
            return (p.Y + VerticalScroll.Value) / ItemHeight;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            ProcessKey(keyData, Keys.None);
            
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private bool ProcessKey(Keys keyData, Keys keyModifiers)
        {
            if (keyModifiers == Keys.None)
            switch (keyData)
            {
                case Keys.Down:
                    SelectNext(+1);
                    return true;
                case Keys.PageDown:
                    SelectNext(+10);
                    return true;
                case Keys.Up:
                    SelectNext(-1);
                    return true;
                case Keys.PageUp:
                    SelectNext(-10);
                    return true;
                //case Keys.Left:
                //case Keys.Right:
                //    SetToolTip();
                //    return true;
                //case Keys.OemPeriod:
                //case Keys.OemOpenBrackets:
                //case Keys.OemCloseBrackets:
                //case Keys.OemQuestion:
                //case Keys.OemQuotes:
                //case Keys.OemPipe:
                //case Keys.Oemplus:
                //case Keys.Oemcomma:
                //case Keys.OemMinus:
                //case Keys.OemBackslash:
                ////case Keys.OemSemicolon:
                //case Keys.Oemtilde:
                //case Keys.Subtract:
                ////case Keys.Space:
                //    OnSelecting(false);
                //    return false;
                case Keys.Enter:
                    OnSelecting(false);
                    return true;
                case Keys.Tab:
                    if (!AllowTabKey)
                        break;
                    OnSelecting(true);
                    return true;
                case Keys.Escape:
                    Menu.Close();
                    return true;
                //case Keys.Space:
                //    Menu.Close();
                //    return true;
                default:
                Menu.Close();
                return false;
            }

            return false;
        }

        public void SelectNext(int shift)
        {
            FocussedItemIndex = Math.Max(0, Math.Min(FocussedItemIndex + shift, visibleItems.Count - 1));
            DoSelectedVisible();
            //
            Invalidate();
        }

        private void DoSelectedVisible()
        {
            if (FocussedItemIndex < 0) return;
            if (ToolTipRealTimeShow) SetToolTip();

            var y = FocussedItemIndex * ItemHeight - VerticalScroll.Value;
            if (y < 0)
                VerticalScroll.Value = FocussedItemIndex * ItemHeight;
            if (y > ClientSize.Height - ItemHeight)
                VerticalScroll.Value = Math.Min(VerticalScroll.Maximum, FocussedItemIndex * ItemHeight - ClientSize.Height + ItemHeight);
            //some magic for update scrolls
            AutoScrollMinSize -= new Size(1, 0);
            AutoScrollMinSize += new Size(1, 0);
        }

        public void SetToolTip()
        {
            if (FocussedItemIndex > -1 && FocussedItem != null) SetToolTip(FocussedItem);
        }
        private void SetToolTip(Item autocompleteItem)
        {
            var title = autocompleteItem.ToolTipTitle;
            var text = autocompleteItem.ToolTipText;

            if (string.IsNullOrEmpty(title))
            {
                toolTip.ToolTipTitle = null;
                toolTip.SetToolTip(this, null);
                return;
            }

            if (this.Parent != null)
            {
                IWin32Window window = this.Parent ?? this;
                Point location;
                if ((this.PointToScreen(this.Location).X + ToolTipMaxSize.Width + 105) < Screen.FromControl(this.Parent).WorkingArea.Right)
                    location = new Point(Right + 5, 0);
                else
                    location = new Point(Left - 105 - MaximumSize.Width, 0);

                //if (string.IsNullOrEmpty(text))
                //{
                //    toolTip.ToolTipTitle = null;
                //    toolTip.Show(title, window, location.X, location.Y, toolTip.AutoPopDelay);
                //}
                //else
                //{
                toolTip.ToolTipTitle = title;
                toolTip.Show(text, window, location.X, location.Y, toolTip.AutoPopDelay);
                //}
            }
        }

        public void Set(GenericEventHandler<AutoCompleteListView, string, IEnumerable<Item>> items)
        {
            SourceItemsHandler = items;
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
