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
            // 1. 기존 DiceView 전부 제거
            for (int y = 0; y < DiceMap.Height; y++)
            {
                for (int x = 0; x < DiceMap.Width; x++)
                {
                    if (views[x, y] != null)
                    {
                        Destroy(views[x, y].gameObject);
                        views[x, y] = null;
                    }
                }
            }

            // 2. DiceData 새로 생성
            map = DiceMapGenerator.Generate();

            // 3. 전 좌표에 DiceView 재생성
            for (int y = 0; y < DiceMap.Height; y++)
            {
                for (int x = 0; x < DiceMap.Width; x++)
                {
                    DiceView view = Instantiate(dicePrefab, transform);
                    view.Bind(map.Get(x, y));
                    view.transform.localPosition = GetLocalPos(x, y);

                    views[x, y] = view;
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
