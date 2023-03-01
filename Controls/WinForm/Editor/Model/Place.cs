using System;

namespace MiMFa.Controls.WinForm.Editor.Model
{
    /// <summary>
    /// Line index and char index
    /// </summary>
    public struct Place : IEquatable<Place>
    {
        public int CharIndex;
        public int LineIndex;

        public Place(int iChar, int iLine)
        {
            this.CharIndex = iChar;
            this.LineIndex = iLine;
        }

        public void Offset(int dx, int dy)
        {
            CharIndex += dx;
            LineIndex += dy;
        }

        public bool Equals(Place other)
        {
            return CharIndex == other.CharIndex && LineIndex == other.LineIndex;
        }

        public override bool Equals(object obj)
        {
            return (obj is Place) && Equals((Place)obj);
        }

        public override int GetHashCode()
        {
            return CharIndex.GetHashCode() ^ LineIndex.GetHashCode();
        }

        public static bool operator !=(Place p1, Place p2)
        {
            return !p1.Equals(p2);
        }

        public static bool operator ==(Place p1, Place p2)
        {
            return p1.Equals(p2);
        }

        public static bool operator <(Place p1, Place p2)
        {
            if (p1.LineIndex < p2.LineIndex) return true;
            if (p1.LineIndex > p2.LineIndex) return false;
            if (p1.CharIndex < p2.CharIndex) return true;
            return false;
        }

        public static bool operator <=(Place p1, Place p2)
        {
            if (p1.Equals(p2)) return true;
            if (p1.LineIndex < p2.LineIndex) return true;
            if (p1.LineIndex > p2.LineIndex) return false;
            if (p1.CharIndex < p2.CharIndex) return true;
            return false;
        }

        public static bool operator >(Place p1, Place p2)
        {
            if (p1.LineIndex > p2.LineIndex) return true;
            if (p1.LineIndex < p2.LineIndex) return false;
            if (p1.CharIndex > p2.CharIndex) return true;
            return false;
        }

        public static bool operator >=(Place p1, Place p2)
        {
            if (p1.Equals(p2)) return true;
            if (p1.LineIndex > p2.LineIndex) return true;
            if (p1.LineIndex < p2.LineIndex) return false;
            if (p1.CharIndex > p2.CharIndex) return true;
            return false;
        }

        public static Place operator +(Place p1, Place p2)
        {
            return new Place(p1.CharIndex + p2.CharIndex, p1.LineIndex + p2.LineIndex);
        }

        public static Place Empty
        {
            get { return new Place(); }
        }

        public override string ToString()
        {
            return "(" + CharIndex + "," + LineIndex + ")";
        }
    }
}
