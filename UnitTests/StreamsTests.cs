﻿using NUnit.Framework;
using Streams;

namespace UnitTests
{
    [TestFixture]
    public class StreamsTests
    {
        [Test]
        public void ConsOfHeadOnly()
        {
            var stream = Stream<int>.Cons(1);
            Assert.That(stream.Tail, Is.SameAs(Stream<int>.EmptyStream));
        }

        [Test]
        public void ConsOfHeadAndTail()
        {
            var stream = Stream<int>.Cons(1, () => Stream<int>.Cons(2));
            Assert.That(stream.Tail, Is.Not.SameAs(Stream<int>.EmptyStream));
            Assert.That(stream.Tail.Tail, Is.SameAs(Stream<int>.EmptyStream));
        }

        [Test]
        public void MatchActionEmpty()
        {
            var stream = Stream<int>.EmptyStream;
            var emptyActionInvoked = false;
            var nonEmptyActionInvoked = false;
            stream.Match(() => { emptyActionInvoked = true; }, (_, __) => { nonEmptyActionInvoked = true; });
            Assert.That(emptyActionInvoked, Is.True);
            Assert.That(nonEmptyActionInvoked, Is.False);
        }

        [Test]
        public void MatchActionNonEmpty()
        {
            var stream = Stream<int>.Cons(1);
            var emptyActionInvoked = false;
            var nonEmptyActionInvoked = false;
            var head = 0;
            var tail = null as Stream<int>;
            stream.Match(() => { emptyActionInvoked = true; }, (h, t) =>
            {
                nonEmptyActionInvoked = true;
                head = h;
                tail = t;
            });
            Assert.That(emptyActionInvoked, Is.False);
            Assert.That(nonEmptyActionInvoked, Is.True);
            Assert.That(head, Is.EqualTo(1));
            Assert.That(tail, Is.SameAs(Stream<int>.EmptyStream));
        }

        [Test]
        public void MatchFuncEmpty()
        {
            var stream = Stream<int>.EmptyStream;
            var actual = stream.Match(() => 1, (_, __) => 2);
            Assert.That(actual, Is.EqualTo(1));
        }

        [Test]
        public void MatchFuncNonEmpty()
        {
            var stream = Stream<int>.Cons(1);
            var head = 0;
            var tail = null as Stream<int>;
            var actual = stream.Match(() => 1, (h, t) =>
            {
                head = h;
                tail = t;
                return 2;
            });
            Assert.That(actual, Is.EqualTo(2));
            Assert.That(head, Is.EqualTo(1));
            Assert.That(tail, Is.SameAs(Stream<int>.EmptyStream));
        }
    }
}