//ErrorMessage.cs
//Created by: Wiktor Frączek

namespace Arkanoid.Utils
{
    public static class ErrorMessage
    {
        public static string NoComponentAttached<T>(string parentScriptName)
        {
            return parentScriptName + " has not " + typeof(T).Name + " attached!";
        }

        public static string NoMainCore = "Error during loading MainCore!";

        public static string BlocksLessThanZero = "In BlocksManager number of blocks is less than zero! This is unexpected behaviour and might cause errors!";

        public static string TriggerShouldBe(string parentScriptName, bool falseOrTrue)
        {
            return "In " + parentScriptName + " in component Collider2D IsTrigger should be set to: " + falseOrTrue;
        }
    }
}
