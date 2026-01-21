using UnityEngine;

[CreateAssetMenu(menuName = "Dice/Encounter")]
public class EncounterDefinition : ScriptableObject
{
    public string encounterName;

    [TextArea]
    public string description;

    public bool isBlockedByContamination = true;

    // 나중에 확장용
    // public Sprite icon;
    // public Color uiColor;
}
