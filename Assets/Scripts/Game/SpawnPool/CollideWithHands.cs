using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollideWithHands : MonoBehaviour
{
	public LayerMask layer;

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.CompareTag("Hand"))
		{
			LayerMask colLayer = collision.gameObject.layer;

			if ((layer & 1 << colLayer) == 1 << colLayer)
			{

			}
		}
	}
}
