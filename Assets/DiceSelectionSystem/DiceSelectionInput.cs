using DiceSystem;
using UnityEngine;

public class DiceSelectionInput : MonoBehaviour
{
    private Camera cam;

    void Awake()
    {
        cam = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TrySelectDice();
        }
    }

    void TrySelectDice()
    {
        Vector2 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero);

        Debug.Log($"Raycast hits count: {hits.Length}");

        DiceView topDice = null;
        int highestOrder = int.MinValue;

        foreach (var hit in hits)
        {
            Debug.Log($"Hit: {hit.collider.name} / layer={hit.collider.gameObject.layer}");

            var dice = hit.collider.GetComponentInParent<DiceSystem.DiceView>();
            if (dice == null) continue;

            var sr = dice.GetComponentInChildren<SpriteRenderer>();
            if (sr == null) continue;

            if (sr.sortingOrder > highestOrder)
            {
                highestOrder = sr.sortingOrder;
                topDice = dice;
            }
        }

        DiceSelectionController.Instance.SelectSingle(topDice);
    }
}
