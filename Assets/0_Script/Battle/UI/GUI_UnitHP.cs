﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_UnitHP : MonoBehaviour
{
    //[SerializeField]
    //Slider _unitUP;

    [SerializeField]
    GameObject _unitStatusObj;

    [SerializeField]
    Image _hpCircle;

    // Start is called before the first frame update
    void Start()
    {
        _unitStatusObj.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GUIScript._instance.ExistPauseWnd())
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if(Physics.Raycast(ray,out hit))
        {

            if (hit.transform.CompareTag("Monster") || hit.transform.CompareTag("Summoner"))
            {
                Unit tempUnit = hit.transform.GetComponent<Unit>();
                Transform tempTransform = _unitStatusObj.transform.GetChild(1);
                for(int i = 0; i < tempUnit._State.Length; i++)
                {
                    if (tempUnit._State[i])
                        tempTransform.GetChild(i).GetChild(0).GetComponent<Text>().color = Color.black;
                    else
                        tempTransform.GetChild(i).GetChild(0).GetComponent<Text>().color = Color.white;
                }
                
                //_unitUP.gameObject.SetActive(true);
                //_unitUP.value = (float)tempUnit._Health / tempUnit._MaxHealth;


                _unitStatusObj.SetActive(true);
                _hpCircle.fillAmount = (float)((float)tempUnit._MaxHealth - tempUnit._Health) / tempUnit._MaxHealth;
                
                _unitStatusObj.transform.position = Input.mousePosition +
                    _unitStatusObj.GetComponent<RectTransform>().rect.width/2 * Vector3.left +
                    _unitStatusObj.GetComponent<RectTransform>().rect.height / 2 * Vector3.up;

                if (_unitStatusObj.transform.position.x < _unitStatusObj.GetComponent<RectTransform>().rect.width)
                {
                    _unitStatusObj.transform.position += _unitStatusObj.GetComponent<RectTransform>().rect.width * Vector3.right;
                }

                if (_unitStatusObj.transform.position.y + _unitStatusObj.GetComponent<RectTransform>().rect.height > Screen.height)
                {
                    _unitStatusObj.transform.position += _unitStatusObj.GetComponent<RectTransform>().rect.height * Vector3.down;
                }

            }
            else
            {
                //_unitUP.gameObject.SetActive(false);

                _unitStatusObj.SetActive(false);
            }
        }
    }
}
