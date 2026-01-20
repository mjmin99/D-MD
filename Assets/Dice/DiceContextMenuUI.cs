using System.Collections.Generic;
using DiceSystem;
using UnityEngine;
using UnityEngine.UI;

public class DiceContextMenuUI : MonoBehaviour
{
    [Header("Buttons")]
    public Button btnChangeFace;
    public Button btnReroll;
    public Button btnReplace;
    public Button btnRemove;
    public Button btnClose;
    public Button btnSetToContaminated;
    public Button btnChangeColor;

    [SerializeField] private DiceFacePickerUI facePicker;
    [SerializeField] private DiceColorPickerUI colorPicker;
    private void Awake()
    {
        gameObject.SetActive(false);

        if (btnClose != null)
            btnClose.onClick.AddListener(Close);

        if (btnReroll != null)
            btnReroll.onClick.AddListener(OnClickReroll);

        if (btnChangeFace != null)
            btnChangeFace.onClick.AddListener(OnClickChangeFace);

        if (btnReplace != null)
            btnReplace.onClick.AddListener(OnClickReplace);

        if (btnRemove != null)
            btnRemove.onClick.AddListener(OnClickRemove);

        if (btnSetToContaminated != null)
            btnSetToContaminated.onClick.AddListener(OnClickSetToContaminated);

        if (btnChangeColor != null)
            btnChangeColor.onClick.AddListener(OnClickChangeColor);
    }

    public void Open(Vector2 screenPos)
    {
        var rt = (RectTransform)transform;
        var canvas = GetComponentInParent<Canvas>();
        var canvasRT = canvas.transform as RectTransform;

        // Screen → Canvas Local 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRT,
            screenPos,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out Vector2 localPos
        );

        Vector2 size = rt.rect.size;
        Vector2 pivot = rt.pivot;
        Vector2 pos = localPos;

        float left = pos.x - size.x * pivot.x;
        float right = pos.x + size.x * (1f - pivot.x);
        float bottom = pos.y - size.y * pivot.y;
        float top = pos.y + size.y * (1f - pivot.y);

        Vector2 canvasSize = canvasRT.rect.size;

        if (right > canvasSize.x / 2f)
            pos.x -= (right - canvasSize.x / 2f);

        if (left < -canvasSize.x / 2f)
            pos.x -= (left + canvasSize.x / 2f);

        if (top > canvasSize.y / 2f)
            pos.y -= (top - canvasSize.y / 2f);

        if (bottom < -canvasSize.y / 2f)
            pos.y -= (bottom + canvasSize.y / 2f);

        rt.anchoredPosition = pos;
        gameObject.SetActive(true);
    }


    public void Close()
    {
        gameObject.SetActive(false);

        if (DiceContextMenuSystem.Instance != null)
            DiceContextMenuSystem.Instance.NotifyClosed();
    }

    private void OnClickReroll()
    {
        Debug.Log("ContextMenu: Reroll clicked");
        DiceCommandExecutor.RerollSelected();
        Close();
    }
    private void OnClickChangeFace()
    {
        if (facePicker == null)
            return;

        facePicker.Open(this); // target 제거
    }

    private void OnClickReplace()
    {
        DiceCommandExecutor.ReplaceSelected();
        Close();
    }

    private void OnClickRemove()
    {
        var selected = DiceSelectionController.Instance.Selected;

        // snapshot을 떠야 안전 (Destroy 중 컬렉션 변경 방지)
        var toRemove = new List<DiceView>(selected);

        foreach (var dice in toRemove)
        {
            if (!dice)
                continue;

            DiceSelectionController.Instance.RemoveFromSelection(dice);
            Destroy(dice.gameObject);
        }

        Close();
    }

    private void OnClickSetToContaminated()
    {
        DiceCommandExecutor.SetToContaminatedSelected();
        Close();
    }

    private void OnClickChangeColor()
    {
        colorPicker.Open(this);
    }
}
