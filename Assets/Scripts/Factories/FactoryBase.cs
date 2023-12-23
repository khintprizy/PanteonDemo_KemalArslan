using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FactoryBase : MonoBehaviour
{
    public abstract BaseActor GetActor(ActorData data);
}
