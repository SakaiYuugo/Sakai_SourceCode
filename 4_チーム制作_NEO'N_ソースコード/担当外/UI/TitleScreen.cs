using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
   
    public void ClickStart()
    {
        GameSoundPlayer sound = Camera.main.GetComponent<GameSoundPlayer>();
        AudioSource.PlayClipAtPoint(sound.Button.file,transform.position, sound.Button.volume);

        SceneManager.LoadScene("StageSelect");
    }

    public void ClickOption()
    {
        //
    }
}
