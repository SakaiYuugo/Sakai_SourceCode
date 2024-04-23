using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.HighDefinition;

public class Obstacle : MonoBehaviour
{
    public AnimationCurve curve;
    float counter = 0f;
    public float distance;
    public float reactionDistance = 50f;
    public float fadetime;
    [SerializeField] private bool isBreak;
    public int needVelocity;
    [SerializeField]private bool MoveMode;
    [SerializeField]private float MoveTime;
    private TextMeshProUGUI display;

    public GameObject DestroyPart;
    
    public float fadeTime = 0.5f;
    private float timeCount;
    
    private bool failedOn = false;
    
    private bool playSound1 = false;
    private bool playSound2 = false;
    
    void Start()
    {
        display = GetComponentInChildren<TextMeshProUGUI>();
        counter = 0f;
        if(isBreak)display.text = "" + needVelocity;
        else if (MoveMode) StartCoroutine("Move");
        else StartCoroutine("Fade");
    }

    void FixedUpdate()
    {
        if (isBreak)
        {
            if (FindFirstObjectByType<VelocityDisplay>().playervel <= needVelocity)
            {
                GetComponent<WallSprite>().type = 0;
                display.color = Color.red;
            }
            else
            {
                GetComponent<WallSprite>().type = 1;
                display.color = Color.green;
            }
        }
        else if(!MoveMode)
        {
            Vector3 dis = Player.player.transform.position - gameObject.transform.position;
            distance = dis.magnitude;
            reactionDistance = (Player.player.GetComponent<Rigidbody>().velocity.magnitude) + 50;
            if (distance < reactionDistance)
            {
                // 警告SE
                if (playSound1 == false)
                {
                    GetComponent<AudioSource>().Play();
                    playSound1 = true;
                }

                gameObject.GetComponent<SpriteRenderer>().enabled = true;
                //gameObject.GetComponent<BoxCollider>().enabled = true;
                counter += Time.deltaTime;
                Transform child = transform.GetChild(0);
                // 壁展開SE
                if (counter >= 0.7f && playSound2 == false)
                {
                    GetComponent<AudioSource>().Stop();
                    child.GetComponent<AudioSource>().Play();
                    playSound2 = true;
                }
                child.localPosition = new Vector3(0f, curve.Evaluate(counter), child.localPosition.z);
            }
            else
            {
                counter = 0f;
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                //gameObject.GetComponent<BoxCollider>().enabled = false;
            }
        }

    }

    IEnumerator Move()
    {
        transform.DOMoveX(transform.position.x + 14, MoveTime).SetEase(Ease.Linear);
        yield return new WaitForSeconds(MoveTime);
        transform.DOMoveX(transform.position.x - 14, MoveTime).SetEase(Ease.Linear);
        yield return new WaitForSeconds(MoveTime);
        StartCoroutine("Move");
    }

    IEnumerator Fade()
    {
        this.GetComponent<SpriteRenderer>().DOFade(endValue: 0f, duration: fadetime);
        yield return new WaitForSeconds(fadetime);
        this.GetComponent<SpriteRenderer>().DOFade(endValue: 1.0f, duration: fadetime);
        yield return new WaitForSeconds(fadetime);
        StartCoroutine("Fade");
    }

    IEnumerator Blink()
    {
        int i = 0;
        while(i == 0)
        {
            yield return new WaitForSeconds(0.3f);

            GetComponent<SpriteRenderer>().color = Color.red;

            yield return new WaitForSeconds(0.3f);

            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (FindObjectOfType<Player>().SuperDashMode)
            {
                GameObject obj = Instantiate(other.GetComponent<Player>().obstacleDestroyEffect.gameObject, transform);
                obj.transform.parent = null;
                Destroy(obj, 0.6f);
                Destroy(gameObject);
                Destroy(gameObject);
                return;
            }
            
            if (isBreak)
            {
                Destroy(this.GetComponent<BoxCollider>());
                if ((int)other.GetComponent<Rigidbody>().velocity.z >= needVelocity)
                {
                    // 突破SE
                    GetComponent<AudioSource>().Play(); 
                    
                    GameObject particle = Instantiate(DestroyPart,transform);
                    Destroy(particle, 1.0f);
                    particle.transform.parent = null;
                    other.GetComponent<Player>().nearCheck = false;
                    Destroy(gameObject);
                    return;
                }
            }
            other.GetComponent<Player>().Death();
        }
        
    }
}
