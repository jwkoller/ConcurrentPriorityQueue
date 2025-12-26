using System.Diagnostics.CodeAnalysis;

namespace ConcurrentPriorityQueue
{
    public class ConcurrentPriorityQueue<TElement, TPriority>
    {
        private readonly Func<TElement, TPriority>? _priorityEvaluator;
        private readonly SemaphoreSlim _lock;
        private readonly PriorityQueue<TElement, TPriority> _priorityQueue;

        public ConcurrentPriorityQueue()
        {
            _lock = new SemaphoreSlim(1, 1);
            _priorityQueue = new PriorityQueue<TElement, TPriority>();
        }

        public ConcurrentPriorityQueue(IComparer<TPriority> comparer)
        {
            _lock = new SemaphoreSlim(1, 1);
            _priorityQueue = new PriorityQueue<TElement, TPriority>(comparer);
        }

        public ConcurrentPriorityQueue(Func<TElement, TPriority> priorityEvaluator) : this()
        {
            _priorityEvaluator = priorityEvaluator;
        }

        public ConcurrentPriorityQueue(Func<TElement, TPriority> priorityEvaluator, IComparer<TPriority> comparer) : this(comparer)
        {
            _priorityEvaluator = priorityEvaluator;
        }

        public ConcurrentPriorityQueue(Func<TElement, TPriority> priorityEvaluator, IEnumerable<TElement> elements) : this(priorityEvaluator)
        {
            _lock.Wait();
            foreach (var element in elements)
            {
                TPriority priority = priorityEvaluator(element);
                _priorityQueue.Enqueue(element, priority);
            }
            _lock.Release();
        }

        public ConcurrentPriorityQueue(IEnumerable<Tuple<TElement,TPriority>> elements) : this()
        {
            _lock.Wait();
            foreach(var element in elements)
            {
                _priorityQueue.Enqueue(element.Item1, element.Item2);
            }
            _lock.Release();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>A collection of the elements and their associated priority for the current queue.</returns>
        public async Task<IReadOnlyCollection<(TElement, TPriority)>> UnorderedItems()
        {
            await _lock.WaitAsync();
            var items = _priorityQueue.UnorderedItems;
            _lock.Release();
            
            return items;
        }

        /// <summary>
        /// Enqueues an element with a priority defined by the default evaluator.
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        public async Task Enqueue(TElement element)
        {
            ArgumentNullException.ThrowIfNull(_priorityEvaluator, "Priority Evaluator");

            TPriority priority = _priorityEvaluator(element);
            await _lock.WaitAsync();
            _priorityQueue.Enqueue(element, priority);
            _lock.Release();
        }

        /// <summary>
        /// Enqueues an element with the specified priority.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="priority"></param>
        /// <returns></returns>
        public async Task Enqueue(TElement element, TPriority priority)
        {
            await _lock.WaitAsync();
            _priorityQueue.Enqueue(element, priority);
            _lock.Release();
        }

        /// <summary>
        /// Enqueues the elements with each priority defined by the default evaluator.
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public async Task EnqueueRange(IEnumerable<TElement> elements)
        {
            ArgumentNullException.ThrowIfNull(_priorityEvaluator,"Priority Evaluator");

            await _lock.WaitAsync();
            foreach (var item in elements)
            {
                TPriority priority = _priorityEvaluator(item);
                _priorityQueue.Enqueue(item, priority);
            }
            _lock.Release();
        }

        /// <summary>
        /// Enqueues the elements with with each specified priority.
        /// </summary>
        /// <param name="elements"></param>
        /// <returns></returns>
        public async Task EnqueueRange(IEnumerable<Tuple<TElement, TPriority>> elements)
        {
            await _lock.WaitAsync();
            foreach(var item in elements)
            {
                _priorityQueue.Enqueue(item.Item1, item.Item2);
            }
            _lock.Release();
        }

        /// <summary>
        /// Removes and returns the next element defined as the highest priority item.
        /// </summary>
        /// <returns></returns>
        public async Task<TElement> Dequeue()
        {
            await _lock.WaitAsync();
            TElement element = _priorityQueue.Dequeue();
            _lock.Release();

            return element;
        }

        /// <summary>
        /// Removes the next element defined as the highest priority item and copies it's value and priority to the corresponding arguments.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="priority"></param>
        /// <returns><see langword="true"/> if the element and priority were successfully removed, <see langword="false"/> if not.</returns>
        public bool TryDequeue([MaybeNullWhen(false)] out TElement element, [MaybeNullWhen(false)] out TPriority priority)
        {
            _lock.Wait();
            bool result = _priorityQueue.TryDequeue(out element, out priority);
            _lock.Release();

            return result;
        }

        /// <summary>
        /// Returns the next element defined as the highest priority without removing it from the queue.
        /// </summary>
        /// <returns></returns>
        public async Task<TElement> Peek()
        {
            await _lock.WaitAsync();
            TElement element = _priorityQueue.Peek();
            _lock.Release();

            return element;
        }

        /// <summary>
        /// Retrieves the next element and associated priority value defined as the highest priority and copies them to the corresponding arguments.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="priority"></param>
        /// <returns><see cref="true"/> if the element and priority were successfully retrieved.</returns>
        public bool TryPeek([MaybeNullWhen(false)] out TElement element, [MaybeNullWhen(false)] out TPriority priority)
        {
            _lock.Wait();
            bool result = _priorityQueue.TryPeek(out element, out priority);
            _lock.Release();

            return result;
        }

        /// <summary>
        /// Removes all items from the queue.
        /// </summary>
        /// <returns></returns>
        public async Task Clear()
        {
            await _lock.WaitAsync();
            _priorityQueue.Clear();
            _lock.Release();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns>The number of elements currently in the queue.</returns>
        public async Task<int> Count()
        {
            await _lock.WaitAsync();
            int count = _priorityQueue.Count;
            _lock.Release();

            return count;
        }
    }
}
