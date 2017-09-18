//SaveGameController.cs
//Created by: Wiktor Frączek
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using Arkanoid.Utils;

namespace Arkanoid.Game
{
    /// <summary>
    /// SaveGameController is responsible for saving game state to file and for loading it from file to SavedGameData.
    /// It provides callbacks for all systems which have to save data.
    /// </summary>
	public class SaveGameController 
	{
        //#region PRIVATE_FIELDS ----------------------------------------------------------------------------------------

        private GameCore _gameCore = null;
        public SavedGameData SavedGameData;

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region INIT --------------------------------------------------------------------------------------------------

        public void Init(GameCore gameCore, bool loadFromSavedState)
        {
            _gameCore = gameCore;

            if (loadFromSavedState)
            {
                LoadDataFromFile();
            }
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region PRIVATE_METHODS ---------------------------------------------------------------------------------------

        private void LoadDataFromFile()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(FilePath.SAVED_GAME_STATE, FileMode.Open);
            SavedGameData = bf.Deserialize(fs) as SavedGameData;
            fs.Close();
        }

        //#region PUBLIC_METHODS ----------------------------------------------------------------------------------------

        public void SaveGameData()
        {
            if (SavedGameData == null)
            {
                SavedGameData = new SavedGameData();
            }

            _gameCore.Dispatcher.DispatchEvent(EventNames.SAVE_DATA, this); //call all callbacks

            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(FilePath.SAVED_GAME_STATE, FileMode.Create);
            bf.Serialize(fs, SavedGameData);
            fs.Close();
        }


        public void SaveLifesDataCallback(int lifes)
        {
            SavedGameData.Lifes = lifes;
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region PUBLIC_METHODS (CALLBACKS) ----------------------------------------------------------------------------

        public void SaveBallDataCallback(BallController.BallGameState state, float speed, Vector3 position, Vector3 velocity)
        {
            SavedGameData.Ball.State = state;
            SavedGameData.Ball.Speed = speed;
            SavedGameData.Ball.PositionX = position.x;
            SavedGameData.Ball.PositionY = position.y;
            SavedGameData.Ball.MovementDirectonX = velocity.x;
            SavedGameData.Ball.MovementDirectonY = velocity.y;
        }

        public void SavePlayerDataCallback(Vector3 position)
        {
            SavedGameData.Player.PositionX = position.x;
            SavedGameData.Player.PositionY = position.y;
        }

        public void SaveScoreDataCallback(int score)
        {
            SavedGameData.Score = score;
        }

        public void SaveBlocksDataCallback(List<BlockController> blockControllersList)
        {
            if (SavedGameData.Blocks == null)
            {
                SavedGameData.Blocks = new List<BlockData>();
            }
            SavedGameData.Blocks.Clear(); //ensurance

            foreach (BlockController block in blockControllersList)
            {
                BlockData data = new BlockData(block.Type, block.transform.position);
                SavedGameData.Blocks.Add(data);
            }
        }

        public void SaveTimerDataCallback(float currentTimeScale, float bulletTimeTimer, bool isBulletTimeEnabled)
        {
            SavedGameData.Time.CurrentTimeScale = currentTimeScale;
            SavedGameData.Time.BulletTimeTimer = bulletTimeTimer;
            SavedGameData.Time.IsBulletTimeEnabled = isBulletTimeEnabled;
        }

        //#endregion ----------------------------------------------------------------------------------------------------
    }
}

