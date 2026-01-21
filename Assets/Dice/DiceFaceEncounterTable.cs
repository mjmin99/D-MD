using UnityEngine;

[CreateAssetMenu(menuName = "Dice/Face Encounter Table")]
public class DiceFaceEncounterTable : ScriptableObject
{
    [System.Serializable]
    public struct FaceEntry
    {
        [Range(1, 6)]
        public int face;

        public EncounterDefinition encounter;
    }

    public FaceEntry[] entries;

    public EncounterDefinition Get(int face)
    {
        foreach (var e in entries)
        {
            if (e.face == face)
                return e.encounter;
        }
        return null;
    }
}
