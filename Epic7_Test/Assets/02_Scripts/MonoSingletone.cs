using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingletone<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            instance = FindObjectOfType(typeof(T)) as T;

            if (instance == null)
            {
                instance = new GameObject(typeof(T).ToString(), typeof(T)).AddComponent<T>();
            }

            return instance;
        }
    }
}

public class SingleTone<T> where T : class
{
    private static T instance = null;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Activator.CreateInstance(typeof(T)) as T;
            }

            return instance;
        }
    }
}
