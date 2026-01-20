using System.Collections.Generic;
using DiceSystem;
using UnityEngine;

public class DiceSelectionController : MonoBehaviour
{
    public static DiceSelectionController Instance { get; private set; }

    private readonly HashSet<DiceView> selected = new();

    public IReadOnlyCollection<DiceView> Selected => selected;
    public DiceView Primary => selected.Count == 1 ? GetFirst() : null;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    DiceView GetFirst()
    {
        foreach (var d in selected)
            return d;
        return null;
    }

    // 단일 선택 (MVP 핵심)
    public void SelectSingle(DiceView dice)
    {
        // Debug.Log($"SelectSingle: {dice}");
        if (dice == null)
        {
            Clear();
            return;
        }

        if (selected.Count == 1 && selected.Contains(dice))
            return; // 이미 선택됨

        ClearInternal();

        selected.Add(dice);
        SetVisual(dice, true);
    }

    public void Clear()
    {
        if (selected.Count == 0)
            return;

        ClearInternal();
    }

    void ClearInternal()
    {
        foreach (var d in selected)
            SetVisual(d, false);

        selected.Clear();
    }

    void SetVisual(DiceView dice, bool selected)
    {
        var visual = dice.GetComponent<DiceSelectionVisual>();

        // Debug.Log($"SetVisual: {dice.name}, visual={(visual != null)}, selected={selected}");

        if (visual != null)
            visual.SetSelected(selected);
    }

    public void SetSelection(IEnumerable<DiceView> dices)
    {
        ClearInternal();

        foreach (var dice in dices)
        {
            selected.Add(dice);
            SetVisual(dice, true);
        }
    }
}
