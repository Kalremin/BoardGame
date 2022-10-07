using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 중복 생성 방지 싱글턴
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
