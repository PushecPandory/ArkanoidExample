//IResetable.cs
//Created by: Wiktor Frączek

namespace Arkanoid.Game
{
    /// <summary>
    /// All scripts which should be reset on new round should have this interface.
    /// </summary>
	public interface IResetable 
	{
        void OnResetToNewRound(object obj);
	}	
}

