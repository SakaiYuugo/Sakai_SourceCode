using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEndPause : BaseSelectState
{
    [SerializeField]
    GameObject GameEndPanel;

    Canvas canvas;

    private void Start()
    {
        canvas = transform.parent.root.GetComponent<Canvas>();
    }
    public override void DecisionProcess()
    {
        Instantiate(GameEndPanel);
        GameObject.Find("GameEventManager").GetComponent<GameEventManager>().PauseEnd();
        //Destroy(transform.parent.gameObject);
    }
}
