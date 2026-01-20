using DiceSystem;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;

public class DiceSelectionInput : MonoBehaviour
{
    private Camera cam;

    [Header("Selection Box UI")]
    [SerializeField] private RectTransform selectionBox;
    [SerializeField] private Canvas canvas;

    private bool isDragging;

    private Vector2 dragStartScreen;
    private Vector2 dragEndScreen;

    private bool isGroupDragging;
    private Vector3 dragStartMouseWorld;
    private Dictionary<DiceView, Vector3> dragStartPositions = new();

    void Awake()
    {
        cam = Camera.main;
        selectionBox.gameObject.SetActive(false);
    }

    void Update()
    {
        if (DiceContextMenuSystem.Instance != null &&
        DiceContextMenuSystem.Instance.IsOpen)
            return;

        if (Input.GetMouseButtonDown(0))
            OnMouseDown();

        if (isGroupDragging && Input.GetMouseButton(0))
            UpdateGroupDrag();

        if (Input.GetMouseButtonUp(0))
            EndGroupDrag();

        if (Input.GetMouseButton(0) && isDragging)
            UpdateSelectionBox();

        if (Input.GetMouseButtonUp(0) && isDragging)
            CompleteBoxSelection();
    }

    void OnMouseDown()
    {
        DiceView clickedDice = RaycastDice();

        if (clickedDice != null)
        {
            var selection = DiceSelectionController.Instance.Selected;

            // 이미 선택된 주사위를 클릭했다면 → 집단 이동
            if (selection.Contains(clickedDice) && selection.Count > 1)
            {
                StartGroupDrag();
            }
            else
            {
                DiceSelectionController.Instance.SelectSingle(clickedDice);
            }

            isDragging = false;
        }
        else
        {
            isDragging = true;
            dragStartScreen = Input.mousePosition;

            selectionBox.gameObject.SetActive(true);
            UpdateSelectionBox();
        }
    }

    void UpdateGroupDrag()
    {
        Vector3 currentMouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 delta = currentMouseWorld - dragStartMouseWorld;
        delta.z = 0f;

        foreach (var pair in dragStartPositions)
        {
            if (!pair.Key) continue;
            pair.Key.transform.position = pair.Value + delta;
        }
    }

    void EndGroupDrag()
    {
        isGroupDragging = false;
        dragStartPositions.Clear();
    }

    void StartGroupDrag()
    {
        isGroupDragging = true;
        dragStartMouseWorld = cam.ScreenToWorldPoint(Input.mousePosition);

        dragStartPositions.Clear();

        foreach (var dice in DiceSelectionController.Instance.Selected)
        {
            if (!dice) continue;
            dragStartPositions[dice] = dice.transform.position;
        }
    }

    DiceView RaycastDice()
    {
        Vector2 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(worldPos, Vector2.zero);

        DiceView topDice = null;
        int highestOrder = int.MinValue;

        foreach (var hit in hits)
        {
            var dice = hit.collider.GetComponentInParent<DiceView>();
            if (dice == null) continue;

            var sr = dice.GetComponentInChildren<SpriteRenderer>();
            if (sr == null) continue;

            if (sr.sortingOrder > highestOrder)
            {
                highestOrder = sr.sortingOrder;
                topDice = dice;
            }
        }

        return topDice;
    }

    void UpdateSelectionBox()
    {
        Vector2 currentScreen = Input.mousePosition;

        RectTransform canvasRect = canvas.transform as RectTransform;

        Vector2 startLocal;
        Vector2 currentLocal;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            dragStartScreen,
            null,
            out startLocal
        );

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            currentScreen,
            null,
            out currentLocal
        );

        Vector2 min = Vector2.Min(startLocal, currentLocal);
        Vector2 max = Vector2.Max(startLocal, currentLocal);

        selectionBox.anchoredPosition = min;
        selectionBox.sizeDelta = max - min;
    }

    void CompleteBoxSelection()
    {
        selectionBox.gameObject.SetActive(false);
        isDragging = false;

        dragEndScreen = Input.mousePosition;

        Vector2 min = Vector2.Min(dragStartScreen, dragEndScreen);
        Vector2 max = Vector2.Max(dragStartScreen, dragEndScreen);

        Rect selectionRect = Rect.MinMaxRect(min.x, min.y, max.x, max.y);

        List<DiceView> selected = new();

        foreach (var dice in FindObjectsByType<DiceView>(FindObjectsSortMode.None))
        {
            var col = dice.GetComponent<Collider2D>();
            if (col == null) continue;

            Bounds b = col.bounds;

            Vector2 screenMin = cam.WorldToScreenPoint(b.min);
            Vector2 screenMax = cam.WorldToScreenPoint(b.max);

            // 완전 포함 판정
            if (selectionRect.Contains(screenMin) &&
                selectionRect.Contains(screenMax))
            {
                selected.Add(dice);
            }
        }

        DiceSelectionController.Instance.SetSelection(selected);
    }
}
