using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    public bool alphaAnimation = false;
    public bool FadeOut = false;
    public bool FadeIn = false;
    public float FadeSpeed = 2f;
    private Image Fader;
    public float counter = 1f;
    public string Name;
    bool fadeInBool;
    bool fadeOutBool;
    public SoundFile fadeInSound;
    public SoundFile fadeOutSound;
    private AudioSource audioSource;
    // Start is called before the first frame update
    private void Awake()
    {
        Fader = GetComponent<Image>();

        fadeInBool = false;
        fadeOutBool = false;

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (FadeOut == true)
        {
            if (counter > 0f) 
            { 
                counter -= Time.deltaTime * FadeSpeed;
                if (alphaAnimation == false)
                    GetComponent<Animator>().Play("fadeOut");
                else
                    GetComponent<Animator>().Play("alpha");
                if (fadeOutBool == false) 
                {
                    fadeOutBool = true;
                    audioSource.PlayOneShot(fadeOutSound.file, fadeOutSound.volume);
                } 
            }
            else 
            { 
                counter = 0f; 
                FadeOut = false;
                if(alphaAnimation == true)
                {
                    GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
                }
            }
        }
        if (FadeIn == true)
        {
            if (counter < 1f) 
            { 
                counter += Time.deltaTime * FadeSpeed; 
                GetComponent<Animator>().Play("fadeIn"); 
                if(fadeInBool == false)
                {
                    fadeInBool = true;
                }
            }
            else
            {
                counter = 1f;
                FadeIn = false;

                if (Name != "") SceneManager.LoadScene(Name);
                else FadeOut = true;
            }
        }
    }

    public void ChangeScene(string name = "")
    {
        FadeIn = true;
        Name = name;
    }

    public bool GetControl()
    {
        if (FadeIn == true || FadeOut == true)
        {
            return false;
        }

        if (FadeIn == false && FadeIn == false)
        {
            return true;
        }

        return false;
    }

}
