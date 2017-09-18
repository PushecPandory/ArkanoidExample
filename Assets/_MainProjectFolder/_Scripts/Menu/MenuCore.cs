//MenuCore.cs
//Created by: Wiktor Frączek
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;
using Arkanoid.Utils;
using Arkanoid.Main;

namespace Arkanoid.Menu
{
    /// <summary>
    /// MainCore is singleton with access via static property MainCore.Insctance.
    /// It provides API for switching scenes, quit application and HighScoreController.
    /// It also disables mouse cursor on Init().
    /// </summary>
	public class MenuCore : CoreSingleton<MenuCore>
    {
        //#region FIELDS_AND_ACCESSORS ----------------------------------------------------------------------------------

        [SerializeField]
        private MainPanel _mainPanel = null;
        [SerializeField]
        private HighScorePanel _highScorePanel = null;
        [SerializeField]
        private EventSystem _eventSystem = null;

        public EventSystem MenuEventSystem { get { return _eventSystem; } }
        public MainPanel MainPanel { get { return _mainPanel; } }
        public HighScorePanel HighScorePanel { get { return _highScorePanel; } }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region VALIDATION --------------------------------------------------------------------------------------------

        protected void OnValidate()
        {
            Assert.IsNotNull<MainPanel>(_mainPanel, ErrorMessage.NoComponentAttached<MainPanel>(typeof(MenuCore).Name));
            Assert.IsNotNull<HighScorePanel>(_highScorePanel, ErrorMessage.NoComponentAttached<HighScorePanel>(typeof(MenuCore).Name));
            Assert.IsNotNull<EventSystem>(_eventSystem, ErrorMessage.NoComponentAttached<EventSystem>(typeof(MenuCore).Name));
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region INIT_AND_EXIT -----------------------------------------------------------------------------------------

        public override void Init()
        {
            if (MainCore.Instance == null)
            {
                GameObject go = Resources.Load<GameObject>(PathToMainCorePrefab);
                if (go == null)
                {
                    Debug.LogError(ErrorMessage.NoMainCore);
                }
                Instantiate<GameObject>(go);
            }

            _mainPanel.Init(this);
            _highScorePanel.Init(this);
            _mainPanel.ShowPanel();        
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region PUBLIC_METHODS ----------------------------------------------------------------------------------------

        public void SwitchToGameScene()
        {
            MainCore.Instance.SwitchToGameScene();
        }

        public void QuitApplication()
        {
            MainCore.Instance.QuitApplication();
        }

        //#endregion ----------------------------------------------------------------------------------------------------
    }
}

