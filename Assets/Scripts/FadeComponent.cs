using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeComponent : MonoBehaviour {

	public float initAtAlpha = 0;

	private Image image;
	private float startAlpha;
	private float finalAlpha;
	private float delay;
	private float time;

	void Start () {
		Color curColor = GetImage().color;
		curColor.a = initAtAlpha;
		GetImage().color = curColor;
	}

	public void Fade (float startAlpha, float finalAlpha, float delay, float time) {
		this.startAlpha = startAlpha;
		this.finalAlpha = finalAlpha;
		this.delay = delay;
		this.time = time;

		Color curColor = GetImage().color;
		curColor.a = startAlpha;
		GetImage().color = curColor;

		StartCoroutine(FadeIn());
	}

	private Image GetImage() {
		if (image == null) {
			image = GetComponent<Image> ();
		}
		return image;
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
			GetImage().color = curColor;
			yield return null;
		}

		curColor.a = finalAlpha;


	}

}
