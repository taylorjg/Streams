using System;

namespace Streams
{
    public sealed class Stream<T>
    {
        private readonly Tuple<T, Lazy<Stream<T>>> _pair;

        private Stream()
        {
            _pair = null;
        }

        private Stream(T head, Func<Stream<T>> tail)
        {
            if (tail == null)
                throw new ArgumentNullException("tail");
            _pair = Tuple.Create(head, new Lazy<Stream<T>>(tail));
        }

        public static Stream<T> Cons(T head)
        {
            return new Stream<T>(head, () => Nil);
        }

        public static Stream<T> Cons(T head, Func<Stream<T>> tail)
        {
            return new Stream<T>(head, tail);
        }

        public static readonly Stream<T> Nil = new Stream<T>();

        public bool IsEmpty { get { return this == Nil; } }

        public T Head
        {
            get
            {
                PerformEmptyCheck("Head");
                return _pair.Item1;
            }
        }

        public Stream<T> Tail
        {
            get
            {
                PerformEmptyCheck("Tail");
                return _pair.Item2.Value;
            }
        }

        private void PerformEmptyCheck(string functionName)
        {
            if (_pair == null)
                throw new InvalidOperationException(string.Format("{0} called on empty stream.", functionName));
        }

        public void Match(Action<T, Stream<T>> actionNonEmpty, Action actionEmpty)
        {
            if (IsEmpty) actionEmpty(); else actionNonEmpty(Head, Tail);
        }

        public TResult Match<TResult>(Func<T, Stream<T>, TResult> funcNonEmpty, Func<TResult> funcEmpty)
        {
            return IsEmpty ? funcEmpty() : funcNonEmpty(Head, Tail);
        }
    }
}
