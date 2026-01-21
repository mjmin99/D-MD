using TMPro;
using UnityEngine;

namespace DiceSystem
{
    public class DiceTooltipUI : MonoBehaviour
    {
        [Header("UI Refs")]
        [SerializeField] private RectTransform root; // DiceTooltip Panel
        [SerializeField] private TMP_Text text;

        [Header("Layout")]
        [SerializeField] private Vector2 screenOffset = new Vector2(16f, -16f);

        [SerializeField] private DiceColorPalette colorPalette;

        private Camera cam;

        private void Awake()
        {
            cam = Camera.main;

            if (root == null)
                root = (RectTransform)transform;

            Hide();
        }

        private string ColorToHex(Color color)
        {
            return ColorUtility.ToHtmlStringRGB(color);
        }

        public void Show(DiceData data, int currentFace, Vector2 hoverWorldPos)
        {
            if (data == null)
                return;

            var canvas = GetComponentInParent<Canvas>();
            var canvasRT = canvas.transform as RectTransform;

            root.gameObject.SetActive(true);

            // 월드 → 스크린
            Vector2 screenPos = cam.WorldToScreenPoint(hoverWorldPos);

            // 스크린 → Canvas 로컬
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRT,
                screenPos,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
                out Vector2 localPos
            );

            Vector2 pos = localPos + screenOffset;

            // ===== Canvas 기준 Clamp =====
            Vector2 size = root.rect.size;
            Vector2 pivot = root.pivot;
            Vector2 canvasSize = canvasRT.rect.size;

            float left = pos.x - size.x * pivot.x;
            float right = pos.x + size.x * (1f - pivot.x);
            float bottom = pos.y - size.y * pivot.y;
            float top = pos.y + size.y * (1f - pivot.y);

            if (right > canvasSize.x / 2f)
                pos.x -= (right - canvasSize.x / 2f);

            if (left < -canvasSize.x / 2f)
                pos.x -= (left + canvasSize.x / 2f);

            if (top > canvasSize.y / 2f)
                pos.y -= (top - canvasSize.y / 2f);

            if (bottom < -canvasSize.y / 2f)
                pos.y -= (bottom + canvasSize.y / 2f);
            // ==============================

            root.anchoredPosition = pos;
            text.text = BuildText(data, currentFace);
        }

        public void Hide()
        {
            if (root != null)
                root.gameObject.SetActive(false);
        }

        private string BuildText(DiceData data, int currentFace)
        {
            string colorHex = ColorToHex(colorPalette.Get(data.color));

            string colorLine = $"색상: <color=#{colorHex}>{data.color}</color>\n";

            // 오염면 여부 판단
            bool isContaminated = data.contaminatedFace > 0 &&
                                  data.contaminatedFace == currentFace;

            // 오염면 표시(White는 -1이니까 예외처리)
            string contam;
            string magentaHex = ColorUtility.ToHtmlStringRGB(Color.magenta);
            if (data.contaminatedFace <= 0)
            {
                contam = "없음";
            }
            else
            {
                contam = $"<color=#{magentaHex}>{data.contaminatedFace}</color>";
            }

            if (isContaminated)
            {
                return
                    $"{colorLine}" +
                    $"현재 눈금: {currentFace}   오염면: {contam}\n\n" +

                    $"해당 주사위는 <color=#{magentaHex}>오염</color>되었습니다.\n\n" +

                    "- 오염된 주사위는 인카운트 할 수 없습니다.\n\n" +

                    "- 협동 인카운트 시 공유주사위로 지정할 수 \n" +
                    "  없습니다.";
            }

            var encounter = data.encounterTable?.Get(currentFace);

            if (encounter == null)
            {
                return
                    $"{colorLine}" +
                    $"현재 눈금: {currentFace}   오염면: {contam}\n\n" +

                    "인카운트: 없음";
            }

            return
                $"{colorLine}" +
                $"현재 눈금: {currentFace}   오염면: {contam}\n\n" +

                $"인카운트: {encounter.encounterName}\n" +
                $"{encounter.description}";
        }
    }
}
