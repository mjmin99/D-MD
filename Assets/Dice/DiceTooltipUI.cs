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
            if (data == null || cam == null)
                return;

            root.gameObject.SetActive(true);

            // 1. 월드 → 스크린
            Vector2 screenPos = cam.WorldToScreenPoint(hoverWorldPos);

            // 2. 네가 만든 예쁜 오프셋 그대로 사용
            Vector2 targetPos = screenPos + screenOffset;

            // 3. 화면 밖 방지
            targetPos = ClampToScreen(targetPos);

            root.position = targetPos;
            text.text = BuildText(data);
        }

        private Vector2 ClampToScreen(Vector2 pos)
        {
            float minX = screenMargin;
            float maxX = Screen.width - screenMargin;

            float minY = screenMargin;
            float maxY = Screen.height - screenMargin;

            pos.x = Mathf.Clamp(pos.x, minX, maxX);
            pos.y = Mathf.Clamp(pos.y, minY, maxY);

            return pos;
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
