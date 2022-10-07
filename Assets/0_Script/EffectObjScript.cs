using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 오브젝트 이펙트 스크립트
public class EffectObjScript : MonoBehaviour
{
    [SerializeField] float _time = 5f;
    [SerializeField] eEffect _effectKind;

    public eEffect EffectKind =>_effectKind;
    private void OnEnable()
    {
        Invoke("ReturnScript", _time);
    }

    void ReturnScript()
    {
        EffectManager._instance.ReturnEffectObj(this);
    }

}
