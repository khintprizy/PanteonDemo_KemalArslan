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
    public SoldierData[] producableSoldiers;
}

[Serializable]
public class SoldierData : ActorData
{
    public float attackPower;
    public float attackSpeed;
    public float movementSpeed;
}
