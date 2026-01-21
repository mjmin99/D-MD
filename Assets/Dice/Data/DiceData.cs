
using static DiceSystem.DiceTooltipUI;

public enum DiceColor
{
    White,
    Red,
    Yellow,
    Green,
    Blue,
    Purple
}

/// <summary>
/// 주사위의 변하지 않는 정체성 데이터
/// </summary>
public class DiceData
{
    public int id;
    public DiceColor color;

    /// <summary>
    /// 오염된 면 (1~6), White 주사위는 -1
    /// </summary>
    public int contaminatedFace;

    public DiceFaceEncounterTable encounterTable;
}



