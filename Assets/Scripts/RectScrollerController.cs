using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class RectScrollerController : MonoBehaviour {

	private Vector2 startPosition;
	private RectTransform rectTransform;
	private Rect canvasRect;
	public Vector2 Speed;

	void Start () {
		rectTransform = GetComponent<RectTransform> ();
		startPosition = rectTransform.anchoredPosition;
		canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform> ().rect;
	}

	void Update () {
		rectTransform.anchoredPosition += Speed * Time.deltaTime;

		if (Speed.x < 0 && rectTransform.anchoredPosition.x <= -(rectTransform.rect.width - canvasRect.width)) {
			rectTransform.anchoredPosition = new Vector2(startPosition.x, rectTransform.anchoredPosition.y);
		}
			
	}
}
