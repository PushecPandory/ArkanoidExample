//HUDController.cs
//Created by: Wiktor Frączek
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using Arkanoid.Utils;

namespace Arkanoid.Game
{
    /// <summary>
    /// HUDControlles provides public methods for updating: score, lifes; and showing/hiding popups: LoseRound, WinRound, GameOver;
    /// </summary>
	public class HUDController : MonoBehaviour 
	{
        //#region PRIVATE_FIELDS ----------------------------------------------------------------------------------------

        private GameObject _activePopUp = null;

        [SerializeField]
        private GameObject _loseRoundPopUp = null;
        [SerializeField]
        private GameObject _winRoundPopUp = null;
        [SerializeField]
        private GameObject _gameOverRoundPopUp = null;
        [SerializeField]
        private Text _lifesText = null;
        [SerializeField]
        private Text _scoreText = null;

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region VALIDATION --------------------------------------------------------------------------------------------

        protected void OnValidate()
        {
            Assert.IsNotNull<GameObject>(_loseRoundPopUp, ErrorMessage.NoComponentAttached<GameObject>(typeof(HUDController).Name));
            Assert.IsNotNull<GameObject>(_winRoundPopUp, ErrorMessage.NoComponentAttached<GameObject>(typeof(HUDController).Name));
            Assert.IsNotNull<GameObject>(_gameOverRoundPopUp, ErrorMessage.NoComponentAttached<GameObject>(typeof(HUDController).Name));
            Assert.IsNotNull<Text>(_scoreText, ErrorMessage.NoComponentAttached<Text>(typeof(HUDController).Name));
            Assert.IsNotNull<Text>(_lifesText, ErrorMessage.NoComponentAttached<Text>(typeof(HUDController).Name));
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region PUBLIC_METHODS ----------------------------------------------------------------------------------------

        public void ShowLoseRoundPopUp()
        {
            ShowActivePopUp(_loseRoundPopUp);
        }

        public void ShowWinRoundPopUp()
        {
            ShowActivePopUp(_winRoundPopUp);
        }

        public void ShowGameOverRoundPopUp()
        {
            ShowActivePopUp(_gameOverRoundPopUp);
        }

        public void HideActivePopUp()
        {
            if (_activePopUp != null)
            {
                _activePopUp.SetActive(false);
            }
            _activePopUp = null;                     
        }

        private void ShowActivePopUp(GameObject popUp)
        {
            _activePopUp = popUp;
            if (_activePopUp != null)
            {
                _activePopUp.SetActive(true);
            }
        }

        public void UpdateScoreText(int score)
        {
            _scoreText.text = score.ToString();
        }

        public void UpdateLifesText(int lifes)
        {
            _lifesText.text = lifes.ToString();
        }

        //#endregion ----------------------------------------------------------------------------------------------------
    }
}
