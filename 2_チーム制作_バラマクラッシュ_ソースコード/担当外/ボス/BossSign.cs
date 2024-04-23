using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EffectSpawn))]
[RequireComponent(typeof(BossBeeState))]
public class BossSign : MonoBehaviour
{
    [Tooltip("�U���\���G�t�F�N�g�I���܂ł̃t���[����"), SerializeField] int FinishFrame = 180;

    private EffectSpawn Effectspawn;
    private BossBeeState bossState;

    private int frame;

    // Start is called before the first frame update
    void Start()
    {
        frame = 0;
        Effectspawn = this.GetComponent<EffectSpawn>();
        bossState = this.GetComponent<BossBeeState>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Sign(int i)
    {
        if (frame <= 0)
            Effectspawn.SignEffectSpawn(i);

        frame++;

        // �O�b��
        if (frame > FinishFrame)
        {
            bossState.SetStateBoss(BossBeeState.State.Attack);
            frame = 0;
        }

        
    }
}
