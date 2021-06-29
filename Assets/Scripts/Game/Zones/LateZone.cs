using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LateZone : MonoBehaviour
{
	[SerializeField]
	private LayerMask firstZoneLayer;
	[SerializeField]
	private LayerMask secondZoneLayer;
	[SerializeField]
	private LayerMask thirdZoneLayer;

	private void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == "Item")
		{
			LayerMask otherLayer = other.gameObject.layer;
			Item item = other.GetComponent<Item>();

			if (((firstZoneLayer & 1 << otherLayer) == 1 << otherLayer)
				|| ((secondZoneLayer & 1 << otherLayer) == 1 << otherLayer)
				|| ((thirdZoneLayer & 1 << otherLayer) == 1 << otherLayer))
			{
				item.score = 0;
				item.StopGlow();
				item.Die();
			}
		}
	}
}
