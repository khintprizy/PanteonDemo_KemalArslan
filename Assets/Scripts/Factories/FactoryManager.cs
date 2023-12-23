using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryManager : SingletonMaker<FactoryManager>
{
    [SerializeField] private BuildingFactory buildingFactory;
    [SerializeField] private SoldierFactory soldierFactory;

    public BuildingFactory BuildingFactory { get => buildingFactory; }
    public SoldierFactory SoldierFactory { get => soldierFactory; }

}
