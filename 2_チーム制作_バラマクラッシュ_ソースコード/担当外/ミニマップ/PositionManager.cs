#define POSITION_CONSTRAINT
#undef PARENT_CONSTRAINT

using UnityEngine;
using UnityEngine.Animations; // これがないと Constraintは使えない

public class PositionManager : MonoBehaviour
{
    private ConstraintSource myConstraintSource;        // ConstarintSource
    GameObject Obj;
    [SerializeField] private GameObject BombMiniMapIconEm;//生成したいオブジェクト
    /// <summary>
    /// ConstraintComponentの親オブジェクトを動的に追加する関数
    /// </summary>
    /// <param name="parent">Contraintの親オブジェクト</param>
    private void SetObject2ConstraintSource(Transform parent,PositionConstraint myPositionConstraint)
    {
        // Constraintの参照元を設定(この処理が一つでも欠けると追尾せず即死するので注意)
        myConstraintSource.sourceTransform = parent;
        myConstraintSource.weight = 1.0f; // 影響度を完全支配にする(Addの場合は0になるので)
        myPositionConstraint.AddSource(myConstraintSource);   // Constraintの参照元を追加
        myPositionConstraint.translationOffset = Vector3.zero;     // オフセットを0に
        myPositionConstraint.enabled = true;                       // 有効にする(使わないときはfalse)

    }

    public GameObject CreateMiniMapIcon(GameObject obj_MinimapIcon, PositionConstraint myPositionConstraint)
    {
        Obj = Instantiate(BombMiniMapIconEm, obj_MinimapIcon.transform.position, Quaternion.identity);
        Obj.transform.parent = obj_MinimapIcon.transform;
        SetObject2ConstraintSource(obj_MinimapIcon.transform, myPositionConstraint);
        return Obj;
    }

    bool GetChildren(GameObject obj)
    {
        Transform children = obj.GetComponentInChildren<Transform>();
        //子要素がいなければ終了
        if (children.childCount == 0)
        {
            return false;
        }
        foreach (Transform ob in children)
        {
            if (ob.name.Contains("MinimapIcon"))
            {
                return true;
            }

        }
        return false;
    }

}

