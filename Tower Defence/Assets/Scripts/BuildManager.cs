﻿using System.Collections.Generic;
using Turrets.Blueprints;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    private void Awake()
    {
        // Make sure we only ever have one BuildManager
        if (instance != null)
        {
            Debug.LogError("More than one build manager in scene!");
            return;
        }
        instance = this;
    }
    
    [Tooltip("The effect spawned when a turret is built.")]
    public GameObject buildEffect;
    [Tooltip("The effect spawned when a turret is sold")]
    public GameObject sellEffect;

    private TurretBlueprint _turretToBuild;
    private GameObject _buildingButton;
    private Node _selectedNode;
    
    [Tooltip("The UI to move above the turret")]
    public NodeUI nodeUI;

    public GraphicRaycaster screenRaycaster;
    public EventSystem eventSystem;

    public bool CanBuild => _turretToBuild != null;

    // Used to set the turret we want to build.
    public void SelectTurretToBuild(TurretBlueprint turret, GameObject buttonToDelete)
    {
        _turretToBuild = turret;
        _buildingButton = buttonToDelete;
        DeselectNode();
    }

    public void BuiltTurret()
    {
        Destroy(_buildingButton);
        Deselect();
    }
    
    // Get's the current turret we want to build
    public TurretBlueprint GetTurretToBuild()
    {
        return _turretToBuild;
    }
    
    // Set's the selected node so we can move the NodeUI
    public void SelectNode(Node node)
    {
        if (_selectedNode == node)
        {
            DeselectNode();
            return;
        }
        _selectedNode = node;
        _turretToBuild = null;

        nodeUI.SetTarget(node);
    }
    
    // Deselects the node
    public void DeselectNode()
    {
        _selectedNode = null;
        nodeUI.Hide();
    }

    private void Deselect()
    {
        DeselectNode();
        _turretToBuild = null;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }
        
        //Set up the new Pointer Event
        var pointerEventData = new PointerEventData(eventSystem)
        {
            //Set the Pointer Event Position to that of the mouse position
            position = Input.mousePosition
        };

        //Create a list of Raycast Results
        var results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        screenRaycaster.Raycast(pointerEventData, results);

        var cam = Camera.main;
        if (cam is { })
        {
            var origin = cam.ScreenToViewportPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(origin, Vector3.forward, 100);
            if (hit || results.Count != 0) return;
        }

        Deselect();
    }
}
