using DiceSystem;
using UnityEngine;
using UnityEngine.UI;

public class DiceFacePickerUI : MonoBehaviour
{
    [SerializeField] private Button[] faceButtons; // 크기 6

    private DiceContextMenuUI owner;

    public Button btnClose;

    private void Awake()
    {
        gameObject.SetActive(false);

        if (btnClose != null)
            btnClose.onClick.AddListener(Close);

        for (int i = 0; i < faceButtons.Length; i++)
        {
            int face = i + 1;
            faceButtons[i].onClick.AddListener(() => OnPick(face));
        }
    }

    public void Open(DiceContextMenuUI ownerMenu)
    {
        owner = ownerMenu;
        gameObject.SetActive(true);
    }

    private void OnPick(int face)
    {
        DiceCommandExecutor.SetFaceSelected(face);

        owner.Close();      // 메인 메뉴 닫기
        gameObject.SetActive(false);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        owner = null;
    }
}
