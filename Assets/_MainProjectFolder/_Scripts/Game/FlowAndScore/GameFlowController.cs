//GameFlowController.cs
//Created by: Wiktor Frączek
using UnityEngine;
using Arkanoid.Utils;

namespace Arkanoid.Game
{
    /// <summary>
    /// GameFlowController is responsible for managing game objects according to game flow. It handels events: LOSE_ROUND, WIN_ROUND, GAME_OVER; 
    /// and dispatchces PAUSE_GAME_AFTER_LOAD, UNPAUSE_GAME_AFTER_LOAD, UNPAUSE_GAME, PREPARE_NEW_GAME, PREPARE_LOADED_GAME, SHOW_SUMMARY_SCREEN, DISABLE_ESCAPE_MENU, RESET_TO_NEW_ROUND.
    /// It also controls time of showing popups (via HUDController).
    /// </summary>
    public class GameFlowController : MonoBehaviour 
	{
        //region PRIVATE_FIELDS ------------------------------------------------------------------------------------------

        private GameCore _gameCore = null;        
        private bool _isGameOver = false;
        private bool _couldSwitchToSummaryScreen = false;

        private float _showPopUpDurationTime = 2f;
        private string _resetToGameOverMethodName = "ResetToGameOver";
        private string _resetToNewRoundMethodName = "ResetToNewRound";

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region INIT_AND_EXIT -----------------------------------------------------------------------------------------

        public void Init(GameCore gameCore, bool loadFromSavedState)
        {
            _gameCore = gameCore;
            _isGameOver = false;
            _couldSwitchToSummaryScreen = false;
            _gameCore.Dispatcher.AddHandler(EventNames.LOSE_ROUND, OnLoseRound);
            _gameCore.Dispatcher.AddHandler(EventNames.WIN_ROUND, OnWinRound);
            _gameCore.Dispatcher.AddHandler(EventNames.GAME_OVER, OnGameOver);

            if (loadFromSavedState)
            {
                _gameCore.Dispatcher.DispatchEvent(EventNames.PREPARE_LOADED_GAME);
                _gameCore.Dispatcher.DispatchEvent(EventNames.PAUSE_GAME_AFTER_LOAD);
            }
            else
            {
                _gameCore.Dispatcher.DispatchEvent(EventNames.PREPARE_NEW_GAME);
            }
        }

        public void Exit()
        {
            _gameCore.Dispatcher.RemoveHandler(EventNames.LOSE_ROUND, OnLoseRound);
            _gameCore.Dispatcher.RemoveHandler(EventNames.WIN_ROUND, OnWinRound);
            _gameCore.Dispatcher.RemoveHandler(EventNames.GAME_OVER, OnGameOver);
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region UPDATE ------------------------------------------------------------------------------------------------

        protected void Update()
        {
            if (_gameCore.TimeController.IsPauseBecauseOfLoadedGame && Input.anyKeyDown)
            {
                _gameCore.Dispatcher.DispatchEvent(EventNames.UNPAUSE_GAME_AFTER_LOAD);
            }

            if (_couldSwitchToSummaryScreen
                && Input.anyKeyDown)
            {
                _gameCore.Dispatcher.DispatchEvent(EventNames.SHOW_SUMMARY_SCREEN);
            }
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region PUBLIC_METHODS (EVENTS) -------------------------------------------------------------------------------

        public void OnWinRound(object obj)
        {
            _gameCore.HUDController.ShowWinRoundPopUp();
            Invoke(_resetToNewRoundMethodName, _showPopUpDurationTime); //Called rearly, coroutine is overkill.
        }

        public void OnLoseRound(object obj)
        {
            _gameCore.HUDController.ShowLoseRoundPopUp();
            if (_isGameOver)
            {
                Invoke(_resetToGameOverMethodName, _showPopUpDurationTime); //Called rearly, coroutine is overkill.
            }
            else
            {
                Invoke(_resetToNewRoundMethodName, _showPopUpDurationTime); //Called rearly, coroutine is overkill.
            }
        }

        public void OnGameOver(object obj)
        {
            _gameCore.SetSavedGameExists(false);
            _isGameOver = true;
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        public void ResetToNewRound()
        {
            _gameCore.HUDController.HideActivePopUp();
            _gameCore.Dispatcher.DispatchEvent(EventNames.RESET_TO_NEW_ROUND);
        }

        public void ResetToGameOver()
        {
            _gameCore.HUDController.HideActivePopUp();
            _gameCore.Dispatcher.DispatchEvent(EventNames.DISABLE_ESCAPE_MENU);
            _gameCore.HUDController.ShowGameOverRoundPopUp();
            _couldSwitchToSummaryScreen = true;
        }
    }	
}
