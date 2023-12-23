using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductionMenuController : MonoBehaviour
{
    [SerializeField] List<BuildingData> buildingsOnTheMenu;
    private PrefabManager prefabManager;

    public void Init(List<BuildingData> buildingsOnTheMenu)
    {
        prefabManager = GameManagers.Instance.PrefabManager;

        for (int i = 0; i < buildingsOnTheMenu.Count; i++)
        {
            BuildingButton button = prefabManager.GetBuildingButton(transform);
            button.Init(buildingsOnTheMenu[i]);
        }
    }
}
