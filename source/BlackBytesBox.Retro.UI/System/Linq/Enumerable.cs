using System;
using System.Collections;
using System.Collections.Generic;

namespace System.Linq
{
    /// <summary>
    /// Represents a sorted sequence. Supports creating subsequent ordered sequences.
    /// </summary>
    /// <typeparam name="TElement">The type of the elements of the sequence.</typeparam>
    public interface IOrderedEnumerable<TElement> : IEnumerable<TElement>
    {
        /// <summary>
        /// Creates a new IOrderedEnumerable from this sequence using a secondary key.
        /// </summary>
        IOrderedEnumerable<TElement> CreateOrderedEnumerable<TKey>(
            Func<TElement, TKey> keySelector,
            IComparer<TKey> comparer,
            bool descending);
    }

    public static class Enumerable
    {
        // Where
        public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");
            return WhereIterator(source, predicate);
        }
        private static IEnumerable<TSource> WhereIterator<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            foreach (TSource element in source)
                if (predicate(element)) yield return element;
        }

        // Select
        public static IEnumerable<TResult> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (selector == null) throw new ArgumentNullException("selector");
            return SelectIterator(source, selector);
        }
        private static IEnumerable<TResult> SelectIterator<TSource, TResult>(IEnumerable<TSource> source, Func<TSource, TResult> selector)
        {
            foreach (TSource element in source)
                yield return selector(element);
        }

        // FirstOrDefault
        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");
            foreach (TSource element in source) return element;
            return default(TSource);
        }
        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");
            foreach (TSource element in source)
                if (predicate(element)) return element;
            return default(TSource);
        }

        // ToList
        public static List<TSource> ToList<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");
            return new List<TSource>(source);
        }

        // Count
        public static int Count<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");
            int count = 0; foreach (TSource element in source) checked { count++; }
            return count;
        }
        public static int Count<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");
            int count = 0;
            foreach (TSource element in source)
                if (predicate(element)) checked { count++; }
            return count;
        }

        // Any
        public static bool Any<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");
            foreach (TSource element in source) return true;
            return false;
        }
        public static bool Any<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");
            foreach (TSource element in source) if (predicate(element)) return true;
            return false;
        }

        // All
        public static bool All<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (predicate == null) throw new ArgumentNullException("predicate");
            foreach (TSource element in source)
                if (!predicate(element)) return false;
            return true;
        }

        // Take
        public static IEnumerable<TSource> Take<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (count < 0) throw new ArgumentOutOfRangeException("count");
            return TakeIterator(source, count);
        }
        private static IEnumerable<TSource> TakeIterator<TSource>(IEnumerable<TSource> source, int count)
        {
            int taken = 0;
            foreach (TSource element in source)
            {
                if (taken++ == count) yield break;
                yield return element;
            }
        }

        // Skip
        public static IEnumerable<TSource> Skip<TSource>(this IEnumerable<TSource> source, int count)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (count < 0) throw new ArgumentOutOfRangeException("count");
            return SkipIterator(source, count);
        }
        private static IEnumerable<TSource> SkipIterator<TSource>(IEnumerable<TSource> source, int count)
        {
            int skipped = 0;
            foreach (TSource element in source)
            {
                if (skipped < count) { skipped++; continue; }
                yield return element;
            }
        }

        // Concat
        public static IEnumerable<TSource> Concat<TSource>(this IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            return ConcatIterator(first, second);
        }
        private static IEnumerable<TSource> ConcatIterator<TSource>(IEnumerable<TSource> first, IEnumerable<TSource> second)
        {
            foreach (TSource element in first) yield return element;
            foreach (TSource element in second) yield return element;
        }

        // Distinct
        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");
            return DistinctIterator(source, null);
        }
        public static IEnumerable<TSource> Distinct<TSource>(this IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            return DistinctIterator(source, comparer);
        }
        private static IEnumerable<TSource> DistinctIterator<TSource>(IEnumerable<TSource> source, IEqualityComparer<TSource> comparer)
        {
            var set = new HashSet<TSource>(comparer);
            foreach (TSource element in source)
                if (set.Add(element)) yield return element;
        }

        // OrderBy
        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
            => OrderBy(source, keySelector, Comparer<TKey>.Default);

        public static IOrderedEnumerable<TSource> OrderBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> keySelector, IComparer<TKey> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (keySelector == null) throw new ArgumentNullException("keySelector");
            if (comparer == null) throw new ArgumentNullException("comparer");
            var list = new List<TSource>(source);
            list.Sort((a, b) => comparer.Compare(keySelector(a), keySelector(b)));
            return new OrderedEnumerable<TSource>(list);
        }
    }

    internal class OrderedEnumerable<TSource> : IOrderedEnumerable<TSource>
    {
        private readonly List<TSource> _sorted;
        public OrderedEnumerable(List<TSource> sorted) => _sorted = sorted;
        public IEnumerator<TSource> GetEnumerator() => _sorted.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _sorted.GetEnumerator();
        public IOrderedEnumerable<TSource> CreateOrderedEnumerable<TKey>(Func<TSource, TKey> keySelector,
            IComparer<TKey> comparer, bool descending)
            => throw new NotSupportedException("ThenBy is not supported");
    }
}
