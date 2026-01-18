using DiceSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class DiceContextMenuSystem : MonoBehaviour
{
    public static DiceContextMenuSystem Instance { get; private set; }

    [SerializeField] private DiceContextMenuUI menuUI;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (Mouse.current == null)
            return;

        // 우클릭 감지 (신 Input System)
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            TryOpenContextMenu();
        }
    }

    private void TryOpenContextMenu()
    {
        if (DiceHoverSystem.Instance == null)
        {
            Debug.LogWarning("[ContextMenu] DiceHoverSystem.Instance is null");
            return;
        }

        var hovered = DiceHoverSystem.Instance.CurrentHoverDice;
        if (hovered == null)
            return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        OpenMenu(hovered, mousePos);
    }

    private void OpenMenu(DiceView target, Vector2 screenPos)
    {
        // Tooltip이 떠 있었다면 정리
        DiceTooltipController.Instance?.HideAll();

        if (menuUI == null)
        {
            Debug.LogWarning("[ContextMenu] menuUI is null");
            return;
        }

        menuUI.Open(target, screenPos);
    }
}
