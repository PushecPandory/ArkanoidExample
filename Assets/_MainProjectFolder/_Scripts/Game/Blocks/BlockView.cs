//BlockView.cs
//Created by: Wiktor Frączek
using UnityEngine;
using UnityEngine.Assertions;
using Arkanoid.Utils;

namespace Arkanoid.Game
{
    /// <summary>
    /// BlockView holds sprite references for different block views. Provides API for setting blocks view.
    /// </summary>
	public class BlockView : MonoBehaviour 
	{
        [SerializeField]
        private SpriteRenderer _sprite = null;
        [SerializeField]
        private Sprite _normalBlockSprite = null;
        [SerializeField]
        private Sprite _doubleScoreSprite = null;
        [SerializeField]
        private Sprite _bulletTimeSprite = null;
        [SerializeField]
        private Sprite _additionalLifeSprite = null;

        //#region VALIDATION --------------------------------------------------------------------------------------------

        protected void OnValidate()
        {
            Assert.IsNotNull<SpriteRenderer>(_sprite, ErrorMessage.NoComponentAttached<SpriteRenderer>(typeof(BlockView).Name));
            Assert.IsNotNull<Sprite>(_normalBlockSprite, ErrorMessage.NoComponentAttached<Sprite>(typeof(BlockView).Name));
            Assert.IsNotNull<Sprite>(_doubleScoreSprite, ErrorMessage.NoComponentAttached<Sprite>(typeof(BlockView).Name));
            Assert.IsNotNull<Sprite>(_bulletTimeSprite, ErrorMessage.NoComponentAttached<Sprite>(typeof(BlockView).Name));
            Assert.IsNotNull<Sprite>(_additionalLifeSprite, ErrorMessage.NoComponentAttached<Sprite>(typeof(BlockView).Name));
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        public void SetView(BlockController.BlockType type)
        {
            if (type == BlockController.BlockType.NormalBlock)
            {
                _sprite.sprite = _normalBlockSprite;
            }
            else if (type == BlockController.BlockType.DoubleScore)
            {
                _sprite.sprite = _doubleScoreSprite;
            }
            else if(type == BlockController.BlockType.BulletTime)
            {
                _sprite.sprite = _bulletTimeSprite;
            }
            else if(type == BlockController.BlockType.AdditionalLife)
            {
                _sprite.sprite = _additionalLifeSprite;
            }
        }
	}	
}