using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class PressStartChangeScene : MonoBehaviour {

	public enum State {
		TitleAppearing, TitleAppeared,
		NameSelectionAppearing, NameSelectionAppeared
	}

	public FadeComponent Title;
	public FadeComponent Press;

	public State state;



	private Player player1, player2;
	private bool initRewired = false;

	void Start() {
		state = State.TitleAppearing;
		Title.Fade (0f, 1f, 1f, 3f);
		Press.Fade (0f, 1f, 5f, 1f);

		ChangeState (State.TitleAppeared, 5f);
		InitializeRewired ();
	}

	void Update () {
		switch (state) {
		case State.TitleAppearing:
			break;
		case State.TitleAppeared:
			WaitInputForNameSelection ();
			break;

		}
	}


	private void InitializeRewired() {
		if (!initRewired) {
			player1 = ReInput.players.GetPlayer (0);
			player2 = ReInput.players.GetPlayer (1);

			initRewired = true;
		}
	}


	void WaitInputForNameSelection() {
		if(!ReInput.isReady) return;
		InitializeRewired();

		if (player1.GetAnyButton() || player2.GetAnyButton()) {
			state = State.NameSelectionAppearing;
			Title.Fade (1f, 0f, 0f, 1f);
			Press.Fade (1f, 0f, 0f, 1f);

			ChangeState (State.NameSelectionAppeared, 5f);
		}
	}

	void ChangeState(State nextState, float delay) {
		StartCoroutine(DoChangeState(nextState, delay));
	}

	IEnumerator DoChangeState(State nextState, float delay)
	{
		yield return new WaitForSeconds (delay);
		state = nextState;
	}
}
