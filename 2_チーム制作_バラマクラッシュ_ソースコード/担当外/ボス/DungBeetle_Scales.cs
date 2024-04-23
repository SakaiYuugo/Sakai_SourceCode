using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungBeetle_Scales : MonoBehaviour
{
    enum State
    {
        UP,
        Shot,
        Down,
        None,
        MAX
    }
    [SerializeField]
    GameObject Effect;
    [SerializeField]
    Animator animator;

    float countTime;
    State NowState;
    GameObject dungBeetle;
    Vector3 startPos;
    Vector3 endPos;
    Quaternion startRotate;
    Quaternion endRotate;
    bool End;

    private void Awake()
    {
        countTime = 0.0f;
        NowState = State.None;
        End = false;
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    private void FixedUpdate()
    {
        Debug.Log(NowState);

        switch(NowState)
        {
            case State.UP:
                {
                    const float UpTime = 3.0f;
                    float per = countTime / UpTime;
                    float Value = per < 0.5f ? 2 * Mathf.Pow(per,2) : 1.0f - Mathf.Pow(-2.0f * per + 2.0f,2.0f)/2.0f;
                    transform.position = Vector3.Slerp(startPos, endPos, Value);
                    transform.rotation = Quaternion.Slerp(startRotate, endRotate, Value);
                    countTime += Time.deltaTime;

                    if(UpTime < countTime)
                    {
                        countTime = 0.0f;
                        NowState = State.Shot;
                        Effect.SetActive(true);
                    }

                }
                break;
            case State.Shot:
                {
                    countTime += Time.deltaTime;

                    if(countTime > 10.0f)
                    {
                        countTime = 0.0f;
                        NowState = State.Down;
                        Effect.SetActive(false);
                    }

                }
                break;
            case State.Down:
                {
                    const float DownTime = 2.0f;
                    transform.position = Vector3.Slerp( endPos, startPos, countTime / DownTime);
                    transform.rotation = Quaternion.Slerp(endRotate, startRotate, countTime / DownTime);
                    countTime += Time.deltaTime;

                    if (DownTime < countTime)
                    {
                        countTime = 0.0f;
                        transform.rotation = endRotate;
                        NowState = State.None;
                        this.gameObject.SetActive(false); dungBeetle.SetActive(true);
                        End = true;
                        animator.SetTrigger("Walk");
                    }
                }
                break;
            case State.None:
                {

                }
                break;
        }
    }

    public void SetScales(GameObject DungBeetleObject,GameObject BallObject)
    {
        NowState = State.UP;

        dungBeetle = DungBeetleObject;
        dungBeetle.SetActive(false);

        this.gameObject.SetActive(true);
        Effect.SetActive(false);

        float Distance = (BallObject.transform.position - DungBeetleObject.transform.position).magnitude;

        startPos = dungBeetle.transform.position;
        endPos = BallObject.transform.position + (Vector3.up * 3.5f);

        startRotate = dungBeetle.transform.rotation;
        endRotate = startRotate * Quaternion.AngleAxis(15.0f,transform.right);
        transform.rotation = startRotate;

        countTime = 0.0f;
        End = false;

        animator.SetTrigger("Rinpun");
    }

    public bool GetEnd()
    {
        return End;
    }
}