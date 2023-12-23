using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    [SerializeField] private List<BuildingPrefabData> buildingPrefabDatas = new List<BuildingPrefabData>();

    //[SerializeField] private GameObject buildingPrefab;
    [SerializeField] private GameObject soldierPrefab;
    [SerializeField] private GameObject buildingButtonPrefab;
    [SerializeField] private GameObject soldierButtonPrefab;

    public BuildingBase GetBuildingBase(BuildingData buildingData)
    {
        GameObject go = Instantiate(GetThePrefab(buildingData.buildingType));
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

    private GameObject GetThePrefab(BuildingType buildingType)
    {
        for (int i = 0; i < buildingPrefabDatas.Count; i++)
        {
            if (buildingPrefabDatas[i].buildingType == buildingType)
                return buildingPrefabDatas[i].buildingPrefab;
        }
        return buildingPrefabDatas[0].buildingPrefab; ;
    }
}

[Serializable]
public struct BuildingPrefabData
{
    public GameObject buildingPrefab;
    public BuildingType buildingType;
}
