using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionMenuController : MonoBehaviour
{
    private PrefabManager prefabManager;

    [SerializeField] private InfSWController infSWController;

    public void Init(List<BuildingData> buildingsOnTheMenu)
    {
        prefabManager = GameManagers.Instance.PrefabManager;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < buildingsOnTheMenu.Count; j++)
            {
                BuildingButton button = prefabManager.GetBuildingButton(transform);
                button.Init(buildingsOnTheMenu[j]);
                infSWController.AddToButtons(button.GetComponent<RectTransform>());
            }
        }

        infSWController.SetToMiddle();
    }


}
