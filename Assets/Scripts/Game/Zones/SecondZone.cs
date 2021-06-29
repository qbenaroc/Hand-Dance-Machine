using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondZone : MonoBehaviour
{
	[Header("Other Zones Layers")]
	[Tooltip("Used to set the score and get the Item component")]
	[SerializeField]
	private LayerMask firstZoneLayer;
	[SerializeField]
	private LayerMask thirdZoneLayer;

	private void OnTriggerEnter(Collider other)
	{
		LayerMask otherLayer = other.gameObject.layer;

		//Check if an object with the same layer is colliding
		if ((1 << gameObject.layer & 1 << otherLayer) == 1 << gameObject.layer)
		{
			//Set Score to Perfect
			Item item = other.GetComponent<Item>();
			
			item.score = item.perfectScore;
			//Glow item
			//item.Glow();
		}
		else if ((firstZoneLayer & 1 << otherLayer) == 1 << otherLayer)
		{
			//Set Score to the lowest
			Item item = other.GetComponent<Item>();
			
			item.score = item.lowScore;
			//item.StopGlow();
		}
		else if ((thirdZoneLayer & 1 << otherLayer) == 1 << otherLayer)
		{
			//Set Score to 0
			Item item = other.GetComponent<Item>();
			
			item.score = 0;
		}
	}
}
