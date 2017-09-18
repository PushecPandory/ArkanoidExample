//BlocksPoolController.cs
//Created by: Wiktor Frączek
using UnityEngine;
using System.Collections.Generic;

namespace Arkanoid.Game
{
    /// <summary>
    /// BlocksPoolController provides object pooling for NormalBlock prefabs which have a BlockController.
    /// </summary>
	public class BlocksPoolController : MonoBehaviour 
	{
        private Stack<BlockController> _poolStack = null;
        private BlockController _blockPrefab = null;

        public void Init(BlockController blockPrefab, int blocksCount)
        {
            _poolStack = new Stack<BlockController>();
            _blockPrefab = blockPrefab;
            PrepareBlocksPool(blocksCount);
        }

        private void PrepareBlocksPool(int blocksCount)
        {
            for (int i = 0; i < blocksCount; ++i)
            {
                CreateBlockOnStack(_blockPrefab);
            }
        }

        private void CreateBlockOnStack(BlockController blockPrefab)
        {
            BlockController block = Instantiate<BlockController>(blockPrefab);
            block.Init();
            block.transform.SetParent(this.transform);
            block.gameObject.SetActive(false);
            _poolStack.Push(block);
        }

        /// <summary>
        /// Returns BlockController in disabled GameObject.
        /// </summary>
        public BlockController PopBlockFromPool()
        {
            BlockController blockToReturn = _poolStack.Pop();
            if (blockToReturn == null)
            {
                CreateBlockOnStack(_blockPrefab);
            }
            blockToReturn.IsPooled = true;
            return blockToReturn;
        }

        /// <summary>
        /// Gives back to pool BlockController passed in argument and disabling it gameobject.
        /// </summary>
        public void PushBlockToPool(BlockController block)
        {
            block.IsPooled = false;            
            block.gameObject.SetActive(false);
            _poolStack.Push(block);
        }
    }	
}

