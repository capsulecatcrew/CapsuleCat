using System.Collections.Generic;

namespace Player.Special
{
    public static class SpecialMoveEnumController
    {
        private static readonly List<SpecialMoveEnum> AllSpecialMoves = new ()
        {
            SpecialMoveEnum.MoveHeal,
            SpecialMoveEnum.MoveShield,
            SpecialMoveEnum.ShootVampire
        };

        public static List<SpecialMoveEnum> CopyAllSpecialMoves()
        {
            return new List<SpecialMoveEnum>(AllSpecialMoves);
        }
    }
    public enum SpecialMoveEnum
    {
        MoveHeal,
        MoveShield,
        ShootVampire,
    }
}