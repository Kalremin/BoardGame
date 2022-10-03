using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eEffect
{
    Hit,
    Magic,
    Summon,
    Dead,
    Buff,
    Debuff
}

public class EffectManager : MonoBehaviour
{
    public static EffectManager _instance;

    [SerializeField]
    GameObject[] _effectObj;

    Queue<GameObject>[] _effectPooling;
    void Start()
    {
        _instance = this;
        _effectPooling = new Queue<GameObject>[_effectObj.Length];
        for(int i = 0; i < _effectPooling.Length; i++)
            _effectPooling[i] = new Queue<GameObject>();
        
    }


    GameObject GetEffectObj(eEffect enumEffect)
    {
        if (_effectPooling[(int)enumEffect].Count > 0)
        {
            GameObject temp = _effectPooling[(int)enumEffect].Dequeue();
            temp.SetActive(true);
            return temp;

        }
        else
        {
            return Instantiate(_effectObj[(int)enumEffect]);
        }
    }


    public void SpawnEffect(eEffect effect, Transform transformParent, Vector3 localPos, int scale = 5)
    {
        GameObject temp = GetEffectObj(effect);
        temp.transform.SetParent( transformParent);
        temp.transform.localEulerAngles = Vector3.zero;
        temp.transform.localPosition = localPos;
        temp.transform.localScale = Vector3.one * scale;

        SoundManager._instance.PlayEffectSound((eEffectSound)effect, transformParent);
    }

    public void ReturnEffectObj(EffectObjScript effectObj)
    {
        _effectPooling[(int)effectObj.EffectKind].Enqueue(effectObj.gameObject);
        effectObj.transform.SetParent( transform);
        effectObj.gameObject.SetActive(false);

    }
}
