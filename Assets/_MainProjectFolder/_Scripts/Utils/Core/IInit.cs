//IInit.cs
//Created by: Wiktor Frączek

namespace Arkanoid.Utils
{
    /// <summary>
    /// This interface is used by CoreSingleton to initialize classes which derives from CoreSingleton.
    /// </summary>
    public interface IInit 
	{
        void Init();
	}	
}