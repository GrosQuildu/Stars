﻿using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class Star : MonoBehaviour
{

    private HexGrid grid;
    public HexCoordinates Coordinates { get; set; }
    private UIHoverListener uiListener;

    void Awake()
    {
        grid = (GameObject.Find("HexGrid").GetComponent<HexGrid>());
        UpdateCoordinates();
        uiListener = GameObject.Find("Canvas").GetComponent<UIHoverListener>();
    }

    void UpdateCoordinates()
    {
        Coordinates = HexCoordinates.FromPosition(gameObject.transform.position);
        if (grid.FromCoordinates(Coordinates) != null) transform.position = grid.FromCoordinates(Coordinates).transform.localPosition; //Snap object to hex
        if (grid.FromCoordinates(Coordinates) != null) grid.FromCoordinates(Coordinates).AssignObject(this.gameObject);
        //Debug.Log(grid.FromCoordinates(Coordinates).transform.localPosition.ToString() + '\n' + Coordinates.ToString());
    }

    void Update()
    {

    }

    private void OnMouseDown()
    {
        if (!uiListener.IsUIOverride) EventManager.selectionManager.SelectedObject = this.gameObject;
    }
}
