//CoreSingleton.cs
//Created by: Wiktor Frączek
using UnityEngine;

namespace Arkanoid.Utils
{
    /// <summary>
    /// CoreSingleton is a base class for core systems which are managing scenes.
    /// Provides static accessor "Instance", Init on Awake and Exit on OnDestroy.
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public class CoreSingleton<T> : MonoBehaviour, IInit where T : MonoBehaviour, IInit
    {
        //#region FIELDS_AND_ACCESSORS ----------------------------------------------------------------------------------

        private static T _instance;
        private static object _lock = new object();

        protected string PathToMainCorePrefab = "Prefabs/pref_MainCore";

        protected CoreSingleton() { } //Protected constructor to ensure it will be a singleton.

        public static T Instance
        {
            get
            {
                lock(_lock)
                {
                    if (_instance == null)
                    {
                        MakeThisSingletonAndInit();
                    }

                    return _instance;
                }
            }
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region MAKE_SINGLETON_AND_INIT -------------------------------------------------------------------------------

        private static void MakeThisSingletonAndInit()
        {
            T[] list = FindObjectsOfType(typeof(T)) as T[];

            if (list.Length > 1)
            {
                #if UNITY_EDITOR
                    Debug.LogWarning(WarningMessage.MoreThanOne<T>());
                #endif
            }

            if (list.Length == 0)
            {
                #if UNITY_EDITOR
                    Debug.LogWarning(WarningMessage.ThereIsNo<T>());
                #endif
            }
            else
            {
                _instance = list[0];
                _instance.Init();
            }
        }

        //#endregion ----------------------------------------------------------------------------------------------------

        //#region NON_STATIC_METHODS ------------------------------------------------------------------------------------

        protected void Awake()
        {
            if (Instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        public virtual void Init()
        {
            //To override
        }

        public virtual void Exit()
        {
            //To override
        }

        protected void OnDestroy()
        {
            Exit();
            _instance = null;
        }

        //#endregion ----------------------------------------------------------------------------------------------------
    }
}
