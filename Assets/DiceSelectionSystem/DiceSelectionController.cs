using System.Collections.Generic;
using DiceSystem;
using UnityEngine;

public class DiceSelectionController : MonoBehaviour
{
    public static DiceSelectionController Instance { get; private set; }

    private readonly HashSet<DiceView> selected = new();

    public void PruneDestroyed()
    {
        if (selected.Count == 0) return;

        // HashSet은 순회 중 Remove 불가 → 스냅샷
        var toRemove = new List<DiceView>();

        foreach (var d in selected)
        {
            if (!d) // Unity null 체크: Destroy된 오브젝트도 걸림
                toRemove.Add(d);
        }

        for (int i = 0; i < toRemove.Count; i++)
            selected.Remove(toRemove[i]);
    }

    public IReadOnlyCollection<DiceView> Selected
    {
        get
        {
            PruneDestroyed();
            return selected;
        }
    }

    public bool HasSelection => selected.Count > 0;
    public bool IsSingle => selected.Count == 1;
    public bool IsMultiple => selected.Count > 1;

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
        ClearInternal();

        if (!dice)
            return;

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
        if (selected.Count == 0)
            return;

        var snapshot = new List<DiceView>(selected);
        selected.Clear(); // 먼저 비운다

        foreach (var dice in snapshot)
        {
            if (!dice)
                continue;

            var visual = dice.GetComponent<DiceSelectionVisual>();
            if (visual)
                visual.SetSelected(false);
        }
    }

    void SetVisual(DiceView dice, bool selected)
    {
        if (!dice) // Unity Object null 체크
            return;

        var visual = dice.GetComponent<DiceSelectionVisual>();
        if (!visual)
            return;

        visual.SetSelected(selected);
    }

    public void SetSelection(IEnumerable<DiceView> dices)
    {
        ClearInternal();

        foreach (var dice in dices)
        {
            if (!dice)
                continue;

            selected.Add(dice);
            SetVisual(dice, true);
        }
    }

    public void RemoveFromSelection(DiceView dice)
    {
        if (dice == null)
            return;

        if (selected.Remove(dice))
        {
            // 이미 파괴될 예정이므로 visual 접근 X
        }
    }
}
