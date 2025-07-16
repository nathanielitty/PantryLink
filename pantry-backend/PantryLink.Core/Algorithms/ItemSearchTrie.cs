namespace PantryLink.Core.Algorithms;

/// <summary>
/// Trie (Prefix Tree) implementation for fast item search
/// Enables autocomplete functionality like "can" → "canned beans"
/// </summary>
public class ItemSearchTrie
{
    private readonly TrieNode _root;

    public ItemSearchTrie()
    {
        _root = new TrieNode();
    }

    public void Insert(string word, int itemId)
    {
        var current = _root;
        word = word.ToLowerInvariant();

        foreach (char c in word)
        {
            if (!current.Children.ContainsKey(c))
            {
                current.Children[c] = new TrieNode();
            }
            current = current.Children[c];
        }

        current.IsEndOfWord = true;
        current.ItemIds.Add(itemId);
    }

    public IEnumerable<int> Search(string prefix)
    {
        var current = _root;
        prefix = prefix.ToLowerInvariant();

        // Navigate to the prefix
        foreach (char c in prefix)
        {
            if (!current.Children.ContainsKey(c))
            {
                return Enumerable.Empty<int>();
            }
            current = current.Children[c];
        }

        // Collect all item IDs from this node and its descendants
        var results = new HashSet<int>();
        CollectItemIds(current, results);
        return results;
    }

    public IEnumerable<string> GetAutocompleteSuggestions(string prefix, int maxSuggestions = 10)
    {
        var current = _root;
        prefix = prefix.ToLowerInvariant();

        // Navigate to the prefix
        foreach (char c in prefix)
        {
            if (!current.Children.ContainsKey(c))
            {
                return Enumerable.Empty<string>();
            }
            current = current.Children[c];
        }

        // Collect all words with this prefix
        var suggestions = new List<string>();
        CollectWords(current, prefix, suggestions, maxSuggestions);
        return suggestions;
    }

    private void CollectItemIds(TrieNode node, HashSet<int> results)
    {
        if (node.IsEndOfWord)
        {
            foreach (var itemId in node.ItemIds)
            {
                results.Add(itemId);
            }
        }

        foreach (var child in node.Children.Values)
        {
            CollectItemIds(child, results);
        }
    }

    private void CollectWords(TrieNode node, string currentWord, List<string> results, int maxResults)
    {
        if (results.Count >= maxResults) return;

        if (node.IsEndOfWord)
        {
            results.Add(currentWord);
        }

        foreach (var kvp in node.Children)
        {
            if (results.Count >= maxResults) return;
            CollectWords(kvp.Value, currentWord + kvp.Key, results, maxResults);
        }
    }

    public void Clear()
    {
        _root.Children.Clear();
    }

    private class TrieNode
    {
        public Dictionary<char, TrieNode> Children { get; } = new();
        public bool IsEndOfWord { get; set; } = false;
        public HashSet<int> ItemIds { get; } = new();
    }
}
