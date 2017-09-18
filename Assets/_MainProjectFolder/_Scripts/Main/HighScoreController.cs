//MenuCore.cs
//Created by: Wiktor Frączek
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using Arkanoid.Utils;

namespace Arkanoid.Main
{
    /// <summary>
    /// HighScoreController is responsible for providing API for checking is the final score a high score and loading/saving high scores.
    /// This file contains also [Serializable] class HighScoreData to store required data in file.
    /// </summary>
    public class HighScoreController : MonoBehaviour 
	{
        private List<int> _highScoresList = null;
        private readonly int HIGH_SCORE_COLLECTION_LENGHT = 3;

        public List<int> List { get { return _highScoresList; } }

        //#region INIT_AND_EXIT -----------------------------------------------------------------------------------------

        public void Init()
        {
            if (File.Exists(FilePath.HIGH_SCORES))
            {
                LoadDataFromFile();
            }
            else
            {
                InitHighScoresCollection();
            }
        }

        private void LoadDataFromFile()
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(FilePath.HIGH_SCORES, FileMode.Open);
            HighScoreData data = bf.Deserialize(fs) as HighScoreData;
            _highScoresList = data.HighScoresList;
            fs.Close();
        }

        private void InitHighScoresCollection()
        {
            _highScoresList = new List<int>();
            for (int i = 0; i < HIGH_SCORE_COLLECTION_LENGHT; ++i)
            {
                _highScoresList.Add(0);
            }
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region PUBLIC_METHODS ----------------------------------------------------------------------------------------

        public bool IsItNewRecordOnHighScoreList(int finalGameScore)
        {
            int index = 0;

            foreach (int score in _highScoresList)
            {
                if (finalGameScore > score)
                {
                    SaveScoreToHighScores(finalGameScore, index);
                    return true;
                }
                index += 1;
            }

            return false;
        }

        public void SaveScoreToHighScores(int finalGameScore, int index)
        {
            _highScoresList.Insert(index, finalGameScore);
            _highScoresList.RemoveAt(HIGH_SCORE_COLLECTION_LENGHT);

            BinaryFormatter bf = new BinaryFormatter();
            FileStream fs = new FileStream(FilePath.HIGH_SCORES, FileMode.Create);
            HighScoreData data = new HighScoreData();
            data.HighScoresList = _highScoresList;
            bf.Serialize(fs, data);
            fs.Close();
        }

        //#endregion ----------------------------------------------------------------------------------------------------
    }

    [Serializable]
    public class HighScoreData
    {
        public List<int> HighScoresList = null;

        public HighScoreData()
        {
            HighScoresList = new List<int>();
        }
    }	
}

