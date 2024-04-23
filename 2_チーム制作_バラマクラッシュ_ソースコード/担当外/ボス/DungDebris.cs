using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungDebris : MonoBehaviour
{
    [SerializeField] GameObject obj;
    float ImpactHeight;
    [SerializeField] int nQty = 6;  //Ç©ÇØÇÁÇÃêî
    List<Vector3> AddDirect = new List<Vector3>();
    
    void Start()
    {
    }
    
    void FixedUpdate()
    {
        
    }

    public void StartAtk()
    {
        Vector3 temp;
        for (int i = 0; i <= nQty; i++)
        {
            temp = new Vector3(Random.Range(-5, 5), 40, Random.Range(-5, 5));
            GameObject objct = Instantiate(obj, this.transform.position, Quaternion.identity, this.transform);
            objct.GetComponent<Rigidbody>().AddForce(temp,ForceMode.Impulse);
        }
        StartCoroutine(BoxClliderEnabledCoroutine());
    }

    IEnumerator BoxClliderEnabledCoroutine()
    {

        yield return new WaitForSeconds(1.5f);
        GameObject.Find("DungDebris(Clone)").GetComponentInChildren<BoxCollider>().enabled = true;

    }
}
