//GameCamera.cs
//Created by: Wiktor Frączek
using UnityEngine;
using UnityEngine.Assertions;
using Arkanoid.Utils;

namespace Arkanoid.Game
{
    /// <summary>
    /// GameCamera script is responsible for setting in init camera size according to screen resolution. 
    /// </summary>
	public class GameCamera : MonoBehaviour 
	{
        private Camera _camera = null;
        private readonly float BASIC_RESOLUTION_RATIO = 16.0f / 9.0f;
        private readonly float BASIC_CAMERA_SIZE = 5.4f;

        public void Init()
        {
            _camera = this.GetComponent<Camera>();
            SetCameraSizeAccordingToResolution();           
        }

        protected void OnValidate()
        {
            _camera = this.GetComponent<Camera>();
            Assert.IsNotNull<Camera>(_camera, ErrorMessage.NoComponentAttached<Camera>(typeof(GameCamera).Name));
        }

        private void SetCameraSizeAccordingToResolution()
        {
            float resolutionRatio = (float)Screen.width / Screen.height;

            if (resolutionRatio < BASIC_RESOLUTION_RATIO)
            {
                _camera.orthographicSize = (BASIC_CAMERA_SIZE * BASIC_RESOLUTION_RATIO) / (resolutionRatio);
            }
            else if (resolutionRatio > BASIC_RESOLUTION_RATIO)
            {
                _camera.orthographicSize = BASIC_CAMERA_SIZE;
            }
        }
    }	
}

