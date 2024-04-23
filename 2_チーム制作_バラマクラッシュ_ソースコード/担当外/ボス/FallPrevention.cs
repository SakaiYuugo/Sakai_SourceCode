using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPrevention : MonoBehaviour
{
    //---- �n�ʂ�艺���ɍs���Ȃ��悤�ɂ���X�N���v�g ---- 
    //��BoxCollider�������̂ɃA�^�b�`����
    //�����蔲����h�~����p�̃X�N���v�g�ŁA���ɉ��ɂ���ꍇ�͋@�\���Ȃ��݌v
    //���C�̌�����ς���ꍇ�̓��C�̃T�C�Y�̌v�Z�����ς��Ȃ���΂����Ȃ�
    //�ڕW�^�O��ς����背�C�̕�����^���łȂ�������΂��낢�뉞�p���������m��Ȃ�

    BoxCollider _Collider; //�����̃R���C�_�[�T�C�Y�擾�p

    void Start()
    {
        _Collider = GetComponent<BoxCollider>();
    }

    void FixedUpdate()
    {
        //���C�̈ʒu�A�������v�Z
        float Range = _Collider.size.y + _Collider.center.y; //�R���C�_�[�̓����烌�C���΂��B���܂ł̒���
        float rayPosY = transform.position.y + _Collider.size.y + _Collider.center.y;
        Vector3 RayPos = new Vector3(transform.position.x, rayPosY, transform.position.z);

        //���C���΂��A�����������̂��ׂĂ��擾
        Ray ray = new Ray(RayPos, Vector3.down);
        foreach (RaycastHit hit in Physics.RaycastAll(ray, Range))
        {
            //�����������̂��n�ʂ�������
            if (hit.collider.gameObject.tag == "Ground")
            {
                float PosY = transform.position.y + Range - hit.distance;   //�n�ʂ̏�ɗ��Ă�Y���W���v�Z
                transform.position = new Vector3(transform.position.x, PosY, transform.position.z); //���W���X�V
            }
        }
    }
}
