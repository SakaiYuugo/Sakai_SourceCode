using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungBeetle_GoUp : MonoBehaviour
{
    [SerializeField]
    bool GoUP = false;
    [SerializeField]
    GameObject EndSetObject;    
    [SerializeField,Range(0.0f,1.0f)]
    float MigrationRate;
    [SerializeField]
    float MoveTime = 1.0f;

    Vector3 StartPosition;
    Vector3 EndPosition;
    Quaternion StartRotate;
    Quaternion EndRotate;
    float Count;
    bool Init;

    // Start is called before the first frame update
    void Start()
    {
        if (EndSetObject == null)
        {
            Init = false;
            return;
        }

        InitSetting();
    }

    // Update is called once per frame
    void Update()
    {
        if (EndSetObject == null)
        {
            return;
        }

        if (!Init)
        {
            InitSetting();
        }
        
        transform.rotation = Quaternion.Lerp(StartRotate, EndRotate, MigrationRate);

        if (MigrationRate < 1.0f && GoUP)
        {
            Count += Time.deltaTime;

            //•âŠÔ‚Ì”’l
            MigrationRate = 1 - Mathf.Pow(1 - (Count / MoveTime), 3);

            if (MigrationRate >= 1.0f)
            {
                Count = 0.0f;
            }
        }
    }

    void InitSetting()
    {
        Init = true;
        //Å‰‚Ìî•ñ
        StartPosition = transform.position;
        StartRotate = transform.rotation;

        //ÅŒã‚Ìî•ñ
        EndPosition = EndSetObject.transform.position;
        EndRotate = EndSetObject.transform.rotation;

        Count = 0.0f;
    }

    public void SetStart()
    {
        SetStart(0.0f);
    }

    public void SetStart(float Per)
    {
        InitSetting();

        MigrationRate = Per;
        GoUP = true;
    }

    public void SetEnd()
    {
        MigrationRate = 1.0f;
        GoUP = true;
    }

    public bool GetEnd()
    {
        return MigrationRate >= 1.0f;
    }
}
