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

    private DiceView target;
    [SerializeField] private DiceFacePickerUI facePicker;
    private void Awake()
    {
        // 시작 시 숨김
        gameObject.SetActive(false);

        if (btnClose != null)
            btnClose.onClick.AddListener(Close);

        if (btnReroll != null)
            btnReroll.onClick.AddListener(OnClickReroll);

        if (btnChangeFace != null)
            btnChangeFace.onClick.AddListener(OnClickChangeFace);

    }

    public void Open(DiceView targetDice, Vector2 screenPos)
    {
        target = targetDice;

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
        target = null;
    }

    private void OnClickReroll()
    {
        if (target == null)
            return;

        target.Reroll();
        Close();
    }
    private void OnClickChangeFace()
    {
        if (target == null || facePicker == null)
            return;

        facePicker.Open(target, this);
    }
}
