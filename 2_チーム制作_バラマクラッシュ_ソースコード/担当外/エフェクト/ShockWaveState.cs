using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveState : EffectState
{
    private Vector3 hitPosition;

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        RaycastHit raycastHit;
        // ^‰º‚ÖRay‚ğ”ò‚Î‚µ“–‚½‚Á‚½êŠ‚ÉˆÚ“®
        if (Physics.Raycast(transform.position, -transform.up, out raycastHit))
        {
            if (raycastHit.collider.gameObject.tag.Contains("Ground"))
            {
                hitPosition = raycastHit.point;
            }
            hitPosition = raycastHit.point;
        }
        gameObject.transform.position = hitPosition;
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
