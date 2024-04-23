using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SelectBomb : MonoBehaviour
{
	[SerializeField] float distance;
	[SerializeField] float rotationSpeed;

	Vector3 basePos;
	Vector3 randomVector;

	float randomTime;


	private void Start()
	{
		basePos = this.transform.position;
		randomTime = Time.time + Random.Range(-10f, 10f);
		randomVector = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
	}

	private void FixedUpdate()
	{
		this.transform.position = new Vector3(basePos.x, basePos.y + (Mathf.Cos(randomTime) * distance), basePos.z);
		this.transform.Rotate(randomVector * rotationSpeed);

		randomTime += Time.deltaTime;
	}
}
