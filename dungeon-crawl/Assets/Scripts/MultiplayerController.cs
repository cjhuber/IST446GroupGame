using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;

public class MultiplayerController : MonoBehaviour {


	bool mWaitingForAuth = true;

	private const int MinOpponents = 1;
	private const int MaxOpponents = 7;
	const int Variant = 0;

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

	// Update is called once per frame
	void Update () {

	}

	void handleInvitation(Invitation v, bool b) {
		Debug.Log ("Handle Invite");
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

	void OnMatchStarted(bool success, TurnBasedMatch match) {
		if(success) {
			byte[] myData = null;
			Debug.Log ("Successfully Invited Someone");
			string whoIsNext = DecideWhoIsNext(match);

			PlayGamesPlatform.Instance.TurnBased.TakeTurn(match, myData, whoIsNext, (bool successPlay) => {
				if(successPlay) {
					Debug.Log ("Stuff turn done");
				} else {
					Debug.Log ("asdfasdfasdf");
				}
			});
		} else {
			Debug.Log ("Fucking dumbass");
		}
	}
}
