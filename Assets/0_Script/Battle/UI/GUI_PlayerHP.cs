using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUI_PlayerHP : MonoBehaviour
{
    public static GUI_PlayerHP _instance;

    [SerializeField]
    Slider _sliderHP;

    Unit _player = null;
    private void Awake()
    {
        _instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player != null)
        {
            _sliderHP.value = _player._Health;
        }

    }

    public void SetPlayer(Unit unit)
    {
        _player = unit;
        _sliderHP.minValue = 0f;
        _sliderHP.maxValue = _player._MaxHealth;
    }
}
