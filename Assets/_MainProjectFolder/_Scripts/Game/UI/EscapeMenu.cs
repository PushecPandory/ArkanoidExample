//EscapeMenu.cs
//Created by: Wiktor Frączek
using UnityEngine;
using UnityEngine.UI;
using Arkanoid.Utils;
using UnityEngine.Assertions;

namespace Arkanoid.Game
{
    /// <summary>
    /// EscapeMenu controls pause menu which is called by pressing ESC. When escape menu is enabled timeScale = 0f. Provides API for summary escape menu buttons.
    /// </summary>
	public class EscapeMenu : MonoBehaviour 
	{
        //#region PRIVATE_FIELDS ----------------------------------------------------------------------------------------

        private bool _isEnable = false;
        private GameCore _gameCore = null;
        private bool _couldBeEnabled = true;

        [SerializeField]
        private GameObject _canvas = null;
        [SerializeField]
        private Button _saveAndQuitButton = null;
        [SerializeField]
        private Button _buttonSelectedOnShow = null;

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region VALIDATION --------------------------------------------------------------------------------------------

        protected void OnValidate()
        {
            Assert.IsNotNull<GameObject>(_canvas, ErrorMessage.NoComponentAttached<GameObject>(typeof(EscapeMenu).Name));
            Assert.IsNotNull<Button>(_saveAndQuitButton, ErrorMessage.NoComponentAttached<Button>(typeof(EscapeMenu).Name));
            Assert.IsNotNull<Button>(_buttonSelectedOnShow, ErrorMessage.NoComponentAttached<Button>(typeof(EscapeMenu).Name));
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region INIT_AND_EXIT -----------------------------------------------------------------------------------------

        public void Init(GameCore gameCore)
        {
            _gameCore = gameCore;
            _gameCore.Dispatcher.AddHandler(EventNames.DISABLE_ESCAPE_MENU, OnDisableEscapeMenu);
            _gameCore.Dispatcher.AddHandler(EventNames.GAME_OVER, OnGameOver);
            Hide();         
        }

        public void Exit()
        {
            _gameCore.Dispatcher.RemoveHandler(EventNames.DISABLE_ESCAPE_MENU, OnDisableEscapeMenu);
            _gameCore.Dispatcher.RemoveHandler(EventNames.GAME_OVER, OnGameOver);
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region UPDATE ------------------------------------------------------------------------------------------------

        protected void Update()
        {
            if (_couldBeEnabled
                && _gameCore.Input.Esc.State == GameInputButton.ButtonState.JustPressed)
            {
                if (_isEnable)
                {
                    Hide();
                }
                else
                {
                    Show();
                }
            }
        }

        //#endregion -----------------------------------------------------------------------------------------------------

        //#region PRIVATE_METHODS ----------------------------------------------------------------------------------------

        private void SaveGameState()
        {
            _gameCore.SaveGameController.SaveGameData();
        }

        private void Show()
        {            
            _isEnable = true;
            _canvas.SetActive(true);
            _gameCore.Dispatcher.DispatchEvent(EventNames.PAUSE_GAME);
            
            _gameCore.GameEventSystem.SetSelectedGameObject(null);
            _buttonSelectedOnShow.Select();
        }

        private void Hide()
        {
            _isEnable = false;
            _canvas.SetActive(false);
            _gameCore.Dispatcher.DispatchEvent(EventNames.UNPAUSE_GAME);
        }

        //#region PUBLIC_METHODS ----------------------------------------------------------------------------------------

        private void OnDisableEscapeMenu(object obj)
        {
            _couldBeEnabled = false;
        }

        private void OnGameOver(object obj)
        {
            _saveAndQuitButton.interactable = false;
        }

        public void OnResumeButton()
        {
            Hide();
        }

        public void OnSaveAndQuitButton()
        {
            SaveGameState();
            _gameCore.SetSavedGameExists(true);
            _gameCore.SwitchToMenuScene();
        }

        public void OnQuitButton()
        {
            _gameCore.SetSavedGameExists(false);
            _gameCore.SwitchToMenuScene();
        }

        //#endregion ----------------------------------------------------------------------------------------------------
    }
}

