using System;

namespace Streams
{
    public sealed class Stream<T>
    {
        private readonly Tuple<T, Func<Stream<T>>> _pair;
        private Stream<T> _tail;

        private Stream()
        {
            _pair = null;
            _tail = null;
        }

        private Stream(T head, Func<Stream<T>> tail)
        {
            _pair = Tuple.Create(head, tail);
        }

        public static Stream<T> Cons(T head)
        {
            return new Stream<T>(head, () => EmptyStream);
        }

        public static Stream<T> Cons(T head, Func<Stream<T>> tail)
        {
            return new Stream<T>(head, tail);
        }

        public static readonly Stream<T> EmptyStream = new Stream<T>();

        public bool IsEmpty { get { return this == EmptyStream; } }

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
                return _tail ?? (_tail = _pair.Item2());
            }
        }

        private void PerformEmptyCheck(string functionName)
        {
            if (_pair == null) throw new InvalidOperationException(string.Format("{0} called on empty stream.", functionName));
        }

        public void Match(Action actionEmpty, Action<T, Stream<T>> actionNonEmpty)
        {
            if (IsEmpty) actionEmpty(); else actionNonEmpty(Head, Tail);
        }

        public TResult Match<TResult>(Func<TResult> funcEmpty, Func<T, Stream<T>, TResult> funcNonEmpty)
        {
            return IsEmpty ? funcEmpty() : funcNonEmpty(Head, Tail);
        }
    }
}
