﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InkBar : MonoBehaviour {
    [SerializeField]
    private float fillerPercent;

    [SerializeField]
    private Image inkContent;

	// Use this for initialization
	void Start () {
        fillerPercent = 1;
	}
	
	// Update is called once per frame
	void Update () {
        UpdateBar();
	}

    private void UpdateBar()
    {
        fillerPercent = InkStatus.ink.GetInk() / InkStatus.ink.GetMaxInk();
        inkContent.fillAmount = fillerPercent;
    }
}
