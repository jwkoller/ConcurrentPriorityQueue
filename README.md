# ConcurrentPriorityQueue

A thread safe Priority Queue utilizing **SemaphoreSlim** as the locking mechanism with addtional asynchronous queue
management functions. Includes constructor options to define a delegate that extracts element priority based on element value.

### Examples

- Create a queue of strings prioritized by shortest first
```csharp
var queue = new ConcurrentPriorityQueue<string, int>((element) => element.Length);
await queue.EnqueueAsync("three");
await queue.EnqueueAsync("four");
string next = await queue.DequeueAsync();

// next will equal "four"
```

- Create a queue of strings prioritized by longest first using a custom comparer
```csharp
var queue = new ConcurrentPriorityQueue<string, int>((element) => element.Length, Comparer<int>.Create((x,y) => y.CompareTo(x)));
await queue.EnqueueAsync("two");
await queue.EnqueueAsync("three");
await queue.EnqueueAsync("four");
string next = await queue.DequeueAsync();

// next will equal "three"
```

## Additional documentation

- [PriorityQueue Documentation](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.priorityqueue-2?view=net-8.0)
- [SemaphoreSlim Documentation](https://learn.microsoft.com/en-us/dotnet/api/system.threading.semaphoreslim?view=net-8.0)