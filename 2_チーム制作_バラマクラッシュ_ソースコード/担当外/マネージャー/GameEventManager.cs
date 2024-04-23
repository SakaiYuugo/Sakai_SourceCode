using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameEventManager : MonoBehaviour
{
    [SerializeField] GameObject GameOverPanel;
    [SerializeField] GameObject GameClearPanel;
    [SerializeField] GameObject GamePausePanel;
    GameObject InstGameOverPanel;
    GameObject InstGameClearPanel;
    GameObject InstGamePausePanel;

	PauseBlur pauseBlur;
    bool backEskKey;

    public static bool bGameClear;
    public static bool bGameOver;

    private void Awake()
    {
        bGameClear = false;
        bGameOver = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        backEskKey = InputOrder.ESK_Key(false);
        pauseBlur = GameObject.Find("PostEffect").GetComponent<PauseBlur>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (InputOrder.ESK_Key(false) && !backEskKey)
        {
            if(InstGamePausePanel == null)
            {
                pauseBlur.BlurOut(10.0f);
                PauseStart();
            }
        }

        backEskKey = InputOrder.ESK_Key(false);
    }

    public void GameOver()
    {
        InstGameOverPanel = Instantiate(GameOverPanel);
    }

    public void GameClear()
    {
        bGameClear = true;
        InstGameClearPanel = Instantiate(GameClearPanel);
    }

    public void PauseStart()
    {
        InstGamePausePanel = Instantiate(GamePausePanel);
        Time.timeScale = 0.0f;
    }

    public void PauseEnd()
    {
        Destroy(InstGamePausePanel); 
        pauseBlur.BlurIn(10.0f);
        InstGamePausePanel = null;
        Time.timeScale = 1.0f;
    }
}
