using NUnit.Framework;
using UnityEngine;

namespace DiceSystem
{
    public class DiceMapBootstrap : MonoBehaviour
    {
        public static DiceMapBootstrap Instance { get; private set; }


        [SerializeField] private DiceView dicePrefab;
        [SerializeField] private float cellSize = 1.1f;

        [Header("Random Spawn Zone")]
        [SerializeField] private BoxCollider2D spawnZone;

        [Header("Encounter Table (SO)")]
        [SerializeField] private DiceFaceEncounterTable defaultEncounterTable;

        private DiceMap map;
        private DiceView[,] views = new DiceView[DiceMap.Width, DiceMap.Height];

        private void Start()
        {
            GenerateNewMap();
        }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }

        public DiceData CreateDiceData(DiceColor color, int contaminatedFace)
        {
            return new DiceData
            {
                color = color,
                contaminatedFace = contaminatedFace,
                encounterTable = defaultEncounterTable
            };
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

                    var data = map.Get(x, y);
                    EnsureEncounterTable(data);

                    view.Bind(map.Get(x, y));

                    view.gameObject.AddComponent<DiceDragHandler>();

                    views[x, y] = view;
                }
            }
        }

        public void Reshuffle()
        {
            ClearExtraDice();

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

                    var data = map.Get(x, y);
                    EnsureEncounterTable(data);

                    view.Bind(map.Get(x, y));
                    view.transform.localPosition = GetLocalPos(x, y);

                    if (!view.GetComponent<DiceDragHandler>())
                        view.gameObject.AddComponent<DiceDragHandler>();

                    views[x, y] = view;
                }
            }
        }

        private void EnsureEncounterTable(DiceData data)
        {
            if (data == null) return;

            // DiceData에 encounterTable 필드가 존재한다는 가정
            // (없으면 여기서 컴파일 에러 나니까 DiceData에 필드 추가 필요)
            if (data.encounterTable == null)
                data.encounterTable = defaultEncounterTable;
        }

        private Vector2 GetLocalPos(int x, int y)
        {
            return new Vector2(
                x * cellSize,
                y * cellSize
            );
        }

        public void AddOneDice()
        {
            // 1. DiceData 생성 (기존 규칙 재사용)
            DiceData data = CreateRandomDiceData();

            // 2. Dice 생성
            DiceView view = Instantiate(dicePrefab, transform);

            // 3. 위치: 부모 기준 랜덤 근처 (살짝 흩어지게)
            view.transform.localPosition = GetRandomSpawnPosition();

            // 4. DiceView 바인딩
            view.Bind(data, isExtraDice: true);


            if (!view.GetComponent<DiceDragHandler>())
                view.gameObject.AddComponent<DiceDragHandler>();
        }

        private DiceData CreateRandomDiceData()
        {
            // White 제외, 5색 중 랜덤
            DiceColor color = (DiceColor)Random.Range(1, 6);

            // 오염면 랜덤 1~6
            int contaminatedFace = Random.Range(1, 7);

            return new DiceData
            {
                color = color,
                contaminatedFace = contaminatedFace,
                encounterTable = defaultEncounterTable
            };
        }

        private Vector2 GetRandomSpawnPosition()
        {
            if (spawnZone == null)
            {
                Debug.LogWarning("[DiceMapBootstrap] SpawnZone not assigned");
                return Vector2.zero;
            }

            Bounds bounds = spawnZone.bounds;

            float x = Random.Range(bounds.min.x, bounds.max.x);
            float y = Random.Range(bounds.min.y, bounds.max.y);

            // 월드 → 로컬 좌표로 변환
            Vector2 worldPos = new Vector2(x, y);
            return transform.InverseTransformPoint(worldPos);
        }

        private void ClearExtraDice()
        {
            DiceView[] allDice = GetComponentsInChildren<DiceView>();

            foreach (var dice in allDice)
            {
                if (dice.IsExtraDice)
                {
                    Destroy(dice.gameObject);
                }
            }
        }
    }
}
