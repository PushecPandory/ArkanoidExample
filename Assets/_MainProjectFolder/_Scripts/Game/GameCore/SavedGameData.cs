using System;
using System.Collections.Generic;
using UnityEngine;

namespace Arkanoid.Game
{
    /// <summary>
    /// SavedGameData is [Serializable] class storing data for saved game state. This file includes also structures for organize them.
    /// </summary>
    [Serializable]
    public class SavedGameData
    {
        public BallData Ball;
        public PlayerData Player;
        public List<BlockData> Blocks;
        public int Score;
        public TimeData Time;
        public int Lifes;
    }

    [Serializable]
    public struct BallData
    {
        public BallController.BallGameState State;
        public float Speed;
        public float PositionX;
        public float PositionY;
        public float MovementDirectonX;
        public float MovementDirectonY;
    }

    [Serializable]
    public struct PlayerData
    {
        public float PositionX;
        public float PositionY;
    }

    [Serializable]
    public struct BlockData
    {
        public BlockData(BlockController.BlockType blockType, Vector3 position)
        {
            BlockType = blockType;
            PositionX = position.x;
            PositionY = position.y;
        }

        public BlockController.BlockType BlockType;
        public float PositionX;
        public float PositionY;
    }

    [Serializable]
    public struct TimeData
    {
        public float CurrentTimeScale;
        public float BulletTimeTimer;
        public bool IsBulletTimeEnabled;
    }
}
