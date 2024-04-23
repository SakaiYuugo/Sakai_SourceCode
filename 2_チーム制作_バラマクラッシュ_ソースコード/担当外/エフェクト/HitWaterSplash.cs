using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitWaterSplash : MonoBehaviour
{
    [SerializeField]
    float WaterSurfacePos;
    [SerializeField]
    GameObject Effects;

    // Start is called before the first frame update
    void Start()
    {
        WaterSurfacePos = transform.position.y + transform.localScale.y * 0.5f;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //�������Ă���|�W�V������Ԃ�
        Vector3 HitPoint = other.ClosestPointOnBounds(this.transform.position);
        Instantiate(Effects, HitPoint, Quaternion.identity);
    }
}
