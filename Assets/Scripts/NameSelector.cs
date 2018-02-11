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
	public GameObject fourCharsPlease;
	public float Offset = 42f;
	public int MinLength = 4;
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

	private string LastKey = "";
	private float LastKeyPressedSince = 0;
	private float LastMovement = 0;


	 bool ShouldHit() {
		if (LastKey == "") 
			return true;

		if (LastKeyPressedSince > 0.5f) {
			if (LastMovement > 0.1f) {
				LastMovement = 0;
				return true;
			} else {
				LastMovement += Time.deltaTime;
			}
		}

		return  false;
	}

	void Update () {
		Debug.Log ("a");
		if (GetComponentInParent<CanvasGroup> ().alpha < 0.5f)
			return;

		Debug.Log ("b");
		if (IsReady)
			return;
		Debug.Log ("c");

		if (player.GetButton ("ArrowKeyUp")) {
			Debug.Log ("d");
			if(ShouldHit()) MoveWheel (+1);
			if(LastKey == "Up") LastKeyPressedSince += Time.deltaTime;
			else LastKeyPressedSince = 0;
			LastKey = "Up";
			return;
		}

		if (player.GetButton ("ArrowKeyDown")) {
			Debug.Log ("e");
			if(ShouldHit()) MoveWheel (-1);
			if(LastKey == "Down") LastKeyPressedSince += Time.deltaTime;
			else LastKeyPressedSince = 0;
			LastKey = "Down";
			return;
		}

		LastKey = "";

		if (player.GetButton ("ArrowKeyLeft")) {
			Debug.Log ("f");
			if(LastFrameWasFree) ChangeWheel (-1);
			LastFrameWasFree = false;
			return;
		}

		if (player.GetButton ("ArrowKeyRight") || player.GetButton ("Fire")) {
			Debug.Log ("g");
			if(LastFrameWasFree) ChangeWheel (+1);
			LastFrameWasFree = false;
			return;
		}

		if (player.GetButton ("Cancel")) {
			Debug.Log ("h");
			if (LastFrameWasFree) RemoveCurrent ();
			LastFrameWasFree = false;
			return;
		}

		if (player.GetButton ("Start")) {

			Debug.Log ("i");
			if (LastFrameWasFree) {
				if (GetString ().Length < MinLength) {
					fourCharsPlease.GetComponent<FadeComponent> ().Fade (0f, 1f, 0f, 0.2f);
				} else {
					SetReady (true);
				}
			}
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

		Debug.Log (GetString());

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

			//Debug.Log ("a." + GetString());
			SetLastLetter (Mathf.Max (lastLetter, currentWheel));
			//Debug.Log ("b." + GetString());
		}

		//Debug.Log ("c." + GetString());
		SetWheelIndicator ();
		MoveWheelToLetter (currentWheel, currentLetter [currentWheel]);
		Debug.Log (GetString());
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

		/*if (currentWheel == last) {
			currentLetter [last] = 0;
			currentWheel = last-1;
			SetLastLetter (currentWheel);
			MoveWheelToLetter (currentWheel, currentLetter [currentWheel]);
			SetWheelIndicator ();
			Debug.Log (GetString());
			return;
		}*/
		
		for (int i = currentWheel; i < lastLetter; i++) {
			currentLetter [i] = i + 1 > last ? 0 : currentLetter [i + 1];
			MoveWheelToLetter (i, currentLetter [i]);
		}

		SetLastLetter (lastLetter - 1);
		if (currentWheel > lastLetter) currentWheel--;
		SetWheelIndicator ();
		Debug.Log (GetString());
			
	}
	void SetLastLetter(int index) {

		lastLetter = index;
		for (int i = 0; i < wheels.Length; i++) {
			wheels [i].SetActive (lastLetter >= i);
		}

		for (int i = (lastLetter+1); i < wheels.Length; i++) {
			currentLetter [i] = 0;
		}

	}

	public string GetString() {
		string Alphabet = "abcdefghijklmnopqrstuvwxyz";
		string s = "";
		for (int i = 0; i <= lastLetter; i++) {
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
		fourCharsPlease.SetActive (!value);

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
		Hide (0, HalfIndex(s), true);
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

	public Vector2 Position{
		get { 
			return GetComponent<RectTransform> ().anchoredPosition;
		}
		set {
			GetComponent<RectTransform> ().anchoredPosition = value;
		}
	}



	public void MoveTo(Vector2 pos, float delay, float time) {
		StartCoroutine (DoMove (pos, delay, time));
	}


	IEnumerator DoMove(Vector2 finalPos, float delay, float time)
	{
		yield return new WaitForSeconds(delay);

		float t = 0;
		Vector2 startPos = Position;

		float diff = Vector2.Distance (startPos , finalPos);
		while (diff > 0.0001f)
		{
			diff = Vector2.Distance (startPos , finalPos);

			t +=  (Time.deltaTime / time);
			Position = Vector2.Lerp (startPos, finalPos, t*t*t);
			yield return null;
		}

		Position = finalPos;


	}


}
