using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIcon : MonoBehaviour
{
    [SerializeField] private GameObject PlayerMiniMapIcon;//�����������I�u�W�F�N�g

    [SerializeField] private Transform Player;//��������ꏊ�����߂�Ώۂ̃v���C���[

    // Use this for initialization
    void Start()
    {
        Instantiate(PlayerMiniMapIcon, Player.transform.position, Quaternion.identity, Player);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
