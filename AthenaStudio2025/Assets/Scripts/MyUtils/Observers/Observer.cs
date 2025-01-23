using System;
using System.Collections.Generic;
using UnityEngine;
namespace MyUtils
{
    public class Observer : MonoBehaviour
    {
        public static Observer Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

         Dictionary<string, List<Action<object[]>>> Listeners =
            new();

        public  void AddObserver(string name, Action<object[]> callback)
        {
            if (!Listeners.ContainsKey(name))
            {
                Listeners.Add(name, new List<Action<object[]>>());
            }

            Listeners[name].Add(callback);
        }

        public  void RemoveObserver(string name, Action<object[]> callback)
        {
            if (!Listeners.ContainsKey(name))
            {
                return;
            }

            Listeners[name].Remove(callback);
        }

        public  void Notify(string name, params object[] data)
        {
            if (!Listeners.ContainsKey(name))
            {
                return;
            }

            foreach (var listener in Listeners[name])
            {
                try
                {
                    listener.Invoke(data);
                }
                catch (Exception ex)
                {
                    //Debug.LogError("Error on invoke listener: " + ex);
                }
            }
        }

    }
}