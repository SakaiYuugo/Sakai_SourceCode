using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Random = UnityEngine.Random;


public class NaturalObjectRandomCreate : MonoBehaviour
{
	[SerializeField] int divisionX;
	[SerializeField] int divisionZ;
	[SerializeField] [InspectorName("1�Z�N�V�����ɂ�����I�u�W�F�N�g�̐�")] int sectionOfObject;

	[Space]

	[SerializeField] List<GameObject> environments;

	GameObject ground;
	


    void Start()
    {
		Random.InitState(DateTime.Now.Millisecond);
		
		ground = GameObject.Find("Ground");

		//Terrain�̑傫��
		Vector3 terrainScale = ground.GetComponent<Terrain>().terrainData.size;

		//1�}�X�̑傫�����v�Z
		Vector3 scaleSection = terrainScale;
		scaleSection.x = scaleSection.x / divisionX;
		scaleSection.z = scaleSection.z / divisionZ;

		//�}�X�̍ŏ��̏ꏊ���w��
		Vector3 posSection = ground.transform.position;
		posSection.x = ground.transform.position.x;
		posSection.z = ground.transform.position.z;


		for (int i = 0; i < divisionZ; i++)
		{
			for (int j = 0; j < divisionX; j++)
			{
				for (int k = 0; k < sectionOfObject; k++)
				{
					//�}�X����XZ�����_�����W���擾
					float randomPosX = Random.Range(posSection.x - scaleSection.x, posSection.x + scaleSection.x);
					float randomPosZ = Random.Range(posSection.z - scaleSection.z, posSection.z + scaleSection.z);
					float posY = posSection.y + 1000f;
					Vector3 randomPos = new Vector3(randomPosX, posY, randomPosZ);
					
					//�^���̒n�ʂ�T���A�n�ʂ�����΃I�u�W�F�N�g����
					foreach (RaycastHit hit in Physics.RaycastAll(randomPos, Vector3.down, float.PositiveInfinity))
					{
						if (Vector3.Distance(System_ObjectManager.playerObject.transform.position, hit.point) < 20f) { continue; }

						if (hit.collider.name == "Ground" && !hit.collider.name.Contains("Wall"))
						{
							//���R�����X�g�ɓ����Ă钆���烉���_���ɃI�u�W�F�N�g�𐶐�
							float RandomAngle = Random.Range(0.0f, 360.0f);
							GameObject environment = Instantiate(environments[Random.Range(0, environments.Count)], randomPos, Quaternion.AngleAxis(RandomAngle,Vector3.up));
							environment.transform.parent = this.transform;

							//Y���W���ړ�
							float environmentPositionY = hit.point.y + environment.transform.localScale.y / 4;
							environment.transform.position = new Vector3(environment.transform.position.x, environmentPositionY, environment.transform.position.z);
						}
					}
				}
				
				//�}�X��X���W���E�Ɉړ�
				posSection.x += scaleSection.x;
			}

			//X���W����ԍ��ɖ߂�
			posSection.x = ground.transform.position.x;

			//Z���W������Ɉړ�
			posSection.z += scaleSection.z;
		}

		Destroy(this);
    }
}
