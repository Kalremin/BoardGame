using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
