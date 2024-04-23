using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations; // ���ꂪ�Ȃ��� Constraint�͎g���Ȃ�

public class BossIcon : MonoBehaviour
{
    [SerializeField] private GameObject BossMiniMapIcon;//�����������I�u�W�F�N�g

    [SerializeField] private GameObject Boss;//��������ꏊ�����߂�Ώۂ̃{�X

    Transform childTransform;
    GameObject childObject;
    GameObject MapIcon_Obj;
    PositionConstraint myPositionConstraint;    // Position Type
    ConstraintSource myConstraintSource;        // ConstarintSource
    int childCount;
    // Use this for initialization
    void Start()
    {
        BossMiniMapIcon.transform.localScale = new Vector3(50.0f, 50.0f, 50.0f);
        if (Boss.name == "Boss_Centipede")
        {
           
            // �q�I�u�W�F�N�g�̐����擾
            childCount = Boss.transform.childCount;
            // �q�I�u�W�F�N�g�����Ɏ擾����
            for (int i = 0; i < childCount - 1; i++)
            {
                childTransform = Boss.transform.GetChild(i);
                childObject = childTransform.gameObject;

                // �擾�����q�I�u�W�F�N�g����������
                MapIcon_Obj = Instantiate(BossMiniMapIcon, childTransform.transform.position, Quaternion.identity);
                myPositionConstraint = MapIcon_Obj.GetComponent<PositionConstraint>();
                SetObject2ConstraintSource(childTransform.transform, myPositionConstraint);
            }
        }
        else
        {
            MapIcon_Obj = Instantiate(BossMiniMapIcon, Boss.transform.position, Quaternion.identity);
            myPositionConstraint = MapIcon_Obj.GetComponent<PositionConstraint>();
            SetObject2ConstraintSource(Boss.transform, myPositionConstraint);
        }
    }

    //�Ǐ]�ݒ�
    void SetObject2ConstraintSource(Transform parent, PositionConstraint myPositionConstraint)
    {
        // Constraint�̎Q�ƌ���ݒ�(���̏�������ł�������ƒǔ�������������̂Œ���)
        myConstraintSource.sourceTransform = parent;
        myConstraintSource.weight = 1.0f; // �e���x�����S�x�z�ɂ���(Add�̏ꍇ��0�ɂȂ�̂�)
        myPositionConstraint.AddSource(myConstraintSource);   // Constraint�̎Q�ƌ���ǉ�
        myPositionConstraint.translationOffset = Vector3.zero;     // �I�t�Z�b�g��0��
        myPositionConstraint.enabled = true;                       // �L���ɂ���(�g��Ȃ��Ƃ���false)

    }

    void Update()
    {

    }

}
