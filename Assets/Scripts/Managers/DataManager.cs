using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [SerializeField] private List<BuildingData> buildingDatasToBeInitialized = new List<BuildingData>();

    public void InitTheProductionMenu()
    {
        UIManager.Instance.GetProductionMenuController().Init(buildingDatasToBeInitialized);
    }
}
