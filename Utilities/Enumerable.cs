using System.Collections;
using System.Collections.Generic;

namespace ShapesLibrary
{
    public static class EnumerableX
    {
        public static void ForEach<T>(this IEnumerable list, System.Action<T> action)
        {
            foreach (T item in list)
                action(item);
        }

        public static void ForEach<T>(this IEnumerable<T> list, System.Action<T> action)
        {
            foreach (T item in list)
                action(item);
        }

        public static void ForEachExceptLast<T>(this IEnumerable<T> list, System.Action<T> action)
        {
            IEnumerator<T> en = list.GetEnumerator();

            en.MoveNext();
            while (en.Current != null)
            {
                T item = en.Current;
                en.MoveNext();
                if (en.Current != null)
                    action(item);
            }
        }

    }
}
