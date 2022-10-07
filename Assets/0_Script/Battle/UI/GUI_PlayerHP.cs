using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 배틀 씬의 플레이어 체력 표시
public class GUI_PlayerHP : Singleton<GUI_PlayerHP>
{
    
    [SerializeField]
    Slider _sliderHP;

    Unit _player = null;
  
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
