using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class failed : MonoBehaviour
{
    public GameObject FailedUI;
    public float waitTime = 0.5f;
    
    void FixedUpdate()
    { 
        // 死亡時のUI
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().playerDestroy)
        {
            StartCoroutine("FailedWait");
        }
        if((Player.player.playerDestroy || Player.player.isGoal) && Input.GetKeyDown(KeyCode.Space))FindFirstObjectByType<Fade>().ChangeScene("SampleScene");

    }
    
    IEnumerator FailedWait()
    { 
        yield return new WaitForSecondsRealtime(waitTime);
        
        FailedUI.GetComponent<Animator>().enabled = true;
        FindFirstObjectByType<Result>().SetGameOver();
    }
}
