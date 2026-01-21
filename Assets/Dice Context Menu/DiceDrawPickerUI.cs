using System;
using UnityEngine;
using UnityEngine.UI;

public class DiceDrawPickerUI : MonoBehaviour
{
    [SerializeField] private Button[] buttons; // size = 5 (1~5)

    private Action<int> onPick;

    public Button btnClose;

    private void Awake()
    {
        if (btnClose != null)
            btnClose.onClick.AddListener(Close);

        gameObject.SetActive(false);

        for (int i = 0; i < buttons.Length; i++)
        {
            int n = i + 1;
            buttons[i].onClick.AddListener(() => Pick(n));
        }
    }

    public void Open(int selectedCount, Action<int> onPick)
    {
        this.onPick = onPick;
        gameObject.SetActive(true);

        // 선택 개수보다 큰 버튼은 비활성
        for (int i = 0; i < buttons.Length; i++)
        {
            int n = i + 1;
            buttons[i].interactable = selectedCount >= n;
        }
    }

    private void Pick(int n)
    {
        onPick?.Invoke(n);
        Close();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        onPick = null;
    }
}
