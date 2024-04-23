using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlowAway_Move : EffectMove
{
    [Tooltip("拡大値"), SerializeField] Vector3 Value = new Vector3(2f, 2f, 2f);

    private Vector3 OrigiSize;
    [SerializeField] float power;
    PlayerState playerState;
    PlayerMove playerMove;

    override protected void Start()
    {
        OrigiSize = this.transform.localScale;
        playerState = System_ObjectManager.playerObject.GetComponent<PlayerState>();
        playerMove = System_ObjectManager.playerObject.GetComponent<PlayerMove>();
    }

	override protected void FixedUpdate()
	{
		// エフェクトの拡大
		this.transform.localScale += new Vector3(Value.x, Value.y, Value.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            playerMove.BlowAway(this.transform.position, power);
        }
    }
}
