//IDataToSave.cs
//Created by: Wiktor Frączek

namespace Arkanoid.Game
{
    /// <summary>
    /// All scripts which have to save data to SaveGameController should have this interface.
    /// </summary>
    public interface IDataToSave 
	{
        void OnSaveData(object saveGameController);
        void OnPrepareNewGame(object obj);
        void OnPrepareLoadedGame(object obj);
    }	
} 