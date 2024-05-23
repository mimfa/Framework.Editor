using MiMFa.Controls.WinForm.Editor.Model.AutoComplete;
using MiMFa.Controls.WinForm.Editor.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MiMFa.Controls.WinForm.Editor
{
    public partial class EditCodeBox : UserControl
    {
        public event EventHandler<TextChangedEventArgs> TextChanged = (s, o) => { };
        [DefaultValue(true)]
        public bool IsEditBoxSource { get; set; } = true;
        [DefaultValue("(?<=[\\W]|^)([A-z$_]\\w*\\.?)+")]
        public string SourcePattern { get; set; } = "(?<=[\\W]|^)([A-z$_]\\w*\\.?)+";
        public List<Item> SourceItems = new List<Item>();
        public override string Text { get => EditBox.Text; set => EditBox.Text = value; }
        public bool HasEditBox { get => EditBox.Visible; set => EditBox.Visible = value; }
        public bool HasMapBox { get => MapBox.Visible; set => MapBox.Visible = value; }
        public bool HasRulerBox { get => RulerBox.Visible; set => RulerBox.Visible = value; }
        public bool HasLineNumbers { get => EditBox.ShowLineNumbers; set => EditBox.ShowLineNumbers = value; }
        public bool ShowScrollBars { get => EditBox.ShowScrollBars; set => EditBox.ShowScrollBars = value; }
        public Model.Syntax.Language Language { get => EditBox.Language; set => EditBox.Language = value; }
        public bool AutoDetectLanguage { get => EditBox.AutoDetectLanguage; set => EditBox.AutoDetectLanguage = value; }

        public EditCodeBox()
        {
            InitializeComponent();
            IntelliCode.Init(EditBox);
            IntelliCode.ListView.Set(AutoCompleteHandler);
            IntelliCode.ListView.AutoSize = true;
            EditBox.TextChangedDelayed += (s,o)=>TextChanged(s, o);
            EditBox.AutoDetectLanguage = true;
        }

        private IEnumerable<Item> AutoCompleteHandler(AutoCompleteListView sender, string arg)
        {
            if (IsEditBoxSource)
                foreach (var item in GetSourceItems(EditBox.Text))
                    if(arg != item.Text) yield return item;
            foreach (var item in SourceItems)
                yield return item;
        }
        public IEnumerable<Item> GetSourceItems(Interpreters.IInterpreter frominterpreter)
        {
            if (frominterpreter == null) return new Item[0];
            List<string> strs = new List<string>();
            foreach (var item in frominterpreter.GetObjects())
                strs.Add(item.Key);
            return GetSourceItems(strs);
        }
        public IEnumerable<Item> GetSourceItems(string sampleCode)
        {
            if (string.IsNullOrWhiteSpace(sampleCode)) return new Item[0];
            List<string> strs = new List<string>();
            foreach (Match item in Regex.Matches(sampleCode, SourcePattern))
                strs.Add(item.Value);
            return GetSourceItems(strs);
        }
        public IEnumerable<Item> GetSourceItems(IEnumerable<string> items)
        {
            foreach (string item in items.Distinct())
                yield return new Item(item);
        }
        public void SetSourceItems(Interpreters.IInterpreter frominterpreter)
        {
            SourceItems.Clear();
            AddSourceItems(frominterpreter);
        }
        public void SetSourceItems(string fromSampleCode)
        {
            SourceItems.Clear();
            AddSourceItems(fromSampleCode);
        }
        public void SetSourceItems(IEnumerable<string> fromItems)
        {
            SourceItems.Clear();
            AddSourceItems(fromItems);
        }
        public void SetSourceItems(IEnumerable<Item> fromItems)
        {
            SourceItems.Clear();
            AddSourceItems(fromItems);
        }
        public void AddSourceItems(Interpreters.IInterpreter frominterpreter)
        {
            SourceItems.AddRange(GetSourceItems(frominterpreter));
        }
        public void AddSourceItems(string fromSampleCode)
        {
            SourceItems.AddRange(GetSourceItems(fromSampleCode));
        }
        public void AddSourceItems(IEnumerable<string> fromItems)
        {
            SourceItems.AddRange(GetSourceItems(fromItems));
        }
        public void AddSourceItems(IEnumerable<Item> fromItems)
        {
            SourceItems.AddRange(fromItems);
        }
    }
}
