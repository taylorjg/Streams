using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Streams;

namespace UnitTests
{
    [TestFixture]
    public class StreamLinqOperatorsTests
    {
        [Test]
        public void Where_MethodSyntax()
        {
            var stream1 = Enumerable.Range(1, 10).AsStream();
            var stream2 = stream1.Where(IsEven);
            Assert.That(stream2.AsEnumerable(), Is.EqualTo(new[] {2, 4, 6, 8, 10}));
        }

        [Test]
        public void WhereWithIndex_MethodSyntax()
        {
            var stream1 = Enumerable.Range(1, 10).AsStream();
            var tuples = new List<Tuple<int, int>>();
            var stream2 = stream1.Where((n, index) =>
            {
                tuples.Add(Tuple.Create(n, index));
                return IsEven(n);
            });
            Assert.That(stream2.AsEnumerable(), Is.EqualTo(new[] { 2, 4, 6, 8, 10 }));
            Assert.That(tuples, Is.EqualTo(new[]
            {
                Tuple.Create(1, 0),
                Tuple.Create(2, 1),
                Tuple.Create(3, 2),
                Tuple.Create(4, 3),
                Tuple.Create(5, 4),
                Tuple.Create(6, 5),
                Tuple.Create(7, 6),
                Tuple.Create(8, 7),
                Tuple.Create(9, 8),
                Tuple.Create(10, 9)
            }));
        }

        [Test]
        public void Where_QuerySyntax()
        {
            var stream1 = Enumerable.Range(1, 10).AsStream();
            var stream2 = from n in stream1 where IsEven(n) select n;
            Assert.That(stream2.AsEnumerable(), Is.EqualTo(new[] {2, 4, 6, 8, 10}));
        }

        [Test]
        public void Select_MethodSyntax()
        {
            var stream1 = Enumerable.Range(1, 10).AsStream();
            var stream2 = stream1.Select(n => string.Format("{0}", n));
            Assert.That(stream2.AsEnumerable(), Is.EqualTo(new[]{"1", "2", "3", "4", "5", "6", "7", "8", "9", "10" }));
        }

        [Test]
        public void SelectWithIndex_MethodSyntax()
        {
            var stream1 = Enumerable.Range(1, 10).AsStream();
            var tuples = new List<Tuple<string, int>>();
            var stream2 = stream1.Select((n, index) =>
            {
                var s = string.Format("{0}", n);
                tuples.Add(Tuple.Create(s, index));
                return s;
            });
            Assert.That(stream2.AsEnumerable(), Is.EqualTo(new[]{"1", "2", "3", "4", "5", "6", "7", "8", "9", "10" }));
            Assert.That(tuples, Is.EqualTo(new[]
            {
                Tuple.Create("1", 0),
                Tuple.Create("2", 1),
                Tuple.Create("3", 2),
                Tuple.Create("4", 3),
                Tuple.Create("5", 4),
                Tuple.Create("6", 5),
                Tuple.Create("7", 6),
                Tuple.Create("8", 7),
                Tuple.Create("9", 8),
                Tuple.Create("10", 9)
            }));
        }

        [Test]
        public void Select_QuerySyntax()
        {
            var stream1 = Enumerable.Range(1, 10).AsStream();
            var stream2 = from n in stream1 select string.Format("{0}", n);
            Assert.That(stream2.AsEnumerable(), Is.EqualTo(new[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" }));
        }

        [TestCase(new[] {1, 2, 3}, new[] {4, 5, 6}, new[] {1, 2, 3, 4, 5, 6})]
        [TestCase(new[] {1, 2, 3}, new int[0], new[] {1, 2, 3})]
        [TestCase(new int[0], new[] {4, 5, 6}, new[] {4, 5, 6})]
        public void Concat(int[] first, int[] second, int[] expected)
        {
            var stream1 = first.AsStream();
            var stream2 = second.AsStream();
            var actual = stream1.Concat(stream2);
            Assert.That(actual.AsEnumerable(), Is.EqualTo(expected));
        }

        private static bool IsEven(int n)
        {
            return n % 2 == 0;
        }
    }
}
