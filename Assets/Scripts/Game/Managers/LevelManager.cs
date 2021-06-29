using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelManager : MonoBehaviour
{
	public static LevelManager _i;

	public enum ItemTag
	{
		Item,
		BeatMark
	}

	[Serializable]
	public class ItemToSpawn
	{
		public ItemTag tag;
		public float spawnAtBeat;
		public int startId;
	}

	[Header("Items To Spawn")]
	[Space(10)]
	public List<ItemToSpawn> itemToSpawnList = new List<ItemToSpawn>();

	private void Awake() => _i = this;

	private void Start()
	{
		int step = 0;

		for (int i = 0; i < 350; i++)
		{
			ItemToSpawn item = new ItemToSpawn();

			item.tag = ItemTag.Item;
			if (i != 0 && i % 10 == 0)
				step += 5;
			item.spawnAtBeat = i + 1 + step;
			item.startId = UnityEngine.Random.Range(0, Conductor._i.pathList.Count);
			itemToSpawnList.Add(item);
		}
	}
}
