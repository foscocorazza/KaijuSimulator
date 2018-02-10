using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class NameSelector : MonoBehaviour {

	public int playerId;
	public GameObject[] wheels;
	public GameObject wheelIndicator;
	public float Offset = 42f;

	private int currentWheel = 0;
	private int[] currentLetter = new int[10];
	private int lastLetter = 0;
	private Player player;
	private bool LastFrameWasFree = true;
	private int MaxLetters = 26;

	void Start() {
		player = ReInput.players.GetPlayer(playerId);

		for (int i = 1; i < wheels.Length; i++) {
			wheels [i].SetActive (false);
			currentLetter [i] = 0;
		}
			
		SetWheelIndicator ();
	}

	// Update is called once per frame
	void Update () {

		if (player.GetButton ("ArrowKeyUp")) {
			if(LastFrameWasFree) MoveWheel (+Offset);
			LastFrameWasFree = false;
			return;
		}

		if (player.GetButton ("ArrowKeyDown")) {
			if(LastFrameWasFree) MoveWheel (-Offset);
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

		if (player.GetButton ("Fire")) {
			if (LastFrameWasFree)
				Debug.Log (GetString ());
			LastFrameWasFree = false;
			return;
		}


		LastFrameWasFree = true;
	}

	void MoveWheel(float offset) {
		int nextLetter = currentLetter[currentWheel] + (offset > 0 ? 1 : -1);
		bool wheelExists = currentWheel >= 0 && currentWheel < wheels.Length;
		bool letterExists = nextLetter >= 0 && nextLetter < MaxLetters;
		//bool wrapsUp = nextLetter == 0 && currentLetter [currentWheel] == (MaxLetters-1);
		//bool wrapsDown = nextLetter == (MaxLetters-1) && currentLetter [currentWheel] == 0;
		if (wheelExists && letterExists) {
			//if(!wrapsUp && !wrapsDown)
				wheels [currentWheel].GetComponent<RectTransform> ().anchoredPosition += new Vector2 (0.0f, offset);
			//else 
			//	wheels [currentWheel].GetComponent<RectTransform> ().anchoredPosition += new Vector2 (0.0f, -offset*MaxLetters);
			
			currentLetter [currentWheel] = nextLetter;
		}
	}

	void ChangeWheel(int offset) {
		int nextWheel = currentWheel + offset;
		bool wheelExists = nextWheel >= 0 && nextWheel < wheels.Length;
		if (wheelExists) {
			currentWheel = nextWheel;
			lastLetter = Mathf.Max (lastLetter, currentWheel);
			wheels [currentWheel].SetActive (lastLetter >= nextWheel);
		}

		SetWheelIndicator ();
	}

	void SetWheelIndicator() {
		float newX = wheels [currentWheel].transform.position.x;
	
		Transform t = wheelIndicator.transform;
		t.position = new Vector3 (newX, t.position.y, t.position.z);
	}

	string GetString() {
		string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVZ";
		string s = "";
		for (int i = 1; i < wheels.Length; i++) {
			if (wheels [i].activeSelf) {
				s += Alphabet [currentLetter [i]];
			}
		}
		return s;
	}

}
