using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    [SerializeField] private GameObject buildingPrefab;
    [SerializeField] private GameObject soldierPrefab;
    [SerializeField] private GameObject buildingButtonPrefab;
    [SerializeField] private GameObject soldierButtonPrefab;

    public BuildingBase GetBuildingBase()
    {
        GameObject go = Instantiate(buildingPrefab);
        return go.GetComponent<BuildingBase>();
    }

    public SoldierBase GetSoldierBase()
    {
        GameObject go = Instantiate(soldierPrefab);
        return go.GetComponent<SoldierBase>();
    }

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
