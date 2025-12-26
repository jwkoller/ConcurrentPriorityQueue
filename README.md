# ConcurrentPriorityQueue

An predominantly asynchronous, thread safe Priority Queue utilizing **SemaphoreSlim** and WaitAsync as the locking mechanism.
Includes an option to define a delegate to extract element priority from complex objects.

### Examples

```csharp
// Create a que of strings prioritized by shortest first.
var que = new ConcurrentPriorityQueue<string, int>((element) => element.Length);
await que.Enqueue("three");
await que.Enqueue("four");
string next = await que.Dequeue();
// next will equal "four"
```


## Additional documentation

[PriorityQueue Documentation](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.priorityqueue-2?view=net-8.0)
[SemaphoreSlim Documentation](https://learn.microsoft.com/en-us/dotnet/api/system.threading.semaphoreslim?view=net-8.0)