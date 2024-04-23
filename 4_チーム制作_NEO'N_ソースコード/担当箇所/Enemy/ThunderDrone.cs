using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ThunderDrone : MonoBehaviour
{
    // アニメーションカーブ
    public AnimationCurve curveX;
    public AnimationCurve curveY;
    public AnimationCurve curveZ;
    private float counter;

    public Vector3 offsetValue = new Vector3(1f,20f,40f);
    public ParticleSystem particle;

    private bool sound = false;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("AttackThunder");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        counter += Time.deltaTime;

        int dir = (int)(transform.localPosition.x / (Mathf.Abs(transform.localPosition.x) + 0.01f));
        float x = (dir * curveX.Evaluate(counter) * offsetValue.x);
        transform.localPosition = new Vector3(x, curveY.Evaluate(counter) * offsetValue.y, curveZ.Evaluate(counter) * offsetValue.z);
    }

    /// <summary>
    /// 攻撃モーション
    /// </summary>
    IEnumerator AttackThunder()
    {
        yield return new WaitForSeconds(1f);
        Destroy(Instantiate(particle, transform),3f);
        if (sound == false)
        {
            GetComponent<AudioSource>().Play();
            sound = true;
        }
        
        yield return new WaitForSeconds(8f);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // プレイヤーがスーパーダッシュ状態の場合
            if (FindObjectOfType<Player>().SuperDashMode)
            {
                GameObject obj = Instantiate(other.GetComponent<Player>().obstacleDestroyEffect.gameObject, transform);
                obj.transform.parent = null;
                Destroy(obj, 0.6f);
                Destroy(gameObject);
                return;
            }
            
            other.GetComponent<Player>().Death();
        }
    }
}
