using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCamera : MonoBehaviour
{

    [SerializeField] private GameObject TargetPlayer;//�Ǐ]�Ώۃv���C���[
    Vector3 MiniCamera;//�~�j�}�b�v�J�����̍��W
    Vector3 eulerAngles; // ���[�J���ϐ��Ɋi�[
    public GameObject PlayerCamObj;//���C���J�����I�u�W�F�N�g�Ƃ肽���񂶂�

    // Use this for initialization
    void Start()
    {
        MiniCamera = this.transform.position;
        MiniCamera.y = 300.0f;//�Ƃ肠������ɔz�u����
        eulerAngles = this.transform.eulerAngles; // ���[�J���ϐ��Ɋi�[�����l���㏑��
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerCamObj = GameObject.FindWithTag("MainCamera");
        MiniCamera.x = TargetPlayer.transform.position.x;
        MiniCamera.z = TargetPlayer.transform.position.z;
        this.transform.position = MiniCamera;
        eulerAngles = PlayerCamObj.transform.eulerAngles; // ���[�J���ϐ��Ɋi�[�����l���㏑��
        eulerAngles.x = 90.0f;
        this.transform.eulerAngles = eulerAngles;
    }

    public void SetPlayerCam(GameObject PlayerCam)
    {
        PlayerCamObj = PlayerCam;
    }
}
