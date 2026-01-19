using UnityEngine;

namespace DiceSystem
{
    public class DiceView : MonoBehaviour
    {
        [Header("Renderers")]
        [SerializeField] private SpriteRenderer body;
        [SerializeField] private SpriteRenderer face;

        [Header("Face Sprites (Index 0 = 1)")]
        [SerializeField] private Sprite[] faceSprites; // size = 6

        public bool IsExtraDice { get; private set; }

        public DiceData Data { get; private set; }
        private int currentFace;


        private void Awake()
        {
            if (body == null)
                body = transform.Find("Body")?.GetComponent<SpriteRenderer>();

            if (face == null)
                face = transform.Find("Face")?.GetComponent<SpriteRenderer>();
        }

        public void Bind(DiceData data, bool isExtraDice = false)
        {
            Data = data;
            IsExtraDice = isExtraDice;

            ApplyColor();

            // 임시: 시작 눈금
            SetFace(Random.Range(1, 7));
        }

        private void ApplyColor()
        {
            body.color = Data.color switch
            {
                DiceColor.White => Color.white,
                DiceColor.Red => Color.red,
                DiceColor.Yellow => Color.yellow,
                DiceColor.Green => Color.green,
                DiceColor.Blue => Color.blue,
                DiceColor.Purple => new Color(0.6f, 0.3f, 0.8f),
                _ => Color.gray
            };
        }


        /// <summary>
        /// 현재 보이는 눈금 설정
        /// </summary>
        public void SetFace(int value)
        {
            currentFace = value;

            // 1. 스프라이트 설정
            face.sprite = faceSprites[value - 1];

            // 2. 기본 색 (정상 면)
            face.color = Color.black;

            // 3. 오염면이면 분홍색
            if (Data.contaminatedFace == value)
            {
                face.color = new Color(1f, 0.4f, 0.7f);
            }
        }

        public void Reroll()
        {
            int newFace = Random.Range(1, 7);
            SetFace(newFace);
        }
    }
}
