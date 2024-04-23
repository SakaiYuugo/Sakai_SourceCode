#define POSITION_CONSTRAINT
#undef PARENT_CONSTRAINT

using UnityEngine;
using UnityEngine.Animations; // ���ꂪ�Ȃ��� Constraint�͎g���Ȃ�

public class MiniMapIconCreate : MonoBehaviour
{
    //==============�ϐ��錾=======================
    [SerializeField] GameObject MiniMapIcon_Bomb;//�{���A�C�R�������p�@
    [SerializeField] GameObject MiniMapIcon_Boss;//�{�X�A�C�R��
    [SerializeField] GameObject MiniMapIcon_Enemy;//�G������
    [SerializeField] GameObject MiniMapIcon_Player;//player
    [SerializeField] GameObject MiniMapIcon_Item;//�����Ă�����
    [SerializeField] GameObject MiniMapIcon_Cabe;//�����Ă�����

    [SerializeField] GameObject Boss;//��������ꏊ�����߂�Ώۂ̃{�X
    [SerializeField] GameObject Player;//��������ꏊ�����߂�Ώۂ̃v���C���[
    [SerializeField] GameObject Cave;//��������ꏊ�����߂�Ώۂ̓��A

    GameObject MiniMapIcon_Obj;
    GameObject MapIcon_Obj;
    PositionConstraint myPositionConstraint;    // Position Type
    BombPosition BombPosition_script;
    CavePosition CavePosition_script;
    ConstraintSource myConstraintSource;        // ConstarintSource
    Transform childTransform;
    GameObject childObject;

    Triangle Triangle_script;

    int childCount;

    //==============�֐�===================

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

    //�~�j�}�b�v�A�C�R���C���X�^���X��GameObject��Ԃ��֐�
    GameObject CreateMiniMapIcon(GameObject MinimapIcon,Transform Target_Obj)
    {
        MiniMapIcon_Obj = Instantiate(MinimapIcon, Target_Obj.position, Quaternion.identity);
        return MiniMapIcon_Obj;
    }
    // Use this for initialization
    void Start()
    {
        // �q�I�u�W�F�N�g�̐����擾
        childCount = Cave.transform.childCount;
        // �q�I�u�W�F�N�g�����Ɏ擾����
        for (int i = 0; i < childCount; i++)
        {
            childTransform = Cave.transform.GetChild(i);
            childObject = childTransform.gameObject;

            // �擾�����q�I�u�W�F�N�g����������
            MapIcon_Obj = CreateMiniMapIcon(MiniMapIcon_Cabe, childObject.transform);
            CavePosition_script = MapIcon_Obj.GetComponent<CavePosition>();
            CavePosition_script.SetGameObj(childObject);
            myPositionConstraint = MapIcon_Obj.GetComponent<PositionConstraint>();
            SetObject2ConstraintSource(childTransform.transform, myPositionConstraint);
        }
        BombIconCreate(Player);
        BombIconCreate(Boss);
    }
        //==============��������p�u���b�N�֐�=================

        //�{���~�j�}�b�v�A�C�R������
        public void BombIconCreate(GameObject Bomb)
    {
        switch (Bomb.tag)
        {
            case "Player":
                MapIcon_Obj = CreateMiniMapIcon(MiniMapIcon_Player, Bomb.transform);

                Triangle_script = MapIcon_Obj.GetComponent<Triangle>();
                Triangle_script.SetGameObj(Bomb);
                //myPositionConstraint = MapIcon_Obj.GetComponent<PositionConstraint>();
                //SetObject2ConstraintSource(Bomb.transform, myPositionConstraint);
                break;
            case "Enemy":
                MapIcon_Obj = CreateMiniMapIcon(MiniMapIcon_Enemy, Bomb.transform);


                myPositionConstraint = MapIcon_Obj.GetComponent<PositionConstraint>();
                SetObject2ConstraintSource(Bomb.transform, myPositionConstraint);
                break;
            case "Bomb":
                MapIcon_Obj = CreateMiniMapIcon(MiniMapIcon_Bomb, Bomb.transform);
                BombPosition_script = MapIcon_Obj.GetComponent<BombPosition>();
                BombPosition_script.SetGameObj(Bomb);
                myPositionConstraint = MapIcon_Obj.GetComponent<PositionConstraint>();
                SetObject2ConstraintSource(Bomb.transform, myPositionConstraint);
                break;
            case "Item":
                MapIcon_Obj = CreateMiniMapIcon(MiniMapIcon_Item, Bomb.transform);


                myPositionConstraint = MapIcon_Obj.GetComponent<PositionConstraint>();
                SetObject2ConstraintSource(Bomb.transform, myPositionConstraint);
                break;
            case "Boss":
                if (Bomb.name == "Boss_Centipede")
                {

                    // �q�I�u�W�F�N�g�̐����擾
                    childCount = Bomb.transform.childCount;
                    // �q�I�u�W�F�N�g�����Ɏ擾����
                    for (int i = 0; i < childCount - 1; i++)
                    {
                        childTransform = Bomb.transform.GetChild(i);
                        childObject = childTransform.gameObject;

                        // �擾�����q�I�u�W�F�N�g����������
                        MapIcon_Obj = CreateMiniMapIcon(MiniMapIcon_Boss, childObject.transform);
                        myPositionConstraint = MapIcon_Obj.GetComponent<PositionConstraint>();
                        SetObject2ConstraintSource(childTransform.transform, myPositionConstraint);
                    }
                }
                else
                {
                    MapIcon_Obj = CreateMiniMapIcon(MiniMapIcon_Boss, Bomb.transform);
                    myPositionConstraint = MapIcon_Obj.GetComponent<PositionConstraint>();
                    SetObject2ConstraintSource(Bomb.transform, myPositionConstraint);
                }

                break;


            default:
                break;
        }
    }
}
