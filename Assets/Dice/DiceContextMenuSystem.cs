using DiceSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class DiceContextMenuSystem : MonoBehaviour
{
    public static DiceContextMenuSystem Instance { get; private set; }

    public bool IsOpen { get; private set; }
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

        if (!DiceSelectionController.Instance.HasSelection)
            return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        OpenMenu(mousePos);
    }

    private void OpenMenu(Vector2 screenPos)
    {
        DiceTooltipController.Instance?.HideAll();

        if (menuUI == null)
            return;

        IsOpen = true;
        menuUI.Open(screenPos);
    }

    public void NotifyClosed()
    {
        IsOpen = false;

        if (DiceHoverSystem.Instance != null)
            DiceHoverSystem.Instance.ForceClearHover();
    }
}
