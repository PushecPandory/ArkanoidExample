//FilePath.cs
//Created by: Wiktor Frączek
using UnityEngine;

namespace Arkanoid.Utils
{
	public static class FilePath 
	{
        public static readonly string HIGH_SCORES =  Application.persistentDataPath + "/HighScores.dat";
        public static readonly string SAVED_GAME_STATE = Application.persistentDataPath + "/SavedGameState.dat";
    }	
}

