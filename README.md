# ConcurrentPriorityQueue

An predominantly asynchronous, thread safe Priority Queue utilizing **SemaphoreSlim** and WaitAsync as the locking mechanism.
Includes an option to define a delegate that extracts element priority based on element value.

### Examples

```csharp
// Create a queue of strings prioritized by shortest first.
var queue = new ConcurrentPriorityQueue<string, int>((element) => element.Length);
await queue.Enqueue("three");
await queue.Enqueue("four");
string next = await queue.Dequeue();
// next will equal "four"
```


## Additional documentation

- [PriorityQueue Documentation](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.priorityqueue-2?view=net-8.0)
- [SemaphoreSlim Documentation](https://learn.microsoft.com/en-us/dotnet/api/system.threading.semaphoreslim?view=net-8.0)