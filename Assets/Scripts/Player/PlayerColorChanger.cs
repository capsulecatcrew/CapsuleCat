using UnityEngine;

public class PlayerColorChanger : MonoBehaviour
{
    public Color p1Color, p2Color;
    public void SetPlayerColors()
    {
        PlayerColors.SetPlayerColor(1, p1Color);
        PlayerColors.SetPlayerColor(2, p2Color);
    }
}
