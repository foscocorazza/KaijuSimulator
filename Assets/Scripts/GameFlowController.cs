using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class GameFlowController : MonoBehaviour {

	private static GameFlowController _instance;
	public static GameFlowController Instance() {
		if(_instance == null)
			_instance = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameFlowController> ();
		return _instance;
	}

	public enum State {
		TitleAppearing, TitleAppeared,
		NameSelectionAppearing, NameSelectionAppeared,
		PregameScreenAppearing, PregameScreenAppeared
	}

	public FadeComponent Title;
	public FadeComponent Press;
	public FadeComponent Player1Title;
	public FadeComponent Player2Title;
	public FadeComponent NameScreen;

	private Player player1, player2;
	private bool initRewired = false;
	private State state;

	public NameSelector nameSelectorPlayer1;
	public NameSelector nameSelectorPlayer2;

	void Start() {
		state = State.TitleAppearing;
		/*Title.Fade (0f, 1f, 1f, 3f);
		Press.Fade (0f, 1f, 5f, 1f);

		ChangeState (State.TitleAppeared, 5f);*/

		Title.Fade (0f, 1f, 0f, 0.1f);
		Press.Fade (0f, 1f, 0f, 0.1f);
		ChangeState (State.TitleAppeared, 0f);
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
			ChangeState (State.TitleAppearing, 0f);

			Title.Fade (1f, 0f, 0f, 1f);
			Press.Fade (1f, 0f, 0f, 1f);
			NameScreen.Fade (0f, 1f, 1f, 1f);

			ChangeState (State.NameSelectionAppeared, 1f);
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


	public void PrepareGame() {
		string[] names = FeatureGenerator.MixNames (
			nameSelectorPlayer1.GetString (),
			nameSelectorPlayer2.GetString ());

		Player1Title.Fade (1f, 0f, 0f, 1f);
		Player2Title.Fade (1f, 0f, 0f, 1f);

		nameSelectorPlayer1.HideSecondHalf ();
		nameSelectorPlayer2.HideFirstHalf ();

		PlayerPrefs.SetString("PlayerName", names[0]);
		PlayerPrefs.SetString("EnemyName", names[1]);

	
	}

}
