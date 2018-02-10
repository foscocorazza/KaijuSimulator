using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class NameSelector : MonoBehaviour {

	public int playerId;
	public GameObject[] wheels;
	public GameObject wheelsIndicator;
	public GameObject backgroundImage;
	public float Offset = 42f;
	public bool IsReady = false;

	private int currentWheel = 0;
	private int[] currentLetter = new int[10];
	private int lastLetter = 0;
	private Player player;
	private bool LastFrameWasFree = true;
	private int MaxLetters = 26;
	private float StartWheelY = 0;

	void Start() {
		player = ReInput.players.GetPlayer(playerId);

		for (int i = 1; i < wheels.Length; i++) {
			wheels [i].SetActive (false);
			currentLetter [i] = 0;
		}
			
		SetWheelIndicator ();
		StartWheelY = wheels [0].GetComponent<RectTransform> ().anchoredPosition.y;
	}

	void Update () {
		if (GetComponentInParent<CanvasGroup> ().alpha < 0.5f)
			return;
		
		if (IsReady)
			return;

		if (player.GetButton ("ArrowKeyUp")) {
			if(LastFrameWasFree) MoveWheel (+1);
			LastFrameWasFree = false;
			return;
		}

		if (player.GetButton ("ArrowKeyDown")) {
			Debug.Log ("hduisfua");
			if(LastFrameWasFree) MoveWheel (-1);
			LastFrameWasFree = false;
			return;
		}

		if (player.GetButton ("ArrowKeyLeft")) {
			if(LastFrameWasFree) ChangeWheel (-1);
			LastFrameWasFree = false;
			return;
		}

		if (player.GetButton ("ArrowKeyRight") || player.GetButton ("Fire")) {
			if(LastFrameWasFree) ChangeWheel (+1);
			LastFrameWasFree = false;
			return;
		}

		if (player.GetButton ("Cancel")) {
			if (LastFrameWasFree) RemoveCurrent ();
			LastFrameWasFree = false;
			return;
		}

		if (player.GetButton ("Start")) {
			if (LastFrameWasFree)
				SetReady (true);
			LastFrameWasFree = false;
			return;
		}


		LastFrameWasFree = true;
	}

	void MoveWheel(int offset) {
		int nextLetter = currentLetter[currentWheel] + offset;

		// Wrap
		if (nextLetter < 0) {
			nextLetter = MaxLetters - 1;
		} else {
			nextLetter = nextLetter % MaxLetters;
		}

		bool wheelExists = currentWheel >= 0 && currentWheel < wheels.Length;
		bool letterExists = nextLetter >= 0 && nextLetter < MaxLetters;


		if (wheelExists && letterExists) {
			MoveWheelToLetter (currentWheel, nextLetter);
	
		}
	}

	void MoveWheelToLetter(int wheel, int nextLetter) {
		RectTransform transform = wheels [wheel].GetComponent<RectTransform> ();
		currentLetter [wheel] = nextLetter;
		transform.anchoredPosition = new Vector2 (transform.anchoredPosition.x, StartWheelY + nextLetter*Offset);
	}

	void ChangeWheel(int offset) {
		int nextWheel = currentWheel + offset;
		bool wheelExists = nextWheel >= 0 && nextWheel < wheels.Length;
		if (wheelExists) {
			currentWheel = nextWheel;
			SetLastLetter (Mathf.Max (lastLetter, currentWheel));
		}

		SetWheelIndicator ();
	}

	void SetWheelIndicator() {
		float newX = wheels [currentWheel].transform.position.x;
		Transform t = wheelsIndicator.transform;
		t.position = new Vector3 (newX, t.position.y, t.position.z);
	}


	void RemoveCurrent() {
		int last = wheels.Length - 1;
		if (currentWheel == 0)
			return;

		if (currentWheel == last) {
			currentLetter [last] = 0;
			currentWheel = last-1;
			SetLastLetter (currentWheel);
			SetWheelIndicator ();
			return;
		}
		
		for (int i = currentWheel; i < lastLetter-1; i++) {
			currentLetter [i] = currentLetter [i + 1];
			MoveWheelToLetter (i, currentLetter [i]);
		}

		SetLastLetter (lastLetter - 1);
		if (currentWheel > lastLetter) currentWheel--;
		SetWheelIndicator ();
			
	}
	void SetLastLetter(int index) {
		lastLetter = index;
		for (int i = 0; i < wheels.Length; i++) {
			wheels [i].SetActive (lastLetter >= i);
		}

		for (int i = lastLetter; i < wheels.Length; i++) {
			currentLetter [lastLetter] = 0;
		}



	}

	public string GetString() {
		string Alphabet = "abcdefghijklmnopqrstuvwxyz";
		string s = "";
		for (int i = 0; i < wheels.Length; i++) {
			if (wheels [i].activeSelf) {
				s += Alphabet [currentLetter [i]];
			}
		}
		return s.ToLower();
	}

	void SetReady(bool value) {
		IsReady = value;
		wheelsIndicator.SetActive (!value);
		backgroundImage.SetActive (!value);

		if (BothReady ()) {
			Continue();
		}
	}


	bool BothReady() {
		return 
			GameFlowController.Instance ().nameSelectorPlayer1.IsReady &&
			GameFlowController.Instance ().nameSelectorPlayer2.IsReady;
	}

	void Continue() {
		GameFlowController.Instance ().PrepareGame();
	}



	// Hiding and Moving
	public void Hide(int start, int end, bool LeftToRight) {
		Debug.Log (start + " " + end);

		int diff = end - start;
	
		for (int i = start; i < end; i++) {
			float delay = 0;
			if (LeftToRight) {
				delay = (i - diff) * 0.1f;
			} else {
				delay = (2*diff - i) * 0.1f;
			}
			wheels [i].GetComponent<FadeComponent> ().Fade (1f, 0f, delay, 0.2f);
		}

	}


	public void HideFirstHalf() {
		string s = GetString ();
		Hide (0, HalfIndex(s)-1, true);
	}

	public void HideSecondHalf() {
		string s = GetString ();
		Hide (HalfIndex(s), s.Length, false);
	}

	public int HalfIndex(string s) {
		return (s.Length - 1) / 2 + 1;
	}
		
	public Vector2 MaskPosition{
		get { 
			return GetComponentInChildren<RectMask2D> ().GetComponent<RectTransform> ().anchoredPosition;
		}
		set {
			GetComponentInChildren<RectMask2D> ().GetComponent<RectTransform> ().anchoredPosition = value;
		}
	}

	public Vector2 MaskCenterAtLetter(int index) {
		int last = wheels.Length - 1;
		float s = wheels[0].GetComponent<RectTransform> ().anchoredPosition.x;
		float e = wheels[last].GetComponent<RectTransform> ().anchoredPosition.x;
		float t = e - s;

		return new Vector2(MaskPosition.x + t/2 + index * t /(last), MaskPosition.y);

	}


}
