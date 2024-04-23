using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIcon : MonoBehaviour
{
    [SerializeField] private GameObject PlayerMiniMapIcon;//生成したいオブジェクト

    [SerializeField] private Transform Player;//生成する場所を決める対象のプレイヤー

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
