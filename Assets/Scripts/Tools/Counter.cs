using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Counter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _number;

    public void ModifyNumber(int changeValue)
    {
        var newNumber = int.Parse(_number.text) + changeValue;
        if (newNumber < 1)
            return;
        _number.text = (newNumber).ToString();
    }
}
