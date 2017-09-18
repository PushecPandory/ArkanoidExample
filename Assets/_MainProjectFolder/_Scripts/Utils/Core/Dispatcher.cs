//Dispatcher.cs
//Created by: Wiktor Frączek
using System;
using System.Collections.Generic;

namespace Arkanoid.Utils
{
    /// <summary>
    /// Dispatcher is a mediator for game event system. Scope of the game is small enough to have only one dispatcher.
    /// </summary>
    public class Dispatcher 
	{
        private Dictionary<string, Action<object>> _map = null;

        public Dispatcher() //ctr
        {
            _map = new Dictionary<string, Action<object>>();
        }

        public void AddHandler(string eventName, Action<object> func)
        {
            if (_map.ContainsKey(eventName))
            {
                _map[eventName] += func;
            }
            else
            {
                _map.Add(eventName, func);
            }
        }

        public void RemoveHandler(string eventName, Action<object> func)
        {
            if (_map.ContainsKey(eventName))
            {
                _map[eventName] -= func;
            }
        }

        public void DispatchEvent(string eventName, object obj = null)
        {
            if (_map.ContainsKey(eventName) && _map[eventName] != null)
            {
                _map[eventName](obj);
            }
        }
    }	
}

