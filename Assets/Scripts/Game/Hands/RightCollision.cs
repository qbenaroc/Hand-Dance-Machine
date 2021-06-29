using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RightHand))]
public class RightCollision : MonoBehaviour
{
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.gameObject.tag == "Item")
		{
			Item item = collision.gameObject.GetComponent<Item>();

			//Collision with force TRY
			//float collisionForce = collision.relativeVelocity.magnitude;
			//float collisionForce = collision.impulse.Magnitude / Time.fixedDeltaTime;
			//Debug.Log(collisionForce);

			if (item._activated)
			{
				if (item.score == item.perfectScore)
				{
					RightHand._i.ValidHaptics();
				}
				else if (item.score == item.lowScore)
				{
					RightHand._i.HapticImpulse(0, 0.75f, 0.125f);
					RightHand._i.HapticImpulse(0, 0.5f, 0.25f);
				}
				else if (item.score == 0)
				{
					RightHand._i.NonValidHaptics();
				}
				item.StopGlow();
				item.Die();
			}
		}
	}
}
