using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIcon : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject[] Enemy;
    [SerializeField] private GameObject EnemyMiniMapIcon;//生成したいオブジェクト
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
            Check("Enemy");
            timer = 0;
        }
    }


    void Check(string tagname)
    {
        Enemy = GameObject.FindGameObjectsWithTag(tagname);
        for (int i = 0; i < Enemy.Length; i++)
        {
            if (!GetChildren(Enemy[i]))
            {
                GameObject Obj = Instantiate(EnemyMiniMapIcon, Enemy[i].transform.position, Quaternion.identity);
                Obj.transform.parent = Enemy[i].transform;
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

