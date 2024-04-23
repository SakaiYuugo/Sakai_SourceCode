using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombIcon : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] Bomb;
    [SerializeField] private GameObject BombMiniMapIcon;//生成したいオブジェクト
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
            Check("Bomb");
            timer = 0;
        }
    }


    void Check(string tagname)
    {
        Bomb = GameObject.FindGameObjectsWithTag(tagname);
        for (int i = 0; i < Bomb.Length; i++)
        {
            if (!GetChildren(Bomb[i]))
            {
                GameObject Obj = Instantiate(BombMiniMapIcon, Bomb[i].transform.position, Quaternion.identity);
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
