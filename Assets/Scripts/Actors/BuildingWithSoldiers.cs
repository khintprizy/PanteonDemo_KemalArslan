using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingWithSoldiers : BuildingBase
{
    [SerializeField] private SoldierData[] producableSoldiers;

    public override void OnActorClickedOnBoard()
    {
        base.OnActorClickedOnBoard();

        UIManager.Instance.GetInformationMenuController().SetSoldiers(producableSoldiers);
    }
}
