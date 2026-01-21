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
        public int currentFace;

        [SerializeField] private DiceColorPalette colorPalette;

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
            if (colorPalette != null)
                body.color = colorPalette.Get(Data.color);
        }

        // private void ApplyColor()
        // {
        //     body.color = Data.color switch
        //     {
        //         DiceColor.White => Color.white,
        //         DiceColor.Red => Color.red,
        //         DiceColor.Yellow => Color.yellow,
        //         DiceColor.Green => Color.green,
        //         DiceColor.Blue => Color.blue,
        //         DiceColor.Purple => new Color(0.6f, 0.3f, 0.8f),
        //         _ => Color.gray
        //     };
        // }


        /// <summary>
        /// 현재 보이는 눈금 설정
        /// </summary>
        public void SetFace(int value)
        {
            if (!this || face == null)
                return;

            currentFace = value;

            int index = Mathf.Clamp(value - 1, 0, faceSprites.Length - 1);
            face.sprite = faceSprites[index];

            RefreshFaceColor();
        }

        public void Reroll()
        {
            int newFace = Random.Range(1, 7);
            SetFace(newFace);
        }

        public void RefreshColor()
        {
            ApplyColor();
            // 필요하면 나중에 다른 비주얼 갱신도 여기에 추가
        }

        public void SetColor(DiceColor newColor)
        {
            if (Data == null)
                return;

            Data.color = newColor;
            ApplyColor();
        }

        private void RefreshFaceColor()
        {
            if (Data == null || face == null)
                return;

            if (Data.contaminatedFace == currentFace)
                face.color = Color.magenta; // 분홍
            else
                face.color = Color.black;    // 기본 회색
        }
    }
}
