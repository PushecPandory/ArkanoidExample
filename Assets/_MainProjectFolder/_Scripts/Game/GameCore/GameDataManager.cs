//GameDataManager.cs
//Created by: Wiktor Frączek
using UnityEngine;
using UnityEngine.Assertions;
using Arkanoid.Utils;

namespace Arkanoid.Game
{
    /// <summary>
    /// GameDataManager holds reference to DesignDataSettings scriptable object.
    /// </summary>
    public class GameDataManager : MonoBehaviour
    {
        [SerializeField]
        private DesignDataSettings _designDataSettings = null;

        public DesignDataSettings DesignData { get { return _designDataSettings; } }

        protected void OnValidate()
        {
            Assert.IsNotNull<DesignDataSettings>(_designDataSettings, ErrorMessage.NoComponentAttached<DesignDataSettings>(typeof(GameDataManager).Name));
        }
    }
}

