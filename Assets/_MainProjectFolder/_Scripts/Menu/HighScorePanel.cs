//HighScorePanel.cs
//Created by: Wiktor Frączek
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Arkanoid.Main;
using Arkanoid.Utils;
using UnityEngine.Assertions;

namespace Arkanoid.Menu
{
    /// <summary>
    /// HighScorePanel is responsible for loading and showing high scores in menu scene.
    /// </summary>
	public class HighScorePanel : MonoBehaviour 
	{
        //#region PRIVATE_FIELDS ----------------------------------------------------------------------------------------

        private MenuCore _menuCore = null;

        [SerializeField]
        private List<Text> _bestScoresList;
        [SerializeField]
        private Button _buttonSelectedOnShow = null;

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region VALIDATION --------------------------------------------------------------------------------------------

        protected void OnValidate()
        {
            Assert.IsNotNull<List<Text>>(_bestScoresList, ErrorMessage.NoComponentAttached<List<Text>>(typeof(HighScorePanel).Name));
            Assert.IsNotNull<Button>(_buttonSelectedOnShow, ErrorMessage.NoComponentAttached<Button>(typeof(HighScorePanel).Name));
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region INIT --------------------------------------------------------------------------------------------------

        public void Init(MenuCore menuCore)
        {
            _menuCore = menuCore;
            LoadAndSetHighScores();
        }

        private void LoadAndSetHighScores()
        {
            MainCore mainCore = MainCore.Instance;
            List<int> highScoresList = mainCore.HighScoreController.List;
            int index = 0;

            foreach (Text t in _bestScoresList)
            {
                t.text = highScoresList[index].ToString();
                index += 1;
            }
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region PUBLIC_METHODS ----------------------------------------------------------------------------------------

        public void OnBackButton()
        {
            _menuCore.MainPanel.ShowPanel();
            this.HidePanel();
        }

        public void ShowPanel()
        {            
            this.gameObject.SetActive(true);
            _buttonSelectedOnShow.Select();
        }

        public void HidePanel()
        {
            this.gameObject.SetActive(false);
        }

        //#endregion ----------------------------------------------------------------------------------------------------
    }
}

