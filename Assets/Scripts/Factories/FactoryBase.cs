using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FactoryBase : MonoBehaviour
{
    /// <summary>
    /// Returns actor from the factory from pool system
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public abstract BaseActor GetActor(ActorData data);


    // Different types of prefabs have different pool list
    [SerializeField] private List<PoolListData> poolListDatas;

    [SerializeField] private int instantiateCount;

    protected GameObject CreateNewObject(GameObject prefab, PoolType poolType)
    {
        GameObject go = Instantiate(prefab);
        GetPoolListData(poolType).poolList.Add(go);
        go.GetComponent<IPooledObject>().OnObjectInstantiate();
        go.SetActive(false);
        return go;
    }

    public GameObject GetObjectFromPool(GameObject prefab, PoolType poolType)
    {
        if (GetPoolListData(poolType).poolList.Count < 1)
        {
            for (int i = 0; i < instantiateCount; i++)
            {
                CreateNewObject(prefab, poolType);
            }
        }

        GameObject obj = GetPoolListData(poolType).poolList[0];
        GetPoolListData(poolType).poolList.Remove(obj);
        obj.GetComponent<IPooledObject>().OnObjectGetFromPool();
        obj.SetActive(true);
        return obj;
    }

    public void SendObjectToPool(GameObject pooledObject, PoolType poolType)
    {
        pooledObject.GetComponent<IPooledObject>().OnObjectSendToPool();
        pooledObject.SetActive(false);
        GetPoolListData(poolType).poolList.Add(pooledObject);
    }

    private PoolListData GetPoolListData(PoolType poolType)
    {
        for (int i = 0; i < poolListDatas.Count; i++)
        {
            if (poolType == poolListDatas[i].poolType)
                return poolListDatas[i];
        }
        return poolListDatas[0];
    }
}
