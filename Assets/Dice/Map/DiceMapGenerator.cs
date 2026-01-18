using System.Collections.Generic;
using UnityEngine;

namespace DiceSystem
{
    /// <summary>
    /// 규칙을 만족하는 19x19 주사위 맵 생성기
    /// </summary>
    public static class DiceMapGenerator
    {
        public static DiceMap Generate()
        {
            var dicePool = CreateDicePool();
            Shuffle(dicePool);

            DiceMap map = new DiceMap();

            int index = 0;
            for (int y = 0; y < DiceMap.Height; y++)
            {
                for (int x = 0; x < DiceMap.Width; x++)
                {
                    map.Set(x, y, dicePool[index++]);
                }
            }

            return map;
        }

        private static List<DiceData> CreateDicePool()
        {
            List<DiceData> pool = new List<DiceData>();
            int idCounter = 0;

            // White start dice
            pool.Add(new DiceData
            {
                id = idCounter++,
                color = DiceColor.White,
                contaminatedFace = -1
            });

            DiceColor[] colors =
            {
                DiceColor.Red,
                DiceColor.Yellow,
                DiceColor.Green,
                DiceColor.Blue,
                DiceColor.Purple
            };

            foreach (var color in colors)
            {
                for (int face = 1; face <= 6; face++)
                {
                    for (int i = 0; i < 12; i++)
                    {
                        pool.Add(new DiceData
                        {
                            id = idCounter++,
                            color = color,
                            contaminatedFace = face
                        });
                    }
                }
            }

            return pool;
        }

        private static void Shuffle(List<DiceData> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int rand = Random.Range(i, list.Count);
                (list[i], list[rand]) = (list[rand], list[i]);
            }
        }
    }
}
