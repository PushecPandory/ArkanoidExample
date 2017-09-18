//GameCore.cs
//Created by: Wiktor Frączek
using UnityEngine;
using UnityEngine.Assertions;
using System.IO;
using UnityEngine.EventSystems;
using Arkanoid.Utils;
using Arkanoid.Main;

namespace Arkanoid.Game
{
    /// <summary>
    /// GameCore is singleton with access via static property GameCore.Insctance.
    /// It is responsible for initialize and exit other scripts in proper order. As a singleton offers also access to controllers and managers of them via accessors.
    /// Provides also API for switching scene and setting SAVED_GAME_EXISTS.
    /// </summary>
    [RequireComponent(typeof(GameInput))]
    [RequireComponent(typeof(TimeController))]
    [RequireComponent(typeof(ScoreController))]
    [RequireComponent(typeof(LifesController))]
    [RequireComponent(typeof(GameFlowController))]
    public class GameCore : CoreSingleton<GameCore> 
	{
        //#region PRIVATE_FIELDS ----------------------------------------------------------------------------------------

        private Dispatcher _dispatcher = null;
        private GameInput _gameInput = null;
        private TimeController _timeController = null;       
        private ScoreController _scoreController = null;
        private LifesController _lifesController = null;        
        private GameFlowController _gameFlowController = null;
        private SaveGameController _saveGameController = null;

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region SERIALIZE_FIELDS --------------------------------------------------------------------------------------

        [SerializeField]
        private EventSystem _eventSystem = null;
        [SerializeField]
        private PlayerController _playerController = null;
        [SerializeField]
        private BallController _ballController = null;
        [SerializeField]
        private HUDController _HUDController = null;
        [SerializeField]
        private EscapeMenu _escapeMenu = null;
        [SerializeField]
        private SummaryScreen _summaryScreen = null;
        [SerializeField]
        private BlocksManager _blocksManager = null;
        [SerializeField]
        private GameDataManager _gameDataManager = null;
        [SerializeField]
        private GameCamera _gameCamera = null;

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region ACCESSORS ---------------------------------------------------------------------------------------------

        public EventSystem GameEventSystem { get { return _eventSystem; } }
        public Dispatcher Dispatcher { get { return _dispatcher; } }
        public TimeController TimeController { get { return _timeController; } }
        public GameInput Input { get { return _gameInput; } }
        public PlayerController Player { get { return _playerController; } }
        public BallController Ball { get { return _ballController; } }        
        public HUDController HUDController { get { return _HUDController; } }
        public BlocksManager BlocksManager { get { return _blocksManager; } }
        public GameDataManager GameDataManager { get { return _gameDataManager; } }
        public ScoreController ScoreController { get { return _scoreController; } }
        public SaveGameController SaveGameController { get { return _saveGameController; } }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region VALIDATION --------------------------------------------------------------------------------------------

        protected void OnValidate()
        {
            Assert.IsNotNull<EventSystem>(_eventSystem, ErrorMessage.NoComponentAttached<EventSystem>(typeof(GameCore).Name));
            Assert.IsNotNull<PlayerController>(_playerController, ErrorMessage.NoComponentAttached<PlayerController>(typeof(GameCore).Name));
            Assert.IsNotNull<BallController>(_ballController, ErrorMessage.NoComponentAttached<BallController>(typeof(GameCore).Name));
            Assert.IsNotNull<HUDController>(_HUDController, ErrorMessage.NoComponentAttached<HUDController>(typeof(GameCore).Name));
            Assert.IsNotNull<EscapeMenu>(_escapeMenu, ErrorMessage.NoComponentAttached<EscapeMenu>(typeof(GameCore).Name));
            Assert.IsNotNull<SummaryScreen>(_summaryScreen, ErrorMessage.NoComponentAttached<SummaryScreen>(typeof(GameCore).Name));
            Assert.IsNotNull<BlocksManager>(_blocksManager, ErrorMessage.NoComponentAttached<BlocksManager>(typeof(GameCore).Name));
            Assert.IsNotNull<GameDataManager>(_gameDataManager, ErrorMessage.NoComponentAttached<GameDataManager>(typeof(GameCore).Name));
            Assert.IsNotNull<GameCamera>(_gameCamera, ErrorMessage.NoComponentAttached<GameCamera>(typeof(GameCore).Name));
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region INIT_AND_EXIT -----------------------------------------------------------------------------------------

        public override void Init()
        {
            InstantiateMainCoreIfItIsNull();
            GetComponents();
            InitializeScripts();
        }

        public override void Exit()
        {
            _timeController.Exit();
            _playerController.Exit();
            _ballController.Exit();
            _escapeMenu.Exit();
            _summaryScreen.Exit();
            _blocksManager.Exit();
            _scoreController.Exit();
            _lifesController.Exit();
            _gameFlowController.Exit();
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region PRIVATE_METHODS ---------------------------------------------------------------------------------------

        private void InstantiateMainCoreIfItIsNull()
        {
            if (MainCore.Instance == null)
            {
                GameObject go = Resources.Load<GameObject>(PathToMainCorePrefab);

                #if UNITY_EDITOR
                    if (go == null)
                    {
                        Debug.LogError(ErrorMessage.NoMainCore);
                    }
                #endif

                Instantiate<GameObject>(go);
            }
        }

        private void GetComponents()
        {
            _dispatcher = new Dispatcher();
            _saveGameController = new SaveGameController();
            _timeController = GetComponent<TimeController>();
            _gameInput = GetComponent<GameInput>();
            _scoreController = GetComponent<ScoreController>();
            _lifesController = GetComponent<LifesController>();
            _gameFlowController = GetComponent<GameFlowController>();
        }

        private void InitializeScripts()
        {
            bool loadFromSavedState =
                (PlayerPrefs.GetInt(PlayerPrefsNames.SAVED_GAME_EXISTS) != 0)
                && (File.Exists(FilePath.SAVED_GAME_STATE));

            _saveGameController.Init(this, loadFromSavedState);
            _gameInput.Init();
            _timeController.Init(this);
            _playerController.Init(this);
            _ballController.Init(this);
            _escapeMenu.Init(this);
            _summaryScreen.Init(this);
            _blocksManager.Init(this);
            _scoreController.Init(this);
            _lifesController.Init(this);
            _gameCamera.Init();
            _gameFlowController.Init(this, loadFromSavedState);
        }       

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region PUBLIC_METHODS ----------------------------------------------------------------------------------------

        public void SwitchToMenuScene()
        {
            MainCore.Instance.SwitchToMenuScene();
        }

        public void SwitchToGameScene()
        {
            MainCore.Instance.SwitchToGameScene();
        }

        public void SetSavedGameExists(bool exists)
        {
            if (exists)
            {
                PlayerPrefs.SetInt(PlayerPrefsNames.SAVED_GAME_EXISTS, 1); //1 == true, PlayerPrefs has no API for booleans, so used int instead;
            }
            else
            {
                PlayerPrefs.SetInt(PlayerPrefsNames.SAVED_GAME_EXISTS, 0); //0 == false, PlayerPrefs has no API for booleans, so used int instead;
            }
        }

        //#endregion ----------------------------------------------------------------------------------------------------
    }
}