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

        [Header("Screen Clamp")]
        [SerializeField] private float screenMargin = 20f;

        private Camera cam;

        private void Awake()
        {
            cam = Camera.main;

            if (root == null)
                root = (RectTransform)transform;

            Hide();
        }

        public void Show(DiceData data, Vector2 hoverWorldPos)
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
            text.text = BuildText(data);
        }

        public void Hide()
        {
            if (root != null)
                root.gameObject.SetActive(false);
        }

        private string BuildText(DiceData data)
        {
            // 오염면 표시(White는 -1이니까 예외처리)
            string contam = data.contaminatedFace <= 0 ? "없음" : data.contaminatedFace.ToString();

            // 지금은 “눈금→인카운트”를 임시 문자열로.
            // 나중에 EncounterTable 붙이면 여기만 교체하면 됨.
            return
                $"색상: {data.color}\n" +
                $"오염면: {contam}\n\n" +
                "눈금별 인카운트:\n" +
                "1 → (임시) 인카운트\n" +
                "2 → (임시) 인카운트\n" +
                "3 → (임시) 인카운트\n" +
                "4 → (임시) 인카운트\n" +
                "5 → (임시) 인카운트\n" +
                "6 → (임시) 인카운트";
        }
    }
}
