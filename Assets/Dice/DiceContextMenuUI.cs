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

    private void Awake()
    {
        // 시작 시 숨김
        gameObject.SetActive(false);

        if (btnClose != null)
            btnClose.onClick.AddListener(Close);
    }

    public void Open(DiceView targetDice, Vector2 screenPos)
    {
        target = targetDice;

        var rt = (RectTransform)transform;
        rt.position = screenPos;

        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        target = null;
    }
}
