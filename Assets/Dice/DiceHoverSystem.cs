using UnityEngine;
using UnityEngine.InputSystem;

namespace DiceSystem
{
    public class DiceHoverSystem : MonoBehaviour
    {
        [SerializeField] private float hoverDelay = 0.2f;

        private Camera cam;
        private DiceView currentHover;
        private float hoverTime;
        private bool tooltipShown;

        private void Awake()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            if (Mouse.current == null || cam == null)
                return;

            Vector2 mouseWorld =
                cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            RaycastHit2D hit = Physics2D.Raycast(mouseWorld, Vector2.zero);
            DiceView hitView = hit.collider
                ? hit.collider.GetComponentInParent<DiceView>()
                : null;

            if (hitView != null)
            {
                // 🔥 딜레이, 상태 전부 무시하고 바로 표시
                DiceTooltipController.Instance.Show(hitView, mouseWorld);
            }
            else
            {
                DiceTooltipController.Instance.HideAll();
            }
        }

        private void ResetHover()
        {
            if (tooltipShown && currentHover != null)
            {
                DiceTooltipController.Instance?.HideAll();
            }

            hoverTime = 0f;
            tooltipShown = false;
            currentHover = null;
        }
    }
}
