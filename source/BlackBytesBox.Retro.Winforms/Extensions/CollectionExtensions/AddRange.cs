using System;
using System.Collections.Generic;
using System.Text;

namespace BlackBytesBox.Retro.UI.Extensions
{
    public static partial class CollectionExtensions
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            if (collection == null) throw new ArgumentNullException("collection");
            if (items == null) throw new ArgumentNullException("items");

            foreach (T item in items)
            {
                collection.Add(item);
            }
        }
    }
}
