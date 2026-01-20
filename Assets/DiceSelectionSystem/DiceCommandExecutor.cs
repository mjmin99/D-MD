using System.Collections.Generic;
using DiceSystem;
using UnityEngine;

public static class DiceCommandExecutor
{
    public static void RerollSelected()
    {
        var selected = DiceSelectionController.Instance.Selected;
        Debug.Log($"RerollSelected count = {selected.Count}");

        foreach (var dice in selected)
        {
            Debug.Log($" - reroll {dice.name}");
            dice.Reroll();
        }
    }

    public static void SetFaceSelected(int face)
    {
        var selected = DiceSelectionController.Instance.Selected;
        var snapshot = new List<DiceView>(selected);

        foreach (var dice in snapshot)
        {
            if (!dice) continue;
            dice.SetFace(face);
        }
    }

    public static void SetToContaminatedSelected()
    {
        var selected = DiceSelectionController.Instance.Selected;

        foreach (var dice in selected)
        {
            int contamFace = dice.Data.contaminatedFace;
            if (contamFace < 1)
                continue;

            dice.SetFace(contamFace);
        }
    }

    public static void SetColorSelected(DiceColor color)
    {
        var selected = DiceSelectionController.Instance.Selected;

        foreach (var dice in selected)
        {
            dice.SetColor(color);
        }
    }

    public static void ReplaceSelected()
    {
        var selected = DiceSelectionController.Instance.Selected;
        var snapshot = new List<DiceView>(selected);

        foreach (var dice in snapshot)
        {
            if (!dice) continue;
            DiceReplaceService.Instance.ReplaceWithRandom(dice);
        }
    }
}