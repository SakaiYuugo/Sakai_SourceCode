using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BombBase : MonoBehaviour
{
	[SerializeField] AudioClip clip;

	static MiniMapIconCreate iconCreate;

	
	public static int sphereLevel { get; private set; }

	public static int cubeLevel { get; private set; }

	public static int triangleLevel { get; private set; }

	public static int cylinderLevel { get; private set; }


	public static void SetLevel(ChoiceObject.StrewType bombType, int level)
	{
		switch (bombType)
		{
		case ChoiceObject.StrewType.SPHERE:
		{ sphereLevel = level; }
		break;

		case ChoiceObject.StrewType.CUBE:
		{ cubeLevel = level; }
		break;

		case ChoiceObject.StrewType.TRIANGLE:
		{ triangleLevel = level; }
		break;

		case ChoiceObject.StrewType.CYLINDER:
		{ cylinderLevel = level; }
		break;
		}
	}

	
	private void Start()
	{
		if (iconCreate == null)
		{
			iconCreate = GameObject.Find("MiniMapManager").GetComponent<MiniMapIconCreate>();
		}

		iconCreate.BombIconCreate(this.gameObject);
	}


	private void FixedUpdate()
	{
		if (this.transform.position.y < -100) { Destroy(this.gameObject); }
	}


	public virtual void BeforeDestroy(GameObject explosion)
	{
		explosion.GetComponent<AudioSource>().clip = clip;
	}
}
