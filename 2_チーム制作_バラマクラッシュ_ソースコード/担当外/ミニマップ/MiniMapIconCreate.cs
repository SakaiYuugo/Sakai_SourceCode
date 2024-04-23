#define POSITION_CONSTRAINT
#undef PARENT_CONSTRAINT

using UnityEngine;
using UnityEngine.Animations; // これがないと Constraintは使えない

public class MiniMapIconCreate : MonoBehaviour
{
    //==============変数宣言=======================
    [SerializeField] GameObject MiniMapIcon_Bomb;//ボムアイコン生成用　
    [SerializeField] GameObject MiniMapIcon_Boss;//ボスアイコン
    [SerializeField] GameObject MiniMapIcon_Enemy;//雑魚がよ
    [SerializeField] GameObject MiniMapIcon_Player;//player
    [SerializeField] GameObject MiniMapIcon_Item;//あいてｍｍｍ
    [SerializeField] GameObject MiniMapIcon_Cabe;//あいてｍｍｍ

    [SerializeField] GameObject Boss;//生成する場所を決める対象のボス
    [SerializeField] GameObject Player;//生成する場所を決める対象のプレイヤー
    [SerializeField] GameObject Cave;//生成する場所を決める対象の洞窟

    GameObject MiniMapIcon_Obj;
    GameObject MapIcon_Obj;
    PositionConstraint myPositionConstraint;    // Position Type
    BombPosition BombPosition_script;
    CavePosition CavePosition_script;
    ConstraintSource myConstraintSource;        // ConstarintSource
    Transform childTransform;
    GameObject childObject;

    Triangle Triangle_script;

    int childCount;

    //==============関数===================

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

    //ミニマップアイコンインスタンスしGameObjectを返す関数
    GameObject CreateMiniMapIcon(GameObject MinimapIcon,Transform Target_Obj)
    {
        MiniMapIcon_Obj = Instantiate(MinimapIcon, Target_Obj.position, Quaternion.identity);
        return MiniMapIcon_Obj;
    }
    // Use this for initialization
    void Start()
    {
        // 子オブジェクトの数を取得
        childCount = Cave.transform.childCount;
        // 子オブジェクトを順に取得する
        for (int i = 0; i < childCount; i++)
        {
            childTransform = Cave.transform.GetChild(i);
            childObject = childTransform.gameObject;

            // 取得した子オブジェクトを処理する
            MapIcon_Obj = CreateMiniMapIcon(MiniMapIcon_Cabe, childObject.transform);
            CavePosition_script = MapIcon_Obj.GetComponent<CavePosition>();
            CavePosition_script.SetGameObj(childObject);
            myPositionConstraint = MapIcon_Obj.GetComponent<PositionConstraint>();
            SetObject2ConstraintSource(childTransform.transform, myPositionConstraint);
        }
        BombIconCreate(Player);
        BombIconCreate(Boss);
    }
        //==============ここからパブリック関数=================

        //ボムミニマップアイコン生成
        public void BombIconCreate(GameObject Bomb)
    {
        switch (Bomb.tag)
        {
            case "Player":
                MapIcon_Obj = CreateMiniMapIcon(MiniMapIcon_Player, Bomb.transform);

                Triangle_script = MapIcon_Obj.GetComponent<Triangle>();
                Triangle_script.SetGameObj(Bomb);
                //myPositionConstraint = MapIcon_Obj.GetComponent<PositionConstraint>();
                //SetObject2ConstraintSource(Bomb.transform, myPositionConstraint);
                break;
            case "Enemy":
                MapIcon_Obj = CreateMiniMapIcon(MiniMapIcon_Enemy, Bomb.transform);


                myPositionConstraint = MapIcon_Obj.GetComponent<PositionConstraint>();
                SetObject2ConstraintSource(Bomb.transform, myPositionConstraint);
                break;
            case "Bomb":
                MapIcon_Obj = CreateMiniMapIcon(MiniMapIcon_Bomb, Bomb.transform);
                BombPosition_script = MapIcon_Obj.GetComponent<BombPosition>();
                BombPosition_script.SetGameObj(Bomb);
                myPositionConstraint = MapIcon_Obj.GetComponent<PositionConstraint>();
                SetObject2ConstraintSource(Bomb.transform, myPositionConstraint);
                break;
            case "Item":
                MapIcon_Obj = CreateMiniMapIcon(MiniMapIcon_Item, Bomb.transform);


                myPositionConstraint = MapIcon_Obj.GetComponent<PositionConstraint>();
                SetObject2ConstraintSource(Bomb.transform, myPositionConstraint);
                break;
            case "Boss":
                if (Bomb.name == "Boss_Centipede")
                {

                    // 子オブジェクトの数を取得
                    childCount = Bomb.transform.childCount;
                    // 子オブジェクトを順に取得する
                    for (int i = 0; i < childCount - 1; i++)
                    {
                        childTransform = Bomb.transform.GetChild(i);
                        childObject = childTransform.gameObject;

                        // 取得した子オブジェクトを処理する
                        MapIcon_Obj = CreateMiniMapIcon(MiniMapIcon_Boss, childObject.transform);
                        myPositionConstraint = MapIcon_Obj.GetComponent<PositionConstraint>();
                        SetObject2ConstraintSource(childTransform.transform, myPositionConstraint);
                    }
                }
                else
                {
                    MapIcon_Obj = CreateMiniMapIcon(MiniMapIcon_Boss, Bomb.transform);
                    myPositionConstraint = MapIcon_Obj.GetComponent<PositionConstraint>();
                    SetObject2ConstraintSource(Bomb.transform, myPositionConstraint);
                }

                break;


            default:
                break;
        }
    }
}
