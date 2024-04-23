using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungBeetle_BallRotate : MonoBehaviour
{
    [SerializeField]
    bool Rotate = false;
    [SerializeField]
    GameObject DungBeetleObject;
    [SerializeField, Range(0.0f, 1.0f)]
    float TurnoverRate = 0.0f;
    [SerializeField]
    float RotateTime = 1.0f;

    Quaternion StartRotate;
    Quaternion EndRotate;
    float Count;
    bool Init;

    // Start is called before the first frame update
    void Start()
    {
        
        if(DungBeetleObject == null)
        {
            Init = false;
            return;
        }

        InitSetting();
    }

    // Update is called once per frame
    void Update()
    {
        if(DungBeetleObject == null)
        {
            return;
        }

        if(!Init)
        {
            InitSetting();
        }

        transform.rotation = Quaternion.Lerp(StartRotate, EndRotate, TurnoverRate);

        if(TurnoverRate < 1.0f && Rotate)
        {
            Count += Time.deltaTime;

            //•âŠÔ‚Ì”’l
            TurnoverRate = 1 - Mathf.Pow(1 - (Count / RotateTime), 3);

            if(TurnoverRate >= 1.0f)
            {
                Count = 0.0f;
            }
        }
    }

    void InitSetting()
    {
        Init = true;
        //Å‰‚Ì‰ñ“]—Ê
        StartRotate = DungBeetleObject.transform.rotation;

        //’Ž‚ÉŒü‚©‚¤ƒxƒNƒgƒ‹‚ð‹‚ß‚é
        Vector3 BeetleVec = DungBeetleObject.transform.position - transform.position;
        BeetleVec = BeetleVec.normalized;

        //‰ñ“]—Ê‚ð‹‚ß‚é
        float Angle = Mathf.Acos(Vector3.Dot( BeetleVec, Vector3.up));
        Angle = Angle * 180.0f / 3.14f;

        //‰ñ“]Ž²‚ð‹‚ß‚é
        Vector3 RotateAxis =
            Vector3.Cross(BeetleVec, Vector3.up);

        //ÅŒã‚Ì‰ñ“]—Ê
        EndRotate = Quaternion.AngleAxis(Angle, RotateAxis) * StartRotate;

        Count = 0.0f;
    }

    public void SetStart()
    {
        SetStart(0.0f);
    }

    public void SetStart(float Per)
    {
        TurnoverRate = Per;
        Rotate = true;
    }

    public void SetEnd()
    {
        TurnoverRate = 1.0f;
        Rotate = true;
    }

    public bool GetEnd()
    {
        return TurnoverRate >= 1.0f;
    }
}
