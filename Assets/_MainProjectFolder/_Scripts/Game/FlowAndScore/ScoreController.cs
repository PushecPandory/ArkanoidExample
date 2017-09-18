//ScoreController.cs
//Created by: Wiktor Frączek
using UnityEngine;
using Arkanoid.Utils;

namespace Arkanoid.Game
{
    /// <summary>
    /// ScoreController controls amount of score by handling events: ADD_SCORE.
    /// It also handles PREPARE_NEW_GAME and PREPARE_LOADED_GAME to prepare proper amount of score on the beginning of the game, and SAVE_DATA event.
    /// </summary>
    public class ScoreController : MonoBehaviour, IDataToSave
    {
        private GameCore _gameCore = null;
        private int _score = 0;

        public int Score { get { return _score; } }

        //#region INIT_AND_EXIT -----------------------------------------------------------------------------------------

        public void Init(GameCore gameCore)
        {
            _gameCore = gameCore;

            _gameCore.Dispatcher.AddHandler(EventNames.ADD_SCORE, OnAddScore);
            _gameCore.Dispatcher.AddHandler(EventNames.SAVE_DATA, OnSaveData);
            _gameCore.Dispatcher.AddHandler(EventNames.PREPARE_NEW_GAME, OnPrepareNewGame);
            _gameCore.Dispatcher.AddHandler(EventNames.PREPARE_LOADED_GAME, OnPrepareLoadedGame);
        }

        public void Exit()
        {
            _gameCore.Dispatcher.RemoveHandler(EventNames.ADD_SCORE, OnAddScore);
            _gameCore.Dispatcher.RemoveHandler(EventNames.SAVE_DATA, OnSaveData);
            _gameCore.Dispatcher.RemoveHandler(EventNames.PREPARE_NEW_GAME, OnPrepareNewGame);
            _gameCore.Dispatcher.RemoveHandler(EventNames.PREPARE_LOADED_GAME, OnPrepareLoadedGame);
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region PUBLIC_METHODS (EVENTS) -------------------------------------------------------------------------------

        public void OnAddScore(object score)
        {
            _score += (int)score;
            _gameCore.HUDController.UpdateScoreText(_score);
        }

        public void OnPrepareNewGame(object obj)
        {
            _score = 0;
            _gameCore.HUDController.UpdateScoreText(_score);
        }

        public void OnPrepareLoadedGame(object obj)
        {
            _score = _gameCore.SaveGameController.SavedGameData.Score;
            _gameCore.HUDController.UpdateScoreText(_score);
        }

        public void OnSaveData(object saveGameController)
        {
            SaveGameController controller = (SaveGameController)saveGameController;
            controller.SaveScoreDataCallback(_score);
        }

        //#endregion ----------------------------------------------------------------------------------------------------
    }
}
