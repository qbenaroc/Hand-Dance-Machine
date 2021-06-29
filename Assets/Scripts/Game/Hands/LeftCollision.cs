using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LeftHand))]
public class LeftCollision : MonoBehaviour
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
					LeftHand._i.ValidHaptics();
				}
				else if (item.score == item.lowScore)
				{
					LeftHand._i.HapticImpulse(0, 0.75f, 0.125f);
					LeftHand._i.HapticImpulse(0, 0.5f, 0.25f);
				}
				else if (item.score == 0)
				{
					LeftHand._i.NonValidHaptics();
				}
				item.StopGlow();
				item.Die();
			}
		}
	}
}