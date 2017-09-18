//GameInputButton.cs
//Created by: Wiktor Frączek
using UnityEngine;

namespace Arkanoid.Utils
{
    /// <summary>
    /// Part of subsystem which provides intermediate logic layer between Unity input and game. It defines button states: JustPressed, Pressed, JustReleased, Released'
    /// and brings API for updating button state.
    /// </summary>
    public class GameInputButton
    {
        public enum ButtonState
        {
            JustPressed,
            Pressed,
            JustReleased,
            Released
        }

        private string _unityInputName = string.Empty;
        private ButtonState _state = ButtonState.Released;

        public ButtonState State
        {
            get
            {
                return _state;
            }
        }

        public GameInputButton(string unityInputName)
        {
            _unityInputName = unityInputName;
        }

        public void UpdateState()
        {
            if (Input.GetButtonUp(_unityInputName))
            {
                _state = ButtonState.JustReleased;
            }
            else if (Input.GetButtonDown(_unityInputName))
            {
                _state = ButtonState.JustPressed;
            }
            else if (Input.GetButton(_unityInputName))
            {
                _state = ButtonState.Pressed;
            }
            else
            {
                _state = ButtonState.Released;
            }
        }
    }
}