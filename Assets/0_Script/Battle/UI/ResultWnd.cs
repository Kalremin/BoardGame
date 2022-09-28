using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultWnd : MonoBehaviour
{
    [SerializeField]
    Text _txtVictory, _txtDefeat;

    private void OnEnable()
    {
        

        if (PlayerData._instance.GetHealth() > 0)
        {
            _txtVictory.gameObject.SetActive(true);
            _txtDefeat.gameObject.SetActive(false);
        }
        else
        {
            _txtVictory.gameObject.SetActive(false);
            _txtDefeat.gameObject.SetActive(true);
        }
    }
}
