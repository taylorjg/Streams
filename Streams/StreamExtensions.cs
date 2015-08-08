﻿using System.Collections.Generic;

namespace Streams
{
    public static class StreamExtensions
    {
        public static IEnumerable<T> AsEnumerable<T>(this Stream<T> s)
        {
            var currentStream = s;
            for (;;)
            {
                if (currentStream.IsEmpty) yield break;
                yield return currentStream.Head;
                currentStream = currentStream.Tail;
            }
        }

        public static Stream<T> AsStream<T>(this IEnumerable<T> source)
        {
            using (var e = source.GetEnumerator()) return e.AsStream();
        }

        private static Stream<T> AsStream<T>(this IEnumerator<T> e)
        {
            return e.MoveNext() ? Stream<T>.Cons(e.Current, e.AsStream) : Stream<T>.EmptyStream;
        }
    }
}
