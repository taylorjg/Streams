using System;
using System.Linq;
using NUnit.Framework;
using Streams;

namespace UnitTests
{
    [TestFixture]
    public class StreamExtensionsTests
    {
        [Test]
        public void AsEnumerable()
        {
            var stream = 1.Cons(() => 2.Cons(() => 3.Cons()));
            var enumerable = stream.AsEnumerable();

            Assert.That(enumerable, Is.EqualTo(new[] { 1, 2, 3 }));
            Assert.That(enumerable, Is.EqualTo(new[] { 1, 2, 3 }));
            Assert.That(enumerable, Is.EqualTo(new[] { 1, 2, 3 }));
        }

        [Test]
        public void AsStream()
        {
            var enumerable = Enumerable.Range(1, 3);
            var stream = enumerable.AsStream();

            Assert.That(stream.Head, Is.EqualTo(1));
            Assert.That(stream.Tail.Head, Is.EqualTo(2));
            Assert.That(stream.Tail.Tail.Head, Is.EqualTo(3));
            Assert.That(stream.Tail.Tail.Tail, Is.SameAs(Stream<int>.Nil));

            Assert.That(stream.Head, Is.EqualTo(1));
            Assert.That(stream.Tail.Head, Is.EqualTo(2));
            Assert.That(stream.Tail.Tail.Head, Is.EqualTo(3));
            Assert.That(stream.Tail.Tail.Tail, Is.SameAs(Stream<int>.Nil));

            Assert.That(stream.Head, Is.EqualTo(1));
            Assert.That(stream.Tail.Head, Is.EqualTo(2));
            Assert.That(stream.Tail.Tail.Head, Is.EqualTo(3));
            Assert.That(stream.Tail.Tail.Tail, Is.SameAs(Stream<int>.Nil));
        }

        [Test]
        public void ConsOfHeadOnly()
        {
            var stream = 1.Cons();
            Assert.That(stream.Head, Is.EqualTo(1));
            Assert.That(stream.Tail.IsEmpty, Is.True);
        }

        [Test]
        public void ConsOfHeadAndTail()
        {
            var stream = 1.Cons(() => 2.Cons());
            Assert.That(stream.Head, Is.EqualTo(1));
            Assert.That(stream.Tail.Head, Is.EqualTo(2));
            Assert.That(stream.Tail.Tail.IsEmpty, Is.True);
        }

        [Test]
        public void ConsWithNullTailFuncThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => 1.Cons(null));
        }
    }
}
