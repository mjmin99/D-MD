using UnityEngine;

namespace DiceSystem
{
    public class DiceTooltipController : MonoBehaviour
    {
        public static DiceTooltipController Instance { get; private set; }

        [SerializeField] private DiceTooltipUI tooltipUI;

        private DiceView current;

        

        private void Awake()
        {
            Instance = this;
        }

        public void Show(DiceView view, Vector2 worldPos)
        {
            if (view == null || tooltipUI == null)
                return;

            current = view;
            tooltipUI.Show(view.Data, worldPos);
        }

        public void HideAll()
        {
            current = null;
            tooltipUI?.Hide();
        }
    }
}
