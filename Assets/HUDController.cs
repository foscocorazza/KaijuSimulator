using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour {

	[Header("Data")]
	public float maxTime = 60f;


	[Header("GUI Elements")]
	public TextMeshProUGUI TimeLabel;
	public TextMeshProUGUI ScoreLabel;
	public RectTransform BarMask;

	private float maxMaskWidth;
	private float time;
	private int score;
	private bool started = false;

	void Start() { 
		StartTimer ();
		SetScore (0);
	}

	void StartTimer() {
		time = maxTime;
		maxMaskWidth = BarMask.rect.width;
		started = true;
	}

	void Update() { 
		if (started) {
			time -= Time.deltaTime;
			SetSeconds (time);
		}
	}

	public void SetSeconds(float time) {
		this.time = time;
		TimeLabel.text = (int)time + "s";

		BarMask.sizeDelta = new Vector2(
			time * maxMaskWidth / maxTime, 
			BarMask.rect.height);
	}

	public void SetScore(int score) {
		this.score = score;
		ScoreLabel.text = score.ToString();
	}

}
