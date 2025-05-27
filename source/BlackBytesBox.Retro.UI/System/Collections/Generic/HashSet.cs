
namespace System.Collections.Generic
{
    public class HashSet<T> : ICollection<T>, IEnumerable<T>
    {
        private Dictionary<T, bool> dict;

        public HashSet()
        {
            dict = new Dictionary<T, bool>();
        }

        public HashSet(IEqualityComparer<T> comparer)
        {
            dict = new Dictionary<T, bool>(comparer);
        }

        public int Count { get { return dict.Count; } }
        public bool IsReadOnly { get { return false; } }

        public bool Add(T item)
        {
            if (dict.ContainsKey(item))
                return false;
            dict[item] = true;
            return true;
        }

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        public bool Contains(T item)
        {
            return dict.ContainsKey(item);
        }

        public bool Remove(T item)
        {
            return dict.Remove(item);
        }

        public void Clear()
        {
            dict.Clear();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            dict.Keys.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return dict.Keys.GetEnumerator();
        }

        Collections.IEnumerator Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}