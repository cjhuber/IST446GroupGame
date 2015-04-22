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

	void OnMatchStarted(bool success, TurnBasedMatch match) {
		if(success) {
			Debug.Log ("Successfully Invited Someone");
		} else {
			Debug.Log ("Fucking dumbass");
		}
	}
}
