using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
	public static PoolManager _i;

	[HideInInspector]
	public List<GameObject> pooledObjects;

	private void Awake() => _i = this;
	
	private void Start()
	{
		pooledObjects = new List<GameObject>();
		foreach (ObjectPoolItem item in AssetsManager.sharedInstance.itemsToPool)
		{
			for (int i = 0; i < item.amountToPool; i++)
			{
				GameObject obj = Instantiate(item.objectToPool);
				obj.SetActive(false);
				obj.transform.parent = transform;
				pooledObjects.Add(obj);
			}
		}
	}

	public GameObject GetPooledObject(string tag)
	{
		for (int i = 0; i < pooledObjects.Count; i++)
		{
			if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag)
				return pooledObjects[i];
		}
		foreach (ObjectPoolItem item in AssetsManager.sharedInstance.itemsToPool)
		{
			if (item.objectToPool.tag == tag)
			{
				if (item.shouldExpand)
				{
					GameObject obj = Instantiate(item.objectToPool);
					obj.SetActive(false);
					pooledObjects.Add(obj);
					obj.transform.parent = transform;
					return obj;
				}
			}
		}
		Debug.LogError("PooledObject" + tag + " not found");
		return null;
	}

	public void ReturnToPoolParentage(GameObject obj)
	{
		if (obj != null)
		{
			obj.gameObject.SetActive(false);
			obj.transform.parent = transform;
		}
	}
}
