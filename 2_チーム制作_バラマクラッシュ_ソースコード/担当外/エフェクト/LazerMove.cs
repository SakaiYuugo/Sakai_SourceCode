using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerMove : EffectMove
{
    private GameObject Target;

    private Vector3 TargetPos;

    // Start is called before the first frame update
    override protected void Start()
    {
        Target = GameObject.Find("Player");
        TargetPos = Target.transform.position;
    }

    // Update is called once per frame
    override protected void FixedUpdate()
    {
        // �Ώە��ƃG�t�F�N�g�������W����x�N�g�����Z�o
        Vector3 vector3 = TargetPos - gameObject.transform.position;
        // Quaternion(��]�l)���擾
        Quaternion quaternion = Quaternion.LookRotation(vector3);
        gameObject.transform.rotation = quaternion;
    }
}
