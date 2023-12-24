using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private List<BuildingData> buildingDatasToBeInitialized = new List<BuildingData>();
    [SerializeField] private List<SoldierData> soldierDatas = new List<SoldierData>();

    public void InitTheProductionMenu()
    {
        UIManager.Instance.GetProductionMenuController().Init(buildingDatasToBeInitialized);
    }

    public SoldierData GetSoldierData(SoldierType soldierType)
    {
        for (int i = 0; i < soldierDatas.Count; i++)
        {
            if (soldierDatas[i].soldierType == soldierType)
                return soldierDatas[i];
        }
        return soldierDatas[0];
    }
}
