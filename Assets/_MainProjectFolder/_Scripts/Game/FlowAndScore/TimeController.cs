//TimeController.cs
//Created by: Wiktor Frączek
using UnityEngine;
using Arkanoid.Utils;

namespace Arkanoid.Game
{
    /// <summary>
    /// TimeController provides API for pausing/unpausing game and for bullet time (for BulletTime blocks).
    /// It handels events: PAUSE_GAME, UNPAUSE_GAME, PAUSE_GAME_AFTER_LOAD, UNPAUSE_GAME_AFTER_LOADBULLET_TIME, PREPARE_NEW_GAME, PREPARE_LOADED_GAME, SAVE_DATA.
    /// </summary>
	public class TimeController : MonoBehaviour, IDataToSave
	{
        //#region FIELDS_AND_ACCESSORS -----------------------------------------------------------------------------------

        private GameCore _gameCore = null;
        private bool _isPaused = false;
        private bool _isPauseBecauseOfLoadedGame = false;
        private float _currentTimeScale = 1f;
        private float _bulletTimeScale = 0.5f;
        private float _bulletTimeDuration = 2f; //(in secondcs)
        private float _bulletTimeTimer = 0f;
        private bool _isBulletTimeEnabled = false;

        public bool IsPaused
        {
            get { return _isPaused; }
        }

        public bool IsPauseBecauseOfLoadedGame
        {
            get { return _isPauseBecauseOfLoadedGame; }
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region INIT_AND_EXIT -----------------------------------------------------------------------------------------

        public void Init(GameCore gameCore)
        {
            _gameCore = gameCore;
            _gameCore.Dispatcher.AddHandler(EventNames.PAUSE_GAME, PauseGame);
            _gameCore.Dispatcher.AddHandler(EventNames.UNPAUSE_GAME, UnpauseGame);
            _gameCore.Dispatcher.AddHandler(EventNames.PAUSE_GAME_AFTER_LOAD, PauseBecauseOfLoadedGame);
            _gameCore.Dispatcher.AddHandler(EventNames.UNPAUSE_GAME_AFTER_LOAD, UnpauseBecauseOfLoadedGame);
            _gameCore.Dispatcher.AddHandler(EventNames.BULLET_TIME, OnBulletTime);
            _gameCore.Dispatcher.AddHandler(EventNames.SAVE_DATA, OnSaveData);
            _gameCore.Dispatcher.AddHandler(EventNames.PREPARE_NEW_GAME, OnPrepareNewGame);
            _gameCore.Dispatcher.AddHandler(EventNames.PREPARE_LOADED_GAME, OnPrepareLoadedGame);
        }

        public void Exit()
        {
            _gameCore.Dispatcher.RemoveHandler(EventNames.BULLET_TIME, OnBulletTime);
            _gameCore.Dispatcher.RemoveHandler(EventNames.SAVE_DATA, OnSaveData);
            _gameCore.Dispatcher.RemoveHandler(EventNames.PREPARE_NEW_GAME, OnPrepareNewGame);
            _gameCore.Dispatcher.RemoveHandler(EventNames.PREPARE_LOADED_GAME, OnPrepareLoadedGame);
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region UPDATE ------------------------------------------------------------------------------------------------

        protected void Update()
        {
            if (_isBulletTimeEnabled)
            {
                _bulletTimeTimer += Time.deltaTime;
                if (_bulletTimeTimer >= _bulletTimeDuration)
                {
                    DisableBulletTime();
                }
            }
        }

        private void DisableBulletTime()
        {
            _currentTimeScale = 1f; // 1 is a normal timeScale
            Time.timeScale = _currentTimeScale;
            _isBulletTimeEnabled = false;
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region PUBLIC_METHODS (EVENTS) -------------------------------------------------------------------------------

        public void PauseGame(object obj)
        {
            _isPaused = true;
            _currentTimeScale = Time.timeScale;
            Time.timeScale = 0f;
        }

        public void UnpauseGame(object obj)
        {
            _isPaused = false;
            Time.timeScale = _currentTimeScale;
        }

        public void PauseBecauseOfLoadedGame(object obj)
        {
            _isPaused = true;
            _isPauseBecauseOfLoadedGame = true;
            Time.timeScale = 0f;
        }

        public void UnpauseBecauseOfLoadedGame(object obj)
        {
            _isPaused = false;
            _isPauseBecauseOfLoadedGame = false;
            Time.timeScale = _currentTimeScale;
        }

        public void OnSaveData(object saveGameController)
        {
            SaveGameController controller = (SaveGameController)saveGameController;
            controller.SaveTimerDataCallback(_currentTimeScale, _bulletTimeTimer, _isBulletTimeEnabled);
        }

        public void OnPrepareNewGame(object obj)
        {
            _bulletTimeScale = _gameCore.GameDataManager.DesignData.BulletTimeScale;
            _bulletTimeDuration = _gameCore.GameDataManager.DesignData.BulletTimeDuration * _bulletTimeScale;
        }

        public void OnPrepareLoadedGame(object obj)
        {
            SavedGameData data = _gameCore.SaveGameController.SavedGameData;

            _isBulletTimeEnabled = data.Time.IsBulletTimeEnabled;
            _currentTimeScale = data.Time.CurrentTimeScale;
            _bulletTimeTimer = data.Time.BulletTimeTimer;
            _bulletTimeScale = _gameCore.GameDataManager.DesignData.BulletTimeScale;
            _bulletTimeDuration = _gameCore.GameDataManager.DesignData.BulletTimeDuration * _bulletTimeScale;
        }

        public void OnBulletTime(object obj)
        {
            _currentTimeScale = _bulletTimeScale;
            Time.timeScale = _currentTimeScale;
            _bulletTimeTimer = 0f;
            _isBulletTimeEnabled = true;
        }

        //#endregion ----------------------------------------------------------------------------------------------------
    }
}

