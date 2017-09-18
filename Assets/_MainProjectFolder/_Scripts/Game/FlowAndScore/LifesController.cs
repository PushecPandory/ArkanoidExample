//LifesController.cs
//Created by: Wiktor Frączek
using UnityEngine;
using Arkanoid.Utils;

namespace Arkanoid.Game
{
    /// <summary>
    /// LifesController controls amount of lifes by handling events: ADD_LIFE, LOSE_ROUND. 
    /// It also handles PREPARE_NEW_GAME and PREPARE_LOADED_GAME to prepare proper amount of lifes on the beginning of the game, and SAVE_DATA event.
    /// </summary>
	public class LifesController : MonoBehaviour, IDataToSave
    {
        private GameCore _gameCore = null;
        private int _lifes = 3;

        //#region INIT_AND_EXIT -----------------------------------------------------------------------------------------

        public void Init(GameCore gameCore)
        {
            _gameCore = gameCore;

            _gameCore.Dispatcher.AddHandler(EventNames.ADD_LIFE, OnAddLife);
            _gameCore.Dispatcher.AddHandler(EventNames.LOSE_ROUND, OnLoseRound);
            _gameCore.Dispatcher.AddHandler(EventNames.SAVE_DATA, OnSaveData);
            _gameCore.Dispatcher.AddHandler(EventNames.PREPARE_NEW_GAME, OnPrepareNewGame);
            _gameCore.Dispatcher.AddHandler(EventNames.PREPARE_LOADED_GAME, OnPrepareLoadedGame);
        }

        public void Exit()
        {
            _gameCore.Dispatcher.RemoveHandler(EventNames.ADD_LIFE, OnAddLife);
            _gameCore.Dispatcher.RemoveHandler(EventNames.LOSE_ROUND, OnLoseRound);
            _gameCore.Dispatcher.RemoveHandler(EventNames.SAVE_DATA, OnSaveData);
            _gameCore.Dispatcher.RemoveHandler(EventNames.PREPARE_NEW_GAME, OnPrepareNewGame);
            _gameCore.Dispatcher.RemoveHandler(EventNames.PREPARE_LOADED_GAME, OnPrepareLoadedGame);
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region PUBLIC_METHODS (EVENTS) -------------------------------------------------------------------------------

        public void OnAddLife(object life)
        {
            _lifes += (int)life;
            _gameCore.HUDController.UpdateLifesText(_lifes); //to dispatcher
        }

        public void OnLoseRound(object obj)
        {
            _lifes -= 1;
            _gameCore.HUDController.UpdateLifesText(_lifes); //to dispatcher
            if (_lifes <= 0)
            {
                _gameCore.Dispatcher.DispatchEvent(EventNames.GAME_OVER);
            }
        }

        public void OnPrepareNewGame(object obj)
        {
            _lifes = _gameCore.GameDataManager.DesignData.Lifes;
            _gameCore.HUDController.UpdateLifesText(_lifes); //to dispatcher
        }

        public void OnPrepareLoadedGame(object obj)
        {
            _lifes = _gameCore.SaveGameController.SavedGameData.Lifes;
            _gameCore.HUDController.UpdateLifesText(_lifes); //to dispatcher
        }

        public void OnSaveData(object saveGameController)
        {
            SaveGameController controller = (SaveGameController)saveGameController;
            controller.SaveLifesDataCallback(_lifes);
        }

        //#endregion ----------------------------------------------------------------------------------------------------
    }
}
