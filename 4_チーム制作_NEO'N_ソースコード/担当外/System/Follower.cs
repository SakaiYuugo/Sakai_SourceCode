using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Follower : MonoBehaviour
{
    GameObject player;
    public Vector3 offset;

    public bool lerp = false;

    public float lerpTime = 0.07f;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>().gameObject;
    }


    private void FixedUpdate()
    {
        if (lerp == false)
        {
            Vector3 pos = new Vector3(0f, 0f, player.transform.position.z);
            transform.position = pos + offset;
        }
        else
        {
            //transform.LookAt(player.transform);
            //transform.position = new Vector3(player.transform.position.x,player.transform.position.y + 3.0f,)



            //transform.DOLocalMove(player.transform.position, 0.0f);
            //transform.DOMoveX( player.transform.position.x, lerpTime);
            //transform.DOMoveY( player.transform.position.y + 6.0f, 0.0f);
            //transform.DOMoveZ( player.transform.position.z - 9.5f, 0.0f);
            transform.DOMoveX(player.transform.position.x, lerpTime);
            transform.DOMoveY(player.transform.position.y + 6.0f, 0.0f);
            transform.DOMoveZ(player.transform.position.z - 9.5f, 0.0f);
        }

    }
}
