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
		SetAlpha (initAtAlpha);
	}

	public void Fade (float startAlpha, float finalAlpha, float delay, float time) {
		this.startAlpha = startAlpha;
		this.finalAlpha = finalAlpha;
		this.delay = delay;
		this.time = time;
	
		StartCoroutine(FadeIn());
	}
		
	IEnumerator FadeIn()
	{
		yield return new WaitForSeconds(delay);

		SetAlpha (startAlpha);
		float t = 0;
		float alphaDiff = Mathf.Abs(GetAlpha() - finalAlpha);

		while (alphaDiff > 0.0001f)
		{
			alphaDiff = Mathf.Abs(GetAlpha() - finalAlpha);

			t +=  (Time.deltaTime / time);
			SetAlpha (Mathf.Lerp (startAlpha, finalAlpha, t*t));
			yield return null;
		}

		SetAlpha(finalAlpha);


	}

	void SetAlpha(float alpha) {
		if (GetCanvasGroup () == null) {
			Color curColor = GetImage ().color;
			curColor.a = alpha;
			GetImage ().color = curColor;
		} else {
			GetCanvasGroup ().alpha = alpha;
		}
	}

	public float GetAlpha() {
		if (GetCanvasGroup () == null) {
			return  GetImage ().color.a;
		} else {
			return GetCanvasGroup ().alpha;
		}
	}
		
	CanvasGroup GetCanvasGroup() {
		return GetComponent<CanvasGroup> ();
	}


	private Image GetImage() {
		if (image == null) {
			image = GetComponent<Image> ();
		}
		return image;
	}


	public void Flash(float peakAt) {
		float ftime = 0.1f;
		float dtime = 1f;
		float otime = 3f;
		Fade (0f, 1f, peakAt - ftime, ftime);
		Fade (1f, 0f, peakAt + 9999, otime); // Don't know why, it works.
	}
}
