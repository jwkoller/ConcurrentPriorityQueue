using System.Runtime.ExceptionServices;

namespace ConcurrentPriorityQueue.Tests
{
    [TestClass]
    public sealed class ConcurrentPriorityQueueTests
    {
        [TestMethod]
        public async Task ItemsDequeuedInOrderByPriorityEvaluator()
        {
            var queue = new ConcurrentPriorityQueue<string, int>((element) => element.Length);
            await queue.Enqueue("three");
            await queue.Enqueue("four");
            string next = await queue.Dequeue();

            Assert.AreEqual("four", next);
        }

        [TestMethod]
        public async Task ItemsDequeuedInOrderbyPriorityEvaluatorAndCustomComparer()
        {
            var queue = new ConcurrentPriorityQueue<string, int>((element) => element.Length, Comparer<int>.Create((x,y) => y.CompareTo(x)));
            await queue.Enqueue("three");
            await queue.Enqueue("four");
            string next = await queue.Dequeue();

            Assert.AreEqual("three", next);
        }
    }
}
