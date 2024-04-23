using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centipede_HitBigTree : MonoBehaviour
{
    [SerializeField]
    bool isJump;
    [SerializeField]
    float JumpPower = 10.0f;

    BossCentipede_BodyManager CentipedeBodyManager;
    Vector3 AddJumpVector;
    

    // Start is called before the first frame update
    void Start()
    {
        //自分たちを制御するBodyManagerを探す
        CentipedeBodyManager =
        GameObject.Find("BodyManager").GetComponent<BossCentipede_BodyManager>();
        AddJumpVector = Vector3.up * JumpPower;
        isJump = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!isJump)
        {
            return;
        }

        //当たったら上にジャンプをする
        transform.position = transform.position + AddJumpVector;

        if (transform.position.y > 150.0f)
        {
            transform.position = new Vector3(0.0f, transform.position.y, 0.0f);
            AddJumpVector = Vector3.zero;
            CentipedeBodyManager.ChangeStanMode(transform.position);
            transform.parent.GetComponent<CentipedeState>().StateChange(CentipedeState.STATE.Stun);
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "BigTree":
                //倒れているオブジェクトだった場合
                if(collision.gameObject.GetComponent<BigTree_Fall>().GetNowTreeType() == BigTree_Fall.TREETYPE.STEM)
                {
                    collision.gameObject.GetComponent<BigTree_Stem>().HitWhileCentipede();  //木に当たった時の処理を書いておく
                    isJump = true;  //ジャンプしてスタン状態に
                }
                break;
        }
    }
}
