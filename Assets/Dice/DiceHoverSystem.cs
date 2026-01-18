using UnityEngine;
using UnityEngine.InputSystem;

namespace DiceSystem
{
    public class DiceHoverSystem : MonoBehaviour
    {
        public static DiceHoverSystem Instance { get; private set; }

        [Header("Hover Settings")]
        [SerializeField] private float hoverDelay = 0.5f;

        public DiceView CurrentHoverDice { get; private set; }

        private Camera cam;

        private DiceView currentHover;
        private float hoverTime;
        private bool tooltipShown;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            cam = Camera.main;
        }

        private void Update()
        {
            if (DiceContextMenuSystem.Instance != null &&
                DiceContextMenuSystem.Instance.IsOpen)
            {
                return;
            }

            if (Mouse.current == null || cam == null)
                return;

            Vector2 mouseWorld =
                cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            RaycastHit2D hit =
                Physics2D.Raycast(mouseWorld, Vector2.zero);

            DiceView hitView = hit.collider
                ? hit.collider.GetComponentInParent<DiceView>()
                : null;

            // Hover 대상이 바뀐 경우
            if (hitView != currentHover)
            {
                ClearHover();
                currentHover = hitView;
                CurrentHoverDice = hitView; 
            }

            if (currentHover == null)
                return;

            // Hover 시간 누적
            hoverTime += Time.deltaTime;

            // 지연 후 Tooltip 표시
            if (!tooltipShown && hoverTime >= hoverDelay)
            {
                tooltipShown = true;
                DiceTooltipController.Instance?.Show(
                    currentHover,
                    mouseWorld
                );
            }
        }

        private void ClearHover()
        {
            if (tooltipShown)
            {
                DiceTooltipController.Instance?.HideAll();
            }

            hoverTime = 0f;
            tooltipShown = false;
            currentHover = null;
            CurrentHoverDice = null;  
        }

        public void ForceClearHover()
        {
            ClearHover();
        }
    }
}
