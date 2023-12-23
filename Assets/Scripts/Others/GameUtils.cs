using System;
using UnityEngine;

[Serializable]
public class ActorData
{
    public int actorWidth;
    public int actorHeight;
    public string actorName;
    public string actorDescription;
    public Color actorColor;
    public Sprite actorSprite;
    public float actorHealth;
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
}

public enum BuildingType
{
    BuildingNormal = 0,
    BuildingWithSoldiers = 1,
}

public interface IPooledObject
{
    void OnObjectGetFromPool();
    void OnObjectSendToPool();
    void OnObjectInstantiate();
}
