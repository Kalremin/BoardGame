using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoOverlapSingleton<T> : Singleton<T> where T: MonoBehaviour
{
    protected override void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

}
