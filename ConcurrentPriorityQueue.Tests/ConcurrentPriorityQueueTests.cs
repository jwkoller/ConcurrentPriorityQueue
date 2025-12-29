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
            await queue.EnqueueAsync("three");
            await queue.EnqueueAsync("four");
            string next = await queue.DequeueAsync();

            Assert.AreEqual("four", next);
        }

        [TestMethod]
        public async Task ItemsDequeuedInOrderbyPriorityEvaluatorAndCustomComparer()
        {
            var queue = new ConcurrentPriorityQueue<string, int>((element) => element.Length, Comparer<int>.Create((x,y) => y.CompareTo(x)));
            await queue.EnqueueAsync("three");
            await queue.EnqueueAsync("four");
            string next = await queue.DequeueAsync();

            Assert.AreEqual("three", next);
        }

        [TestMethod]
        public async Task ItemsWithSamePriorityAreDequeuedFirstInFirstOut()
        {
            var queue = new ConcurrentPriorityQueue<string, int>();
            await queue.EnqueueAsync("less important string", 2);
            await queue.EnqueueAsync("first important string", 1);
            await queue.EnqueueAsync("second important string", 1);

            string next = await queue.DequeueAsync();

            Assert.AreEqual("first important string", next);
        }

        [TestMethod]
        public async Task EnqueueItemsFromMultipleThreadsAreCaptured()
        {
            var queue = new ConcurrentPriorityQueue<string, int>();
            List<Task> threads = new List<Task>();
            int numThreads = 100;

            for (int i = 0; i < numThreads; i++)
            {
                threads.Add(Task.Run(async () =>
                {
                    int priority = new Random().Next(100);
                    string threadId = Guid.NewGuid().ToString();

                    await queue.EnqueueAsync($"Thread {threadId}", priority);

                }));
            }

            await Task.WhenAll(threads);

            int numberQueued = await queue.CountAsync();
            Assert.AreEqual(numThreads, numberQueued);

            var items = await queue.UnorderedItemsAsync();
            int distinctCount = items.Select(x => x.Item1).Distinct().Count();
            Assert.AreEqual(numThreads, distinctCount);
        }
    }
}
