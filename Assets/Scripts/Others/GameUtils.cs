using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ActorData
{
    public int actorWidth;
    public int actorHeight;
    public string actorName;
    public string actorDescription;
    public Color actorColor;
    public Sprite actorUISprite;
    public Sprite actorSprite;
    public float actorHealth;
    public PoolType poolType;
}

[Serializable]
public class BuildingData : ActorData
{
    public BuildingType buildingType;
}

[Serializable]
public class SoldierData : ActorData
{
    public float attackPower;
    public float attackSpeed;
    public float movementSpeed;
    public SoldierType soldierType;
}

public enum BuildingType
{
    BuildingNormal = 0,
    BuildingWithSoldiers = 1,
}

public enum SoldierType
{
    Soldier1 = 0,
    Soldier2 = 1,
    Soldier3 = 2,
}

public enum PoolType
{
    BuildingNormal = 0,
    BuildingWithSoldiers = 1,
    Soldier = 2,
}

[Serializable]
public struct PoolListData
{
    public PoolType poolType;
    public List<GameObject> poolList;
}

public interface IPooledObject
{
    void OnObjectGetFromPool();
    void OnObjectSendToPool();
    void OnObjectInstantiate();
}


