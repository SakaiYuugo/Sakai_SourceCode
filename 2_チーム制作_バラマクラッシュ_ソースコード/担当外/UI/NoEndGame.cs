using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoEndGame : BaseSelectState
{
    [SerializeField]
    GameObject TitleManager;
    [SerializeField]
    GameObject PausePanel;

    PauseBlur pauseBlur;

    private void Start()
    {
        pauseBlur = GameObject.Find("PostEffect").GetComponent<PauseBlur>();
    }
    public override void DecisionProcess()
    {
        if (TitleManager)
        {
            TitleManager.GetComponent<MenuTitle_ChangeScene>().enabled = true;
            transform.parent.gameObject.SetActive(false);
            Time.timeScale = 1.0f;
        }           
        else
        {
            Debug.Log("Pause•\Ž¦");
            pauseBlur.BlurOut(10.0f);
            GameObject.Find("GameEventManager").GetComponent<GameEventManager>().PauseStart();
            Destroy(transform.parent.gameObject);
        }
    }
}
