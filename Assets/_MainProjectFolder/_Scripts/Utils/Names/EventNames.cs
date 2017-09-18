//EventNames.cs
//Created by: Wiktor Frączek

namespace Arkanoid.Utils
{
	public static class EventNames 
	{
        public static readonly string PAUSE_GAME = "PauseGame";
        public static readonly string UNPAUSE_GAME = "UnpauseGame";
        public static readonly string PAUSE_GAME_AFTER_LOAD = "PauseGameAfterLoad";
        public static readonly string UNPAUSE_GAME_AFTER_LOAD = "UnpauseGameAfterLoad";

        public static readonly string LOSE_ROUND = "LoseRound";
        public static readonly string WIN_ROUND = "WinRound";
        public static readonly string RESET_TO_NEW_ROUND = "ResetToNewRound";
        public static readonly string GAME_OVER = "GameOver";
        public static readonly string PREPARE_NEW_GAME = "PrepareNewGame";
        public static readonly string PREPARE_LOADED_GAME = "PrepareLoadedGame";

        public static readonly string ADD_SCORE = "AddScore";
        public static readonly string ADD_LIFE = "AddLife";
        public static readonly string BULLET_TIME = "BulletTime";

        public static readonly string REMOVE_BLOCK_FROM_MAP = "RemoveBlockFromMap";
        public static readonly string ADD_BLOCK_TO_MAP = "AddBlockToMap";
        public static readonly string CLEAR_BLOCKS = "ClearBlocks";

        public static readonly string SHOW_SUMMARY_SCREEN = "ShowSummaryScreen";
        public static readonly string DISABLE_ESCAPE_MENU = "DisableEscapeMenu";

        public static readonly string SAVE_DATA = "SaveData";
    }	
}
