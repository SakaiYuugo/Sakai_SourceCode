using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageDisplay : MonoBehaviour
{
    float damageNum;
    float beforeTotalDamage;
    GameSoundPlayer gsp;

    [Header("SCALE")]
    public float onPressScale = 0.7f;
    public float onAfterPressScale = 1.0f;

    [Header("GRADIENT")]
    public Color topLeft = Color.red;
    public Color topRight = Color.magenta;
    public Color bottomLeft = Color.cyan;
    public Color bottomRight = Color.green;

    [Header("SHAKE")]
    public float magnitude;
    public float duration;
    public AnimationCurve curve;
    // Start is called before the first frame update
    void Start()
    {
        gsp = Camera.main.GetComponent<GameSoundPlayer>();
        beforeTotalDamage = 0f;
        GetComponent<TextMeshProUGUI>().colorGradient = new VertexGradient(topLeft, topRight, bottomLeft, bottomRight);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //damageNum = FindObjectOfType<Observer>().totalDamage;
        GetComponent<TextMeshProUGUI>().text = "" + (int)damageNum;
        GetComponent<TextMeshProUGUI>().colorGradient = new VertexGradient(topLeft, topRight, bottomLeft, bottomRight);
        
        if ((int)damageNum > (int)beforeTotalDamage)
        {
            int length = gsp.OnTotalDamage.Length - 1;
            SoundFile sn = gsp.OnTotalDamage[((int)(damageNum / 1000) < length) ? (int)(damageNum / 1000) : length];
            GetComponent<AudioSource>().PlayOneShot(sn.file, sn.volume);
            
            StartCoroutine("Pressed");
            StartCoroutine("Shake");
        }
    }

    IEnumerator Pressed()
    {
        int length = gsp.OnTotalDamage.Length -1;
        SoundFile sn= gsp.OnTotalDamage[((int)(damageNum / 1000) < length)?(int)(damageNum / 1000):length];
        AudioSource.PlayClipAtPoint(sn.file, transform.position, sn.volume);
        
        GetComponent<TextMeshProUGUI>().text = "" + (int)damageNum;
        GetComponent<RectTransform>().localScale = new Vector3(onPressScale,onPressScale,onPressScale);

        yield return new WaitForSeconds(0.1f);
        
        GetComponent<RectTransform>().localScale = new Vector3(onAfterPressScale,onAfterPressScale,onAfterPressScale);
        beforeTotalDamage = damageNum;

    }

    IEnumerator Shake()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration) * magnitude;
            transform.position = startPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = startPosition;
    }
}
