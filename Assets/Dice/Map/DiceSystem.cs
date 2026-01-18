using DiceSystem;

namespace DiceSystem
{
    /// <summary>
    /// 주사위 맵의 논리 구조
    /// </summary>
    public class DiceMap
    {
        public const int Width = 19;
        public const int Height = 19;

        public DiceData[,] grid = new DiceData[Width, Height];

        public DiceData Get(int x, int y)
        {
            return grid[x, y];
        }

        public void Set(int x, int y, DiceData data)
        {
            grid[x, y] = data;
        }
    }
}
