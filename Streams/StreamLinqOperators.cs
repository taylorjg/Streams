using System;

namespace Streams
{
    // 26.7.2 The Query Expression Pattern
    // https://msdn.microsoft.com/en-us/library/bb308966.aspx#csharp3.0overview_topic19

    public static class StreamLinqOperators
    {
        public static Stream<TSource> Where<TSource>(this Stream<TSource> source, Func<TSource, bool> predicate)
        {
            while (!source.IsEmpty)
            {
                if (predicate(source.Head))
                    return Stream<TSource>.Cons(source.Head, () => source.Tail.Where(predicate));
                source = source.Tail;
            }

            return Stream<TSource>.Nil;
        }

        public static Stream<TSource> Where<TSource>(this Stream<TSource> source, Func<TSource, int, bool> predicate)
        {
            return WhereHelper(source, predicate, 0);
        }

        private static Stream<TSource> WhereHelper<TSource>(this Stream<TSource> source, Func<TSource, int, bool> predicate, int index)
        {
            while (!source.IsEmpty)
            {
                if (predicate(source.Head, index++))
                    return Stream<TSource>.Cons(source.Head, () => source.Tail.WhereHelper(predicate, index));
                source = source.Tail;
            }

            return Stream<TSource>.Nil;
        }

        public static Stream<TResult> Select<TSource, TResult>(this Stream<TSource> source, Func<TSource, TResult> selector)
        {
            while (!source.IsEmpty)
            {
                return Stream<TResult>.Cons(selector(source.Head), () => source.Tail.Select(selector));
            }

            return Stream<TResult>.Nil;
        }

        public static Stream<TResult> Select<TSource, TResult>(this Stream<TSource> source, Func<TSource, int, TResult> selector)
        {
            return SelectHelper(source, selector, 0);
        }

        private static Stream<TResult> SelectHelper<TSource, TResult>(this Stream<TSource> source, Func<TSource, int, TResult> selector, int index)
        {
            while (!source.IsEmpty)
            {
                return Stream<TResult>.Cons(selector(source.Head, index++), () => source.Tail.SelectHelper(selector, index));
            }

            return Stream<TResult>.Nil;
        }

        public static Stream<TSource> Concat<TSource>(this Stream<TSource> first, Stream<TSource> second)
        {
            while (!first.IsEmpty)
            {
                return Stream<TSource>.Cons(first.Head, () => first.Tail.Concat(second));
            }

            while (!second.IsEmpty)
            {
                return Stream<TSource>.Cons(second.Head, () => second.Tail.Concat(Stream<TSource>.Nil));
            }

            return Stream<TSource>.Nil;
        }
    }
}
