using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCentipede_Jump : MonoBehaviour
{
    Vector3 AddJumpVetor;
    bool isJump;

    // Start is called before the first frame update
    void Start()
    {
        AddJumpVetor = Vector3.zero;
        isJump = false;
    }

    private void FixedUpdate()
    {
        if (!isJump)
        {
            return;
        }

        transform.position = transform.position + AddJumpVetor;

        if (transform.position.y > 150.0f )
        {
            transform.position = new Vector3(0.0f,transform.position.y,0.0f);
            AddJumpVetor = Vector3.zero;
            //タイヤモードに切り替える
            GetComponent<BossCentipede_PartsManager>().GetBodyManager().ChangeWheelMode();
        }
    }

    public void SetJump(float jumpPower)
    {
        AddJumpVetor = Vector3.up * jumpPower;
        isJump = true;
    }

    public void SetLiftJamp()
    {
        AddJumpVetor = Vector3.zero;
        isJump = false;
    }

    public void StopJump()
    {
        AddJumpVetor = Vector3.zero;
        isJump = false;
    }
}
