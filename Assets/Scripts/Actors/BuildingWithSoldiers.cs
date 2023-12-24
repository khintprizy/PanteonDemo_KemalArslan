using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingWithSoldiers : BuildingBase
{
    [SerializeField] private List<SoldierType> producableSoldierTypes = new List<SoldierType>();

    public override void OnActorClickedOnBoard()
    {
        base.OnActorClickedOnBoard();

        DataManager dataManager = gameManagers.DataManager;


        SoldierData[] soldierDatas = new SoldierData[producableSoldierTypes.Count];

        for (int i = 0; i < producableSoldierTypes.Count; i++)
        {
            soldierDatas[i] = dataManager.GetSoldierData(producableSoldierTypes[i]);
        }

        UIManager.Instance.GetInformationMenuController().SetSoldiers(soldierDatas);
    }
}
