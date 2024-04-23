using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    public string next = "";

    // Update is called once per frame
    void FixedUpdate()
    {
        if (FindFirstObjectByType<Fade>().GetControl() == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GameSoundPlayer a = Camera.main.GetComponent<GameSoundPlayer>();
                AudioSource.PlayClipAtPoint(a.Button.file, transform.position, a.Button.volume);
                FindFirstObjectByType<Fade>().ChangeScene(next);
            }
        }
    }
}
