using System;
using NUnit.Framework;
using Streams;

namespace UnitTests
{
    [TestFixture]
    public class StreamTests
    {
        [Test]
        public void ConsOfHeadOnly()
        {
            var stream = Stream<int>.Cons(1);
            Assert.That(stream.Head, Is.EqualTo(1));
            Assert.That(stream.Tail.IsEmpty, Is.True);
        }

        [Test]
        public void ConsOfHeadAndTail()
        {
            var stream = Stream<int>.Cons(1, () => Stream<int>.Cons(2));
            Assert.That(stream.Head, Is.EqualTo(1));
            Assert.That(stream.Tail.Head, Is.EqualTo(2));
            Assert.That(stream.Tail.Tail.IsEmpty, Is.True);
        }

        [Test]
        public void ConsWithNullTailFuncThrowsArgumentNullException()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Stream<int>.Cons(1, null));
            Assert.That(ex.ParamName, Is.EqualTo("tail"));
        }

        [Test]
        public void Nil()
        {
            var stream = 99.Cons();
            Assert.That(stream.Tail, Is.SameAs(Stream<int>.Nil));
            Assert.That(Stream<int>.Nil.IsEmpty, Is.True);
        }

        [Test]
        public void MatchActionEmpty()
        {
            var stream = Stream<int>.Nil;
            var emptyActionInvoked = false;
            var nonEmptyActionInvoked = false;
            stream.Match((_, __) => { nonEmptyActionInvoked = true; }, () => { emptyActionInvoked = true; });
            Assert.That(emptyActionInvoked, Is.True);
            Assert.That(nonEmptyActionInvoked, Is.False);
        }

        [Test]
        public void MatchActionNonEmpty()
        {
            var stream = Stream<int>.Cons(99);
            var emptyActionInvoked = false;
            var nonEmptyActionInvoked = false;
            var head = 0;
            var tail = null as Stream<int>;
            stream.Match(
                (h, t) =>
                {
                    nonEmptyActionInvoked = true;
                    head = h;
                    tail = t;
                },
                () => { emptyActionInvoked = true; });
            Assert.That(nonEmptyActionInvoked, Is.True);
            Assert.That(head, Is.EqualTo(99));
            Assert.That(tail.IsEmpty, Is.True);
            Assert.That(emptyActionInvoked, Is.False);
        }

        [Test]
        public void MatchFuncEmpty()
        {
            var stream = Stream<int>.Nil;
            var actual = stream.Match((_, __) => 1, () => 2);
            Assert.That(actual, Is.EqualTo(2));
        }

        [Test]
        public void MatchFuncNonEmpty()
        {
            var stream = 99.Cons();
            var head = 0;
            var tail = null as Stream<int>;
            var actual = stream.Match(
                (h, t) =>
                {
                    head = h;
                    tail = t;
                    return 1;
                },
                () => 2);
            Assert.That(actual, Is.EqualTo(1));
            Assert.That(head, Is.EqualTo(99));
            Assert.That(tail.IsEmpty, Is.True);
        }
    }
}
