using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class NameSelector : MonoBehaviour {

	public int playerId;
	public GameObject[] wheels;
	public float Offset = 42f;

	public int currentWheel = 0;
	public int currentLetter = 0;
	private Player player;
	private bool LastFrameWasFree = true;

	void Start() {
		player = ReInput.players.GetPlayer(playerId);
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


		LastFrameWasFree = true;
	}

	void MoveWheel(float offset) {
		int nextLetter = currentLetter + (offset > 0 ? 1 : -1);
		bool wheelExists = currentWheel >= 0 && currentWheel < wheels.Length;
		bool letterExists = nextLetter >= 0 && nextLetter < 26;
		if (wheelExists && letterExists) {
			wheels [currentWheel].GetComponent<RectTransform> ().anchoredPosition += new Vector2 (0.0f, offset);
			currentLetter = nextLetter;
		}
	}
}
