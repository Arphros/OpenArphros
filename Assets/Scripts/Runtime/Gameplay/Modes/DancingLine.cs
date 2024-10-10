using UnityEngine;

namespace Arphros.Gameplay
{
    public class DancingLine : Gamemode
    {
        [Header("Properties")]
        public Vector3 left;
        public Vector3 right = new Vector3(0, 90, 0);
        public TailType tailType = TailType.Overlap;
        public bool funkyTail;

        private int loopCount = 0;

        public override void OnInitialization()
        {
            base.OnInitialization();
        }

        public override void OnUpdate()
        {

        }

        public override bool OnKeyPressed()
        {
            return false;
        }

        public enum OverType
        {
            Win,
            DieCollide,
            DieWater,
            DieFloating,
            None
        }

        public enum TailType
        {
            Overlap,
            Separate,
            None
        }
    }
}