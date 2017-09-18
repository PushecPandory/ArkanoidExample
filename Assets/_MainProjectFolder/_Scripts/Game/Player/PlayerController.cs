//PlayerController.cs
//Created by: Wiktor Frączek
using UnityEngine;
using UnityEngine.Assertions;
using System;
using Arkanoid.Utils;

namespace Arkanoid.Game
{
    /// <summary>
    /// PlayerController class is responsible for definig player behaviour, changing state of player (and thus current behaviour) and sending data to save to SaveGameController on proper event.
    /// PlayerGameState changes when one of events is recived: LOSE_ROUND, WIN_ROUND, RESET_TO_NEW_ROUND, PREPARE_NEW_GAME, PREPARE_LOADED_GAME.
    /// In class there is an Action "_gameStateBehaviour" which is called in FixedUpdate(). Depending on state it has proper method assigned (assigning is in PlayerGameState State accessor).
    /// In GameStateRunningBehaviour() it checks proper input to decide what to do.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
	public class PlayerController : MonoBehaviour, IResetable, IDataToSave
    {
        public enum PlayerGameState
        {
            Reset,
            Running
        }

        //region PRIVATE_FIELDS -----------------------------------------------------------------------------------------

        private GameCore _gameCore = null;
        private Vector2 _leftMovementSpeedVector;
        private Vector2 _rightMovementSpeedVector;
        private float _movementSpeed = 3f;
        private Action _gameStateBehaviour = null;

        [SerializeField]
        private Transform _resetPosition = null;
        [SerializeField]
        private Rigidbody2D _rigidbody = null;

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region ACCESSORS ---------------------------------------------------------------------------------------------

        private PlayerGameState State
        {
            set
            {
                if (value == PlayerGameState.Reset)
                {
                    _gameStateBehaviour = GameStateResetBehaviour;
                }
                else if (value == PlayerGameState.Running)
                {
                    _gameStateBehaviour = GameStateRunningBehaviour;
                }
            }
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region VALIDAION ---------------------------------------------------------------------------------------------

        protected void OnValidate()
        {
            Assert.IsNotNull<Transform>(_resetPosition, ErrorMessage.NoComponentAttached<Transform>(typeof(PlayerController).Name));
            Assert.IsNotNull<Rigidbody2D>(_rigidbody, ErrorMessage.NoComponentAttached<Rigidbody2D>(typeof(PlayerController).Name));
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region INIT_AND_EXIT -----------------------------------------------------------------------------------------

        public void Init(GameCore gameCore)
        {
            _gameCore = gameCore;
            
            _gameCore.Dispatcher.AddHandler(EventNames.WIN_ROUND, OnEndRound);
            _gameCore.Dispatcher.AddHandler(EventNames.LOSE_ROUND, OnEndRound);
            _gameCore.Dispatcher.AddHandler(EventNames.RESET_TO_NEW_ROUND, OnResetToNewRound);
            _gameCore.Dispatcher.AddHandler(EventNames.SAVE_DATA, OnSaveData);
            _gameCore.Dispatcher.AddHandler(EventNames.PREPARE_NEW_GAME, OnPrepareNewGame);
            _gameCore.Dispatcher.AddHandler(EventNames.PREPARE_LOADED_GAME, OnPrepareLoadedGame);

            _rigidbody = GetComponent<Rigidbody2D>();
            _movementSpeed = _gameCore.GameDataManager.DesignData.PlayerSpeedMovement;
            _leftMovementSpeedVector = new Vector2(-_movementSpeed, 0f);
            _rightMovementSpeedVector = new Vector2(_movementSpeed, 0f);
        }

        public void Exit()
        {
            _gameCore.Dispatcher.RemoveHandler(EventNames.WIN_ROUND, OnEndRound);
            _gameCore.Dispatcher.RemoveHandler(EventNames.LOSE_ROUND, OnEndRound);
            _gameCore.Dispatcher.RemoveHandler(EventNames.RESET_TO_NEW_ROUND, OnResetToNewRound);
            _gameCore.Dispatcher.RemoveHandler(EventNames.SAVE_DATA, OnSaveData);
            _gameCore.Dispatcher.RemoveHandler(EventNames.PREPARE_NEW_GAME, OnPrepareNewGame);
            _gameCore.Dispatcher.RemoveHandler(EventNames.PREPARE_LOADED_GAME, OnPrepareLoadedGame);
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region PLAYER_BEHAVIOUR --------------------------------------------------------------------------------------

        protected void FixedUpdate()
        {
            _gameStateBehaviour();
        }

        private void GameStateResetBehaviour()
        {
            DontMove();
        }

        private void GameStateRunningBehaviour()
        {
            if (_gameCore.Input.Left.State == GameInputButton.ButtonState.Pressed
                && _gameCore.Input.Right.State == GameInputButton.ButtonState.Pressed)
            {
                DontMove();
            }
            else if (_gameCore.Input.Left.State == GameInputButton.ButtonState.Pressed)
            {
                MoveLeft();
            }
            else if (_gameCore.Input.Right.State == GameInputButton.ButtonState.Pressed)
            {
                MoveRight();
            }
            else
            {
                DontMove();
            }
        }

        private void DontMove()
        {
            _rigidbody.velocity = ConstantValues.VECTOR2_ZERO;
        }

        private void MoveLeft()
        {
            _rigidbody.velocity = _leftMovementSpeedVector;
        }

        private void MoveRight()
        {
            _rigidbody.velocity = _rightMovementSpeedVector;
        }

        //#endregion ---------------------------------------------------------------------------------------------------

        //#region PUBLIC_METHODS (EVENTS) ------------------------------------------------------------------------------

        public void OnEndRound(object obj)
        {
            State = PlayerGameState.Reset;
        }

        public void OnResetToNewRound(object obj)
        {
            this.transform.position = _resetPosition.position;
            State = PlayerGameState.Running;
        }

        public void OnPrepareNewGame(object obj)
        {
            this.transform.position = _resetPosition.position;
            State = PlayerGameState.Running;
        }

        public void OnPrepareLoadedGame(object obj)
        {
            SavedGameData data = _gameCore.SaveGameController.SavedGameData;
            this.transform.position = new Vector3(
                data.Player.PositionX,
                data.Player.PositionY,
                0f);
            State = PlayerGameState.Running;
        }

        public void OnSaveData(object saveGameController)
        {
            SaveGameController controller = (SaveGameController)saveGameController;
            controller.SavePlayerDataCallback(this.transform.position);
        }

        //#endregion ---------------------------------------------------------------------------------------------------
    }
}