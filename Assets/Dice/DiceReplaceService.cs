using UnityEngine;

namespace DiceSystem
{
    public class DiceReplaceService : MonoBehaviour
    {
        public static DiceReplaceService Instance { get; private set; }

        [Header("Spawn")]
        [SerializeField] private DiceView dicePrefab;     // Dice 프리팹 (Root에 DiceView 붙은 그거)
        [SerializeField] private bool includeWhite = false; // Replace 랜덤에 White 포함 여부

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public void ReplaceWithRandom(DiceView oldDice)
        {
            if (oldDice == null) return;

            // 1) 자리 기억
            Transform parent = oldDice.transform.parent;
            Vector3 localPos = oldDice.transform.localPosition;

            // 2) 새 DiceData 생성 (색/오염면 랜덤)
            DiceData newData = CreateRandomDiceData(includeWhite);

            // 3) 새 Dice 생성
            DiceView newDice = Instantiate(dicePrefab, parent);
            newDice.transform.localPosition = localPos;

            // 4) Bind + 현재 눈금 랜덤 지정
            newDice.Bind(newData);

            int face = Random.Range(1, 7);
            newDice.SetFace(face);

            // 5) 기존 Dice 제거
            Destroy(oldDice.gameObject);
        }

        private DiceData CreateRandomDiceData(bool includeWhite)
        {
            DiceColor color = GetRandomColor(includeWhite);

            int contaminated = (color == DiceColor.White)
                ? -1
                : Random.Range(1, 7);

            // 핵심 수정: DiceMapBootstrap을 통해 생성
            return DiceMapBootstrap.Instance.CreateDiceData(
                color,
                contaminated
            );
        }

        private DiceColor GetRandomColor(bool includeWhite)
        {
            // DiceColor가 White/Red/Yellow/Green/Blue/Purple 순서라는 전제
            // (다르면 switch로 바꾸자)
            if (includeWhite)
            {
                int v = Random.Range(0, 6); // 0~5
                return (DiceColor)v;
            }
            else
            {
                int v = Random.Range(1, 6); // 1~5 (White 제외)
                return (DiceColor)v;
            }
        }
    }
}
