#define POSITION_CONSTRAINT
#undef PARENT_CONSTRAINT

using UnityEngine;
using UnityEngine.Animations; // ���ꂪ�Ȃ��� Constraint�͎g���Ȃ�

public class PositionManager : MonoBehaviour
{
    private ConstraintSource myConstraintSource;        // ConstarintSource
    GameObject Obj;
    [SerializeField] private GameObject BombMiniMapIconEm;//�����������I�u�W�F�N�g
    /// <summary>
    /// ConstraintComponent�̐e�I�u�W�F�N�g�𓮓I�ɒǉ�����֐�
    /// </summary>
    /// <param name="parent">Contraint�̐e�I�u�W�F�N�g</param>
    private void SetObject2ConstraintSource(Transform parent,PositionConstraint myPositionConstraint)
    {
        // Constraint�̎Q�ƌ���ݒ�(���̏�������ł�������ƒǔ�������������̂Œ���)
        myConstraintSource.sourceTransform = parent;
        myConstraintSource.weight = 1.0f; // �e���x�����S�x�z�ɂ���(Add�̏ꍇ��0�ɂȂ�̂�)
        myPositionConstraint.AddSource(myConstraintSource);   // Constraint�̎Q�ƌ���ǉ�
        myPositionConstraint.translationOffset = Vector3.zero;     // �I�t�Z�b�g��0��
        myPositionConstraint.enabled = true;                       // �L���ɂ���(�g��Ȃ��Ƃ���false)

    }

    public GameObject CreateMiniMapIcon(GameObject obj_MinimapIcon, PositionConstraint myPositionConstraint)
    {
        Obj = Instantiate(BombMiniMapIconEm, obj_MinimapIcon.transform.position, Quaternion.identity);
        Obj.transform.parent = obj_MinimapIcon.transform;
        SetObject2ConstraintSource(obj_MinimapIcon.transform, myPositionConstraint);
        return Obj;
    }

    bool GetChildren(GameObject obj)
    {
        Transform children = obj.GetComponentInChildren<Transform>();
        //�q�v�f�����Ȃ���ΏI��
        if (children.childCount == 0)
        {
            return false;
        }
        foreach (Transform ob in children)
        {
            if (ob.name.Contains("MinimapIcon"))
            {
                return true;
            }

        }
        return false;
    }

}

