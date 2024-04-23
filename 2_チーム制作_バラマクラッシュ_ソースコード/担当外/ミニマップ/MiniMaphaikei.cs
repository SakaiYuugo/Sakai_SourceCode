using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMaphaikei : MonoBehaviour
{
    [SerializeField] private GameObject TargetCamera;//�Ǐ]�Ώۃv���C���[
    Vector3 Minihaikei;//�~�j�}�b�v�J�����̍��W
    Vector3 eulerAngles; // ���[�J���ϐ��Ɋi�[
    // Start is called before the first frame update
    void Start()
    {
        Minihaikei = this.transform.position;
        Minihaikei.y = -10.0f;//�Ƃ肠�������ɔz�u����
        eulerAngles = this.transform.eulerAngles; // ���[�J���ϐ��Ɋi�[�����l���㏑��
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Minihaikei.x = TargetCamera.transform.position.x;
        Minihaikei.z = TargetCamera.transform.position.z;
        this.transform.position = Minihaikei;
        eulerAngles = TargetCamera.transform.eulerAngles; // ���[�J���ϐ��Ɋi�[�����l���㏑��
        eulerAngles.x = 0.0f;
        this.transform.eulerAngles = eulerAngles;
    }
}
