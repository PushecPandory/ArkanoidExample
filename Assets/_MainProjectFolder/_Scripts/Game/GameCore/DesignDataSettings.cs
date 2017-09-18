//DesignDataSettings.cs
//Created by: Wiktor Frączek
using UnityEngine;

namespace Arkanoid.Game
{
    /// <summary>
    /// DesignDataSettings is responsible for creating scriptlable object with design data. 
    /// Thanks to this solution design data are stored in one place and it is more comfortable to manipulate them.
    /// </summary>
    [CreateAssetMenu(fileName = "DesignDataSettings", menuName = "Arkanoid.Game/DesignDataSettings")]
    public class DesignDataSettings : ScriptableObject
    {
        [Header("Player settings")]

        public int Lifes = 3;
        public float PlayerSpeedMovement = 8f;

        [Header("Ball settings")]

        public float BasicBallSpeedMovement = 5f;
        public float BallSpeedMovementAccelerationPerLevel = 1f;

        [Header("Block settings")]

        public int NumberOfRows = 2;
        public int NumberOfColumns = 18;

        [Range(0, 100)]
        public float ChanceForDoubleScoreBlock = 20f;
        [Range(0, 100)]
        public float ChanceForBulletTimeBlock = 5f;
        [Range(0, 100)]
        public float ChanceForAdditionalLifeBlock = 5f;
        [Range(0, 100)]
        public float ChanceForNormalBlock = 100f;

        public int ScoreForNormalBlock = 100;
        public int ScoreForDoubleScoreBlock = 200;
        public int AddLifesCount = 1;
        public float BulletTimeScale = 0.5f;
        public float BulletTimeDuration = 3f;
    }
}

