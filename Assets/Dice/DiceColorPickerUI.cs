using DiceSystem;
using UnityEngine;
using UnityEngine.UI;

public class DiceColorPickerUI : MonoBehaviour
{
    [SerializeField] private Button[] colorButtons;
    // 순서 기준:
    // 0 = White
    // 1 = Red
    // 2 = Yellow
    // 3 = Green
    // 4 = Blue
    // 5 = Purple

    public Button btnClose;

    private DiceView target;
    private DiceContextMenuUI owner;

    private void Awake()
    {
        gameObject.SetActive(false);

        if (btnClose != null)
            btnClose.onClick.AddListener(Close);

        for (int i = 0; i < colorButtons.Length; i++)
        {
            DiceColor color = (DiceColor)i; // White 포함
            colorButtons[i].onClick.AddListener(() => OnPick(color));
        }
    }

    public void Open(DiceView targetDice, DiceContextMenuUI ownerMenu)
    {
        target = targetDice;
        owner = ownerMenu;
        gameObject.SetActive(true);
    }

    private void OnPick(DiceColor color)
    {
        if (target != null)
        {
            target.Data.color = color;

            // DiceView에 추가해 둔 public 래퍼
            target.RefreshColor();
        }

        owner.Close();          // 메인 컨텍스트 메뉴 닫기
        gameObject.SetActive(false);
        target = null;
    }

    public void Close()
    {
        gameObject.SetActive(false);
        target = null;
    }
}
