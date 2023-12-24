using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    [SerializeField] private GameObject buildingButtonPrefab;
    [SerializeField] private GameObject soldierButtonPrefab;

    public BuildingButton GetBuildingButton(Transform parent)
    {
        GameObject go = Instantiate (buildingButtonPrefab, parent);
        return go.GetComponent<BuildingButton>();
    }

    public SoldierButton GetSoldierButton(Transform parent)
    {
        GameObject go = Instantiate(soldierButtonPrefab, parent);
        return go.GetComponent<SoldierButton>();
    }
}


