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
		//������
		MaxHp = hp;
		BattleType = BATTLETYPE.SUPPORT;
        BeetleType = TYPEBEETLE.KATYDID;
        base.OnEnable();
	}


	protected override void FixedUpdate()
	{
        DistDie(1.0f);

        //�̗͂�0�ɂȂ������Ԃ�Die�ɂ���
        if (life <= 0)
		{
			NowState = ZakoState.Die;
		}

		switch (NowState)
		{
		case ZakoState.Die:
		{
			//�|�����Ƃ��ɔ�������
			GameObject objExplosion = Instantiate(explosion, this.transform.position, Quaternion.identity);
			objExplosion.GetComponent<ExplosionBehaviour>().SetColor(Color.black);

			//�������폜����
            RareEnemyCreater.Release(this.gameObject);

			DeleteEnemyCount();
			base.ItemDrop();
            DieSpone();
        }
		break;
		}
	}
}
