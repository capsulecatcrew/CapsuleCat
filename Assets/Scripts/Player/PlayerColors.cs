using UnityEngine;

public static class PlayerColors 
{
    public static Color P1Color = new Color(182f/255f, 255f/255f, 196f/255f);
    public static Color P2Color = new Color(255f/255f, 181f/255f, 192f/255f);

    public static void SetPlayerColor(int playerNo, Color color)
    {
        if (playerNo == 1)
        {
            P1Color = color;
        }
        else if (playerNo == 2)
        {
            P2Color = color;
        }

    }

    public static Color GetPlayerColor(int playerNo)
    {
        if (playerNo == 1)
        {
            return P1Color;
        }
        else
        {
            return P2Color;
        }
    }
}