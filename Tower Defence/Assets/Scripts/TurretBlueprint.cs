﻿using UnityEngine;

[System.Serializable]
public class TurretBlueprint
{
    [Tooltip("The prefab to use when the turret is built.")]
    public GameObject prefab;
    public int cost;

    public GameObject upgradedPrefab;
    public int upgradeCost;

    public int GetSellAmount()
    {
        return cost / 2;
    }
}
