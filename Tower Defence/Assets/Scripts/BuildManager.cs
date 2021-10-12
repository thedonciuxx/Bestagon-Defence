﻿using Turrets;
using Turrets.Blueprints;
using UI;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager instance;

    void Awake()
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
        
        var cam = Camera.main;
        var origin = cam.ScreenToViewportPoint(Input.mousePosition);
        var hit = Physics2D.Raycast(origin, Vector3.forward, 100);
        if (hit) return;
        
        Deselect();
    }
}
