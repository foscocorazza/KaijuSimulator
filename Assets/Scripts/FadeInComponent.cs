using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInComponent : MonoBehaviour {

	public float delay;
	public float time;

	private Image image;
	private float startAlpha = 0.0f;
	private float finalAlpha = 1.0f;

	void Start () {
		image = this.GetComponent<Image>();

		Color curColor = image.color;
		curColor.a = startAlpha;
		image.color = curColor;

		StartCoroutine(FadeIn());

	}
		
	IEnumerator FadeIn()
	{
		yield return new WaitForSeconds(delay);

		float t = 0;

		Color curColor = image.color;
		float alphaDiff = Mathf.Abs(curColor.a - finalAlpha);
		while (alphaDiff > 0.0001f)
		{
			alphaDiff = Mathf.Abs(curColor.a - finalAlpha);

			t +=  (Time.deltaTime / time);
			curColor.a = Mathf.Lerp(startAlpha, finalAlpha, t);
			image.color = curColor;
			yield return null;
		}

		curColor.a = finalAlpha;


	}

}
