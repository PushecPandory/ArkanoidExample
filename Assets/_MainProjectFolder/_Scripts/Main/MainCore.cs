//MainCore.cs
//by Wiktor Frączek

using UnityEngine;
using UnityEngine.SceneManagement;
using Arkanoid.Utils;
using UnityEngine.Assertions;

namespace Arkanoid.Main
{
    /// <summary>
    /// MainCore is singleton with access via static property MainCore.Insctance.
    /// It provides API for switching scenes, quit application and HighScoreController.
    /// It also disables mouse cursor on Init().
    /// </summary>
	public class MainCore : CoreSingleton<MainCore> 
	{
        [SerializeField]
        private HighScoreController _highScoreController = null;

        public HighScoreController HighScoreController { get { return _highScoreController; } }

        //#region VALIDATION --------------------------------------------------------------------------------------------

        protected void OnValidate()
        {
            Assert.IsNotNull<HighScoreController>(_highScoreController, ErrorMessage.NoComponentAttached<HighScoreController>(typeof(MainCore).Name));
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region INIT_AND_EXIT -----------------------------------------------------------------------------------------

        public override void Init()
        {
            DontDestroyOnLoad(this.gameObject);
            _highScoreController.Init();
            DisableMouseCursor();
        }

        private void DisableMouseCursor()
        {
            Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked; //could cause problems when it is locked
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region PUBLIC_METHODS ----------------------------------------------------------------------------------------

        public void SwitchToGameScene()
        {
            SceneManager.LoadScene(SceneNames.GAME, LoadSceneMode.Single);
        }

        public void SwitchToMenuScene()
        {
            SceneManager.LoadScene(SceneNames.MENU, LoadSceneMode.Single);
        }

        public void QuitApplication()
        {
            Application.Quit();
        }

        //#endregion ----------------------------------------------------------------------------------------------------
    }
}