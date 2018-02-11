using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Rewired;
using TMPro;

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

	[Header("Title")]
	public FadeComponent Title;
	public FadeComponent Press;

	[Header("Name Select")]
	public FadeComponent NameScreen;
	public FadeComponent Flash;
	public FadeComponent PressStart;
	public FadeComponent Player1Title;
	public FadeComponent Player2Title;
	public FadeComponent Background1;
	public FadeComponent Background2;
	public NameSelector nameSelectorPlayer1;
	public NameSelector nameSelectorPlayer2;

	[Header("Pre-game")]
	public GameObject PreName;
	public GameObject FinalName;
	public GameObject PostName;
	public GameObject StartToContinue;
	public GameObject Guy;
	public string NextSceneName;

	private Player player1, player2;
	private bool initRewired = false;
	private State state;
	private bool ChangingScene = false;


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
		case State.PregameScreenAppeared:
			if (!ChangingScene) {
				if (player1.GetButton ("Start") || player2.GetButton ("Start")) {
					SceneManager.LoadScene (NextSceneName);
					ChangingScene = true;
				}
			}
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

		PlayerPrefs.SetString("PlayerName", names[0]);
		PlayerPrefs.SetString("EnemyName", names[1]);

		string name = names [0].Substring(0,1).ToUpper() + names [0].Substring(1,names [0].Length-1).ToLower();
		PreName.GetComponent<TextMeshProUGUI> ().text = RandomPrename();
		FinalName.GetComponent<TextMeshProUGUI> ().text = name;
		PostName.GetComponent<TextMeshProUGUI> ().text = RandomPostname();

		// Fade Titles
		PressStart.Fade (1f, 0f, 0f, 1f);
		Player1Title.Fade (1f, 0f, 0f, 1f);
		Player2Title.Fade (1f, 0f, 0f, 1f);
		/*Background1.Fade (1f, 0f, 0f, 1f);
		Background2.Fade (1f, 0f, 0f, 1f);*/

		// Hide Part of the Names
		nameSelectorPlayer1.HideSecondHalf ();
		nameSelectorPlayer2.HideFirstHalf ();

		// Wait and move to Top Up
		nameSelectorPlayer1.MoveTo (new Vector2(400, -178), 2f, 2f);
		nameSelectorPlayer2.MoveTo (new Vector2(-25, 271), 2f, 2f);

		// Flash (peak a bit before clash)
		Flash.Flash(4f);
		StartCoroutine(ActivateWithDelay (Guy, 4f, true));
		StartCoroutine(ActivateWithDelay (PreName, 4f, true));
		StartCoroutine(ActivateWithDelay (FinalName, 4f, true));
		StartCoroutine(ActivateWithDelay (PostName, 4f, true));
		StartCoroutine(ActivateWithDelay (StartToContinue, 4f, true));
		StartCoroutine(ActivateWithDelay (nameSelectorPlayer1.gameObject, 4f, false));
		StartCoroutine(ActivateWithDelay (nameSelectorPlayer2.gameObject, 4f, false));

		ChangeState (State.PregameScreenAppeared, 5f);
	
	}

	string RandomPrename() {
		string[] str = { 
			"The outstanding",
			"The terrifying",
			"The colossal",
			"The terrible",
			"The gargantuan",
			"The legendary",
			"The evil",
			"The deadly",
			"The scary",
			"The immense",
			"The towering",
			"The tremendous",
			"The monumental",
			"The immense",
			"The powerful",
			"The humongous",
			"The impressive",
			"The mighty",
			"The almighty",
			"The overruling",
			"The wicked",
			"The bulky",
			"The crushing"};
		return str [Random.Range (0, str.Length)];
	}

	string RandomPostname() {
		string[] str = { 
			"Returns!",
			"Strikes again!",
			"Destroys the city!",
			"Kills!",
			"Eradicates the city!",
			"Wipes out humanity!",
			"Knows no fear!",
			"Is angry!",
			"Will stomp the city!",
			"Exterminates!",
			"Annihilates everything!",
			"Wrecks the civilization!",
			"Ruins the environment!",
			"Sent from hell!",
			"Slays with enthusiasm!",
			"Obliterates!",
			"Eradicates the city!",
			"Tears the civilization!",
			"Goes berserk!",
			"Evokes damage!" };
		return str [Random.Range (0, str.Length)];
	}


	IEnumerator ActivateWithDelay(GameObject obj, float delay, bool value) {
		yield return new WaitForSeconds(delay);
       
        obj.SetActive (value);
        if (obj.tag == "PlayerScripts") obj.GetComponentInChildren<Camera>().enabled = false;
    }

}
