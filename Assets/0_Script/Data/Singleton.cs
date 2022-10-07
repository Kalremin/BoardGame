using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T _instance => instance;

    protected virtual void Awake()
    {
        instance = gameObject.GetComponent<T>();
    }

}
