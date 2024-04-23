using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class ZakoKatydidState : EnemyZakoState
{
	[SerializeField] int hp;
	[SerializeField] float buffDelay;
	[SerializeField] GameObject explosion;




	protected override void OnEnable()
    {
		//‰Šú‰»
		MaxHp = hp;
		BattleType = BATTLETYPE.SUPPORT;
        BeetleType = TYPEBEETLE.KATYDID;
        base.OnEnable();
	}


	protected override void FixedUpdate()
	{
        DistDie(1.0f);

        //‘Ì—Í‚ª0‚É‚È‚Á‚½‚çó‘Ô‚ğDie‚É‚·‚é
        if (life <= 0)
		{
			NowState = ZakoState.Die;
		}

		switch (NowState)
		{
		case ZakoState.Die:
		{
			//“|‚³‚ê‚é‚Æ‚«‚É”š”­‚·‚é
			GameObject objExplosion = Instantiate(explosion, this.transform.position, Quaternion.identity);
			objExplosion.GetComponent<ExplosionBehaviour>().SetColor(Color.black);

			//©•ª‚ğíœ‚·‚é
            RareEnemyCreater.Release(this.gameObject);

			DeleteEnemyCount();
			base.ItemDrop();
            DieSpone();
        }
		break;
		}
	}
}
