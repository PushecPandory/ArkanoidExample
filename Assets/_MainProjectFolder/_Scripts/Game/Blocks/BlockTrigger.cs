//BlockTrigger.cs
//Created by: Wiktor Frączek
using UnityEngine;
using Arkanoid.Utils;
using UnityEngine.Assertions;

namespace Arkanoid.Game
{
    /// <summary>
    /// BlockTrigger  purpose is to send information about collision with Ball to BlockController.
    /// This script should be attached to GameObject "NormalBlock/Colliders/CollideWithBall". 
    /// GameObject must have Collider2D component with "isTrigger" == false. Balls collider must be tagged as "BALL".
    /// </summary>
	public class BlockTrigger : MonoBehaviour 
	{
        private BlockController _blockController = null;

        public void Init(BlockController blockController)
        {
            _blockController = blockController;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(TagNames.BALL))
            {
                _blockController.RegisterCollisionWithBall();
            }
        }

        protected void OnValidate()
        {
            Collider2D _collider = this.GetComponent<Collider2D>();
            Assert.IsNotNull<Collider2D>(_collider, ErrorMessage.NoComponentAttached<Collider2D>(typeof(BlockTrigger).Name));
            Assert.IsFalse(_collider.isTrigger, ErrorMessage.TriggerShouldBe(typeof(BlockTrigger).Name, false));
        }
    }	
}

