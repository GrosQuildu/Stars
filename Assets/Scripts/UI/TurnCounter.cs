﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts;
using UnityEngine.UI;
public class TurnCounter : MonoBehaviour
{

    Text label;
    Text _Text;

    void Awake()
    {
        //RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        label = gameObject.GetComponent<Text>();
    }

    void Update()
    {
        label.text = "Year: " + (GameController.GetYear() + 2400).ToString();
    }
}