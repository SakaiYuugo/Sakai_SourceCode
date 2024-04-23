using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cave_LevelInstEnemy : MonoBehaviour
{
    [SerializeField]
    GameObject[] EnemyObjects;

    CaveManager manager;
    int EnemyInstNum;
    float InstCount;
    float BoostCount;

    // Start is called before the first frame update
    void Start()
    {
        manager = GetComponent<CaveManager>();
        EnemyInstNum = manager.GetInstEnemyNum();
        InstCount = Random.Range(0.0f, manager.GetInstTime());
        BoostCount = manager.GetBoostTime();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        //�G�̐���������
        if(InstCount <= 0.0f)
        {
            if(EnemyObjects.Length > 0)
            {
                //�G�𐔂�������
                for(int i = 0;i < EnemyInstNum;i++)
                {
                    int EnemyNum = Random.Range(0, EnemyObjects.Length);
                    Instantiate(EnemyObjects[EnemyNum], transform.position, transform.rotation);
                    if(false)//�G�̏���ɂȂ��Ă����
                    {
                        break;
                    }
                }
            }

            //�J�E���g�̍X�V
            if (BoostCount > 0.0f)
            {
                //�G�𐶐�����Ԋu�͒���
                InstCount = manager.GetInstTime();
            }
            else//�u�[�X�g���|�����Ă���
            {
                //�G�̐������銴�o�͒Z��
                InstCount = manager.GetBoostInstTime();
            }
        }

        if(BoostCount > 0.0f)
        {
            BoostCount -= Time.deltaTime;
        }
        InstCount -= Time.deltaTime;
    }

    public void SetEnemyNum(int num)
    {
        EnemyInstNum = num;
    }
}
