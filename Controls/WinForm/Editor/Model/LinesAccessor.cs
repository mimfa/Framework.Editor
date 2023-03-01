using System;
using System.Collections.Generic;
using System.Text;

namespace MiMFa.Controls.WinForm.Editor.Model
{
    public class LinesAccessor : IList<string>
    {
        IList<Line> ts;

        public LinesAccessor(IList<Line> ts)
        {
            this.ts = ts;
        }

        public int IndexOf(string item)
        {
            for (int i = 0; i < ts.Count; i++)
                if (ts[i].Text == item)
                    return i;

            return -1;
        }


        public void RemoveAt(int index)
        {
            ts.RemoveAt(index);
        }

        public string this[int index]
        {
            get
            {
                return ts[index].Text;
            }
            set
            {
                ts[index].Text = value;
            }
        }

        public void Insert(int index, string item)
        {
            throw new NotImplementedException();
        }
        public void Add(string item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            ts.Clear();
        }

        public bool Contains(string item)
        {
            for (int i = 0; i < ts.Count; i++)
                if (ts[i].Text == item)
                    return true;

            return false;
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            for (int i = 0; i < ts.Count; i++)
                array[i + arrayIndex] = ts[i].Text;
        }

        public int Count
        {
            get { return ts.Count; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public bool Remove(string item)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<string> GetEnumerator()
        {
            for (int i = 0; i < ts.Count; i++)
                yield return ts[i].Text;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
