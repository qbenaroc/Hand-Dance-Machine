using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderEnabler : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Item")
		{
			Item item = GetComponent<Item>();
			Renderer itemMeshRenderer = other.GetComponent<MeshRenderer>();

			itemMeshRenderer.enabled = true;
		}
	}
}