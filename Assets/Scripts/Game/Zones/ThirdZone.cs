using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdZone : MonoBehaviour
{
	[Header("Other Zones Layers")]
	[Tooltip("Used to set the score and get the Item Component")]
	[SerializeField] private LayerMask firstZoneLayer;
	[SerializeField] private LayerMask secondZoneLayer;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Item")
		{
			Item item = other.GetComponent<Item>();
			LayerMask otherLayer = other.gameObject.layer;

			//Check if an object with the same layer is colliding
			if ((1 << gameObject.layer & 1 << otherLayer) == 1 << otherLayer)
			{
				//Set Score to Perfect
				item.score = item.perfectScore;
				//Glow item
				item.Glow();
			}
			else if ((secondZoneLayer & 1 << otherLayer) == 1 << otherLayer)
			{
				//Set Score to the lowest
				item.score = item.lowScore;
				item.StopGlow();
			}
			else if ((firstZoneLayer & 1 << otherLayer) == 1 << otherLayer)
			{
				//Set Score to 0
				item.score = 0;
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Item")
		{
			Item item = other.GetComponent<Item>();

			//item.StopGlow();
		}
	}
}