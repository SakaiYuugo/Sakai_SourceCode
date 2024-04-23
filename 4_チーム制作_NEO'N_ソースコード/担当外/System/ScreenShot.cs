using System;
using UnityEngine;

public class ScreenShot : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    [SerializeField] string folderName = "Resources";

    private string path;
    private Observer obs;

    void Start()
    {
        obs = FindFirstObjectByType<Observer>().GetComponent<Observer>();
        path = Application.dataPath + "/" + folderName + "/";
    }

    private void FixedUpdate()
    {
        //if(Input.GetKeyDown(KeyCode.F)) PrintScreen();


        /*if (Input.GetKeyDown(KeyCode.U))
        {
            //obs.renderTexture = render;
            PrintScreen();
            obs.SetTime();
            FindFirstObjectByType<Fade>().ChangeScene("Result");
        }*/
        
    }

    //　ゴール時にこの関数呼んでください
    public void PrintScreen()
    {
        string fileName = path + "ResultUI.png";
        ScreenCapture.CaptureScreenshot(fileName);
        Sprite sprite = Resources.Load<Sprite>("ResultUI");
        //obs.spriteSS = sprite;
    }
}