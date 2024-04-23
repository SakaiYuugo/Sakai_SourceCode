using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemIcon : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] Item;
    [SerializeField] private GameObject ItemMiniMapIcon;//生成したいオブジェクト
    float timer = 0.0f;
    float interval = 3.0f;
    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer > interval)
        {
            Check("Item");
            timer = 0;
        }
    }


    void Check(string tagname)
    {
        Item = GameObject.FindGameObjectsWithTag(tagname);
        for (int i = 0; i < Item.Length; i++)
        {
            if (!GetChildren(Item[i]))
            {
                GameObject Obj = Instantiate(ItemMiniMapIcon, Item[i].transform.position, Quaternion.identity);
                Obj.transform.parent = Item[i].transform;
            }
        }

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
