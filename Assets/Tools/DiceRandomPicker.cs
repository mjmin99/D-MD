using System.Collections.Generic;
using UnityEngine;

public static class DiceRandomPicker
{
    public static List<T> PickN<T>(IReadOnlyCollection<T> source, int n)
    {
        var list = new List<T>(source);

        if (list.Count == 0)
            return list;

        if (n >= list.Count)
            return list;

        // Fisher–Yates shuffle
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }

        return list.GetRange(0, n);
    }
}
