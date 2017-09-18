//SummaryScreen.cs
//Created by: Wiktor Frączek
using UnityEngine;
using UnityEngine.UI;
using Arkanoid.Utils;
using Arkanoid.Main;
using UnityEngine.Assertions;

namespace Arkanoid.Game
{
    /// <summary>
    /// SummaryScreen controls showing summary and sending question is final score candidate for high scores. Provides API for summary screen buttons.
    /// </summary>
	public class SummaryScreen : MonoBehaviour 
	{
        [SerializeField]
        private GameObject _canvas = null;
        [SerializeField]
        private Text _finalScoreCountText = null;
        [SerializeField]
        private GameObject _newHighScore = null;
        [SerializeField]
        private Button _buttonSelectedOnShow = null;

        private GameCore _gameCore = null;

        //#region INIT_AND_EXIT --------------------------------------------------------------------------------------------

        public void Init(GameCore gameCore)
        {
            _gameCore = gameCore;
            _gameCore.Dispatcher.AddHandler(EventNames.SHOW_SUMMARY_SCREEN, OnShowSummaryScreen);
        }

        public void Exit()
        {
            _gameCore.Dispatcher.RemoveHandler(EventNames.SHOW_SUMMARY_SCREEN, OnShowSummaryScreen);
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region VALIDATION --------------------------------------------------------------------------------------------

        protected void OnValidate()
        {
            Assert.IsNotNull<GameObject>(_canvas, ErrorMessage.NoComponentAttached<GameObject>(typeof(SummaryScreen).Name));
            Assert.IsNotNull<Text>(_finalScoreCountText, ErrorMessage.NoComponentAttached<Text>(typeof(SummaryScreen).Name));
            Assert.IsNotNull<GameObject>(_newHighScore, ErrorMessage.NoComponentAttached<GameObject>(typeof(SummaryScreen).Name));
            Assert.IsNotNull<Button>(_buttonSelectedOnShow, ErrorMessage.NoComponentAttached<Button>(typeof(SummaryScreen).Name));
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        public void OnShowSummaryScreen(object obj)
        {
            int finalScore = _gameCore.ScoreController.Score;
            if (MainCore.Instance.HighScoreController.IsItNewRecordOnHighScoreList(finalScore))
            {
                _newHighScore.SetActive(true);
            }
            _finalScoreCountText.text = finalScore.ToString();
            _canvas.SetActive(true);

            _gameCore.GameEventSystem.SetSelectedGameObject(null);
            _buttonSelectedOnShow.Select();
        }

        public void OnPlayAgainButton()
        {
            _gameCore.SwitchToGameScene();
        }

        public void OnBackToMenuButton()
        {
            _gameCore.SwitchToMenuScene();
        }
    }	
}