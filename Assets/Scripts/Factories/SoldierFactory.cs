using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierFactory : FactoryBase
{
    [SerializeField] private GameObject soldierPrefab;

    public override BaseActor GetActor(ActorData data)
    {
        GameObject go = GetObjectFromPool(soldierPrefab, data.poolType);
        SoldierBase soldierBase = go.GetComponent<SoldierBase>();
        soldierBase.Init(data);
        return soldierBase;
    }

}
