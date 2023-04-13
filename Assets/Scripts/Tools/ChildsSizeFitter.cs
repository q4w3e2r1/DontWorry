using Mono.Data.Sqlite;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChildsSizeFitter : MonoBehaviour
{
    [SerializeField] private RectTransform parent;
    [SerializeField] private RectTransform[] childs;

    public void Update()
    {
        var width = Vector2.zero;
        foreach (var child in childs)
            width += child.sizeDelta;

        width.x += childs.Length * 5 + 20;
        parent.GetComponent<RectTransform>().sizeDelta = width;
    }
}
