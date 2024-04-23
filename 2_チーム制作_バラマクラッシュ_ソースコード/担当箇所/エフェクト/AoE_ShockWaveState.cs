using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoE_ShockWaveState : EffectState
{
	private Vector3 hitPosition;

	// Start is called before the first frame update
	override protected void Start()
    {
		base.Start();

		// 真下にRayを飛ばし当たった場所へ移動
		RaycastHit raycastHit;

		if (Physics.Raycast(transform.position, -transform.up, out raycastHit))
		{
			// 地面のタグに"Ground"が含まれていた場合
			if (raycastHit.collider.gameObject.tag.Contains("Ground"))
			{
				hitPosition = raycastHit.point;
			}

			// エフェクトを使用状態に
			gameObject.GetComponent<AoE_ShockWaveMove>().Set_EfkShockWaveFlg();

			hitPosition = raycastHit.point;
		}

		gameObject.transform.position = hitPosition;
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
	}
}
