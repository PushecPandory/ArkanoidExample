//BlockController.cs
//Created by: Wiktor Frączek
using UnityEngine;
using System.Collections.Generic;
using Arkanoid.Utils;

namespace Arkanoid.Game
{
    /// <summary>
    /// BlockGenerator provides methods for generating new random level or loading it from saved data. For this purpose it uses BlockController pool stored in BlocksPoolController.
    /// </summary>
	public class BlocksGenerator : MonoBehaviour 
	{
        //#region PRIVATE_FIELDS ----------------------------------------------------------------------------------------

        [SerializeField]
        private float _blockWidth = 1f;
        [SerializeField]
        private float _blockHeight = 1f;

        private float _chanceForDoubleScoreBlock = 20f;
        private float _chanceForBulletTimeBlock = 5f;
        private float _chanceForAdditionalLifeBlock = 5f;
        private float _chanceForNormalBlock = 100f;

        private float _doubleScoreRandomRangeThreshold = 0f;
        private float _bulletTimeRandomRangeThreshold = 0f;
        private float _additionalLifeRandomRangeThreshold = 0f;
        private float _normalBlockRandomRangeThreshold = 0f;

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region INIT --------------------------------------------------------------------------------------------------

        public void Init()
        {
            LoadDesignData();
            SetRandomRangeTresholds();
        }

        private void LoadDesignData()
        {
            DesignDataSettings data = GameCore.Instance.GameDataManager.DesignData;
            _chanceForDoubleScoreBlock = data.ChanceForDoubleScoreBlock;
            _chanceForBulletTimeBlock = data.ChanceForBulletTimeBlock;
            _chanceForAdditionalLifeBlock = data.ChanceForAdditionalLifeBlock;
            _chanceForNormalBlock = data.ChanceForNormalBlock;
        }

        private void SetRandomRangeTresholds()
        {
            _doubleScoreRandomRangeThreshold = _chanceForDoubleScoreBlock;
            _bulletTimeRandomRangeThreshold = _doubleScoreRandomRangeThreshold + _chanceForBulletTimeBlock;
            _additionalLifeRandomRangeThreshold = _bulletTimeRandomRangeThreshold + _chanceForAdditionalLifeBlock;
            _normalBlockRandomRangeThreshold = _additionalLifeRandomRangeThreshold + _chanceForNormalBlock;
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region PRIVATE_METHODS ---------------------------------------------------------------------------------------

        private void RandomizeBlockBehaviour(BlockController block)
        {
            float randomNumber = Random.Range(0f, _normalBlockRandomRangeThreshold);

            if (randomNumber < _doubleScoreRandomRangeThreshold)
            {
                block.Type = BlockController.BlockType.DoubleScore;
            }
            else if (randomNumber < _bulletTimeRandomRangeThreshold)
            {
                block.Type = BlockController.BlockType.BulletTime;
            }
            else if (randomNumber < _additionalLifeRandomRangeThreshold)
            {
                block.Type = BlockController.BlockType.AdditionalLife;
            }
            else
            {
                block.Type = BlockController.BlockType.NormalBlock;
            }
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region PUBLIC_METHODS-----------------------------------------------------------------------------------------
   
        public void GenerateNewLevel(int numberOfColumns, int numberOfRows, BlocksPoolController pool)
        {
            for (int i = 0; i < numberOfRows; ++i)
            {
                for (int j = 0; j < numberOfColumns; ++j)
                {
                    BlockController block = pool.PopBlockFromPool();                                    
                    block.gameObject.SetActive(true);
                    block.transform.position = new Vector3(
                        this.transform.position.x + (j * _blockWidth), 
                        this.transform.position.y - (i * _blockHeight), 
                        0f);
                    RandomizeBlockBehaviour(block);
                    GameCore.Instance.Dispatcher.DispatchEvent(EventNames.ADD_BLOCK_TO_MAP, block);
                }
            }
        }

        public void  GenerateLevelFromLoadedData(List<BlockData> blocksData, BlocksPoolController pool)
        {
            foreach (BlockData blockData in blocksData)
            {
                BlockController block = pool.PopBlockFromPool();
                block.gameObject.SetActive(true);
                block.transform.position = new Vector3(
                    blockData.PositionX,
                    blockData.PositionY,
                    0f);
                block.Type = blockData.BlockType;
                GameCore.Instance.Dispatcher.DispatchEvent(EventNames.ADD_BLOCK_TO_MAP, block);
            }
        }

        //#endregion ----------------------------------------------------------------------------------------------------
    }
}

