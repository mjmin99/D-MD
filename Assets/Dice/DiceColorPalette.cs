using UnityEngine;

[CreateAssetMenu(menuName = "Dice/Color Palette")]
public class DiceColorPalette : ScriptableObject
{
    public Color white = new Color(0.92f, 0.92f, 0.92f);
    public Color red = new Color(0.78f, 0.28f, 0.28f);
    public Color yellow = new Color(0.80f, 0.68f, 0.22f);
    public Color green = new Color(0.30f, 0.70f, 0.40f);
    public Color blue = new Color(0.28f, 0.45f, 0.78f);
    public Color purple = new Color(0.58f, 0.38f, 0.72f);

    public Color Get(DiceColor color)
    {
        return color switch
        {
            DiceColor.White => white,
            DiceColor.Red => red,
            DiceColor.Yellow => yellow,
            DiceColor.Green => green,
            DiceColor.Blue => blue,
            DiceColor.Purple => purple,
            _ => Color.gray
        };
    }
}
