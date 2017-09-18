//GameInput.cs
//Created by: Wiktor Frączek
using UnityEngine;
using Arkanoid.Utils;
using System.Collections.Generic;

namespace Arkanoid.Game
{
    /// <summary>
    /// Provides intermediate logic layer between Unity input and game.
    /// </summary>
    public class GameInput : MonoBehaviour 
    {
        private List<GameInputButton> _buttonList = null;

        public GameInputButton Left = null;
        public GameInputButton Right = null;
        public GameInputButton Fire = null;
        public GameInputButton Enter = null;
        public GameInputButton Esc = null;

        public void Init()
        {
            Left = CreateAndAddButton(InputNames.LEFT);
            Right = CreateAndAddButton(InputNames.RIGHT);
            Fire = CreateAndAddButton(InputNames.FIRE);
            Enter = CreateAndAddButton(InputNames.ENTER);
            Esc = CreateAndAddButton(InputNames.ESC);
        }

        protected void Update()
        {
            foreach (var button in _buttonList)
            {
                button.UpdateState();
            }
        }

        private GameInputButton CreateAndAddButton(string unityInputName)
        {
            GameInputButton button = new GameInputButton(unityInputName);
            if (_buttonList == null)
            {
                _buttonList = new List<GameInputButton>();
            }
            _buttonList.Add(button);
            return button;
        }
    }
}
