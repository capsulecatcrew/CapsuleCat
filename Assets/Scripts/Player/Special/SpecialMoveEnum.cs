using System.Collections.Generic;

namespace Player.Special
{
    public static class SpecialMoveEnumController
    {
        private static readonly List<SpecialMoveEnum> AllSpecialMoves = new ()
        {
            SpecialMoveEnum.MoveHeal,
            SpecialMoveEnum.MoveAbsorbShield,
            SpecialMoveEnum.ShootVampire,
            SpecialMoveEnum.ShootLaser
        };

        public static List<SpecialMoveEnum> CopyAllSpecialMoves()
        {
            return new List<SpecialMoveEnum>(AllSpecialMoves);
        }
    }
    public enum SpecialMoveEnum
    {
        MoveHeal,
        MoveAbsorbShield,
        ShootVampire,
        ShootLaser,
    }
}