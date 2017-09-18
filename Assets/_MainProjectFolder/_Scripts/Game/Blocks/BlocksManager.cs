//BlocksManager.cs
//Created by: Wiktor Frączek
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using Arkanoid.Utils;

namespace Arkanoid.Game
{
    /// <summary>
    //  Receives events from dispatcher and reacts to them by:
    //  - sending request to BlocksGenerator to create new or load saved level
    //  - counting bloks on map and dispatching WIN_ROUND event when last block on the map is hit by the ball
    /// </summary>
    [RequireComponent(typeof(BlocksPoolController))]
    [RequireComponent(typeof(BlocksGenerator))]
    public class BlocksManager : MonoBehaviour, IResetable, IDataToSave
    {
        //#region PRIVATE_FIELDS ----------------------------------------------------------------------------------------

        private GameCore _gameCore = null;
        private BlocksGenerator _blocksGenerator = null;
        private BlocksPoolController _blocksPool = null;

        private List<BlockController> _blocksOnMapList = null;

        private int _blocksOnMap = 0;
        private int _numberOfRows = 2;
        private int _numberOfColumns = 18;

        [SerializeField]
        private BlockController _blockPrefab = null;

        //#endregion ----------------------------------------------------------------------------------------------------


        public BlocksPoolController Pool { get { return _blocksPool; } }


        //#region INIT_AND_EXIT -----------------------------------------------------------------------------------------

        public void Init(GameCore gameCore)
        {
            _gameCore = gameCore;
            
            _gameCore.Dispatcher.AddHandler(EventNames.RESET_TO_NEW_ROUND, OnResetToNewRound);
            _gameCore.Dispatcher.AddHandler(EventNames.ADD_BLOCK_TO_MAP, OnAddBlockToMap);
            _gameCore.Dispatcher.AddHandler(EventNames.REMOVE_BLOCK_FROM_MAP, OnRemoveBlockFromMap);
            _gameCore.Dispatcher.AddHandler(EventNames.SAVE_DATA, OnSaveData);
            _gameCore.Dispatcher.AddHandler(EventNames.PREPARE_NEW_GAME, OnPrepareNewGame);
            _gameCore.Dispatcher.AddHandler(EventNames.PREPARE_LOADED_GAME, OnPrepareLoadedGame);

            LoadDesignData(gameCore.GameDataManager.DesignData);

            _blocksOnMapList = new List<BlockController>();

            _blocksPool = this.GetComponent<BlocksPoolController>();
            _blocksPool.Init(_blockPrefab, _numberOfRows * _numberOfColumns);
            
            _blocksGenerator = this.GetComponent<BlocksGenerator>();
            _blocksGenerator.Init();
        }

        public void Exit()
        {
            _gameCore.Dispatcher.RemoveHandler(EventNames.RESET_TO_NEW_ROUND, OnResetToNewRound);
            _gameCore.Dispatcher.RemoveHandler(EventNames.ADD_BLOCK_TO_MAP, OnAddBlockToMap);
            _gameCore.Dispatcher.RemoveHandler(EventNames.REMOVE_BLOCK_FROM_MAP, OnRemoveBlockFromMap);
            _gameCore.Dispatcher.RemoveHandler(EventNames.SAVE_DATA, OnSaveData);
            _gameCore.Dispatcher.RemoveHandler(EventNames.PREPARE_NEW_GAME, OnPrepareNewGame);
            _gameCore.Dispatcher.RemoveHandler(EventNames.PREPARE_LOADED_GAME, OnPrepareLoadedGame);
        }

        private void LoadDesignData(DesignDataSettings data)
        {
            _numberOfRows = data.NumberOfRows;
            _numberOfColumns = data.NumberOfColumns;
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region VALIDATION --------------------------------------------------------------------------------------------

        protected void OnValidate()
        {
            Assert.IsNotNull<BlockController>(_blockPrefab, ErrorMessage.NoComponentAttached<BlockController>(typeof(BlocksManager).Name));
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region PUBLIC_METHODS (EVENTS) -------------------------------------------------------------------------------

        public void OnResetToNewRound(object obj)
        {
            _blocksOnMap = 0;
            _gameCore.Dispatcher.DispatchEvent(EventNames.CLEAR_BLOCKS);

            _blocksGenerator.GenerateNewLevel(_numberOfColumns, _numberOfRows, Pool);
        }

        public void OnAddBlockToMap(object blockController)
        {
            _blocksOnMapList.Add((BlockController)blockController); 
            _blocksOnMap += 1;
        }

        public void OnRemoveBlockFromMap(object blockController)
        {
            _blocksOnMapList.Remove((BlockController)blockController);
            _blocksOnMap -= 1;
            if (_blocksOnMap == 0)
            {
                _gameCore.Dispatcher.DispatchEvent(EventNames.WIN_ROUND);
            }
            else if (_blocksOnMap < 0)
            {
                Debug.LogError(ErrorMessage.BlocksLessThanZero);
            }
        }

        public void OnSaveData(object saveGameController)
        {
            SaveGameController controller = (SaveGameController)saveGameController;
            controller.SaveBlocksDataCallback(_blocksOnMapList);
        }

        public void OnPrepareNewGame(object obj)
        {
            _blocksGenerator.GenerateNewLevel(_numberOfColumns, _numberOfRows, Pool);
        }

        public void OnPrepareLoadedGame(object obj)
        {
            _blocksGenerator.GenerateLevelFromLoadedData(
                _gameCore.SaveGameController.SavedGameData.Blocks, 
                Pool);
        }

        //#endregion ----------------------------------------------------------------------------------------------------
    }
}

