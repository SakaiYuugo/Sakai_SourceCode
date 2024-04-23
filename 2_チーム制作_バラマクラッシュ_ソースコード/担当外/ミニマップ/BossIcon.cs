using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations; // これがないと Constraintは使えない

public class BossIcon : MonoBehaviour
{
    [SerializeField] private GameObject BossMiniMapIcon;//生成したいオブジェクト

    [SerializeField] private GameObject Boss;//生成する場所を決める対象のボス

    Transform childTransform;
    GameObject childObject;
    GameObject MapIcon_Obj;
    PositionConstraint myPositionConstraint;    // Position Type
    ConstraintSource myConstraintSource;        // ConstarintSource
    int childCount;
    // Use this for initialization
    void Start()
    {
        BossMiniMapIcon.transform.localScale = new Vector3(50.0f, 50.0f, 50.0f);
        if (Boss.name == "Boss_Centipede")
        {
           
            // 子オブジェクトの数を取得
            childCount = Boss.transform.childCount;
            // 子オブジェクトを順に取得する
            for (int i = 0; i < childCount - 1; i++)
            {
                childTransform = Boss.transform.GetChild(i);
                childObject = childTransform.gameObject;

                // 取得した子オブジェクトを処理する
                MapIcon_Obj = Instantiate(BossMiniMapIcon, childTransform.transform.position, Quaternion.identity);
                myPositionConstraint = MapIcon_Obj.GetComponent<PositionConstraint>();
                SetObject2ConstraintSource(childTransform.transform, myPositionConstraint);
            }
        }
        else
        {
            MapIcon_Obj = Instantiate(BossMiniMapIcon, Boss.transform.position, Quaternion.identity);
            myPositionConstraint = MapIcon_Obj.GetComponent<PositionConstraint>();
            SetObject2ConstraintSource(Boss.transform, myPositionConstraint);
        }
    }

    //追従設定
    void SetObject2ConstraintSource(Transform parent, PositionConstraint myPositionConstraint)
    {
        // Constraintの参照元を設定(この処理が一つでも欠けると追尾せず即死するので注意)
        myConstraintSource.sourceTransform = parent;
        myConstraintSource.weight = 1.0f; // 影響度を完全支配にする(Addの場合は0になるので)
        myPositionConstraint.AddSource(myConstraintSource);   // Constraintの参照元を追加
        myPositionConstraint.translationOffset = Vector3.zero;     // オフセットを0に
        myPositionConstraint.enabled = true;                       // 有効にする(使わないときはfalse)

    }

    void Update()
    {

    }

}
