using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;

public class MultiplayerController : MonoBehaviour {


	bool mWaitingForAuth = true;

	private const int MinOpponents = 1;
	private const int MaxOpponents = 7;
	const int Variant = 0;

	private TurnBasedMatch currentMatch;

	private bool firstTurn = true;
	private bool isPlayerTwo = false;

	// Use this for initialization
	void Start () {
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
			.EnableSavedGames()
			.WithInvitationDelegate(handleInvitation)
			.WithMatchDelegate(handleTurnBasedNotification)
				.Build ();

		PlayGamesPlatform.InitializeInstance(config);
		PlayGamesPlatform.DebugLogEnabled = true;

		GooglePlayGames.PlayGamesPlatform.Activate();

//		if(!Social.localUser.authenticated) {
//			mWaitingForAuth = true;
//			Social.localUser.Authenticate((bool success) => {
//				mWaitingForAuth = false;
//				if(success) {
//					Debug.Log ("Success in authenticating");
//				} else {
//					Debug.Log ("Did not authenticate successfully");
//				}
//			});
//		} else {
//			PlayGamesPlatform.Instance.TurnBased.CreateWithInvitationScreen(MinOpponents, MaxOpponents, Variant, OnMatchStarted);
//		}
	}

	// Update is called once per frame
	void Update () {

	}

	public void onPlayClick() {
		Debug.Log ("Inside onPlayClick");
		if(!Social.localUser.authenticated) {
			mWaitingForAuth = true;
			Social.localUser.Authenticate((bool success) => {
				mWaitingForAuth = false;
				if(success) {
					Debug.Log ("Success in authenticating");
				} else {
					Debug.Log ("Did not authenticate successfully");
				}
			});
		} else {
			PlayGamesPlatform.Instance.TurnBased.CreateWithInvitationScreen(MinOpponents, MaxOpponents, Variant, OnMatchStarted);
		}
	}

	void handleInvitation(Invitation invitation, bool shouldAutoAccept) {
		Debug.Log ("Handle Invite");
		isPlayerTwo = true;
		PlayGamesPlatform.Instance.TurnBased.AcceptInvitation(invitation.InvitationId, OnMatchStarted);
	}

	void handleTurnBasedNotification(TurnBasedMatch t, bool b) {
		Debug.Log ("Handle Turn Notification");
	}

	private string DecideWhoIsNext(TurnBasedMatch match) {
		if(match.AvailableAutomatchSlots > 0) {
			return null;
		}
		foreach(Participant p in match.Participants) {
			if(!p.ParticipantId.Equals(match.SelfParticipantId)) {
				return p.ParticipantId;
			}
		}

		return null;
	}

	private string GetOtherPlayerID(TurnBasedMatch match) {
		foreach(Participant p in match.Participants) {
			if(!p.ParticipantId.Equals(match.SelfParticipantId)) {
				return p.Player.PlayerId;
			}
		}
		return null;
	}
	public void acceptInvite() {
		Debug.Log ("Looking Invites");
		PlayGamesPlatform.Instance.TurnBased.AcceptFromInbox(OnMatchStarted);
	}

	public void takeTurn() {
		Debug.Log ("Inside Take Turn");
		var score = PlayerPrefs.GetInt("score");

		byte[] scoreInBytes = BitConverter.GetBytes(score);

		string whoIsNext = DecideWhoIsNext(currentMatch);

		PlayGamesPlatform.Instance.TurnBased.TakeTurn(currentMatch, scoreInBytes, whoIsNext, (bool success) => {
			if(success) {
				Debug.Log ("Turn taken successfully");
			}
		});

	}

	void OnMatchStarted(bool success, TurnBasedMatch match) {
		if(success) {
//			byte[] myData = null;

			currentMatch = match;

			if(isPlayerTwo) {
				Debug.Log ("Player two setup");
				var pOneID = GetOtherPlayerID(match);
				var pTwoID = match.Self.Player.PlayerId;
//				var pOneID = DecideWhoIsNext(match);
//				var pTwoID = match.SelfParticipantId;
				PlayerPrefs.SetString("pOneID", pOneID);
				PlayerPrefs.SetString ("pTwoID", pTwoID);
				
				PlayerPrefs.SetString("playerId", pTwoID);
			} else if(firstTurn) {
				Debug.Log ("Player one setup");
				firstTurn = false;
				var pOneID = match.Self.Player.PlayerId;
				var pTwoID = GetOtherPlayerID(match);
//				var pOneID = match.SelfParticipantId;
//				var pTwoID = DecideWhoIsNext(match);
				PlayerPrefs.SetString("pOneID", pOneID);
				PlayerPrefs.SetString("pTwoID", pTwoID);

				PlayerPrefs.SetString("playerId", pOneID);

			}

			Debug.Log ("Successfully Invited Someone");
//			string whoIsNext = DecideWhoIsNext(match);
			GameObject mpController = GameObject.Find("MPController");
			GameObject.DontDestroyOnLoad(mpController);
			Application.LoadLevel("one");
//			PlayGamesPlatform.Instance.TurnBased.TakeTurn(match, myData, whoIsNext, (bool successPlay) => {
//				if(successPlay) {
//					Debug.Log ("Stuff turn done");
//				} else {
//					Debug.Log ("asdfasdfasdf");
//				}
//			});
		} else {
			Debug.Log ("Fucking dumbass");
		}
	}
}
