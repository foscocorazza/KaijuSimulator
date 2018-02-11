using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Rewired;

public class HUDController : MonoBehaviour {

	[Header("Data")]
	public float maxTime = 60f;
	public string TitleSceneName;
	public PlayerController Kaiju;


	[Header("GUI Elements")]
	public TextMeshProUGUI TimeLabel;
	public TextMeshProUGUI ScoreLabel;
	public TextMeshProUGUI EndScoreLabel;
	public FadeComponent EndScreen;
	public RectTransform BarMask;

	private float maxMaskWidth;
	private float time;
	private int score;
	private bool started = false;
	Player p1, p2;

	void Start() { 
		StartTimer ();
		SetScore (0);

		p1 = ReInput.players.GetPlayer(0);
		p2 = ReInput.players.GetPlayer(1);
	}

	void StartTimer() {
		time = maxTime;
		maxMaskWidth = BarMask.rect.width;
		started = true;
	}

	void StopGame() {
		started = false;
		EndScoreLabel.text = score.ToString();
		KillKaiju ();
		EndScreen.Fade (0f, 1f, 1f, 0.2f);
	}

	void Update() { 
		if (started) {
			time -= Time.deltaTime;

			if (time <= 0) {
				StopGame ();
				return;
			}

			SetSeconds (time);
			SetScore(SoundManager.Instance.getScore ());

		} else if (EndScreen.GetAlpha() >= 0.5f){

			if (p1.GetButton("Fire") || p2.GetButton("Fire") ) {
				SceneManager.LoadScene (TitleSceneName);
			}
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


	void KillKaiju() {
		Kaiju.enabled = false;
		AudioSource AudioSource = GameObject.FindGameObjectWithTag ("Player").GetComponent<AudioSource>();
		AudioSource.clip = SoundManager.Instance.GetSound("Dying1");
		AudioSource.volume = 0.9f;
		AudioSource.Play ();
	}

}
