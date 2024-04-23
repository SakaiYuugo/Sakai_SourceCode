using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;


public class NaturalObjectRandomCreate : MonoBehaviour
{
	[SerializeField] int divisionX;
	[SerializeField] int divisionZ;
	[SerializeField] [InspectorName("1セクションにおけるオブジェクトの数")] int sectionOfObject;

	[Space]

	[SerializeField] List<GameObject> environments;

	GameObject ground;
	


    void Start()
    {
		Random.InitState(DateTime.Now.Millisecond);
		
		ground = GameObject.Find("Ground");

		//Terrainの大きさ
		Vector3 terrainScale = ground.GetComponent<Terrain>().terrainData.size;

		//1マスの大きさを計算
		Vector3 scaleSection = terrainScale;
		scaleSection.x = scaleSection.x / divisionX;
		scaleSection.z = scaleSection.z / divisionZ;

		//マスの最初の場所を指定
		Vector3 posSection = ground.transform.position;
		posSection.x = ground.transform.position.x;
		posSection.z = ground.transform.position.z;


		for (int i = 0; i < divisionZ; i++)
		{
			for (int j = 0; j < divisionX; j++)
			{
				for (int k = 0; k < sectionOfObject; k++)
				{
					//マス内のXZランダム座標を取得
					float randomPosX = Random.Range(posSection.x - scaleSection.x, posSection.x + scaleSection.x);
					float randomPosZ = Random.Range(posSection.z - scaleSection.z, posSection.z + scaleSection.z);
					float posY = posSection.y + 1000f;
					Vector3 randomPos = new Vector3(randomPosX, posY, randomPosZ);
					
					//真下の地面を探し、地面があればオブジェクト生成
					foreach (RaycastHit hit in Physics.RaycastAll(randomPos, Vector3.down, float.PositiveInfinity))
					{
						if (Vector3.Distance(System_ObjectManager.playerObject.transform.position, hit.point) < 20f) { continue; }

						if (hit.collider.name == "Ground" && !hit.collider.name.Contains("Wall"))
						{
							//自然物リストに入ってる中からランダムにオブジェクトを生成
							float RandomAngle = Random.Range(0.0f, 360.0f);
							GameObject environment = Instantiate(environments[Random.Range(0, environments.Count)], randomPos, Quaternion.AngleAxis(RandomAngle,Vector3.up));
							environment.transform.parent = this.transform;

							//Y座標を移動
							float environmentPositionY = hit.point.y + environment.transform.localScale.y / 4;
							environment.transform.position = new Vector3(environment.transform.position.x, environmentPositionY, environment.transform.position.z);
						}
					}
				}
				
				//マスのX座標を右に移動
				posSection.x += scaleSection.x;
			}

			//X座標を一番左に戻す
			posSection.x = ground.transform.position.x;

			//Z座標を一個下に移動
			posSection.z += scaleSection.z;
		}

		Destroy(this);
    }
}
