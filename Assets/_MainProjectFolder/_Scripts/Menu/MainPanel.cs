//MainPanel.cs
//Created by: Wiktor Frączek
using UnityEngine;
using UnityEngine.UI;
using Arkanoid.Utils;
using UnityEngine.Assertions;

namespace Arkanoid.Menu
{
    /// <summary>
    /// MainPanel  is responsible for API for buttons in MainPanel in menu scene and disabling interaction on Continue Game Button when there is no saved game.
    /// </summary>
    public class MainPanel : MonoBehaviour 
	{
        //#region PRIVATE_FIELDS ----------------------------------------------------------------------------------------
        private MenuCore _menuCore = null;

        [SerializeField]
        private Button _buttonSelectedOnShow = null;
        [SerializeField]
        private Button _continueButton = null;

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region VALIDATION --------------------------------------------------------------------------------------------

        protected void OnValidate()
        {
            Assert.IsNotNull<Button>(_buttonSelectedOnShow, ErrorMessage.NoComponentAttached<Button>(typeof(MainPanel).Name));
            Assert.IsNotNull<Button>(_continueButton, ErrorMessage.NoComponentAttached<Button>(typeof(MainPanel).Name));
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region INIT --------------------------------------------------------------------------------------------------

        public void Init(MenuCore menuCore)
        {
            _menuCore = menuCore;

            if (PlayerPrefs.GetInt(PlayerPrefsNames.SAVED_GAME_EXISTS) == 0) //0 == false, PlayerPrefs has no API for booleans, so used int instead;
            {
                _continueButton.interactable = false;
            }
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region PUBLIC_METHODS ----------------------------------------------------------------------------------------

        public void OnNewGameButton()
        {
            PlayerPrefs.SetInt(PlayerPrefsNames.SAVED_GAME_EXISTS, 0); //0 == false, PlayerPrefs has no API for booleans, so used int instead;
            PlayerPrefs.SetInt(PlayerPrefsNames.LOAD_GAME_FROM_SAVED_STATE, 0); //0 == false, PlayerPrefs has no API for booleans, so used int instead;
            _menuCore.SwitchToGameScene();
        }

        public void OnContinueGameButton()
        {
            PlayerPrefs.SetInt(PlayerPrefsNames.LOAD_GAME_FROM_SAVED_STATE, 1); //1 == true, PlayerPrefs has no API for booleans, so used int instead;
            _menuCore.SwitchToGameScene();
        }

        public void OnShowHighScoreButton()
        {
            this.HidePanel();
            _menuCore.HighScorePanel.ShowPanel();            
        }

        public void OnQuitButton()
        {
            _menuCore.QuitApplication();
        }

        public void ShowPanel()
        {
            this.gameObject.SetActive(true);

            _menuCore.MenuEventSystem.SetSelectedGameObject(null);
            _buttonSelectedOnShow.Select();
        }

        public void HidePanel()
        {
            this.gameObject.SetActive(false);
        }

        //#endregion ----------------------------------------------------------------------------------------------------
    }
}

