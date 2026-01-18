using UnityEngine;

namespace DiceSystem
{
    public class DiceMapBootstrap : MonoBehaviour
    {
        [SerializeField] private DiceView dicePrefab;
        [SerializeField] private float cellSize = 1.1f;

        private DiceMap map;
        private DiceView[,] views = new DiceView[DiceMap.Width, DiceMap.Height];

        private void Start()
        {
            GenerateNewMap();
        }

        private void GenerateNewMap()
        {
            map = DiceMapGenerator.Generate();

            for (int y = 0; y < DiceMap.Height; y++)
            {
                for (int x = 0; x < DiceMap.Width; x++)
                {
                    DiceView view = Instantiate(dicePrefab, transform);
                    view.transform.localPosition = GetLocalPos(x, y);
                    view.Bind(map.Get(x, y));

                    view.gameObject.AddComponent<DiceDragHandler>();

                    views[x, y] = view;
                }
            }
        }

        public void Reshuffle()
        {
            // 1. DiceData만 다시 섞은 새 맵 생성
            map = DiceMapGenerator.Generate();

            // 2. 기존 DiceView에 새 데이터 재바인딩
            for (int y = 0; y < DiceMap.Height; y++)
            {
                for (int x = 0; x < DiceMap.Width; x++)
                {
                    views[x, y].Bind(map.Get(x, y));
                    views[x, y].transform.localPosition = GetLocalPos(x, y);
                }
            }
        }

        private Vector2 GetLocalPos(int x, int y)
        {
            return new Vector2(
                x * cellSize,
                y * cellSize
            );
        }
    }
}
