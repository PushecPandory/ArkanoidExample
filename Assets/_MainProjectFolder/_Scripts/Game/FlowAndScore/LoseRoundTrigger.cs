//LoseRoundTrigger.cs
//Created by: Wiktor Frączek
using UnityEngine;
using Arkanoid.Utils;
using UnityEngine.Assertions;

namespace Arkanoid.Game
{
    /// <summary>
    /// LoseRoundTrigger purpose is to send information about collision with Ball via LOSE_ROUND event.
    /// GameObject must have Collider2D component. Ball collider must be tagged as "Ball".
    /// </summary>
	public class LoseRoundTrigger : MonoBehaviour 
	{
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.CompareTag(TagNames.BALL))
            {
                GameCore.Instance.Dispatcher.DispatchEvent(EventNames.LOSE_ROUND);
            }
        }

        protected void OnValidate()
        {
            Collider2D _collider = this.GetComponent<Collider2D>();
            Assert.IsNotNull<Collider2D>(_collider, ErrorMessage.NoComponentAttached<Collider2D>(typeof(LoseRoundTrigger).Name));
        }
    }	
}
