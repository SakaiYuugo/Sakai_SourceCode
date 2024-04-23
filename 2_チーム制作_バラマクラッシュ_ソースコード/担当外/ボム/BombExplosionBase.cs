using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class BombExplosionBase : MonoBehaviour
{
	protected BombFlash flashMan;
	protected bool explosionable = false;

	protected GameObject objExplosion { get; }



	private void Awake()
	{
		flashMan = this.GetComponent<BombFlash>();
	}


	protected void Explosion()
	{
		GameObject objExplosion = ExplosionCreater.Create(this.transform.position, Quaternion.identity);

		VisualEffect vfx = objExplosion.transform.Find("Effect").GetComponent<VisualEffect>();
		
		switch (this.name.Substring(0, this.name.IndexOf("(Clone)")))
		{
		case "Bomb_Sphere":		{ vfx.SetVector4("Color", Color.red); }		break;
		case "Bomb_Cylinder":	{ vfx.SetVector4("Color", Color.green); }	break;
		case "Bomb_Triangle":	{ vfx.SetVector4("Color", Color.yellow); }	break;
		case "Bomb_Cube":		{ vfx.SetVector4("Color", Color.cyan); }	break;
		case "Bomb_CubeMini":	{ vfx.SetVector4("Color", Color.cyan); }	break;
		}
		
		this.gameObject.GetComponent<BombBase>().BeforeDestroy(objExplosion);

		vfx.Play();

		Destroy(this.gameObject);
	}
}
