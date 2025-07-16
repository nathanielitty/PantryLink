using PantryLink.Core.Entities;

namespace PantryLink.Core.Algorithms;

/// <summary>
/// Priority Queue implementation for ranking pantries by stock freshness and user rating
/// </summary>
public class PantryPriorityQueue
{
    private readonly PriorityQueue<Pantry, int> _queue;

    public PantryPriorityQueue()
    {
        _queue = new PriorityQueue<Pantry, int>(Comparer<int>.Create((x, y) => y.CompareTo(x))); // Max heap
    }

    public void Enqueue(Pantry pantry)
    {
        var priority = CalculatePantryPriority(pantry);
        _queue.Enqueue(pantry, priority);
    }

    public Pantry? Dequeue()
    {
        return _queue.TryDequeue(out var pantry, out _) ? pantry : null;
    }

    public bool IsEmpty => _queue.Count == 0;

    public int Count => _queue.Count;

    private static int CalculatePantryPriority(Pantry pantry)
    {
        var priority = 0;

        // Rating component (0-50 points)
        priority += (int)(pantry.AverageRating * 10);

        // Stock freshness component (0-100 points)
        var soonToExpireItems = pantry.Inventory
            .Where(item => item.ExpirationDate.HasValue && 
                          (item.ExpirationDate.Value - DateTime.UtcNow).Days <= 7)
            .Count();

        priority += Math.Min(soonToExpireItems * 10, 100);

        // Total stock availability (0-50 points)
        var totalItems = pantry.Inventory.Sum(item => item.Quantity);
        priority += Math.Min(totalItems / 10, 50);

        return priority;
    }

    public IEnumerable<Pantry> GetSortedPantries()
    {
        var tempQueue = new PriorityQueue<Pantry, int>(Comparer<int>.Create((x, y) => y.CompareTo(x)));
        var results = new List<Pantry>();

        // Copy all items to temp queue
        while (!IsEmpty)
        {
            var pantry = Dequeue()!;
            results.Add(pantry);
            tempQueue.Enqueue(pantry, CalculatePantryPriority(pantry));
        }

        // Restore original queue
        while (tempQueue.TryDequeue(out var pantry, out var priority))
        {
            _queue.Enqueue(pantry, priority);
        }

        return results;
    }
}
