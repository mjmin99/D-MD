using UnityEngine;

public class DiceSelectionVisual : MonoBehaviour
{
    [SerializeField] private GameObject selectionRing;

    private void Awake()
    {
        if (selectionRing != null)
            selectionRing.SetActive(false);
    }

    public void SetSelected(bool selected)
    {
        if (selectionRing == null) return;
        selectionRing.SetActive(selected);
    }
}
