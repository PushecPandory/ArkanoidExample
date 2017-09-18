//BallColliderController.cs
//Created by: Wiktor Frączek
using UnityEngine;
using Arkanoid.Utils;
using UnityEngine.Assertions;

namespace Arkanoid.Game
{
    /// <summary>
    /// BallColliderController purpose is to send information about collision with Player to BallController.
    /// This script should be attached to GameObject "BallController/Colliders/CollideWithPlayer". 
    /// GameObject must have Collider2D component with "isTrigger" == false. Players collider must be tagged as "Player".
    /// </summary>
	public class BallColliderController : MonoBehaviour 
	{
        private BallController _ballController = null;

        public void Init(BallController ballController)
        {
            _ballController = ballController;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.collider.CompareTag(TagNames.PLAYER))
            {
                _ballController.BounceFromTheRocket();
            }
        }

        protected void OnValidate()
        {
            Collider2D _collider = this.GetComponent<Collider2D>();
            Assert.IsNotNull<Collider2D>(_collider, ErrorMessage.NoComponentAttached<Collider2D>(typeof(BallColliderController).Name));
            Assert.IsFalse(_collider.isTrigger, ErrorMessage.TriggerShouldBe(typeof(BallColliderController).Name , false));
        }
    }	
}
