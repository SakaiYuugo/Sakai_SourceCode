using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NewsUI : MonoBehaviour
{
	[HideInInspector] public string text;
	[SerializeField] float speed;

	GameObject newsText;
	RectTransform newsTextTrans;


	private void Start()
	{
		newsText = this.transform.Find("NewsText").gameObject;

		//設定された文章に変更
		newsText.GetComponent<Text>().text = this.text;

		//最初の位置と文字幅をテキストの長さによって変更
		newsTextTrans = newsText.GetComponent<RectTransform>();
		newsTextTrans.localPosition = new Vector3(text.Length * 100, newsTextTrans.localPosition.y, 0f);
		newsTextTrans.sizeDelta = new Vector2(text.Length * 100f, newsTextTrans.sizeDelta.y);
	}


	private void FixedUpdate()
	{
		newsTextTrans.localPosition = new Vector3(newsTextTrans.localPosition.x - speed, newsTextTrans.localPosition.y, 0f);

		if (newsTextTrans.localPosition.x < -(text.Length * 100))
		{
			Destroy(this.gameObject);
		}
	}
}
