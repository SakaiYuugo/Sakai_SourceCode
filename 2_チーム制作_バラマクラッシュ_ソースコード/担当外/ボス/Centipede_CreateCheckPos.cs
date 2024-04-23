using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Centipede_CreateCheckPos : MonoBehaviour
{
    [SerializeField]
    int CornerNum = 5;
    [SerializeField]
    GameObject InstPointObject;

    float DisAngle;
    float NowPointAngle;

    private void Awake()
    {
        transform.rotation = Quaternion.identity;

        NowPointAngle = 0.0f;
        DisAngle = 360.0f / (float)CornerNum;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 ChangeCheckPoint()
    {
        //‰½“x‚ÌˆÊ’u‚És‚­‚©
        float RotateAngle = (DisAngle * (float)Random.Range(0, CornerNum) + 60.0f) % 360.0f;
        NowPointAngle += RotateAngle;

        if(NowPointAngle > 360.0f)
        {
            RotateAngle -= -360.0f;
            NowPointAngle -= 360.0f;
        }

        //‰ñ“]‚ð‚·‚é
        transform.rotation *= Quaternion.AngleAxis(RotateAngle, Vector3.up);
        
        return InstPointObject.transform.position;
    }
}
