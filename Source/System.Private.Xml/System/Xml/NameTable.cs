// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;

namespace DCSystemXml
{
    public class NameTable : XmlNameTable
    {
        private sealed class Entry
        {
            internal string str;
            internal int hashCode;
            internal Entry? next;

            internal Entry(string str, int hashCode, Entry? next)
            {
                this.str = str;
                this.hashCode = hashCode;
                this.next = next;
            }
        }

        private Entry?[] _entries;
        private int _count;
        private int _mask;

        public NameTable()
        {
            _mask = 31;
            _entries = new Entry?[_mask + 1];
        }

        /// <devdoc>
        ///      Add the given string to the NameTable or return
        ///      the existing string if it is already in the NameTable.
        /// </devdoc>
        public override string Add(string key)
        {
            ArgumentNullException.ThrowIfNull(key);

            int len = key.Length;
            if (len == 0)
            {
                return string.Empty;
            }

            int hashCode = ComputeHash32(key);

            for (Entry? e = _entries[hashCode & _mask]; e != null; e = e.next)
            {
                if (e.hashCode == hashCode && e.str.Equals(key))
                {
                    return e.str;
                }
            }

            return AddEntry(key, hashCode);
        }


        internal static int ComputeHash32(string key)
        {
            return string.GetHashCode(key.AsSpan());
        }

        private string AddEntry(string str, int hashCode)
        {
            int index = hashCode & _mask;
            Entry e = new Entry(str, hashCode, _entries[index]);
            _entries[index] = e;

            if (_count++ == _mask)
            {
                Grow();
            }

            return e.str;
        }

        private void Grow()
        {
            int newMask = _mask * 2 + 1;
            Entry?[] oldEntries = _entries;
            Entry?[] newEntries = new Entry?[newMask + 1];

            // use oldEntries.Length to eliminate the range check
            for (int i = 0; i < oldEntries.Length; i++)
            {
                Entry? e = oldEntries[i];
                while (e != null)
                {
                    int newIndex = e.hashCode & newMask;
                    Entry? tmp = e.next;
                    e.next = newEntries[newIndex];
                    newEntries[newIndex] = e;
                    e = tmp;
                }
            }

            _entries = newEntries;
            _mask = newMask;
        }
    }
}
