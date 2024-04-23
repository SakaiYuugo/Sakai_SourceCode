using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderState : EnemyZakoState
{
	[SerializeField, Header("HP")] int EnemyHP = 1;
	

	override protected void OnEnable()
    {
		MaxHp = EnemyHP;
        BeetleType = TYPEBEETLE.SPIDER;

        base.OnEnable();
    }


	override protected void FixedUpdate()
    {
        DistDie(1.3f);

        // Ž€‚ñ‚¾‚©‚Ç‚¤‚©”»’è
        if (life <= 0)
		{
			NowState = ZakoState.Die;
		}

		// ó‘Ô‘JˆÚ
		switch (NowState)
		{
			case ZakoState.Move:
				
			break;

			case ZakoState.Attack:
		
			break;

			case ZakoState.Die:
                // Á–Å
                RareEnemyCreater.Release(this.gameObject);
				DeleteEnemyCount();
				base.ItemDrop();
                DieSpone();
                break;
	    }

	}
	
}
