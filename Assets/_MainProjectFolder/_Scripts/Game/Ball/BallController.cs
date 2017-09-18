//BallController.cs
//Created by: Wiktor Frączek
using UnityEngine;
using System;
using UnityEngine.Assertions;
using Arkanoid.Utils;

namespace Arkanoid.Game
{
    /// <summary>
    /// BallController class is responsible for definig ball behaviour, changing state of ball (and thus current behaviour) and sending data to save to SaveGameController on proper event.
    /// BallStateGame changes when ball is fired (in GameStateReadyToFireBehaviour()) or when one of events is recived: LOSE_ROUND, WIN_ROUND, RESET_TO_NEW_ROUND, PREPARE_NEW_GAME, PREPARE_LOADED_GAME.
    /// In class there is an Action "_gameStateBehaviour" which is called in Update(). Depending on state it has proper method assigned (assigning is in BallGameState State accessor). 
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class BallController : MonoBehaviour, IResetable, IDataToSave
    {
        public enum BallGameState
        {
            Reset,
            Running,
            ReadyToFire
        }

        //#region PRIVATE_FIELDS ----------------------------------------------------------------------------------------

        private GameCore _gameCore = null;
        private float _ballMovementSpeed = 3f;
        private float _ballSpeedMovementAccelerationPerLevel = 1f;
        private Action _gameStateBehaviour = null;
        private bool _wasShootedAfterReset = false;
        private BallGameState _state;

        [SerializeField]
        private BallColliderController _ballColliderController = null;
        [SerializeField]
        private Rigidbody2D _rigidbody = null;
        [SerializeField]
        private Transform _resetPosition = null;

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region ACCESSORS ---------------------------------------------------------------------------------------------

        private BallGameState State
        {
            set
            {
                if (value == BallGameState.Reset)
                {
                    _gameStateBehaviour = GameStateResetBehaviour;
                }
                else if (value == BallGameState.Running)
                {
                    _gameStateBehaviour = GameStateRunningBehaviour;
                }
                else if (value == BallGameState.ReadyToFire)
                {
                    _gameStateBehaviour = GameStateReadyToFireBehaviour;
                }

                _state = value;
            }
            get
            {
                return _state;
            }
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region VALIDATION --------------------------------------------------------------------------------------------

        protected void OnValidate()
        {
            Assert.IsNotNull<BallColliderController>(_ballColliderController, ErrorMessage.NoComponentAttached<BallColliderController>(typeof(BallController).Name));
            Assert.IsNotNull<Rigidbody2D>(_rigidbody, ErrorMessage.NoComponentAttached<Rigidbody2D>(typeof(BallController).Name));
            Assert.IsNotNull<Transform>(_resetPosition, ErrorMessage.NoComponentAttached<Transform>(typeof(BallController).Name));
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region INIT_AND_EXIT -----------------------------------------------------------------------------------------

        public void Init(GameCore gameCore)
        {
            _gameCore = gameCore;
            _gameCore.Dispatcher.AddHandler(EventNames.LOSE_ROUND, OnLoseRound);
            _gameCore.Dispatcher.AddHandler(EventNames.WIN_ROUND, OnWinRound);
            _gameCore.Dispatcher.AddHandler(EventNames.RESET_TO_NEW_ROUND, OnResetToNewRound);
            _gameCore.Dispatcher.AddHandler(EventNames.SAVE_DATA, OnSaveData);
            _gameCore.Dispatcher.AddHandler(EventNames.PREPARE_NEW_GAME, OnPrepareNewGame);
            _gameCore.Dispatcher.AddHandler(EventNames.PREPARE_LOADED_GAME, OnPrepareLoadedGame);
            _ballColliderController.Init(this);
        }

        public void Exit()
        {
            _gameCore.Dispatcher.RemoveHandler(EventNames.LOSE_ROUND, OnLoseRound);
            _gameCore.Dispatcher.RemoveHandler(EventNames.WIN_ROUND, OnWinRound);
            _gameCore.Dispatcher.RemoveHandler(EventNames.RESET_TO_NEW_ROUND, OnResetToNewRound);
            _gameCore.Dispatcher.RemoveHandler(EventNames.SAVE_DATA, OnSaveData);
            _gameCore.Dispatcher.RemoveHandler(EventNames.PREPARE_NEW_GAME, OnPrepareNewGame);
            _gameCore.Dispatcher.RemoveHandler(EventNames.PREPARE_LOADED_GAME, OnPrepareLoadedGame);
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region BALL_BEHAVIOUR ----------------------------------------------------------------------------------------

        protected void Update()
        {
            if (!_gameCore.TimeController.IsPaused)
            {
                _gameStateBehaviour();
            }
        }

        private void GameStateResetBehaviour()
        {
            _rigidbody.velocity = ConstantValues.VECTOR2_ZERO;
            _wasShootedAfterReset = false;
        }

        private void GameStateRunningBehaviour()
        {
            //do nothing in this state
        }

        private void GameStateReadyToFireBehaviour()
        {
            if (!_wasShootedAfterReset && _gameCore.Input.Fire.State == GameInputButton.ButtonState.JustPressed)
            {
                _wasShootedAfterReset = true;                
                FireBallOnStartRound();
                State = BallGameState.Running;
            }
        }

        /// <summary>
        /// FireBallOnStartRound() has the same logic as BounceFromTheRocket() but it is another method because 
        /// if we want to change FireBallOnStart game design we can do changes only in one method scope.
        /// </summary>
        private void FireBallOnStartRound()
        {
            BounceFromTheRocket();
        }

        public void BounceFromTheRocket()
        {
            Vector3 playerPosition = _gameCore.Player.transform.position;
            Vector3 ballMovementDirection = (playerPosition - this.transform.position).normalized;

            _rigidbody.velocity = -ballMovementDirection * _ballMovementSpeed;
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region PUBLIC_METHODS (EVENTS) -------------------------------------------------------------------------------

        public void OnWinRound(object obj)
        {
            State = BallGameState.Reset;
            _ballMovementSpeed += _ballSpeedMovementAccelerationPerLevel;
        }

        public void OnLoseRound(object obj)
        {
            State = BallGameState.Reset;
        }

        public void OnResetToNewRound(object obj)
        {
            this.transform.position = _resetPosition.position;
            State = BallGameState.ReadyToFire;
        }

        public void OnPrepareNewGame(object obj)
        {
            this.transform.position = _resetPosition.position;
            State = BallGameState.ReadyToFire;
            _ballMovementSpeed = _gameCore.GameDataManager.DesignData.BasicBallSpeedMovement;
            _ballSpeedMovementAccelerationPerLevel = _gameCore.GameDataManager.DesignData.BallSpeedMovementAccelerationPerLevel;
        }

        public void OnPrepareLoadedGame(object obj)
        {           
            SavedGameData data = _gameCore.SaveGameController.SavedGameData;
            State = data.Ball.State;
            _ballMovementSpeed = data.Ball.Speed;
            _ballSpeedMovementAccelerationPerLevel = _gameCore.GameDataManager.DesignData.BallSpeedMovementAccelerationPerLevel;

            if (State == BallGameState.Reset)
            {
                this.transform.position = _resetPosition.position;
                _rigidbody.velocity = ConstantValues.VECTOR2_ZERO;
                State = BallGameState.ReadyToFire;
            }
            else if (State == BallGameState.Running)
            {
                this.transform.position = new Vector3(
                    data.Ball.PositionX,
                    data.Ball.PositionY,
                    0f);

                _rigidbody.velocity = new Vector2(
                    data.Ball.MovementDirectonX,
                    data.Ball.MovementDirectonY);
            }
            else if (State == BallGameState.ReadyToFire)
            {
                this.transform.position = _resetPosition.position;
                _rigidbody.velocity = ConstantValues.VECTOR2_ZERO;
            }
        }

        public void OnSaveData(object saveGameController)
        {
            SaveGameController controller = (SaveGameController)saveGameController;
            controller.SaveBallDataCallback(
                State,
                _ballMovementSpeed,
                this.transform.position,
                _rigidbody.velocity);
        }

        //#endregion ----------------------------------------------------------------------------------------------------
    }	
}    