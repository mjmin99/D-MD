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

    public void Open(DiceContextMenuUI ownerMenu)
    {
        owner = ownerMenu;
        gameObject.SetActive(true);
    }

    private void OnPick(DiceColor color)
    {
        DiceCommandExecutor.SetColorSelected(color);

        owner.Close();
        Close();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        owner = null;
    }
}
