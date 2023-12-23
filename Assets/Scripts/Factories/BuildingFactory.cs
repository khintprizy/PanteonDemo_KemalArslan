using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingFactory : FactoryBase
{
    [SerializeField] private List<BuildingPrefabData> buildingPrefabDatas = new List<BuildingPrefabData>();

    public override BaseActor GetActor(ActorData data)
    {
        BuildingData buildingData;

        // I cast the data to BuildingData
        buildingData = data as BuildingData; 

        GameObject go = Instantiate(GetThePrefab(buildingData.buildingType));
        BuildingBase buildingBase = go.GetComponent<BuildingBase>();
        buildingBase.Init(buildingData);
        return buildingBase;
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
