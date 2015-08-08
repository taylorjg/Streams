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
            var stream = Stream<int>.Cons(1, () => Stream<int>.Cons(2, () => Stream<int>.Cons(3)));

            var e = stream.AsEnumerable();
            foreach (var _ in Enumerable.Range(1, 3))
            {
                Assert.That(e, Is.EqualTo(new[] { 1, 2, 3 }));
            }
        }

        [Test]
        public void AsStream()
        {
            var stream = new[] {1, 2, 3}.AsStream();

            foreach (var _ in Enumerable.Range(1, 3))
            {
                Assert.That(stream.Head, Is.EqualTo(1));
                Assert.That(stream.Tail.Head, Is.EqualTo(2));
                Assert.That(stream.Tail.Tail.Head, Is.EqualTo(3));
                Assert.That(stream.Tail.Tail.Tail, Is.SameAs(Stream<int>.EmptyStream));
            }
        }
    }
}
