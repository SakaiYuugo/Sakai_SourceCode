using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Menu_GameEnd : MonoBehaviour
{
    [SerializeField]
    int SelectNum = 0;
    [SerializeField]
    GameObject[] SelectObj;
    [SerializeField]
    GameObject EndTextObj;
    [SerializeField]
    GameObject TitleManager;
    [SerializeField]
    AudioClip SelectSound;
    [SerializeField]
    AudioClip EnterSound;


    //private float m_frame;
    private Text[] texts;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        if(TitleManager)
            TitleManager.GetComponent<MenuTitle_ChangeScene>().enabled = false;


        GameObject canvas = GameObject.Find("FrontUI");

        this.gameObject.transform.parent = canvas.transform;

        RectTransform CanvasPosition;

        CanvasPosition = gameObject.GetComponent<RectTransform>();
        CanvasPosition.anchoredPosition = new Vector2(0.0f, 0.0f);

        SelectNum = SelectNum % SelectObj.Length;
        SelectObj[SelectNum].GetComponent<Select_ChangeScale>().StartChangeScale();

        //m_frame = 0.0f;
        texts = new Text[3];
        texts[0] = EndTextObj.GetComponent<Text>();
        texts[1] = SelectObj[0].GetComponent<Text>();
        texts[2] = SelectObj[1].GetComponent<Text>();

        //âπÇÃê›íË
        audioSource = GetComponent<AudioSource>();

        Time.timeScale = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {

        if (InputOrder.Select_DownKey() || Input.GetKeyDown(KeyCode.DownArrow))
        {
            audioSource.PlayOneShot(SelectSound);
            SelectObj[SelectNum].GetComponent<Select_ChangeScale>().StopChangeScale();
            SelectNum = (SelectNum + 1) % SelectObj.Length;
            SelectObj[SelectNum].GetComponent<Select_ChangeScale>().StartChangeScale();
        }

        if (InputOrder.Select_UPKey() || Input.GetKeyDown(KeyCode.UpArrow))
        {
            audioSource.PlayOneShot(SelectSound);
            SelectObj[SelectNum].GetComponent<Select_ChangeScale>().StopChangeScale();
            SelectNum = (SelectNum - 1 + SelectObj.Length) % SelectObj.Length;
            SelectObj[SelectNum].GetComponent<Select_ChangeScale>().StartChangeScale();
        }

        if (InputOrder.Enter_Key())
        {
            audioSource.PlayOneShot(EnterSound);
            StartCoroutine(Process());
        }

        if (Gamepad.current == null)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                audioSource.PlayOneShot(SelectSound);
                StartCoroutine(Process(true));
            }
        }
        else
        {
            if (Gamepad.current.buttonSouth.wasPressedThisFrame || Input.GetKeyDown(KeyCode.Escape))
            {
                audioSource.PlayOneShot(SelectSound);
                StartCoroutine(Process(true));
            }
        }
        //    if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    audioSource.PlayOneShot(SelectSound);
        //    StartCoroutine(Process(true));
        //}
    }

    IEnumerator Process(bool Closeflg = false)
    {
        yield return new WaitForSecondsRealtime(0.3f);
        if(Closeflg)
        {
            SelectObj[SelectObj.Length - 1].GetComponent<BaseSelectState>().DecisionProcess();
        }   
        else
        {
            SelectObj[SelectNum].GetComponent<BaseSelectState>().DecisionProcess();
        }
    }
   

    IEnumerator Flash()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        float rand = Random.Range(0.1f, 3.0f);

        yield return new WaitForSecondsRealtime(rand);
        for (int i = 0; i<SelectObj.Length + 1; i++)
        {
            texts[i].color = new Color(255.0f, 255.0f, 255.0f, 0.0f);
        }

        yield return new WaitForSecondsRealtime(0.1f);
        for (int i = 0; i < SelectObj.Length + 1; i++)
        {
            texts[i].color = new Color(255.0f, 255.0f, 255.0f, 255.0f);
        }
    }
}
