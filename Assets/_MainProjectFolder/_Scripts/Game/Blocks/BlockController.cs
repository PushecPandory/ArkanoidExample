//BlockController.cs
//Created by: Wiktor Frączek
using UnityEngine;
using Arkanoid.Utils;
using UnityEngine.Assertions;

namespace Arkanoid.Game
{
    /// <summary>
    /// BlockController control how block looks and behaves which depends on the BlockType. Type is defining during creation by BlocksGenerator.
    /// When block is beated by ball it dispatches proper events (depends on the BlockType) with proper arguments: ADD_SCORE, BULLET_TIME, ADD_LIFE.
    /// </summary>
    public class BlockController : MonoBehaviour 
	{
        public enum BlockType
        {
            NormalBlock,
            DoubleScore,
            BulletTime,
            AdditionalLife
        }

        //#region PRIVATE_FIELDS -----------------------------------------------------------------------------------------

        private GameCore _gameCore = null;
        private BlockType _blockType = BlockType.NormalBlock;
        private int _scoreForNormalBlock = 100;
        private int _scoreForDoubleScoreBlock = 200;
        private int _addLifesCount = 1;

        [SerializeField]
        private BlockTrigger _blockTrigger = null;
        [SerializeField]
        private BlockView _blockView = null;

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region ACCESSORS ---------------------------------------------------------------------------------------------

        public BlockType Type
        {
            get { return _blockType; }
            set
            {
                _blockView.SetView(value);
                _blockType = value;
            }
        }

        public bool IsPooled { get; set; }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region VALIDATION --------------------------------------------------------------------------------------------

        protected void OnValidate()
        {
            Assert.IsNotNull<BlockTrigger>(_blockTrigger, ErrorMessage.NoComponentAttached<BlockTrigger>(typeof(BlockController).Name));
            Assert.IsNotNull<BlockView>(_blockView, ErrorMessage.NoComponentAttached<BlockView>(typeof(BlockController).Name));
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region INIT_AND_EXIT -----------------------------------------------------------------------------------------

        public void Init()
        {
            _gameCore = GameCore.Instance;
            _gameCore.Dispatcher.AddHandler(EventNames.CLEAR_BLOCKS, OnClearBlocks);
            _blockTrigger.Init(this);
            LoadDesignData();
        }

        public void Exit()
        {
            _gameCore.Dispatcher.RemoveHandler(EventNames.CLEAR_BLOCKS, OnClearBlocks);
        }

        private void LoadDesignData()
        {
            _scoreForNormalBlock = _gameCore.GameDataManager.DesignData.ScoreForNormalBlock;
            _scoreForDoubleScoreBlock = _gameCore.GameDataManager.DesignData.ScoreForDoubleScoreBlock;
            _addLifesCount = _gameCore.GameDataManager.DesignData.AddLifesCount;
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        private void OnClearBlocks(object obj)
        {
            if (IsPooled)
            {
                _gameCore.BlocksManager.Pool.PushBlockToPool(this);
            }           
        }

        public void RegisterCollisionWithBall()
        {
            if (_blockType == BlockType.NormalBlock)
            {
                _gameCore.Dispatcher.DispatchEvent(EventNames.ADD_SCORE, _scoreForNormalBlock);
            }
            else if (_blockType == BlockType.DoubleScore)
            {
                _gameCore.Dispatcher.DispatchEvent(EventNames.ADD_SCORE, _scoreForDoubleScoreBlock);
            }
            else if (_blockType == BlockType.BulletTime)
            {
                _gameCore.Dispatcher.DispatchEvent(EventNames.ADD_SCORE, _scoreForNormalBlock);
                _gameCore.Dispatcher.DispatchEvent(EventNames.BULLET_TIME);
            }
            else if (_blockType == BlockType.AdditionalLife)
            {
                _gameCore.Dispatcher.DispatchEvent(EventNames.ADD_SCORE, _scoreForNormalBlock);
                _gameCore.Dispatcher.DispatchEvent(EventNames.ADD_LIFE, _addLifesCount);
            }

            _gameCore.Dispatcher.DispatchEvent(EventNames.REMOVE_BLOCK_FROM_MAP, this);
            _gameCore.BlocksManager.Pool.PushBlockToPool(this);
        }
    }	
}

