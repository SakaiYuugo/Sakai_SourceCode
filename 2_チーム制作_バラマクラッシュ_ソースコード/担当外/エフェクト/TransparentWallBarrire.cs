using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TransparentWallBarrire : MonoBehaviour
{
	[SerializeField] float maxDistance;
	[SerializeField] float minDistance;

	GameObject player;
	Material material;


	private void Start()
	{
		player = System_ObjectManager.playerObject;
		material = this.GetComponent<MeshRenderer>().material;

		material.SetFloat("_MaxDistance", maxDistance);
		material.SetFloat("_MinDistance", minDistance);
	}


	private void FixedUpdate()
	{
		material.SetVector("_PlayerPos", player.transform.position);
	}
}
