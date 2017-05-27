
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 
using Random = UnityEngine.Random;
using System.Collections.Generic;
using Photon;


namespace Com.Gnula3d.Multiuser
{
	public class Launcher : Photon.PunBehaviour {

		#region Public Variables

		[Tooltip("The Ui Panel to let the user enter name, connect and play")]
		public GameObject controlPanel;

		[Tooltip("The Ui Text to inform the user about the connection progress")]
		public Text feedbackText;

		[Tooltip("The maximum number of players per room")]
		public byte maxPlayersPerRoom = 19;
		public string room;

		[SerializeField]
		private Transform[] spawnPoint;
		private GameObject ConnectButtonGO;
		[SerializeField]
		private Text ConnectingText;

		int positionNumber;
		int newPos;
		int numberPlayers;

		#endregion

		#region Private Variables
		bool isConnecting;

		string _gameVersion = "1";

		#endregion

		#region MonoBehaviour CallBacks

		void Awake()
		{
			PhotonNetwork.autoJoinLobby = false;
			PhotonNetwork.automaticallySyncScene = true;
		}

		#endregion


		#region Public Methods

		public void Connect()
		{
			feedbackText.text = "";

			isConnecting = true;

			controlPanel.SetActive(false);


			if (PhotonNetwork.connected)
			{
				LogFeedback("Joining Room...");
				PhotonNetwork.JoinRandomRoom();
				PhotonNetwork.JoinRandomRoom(null,(byte)(2));

			}else{

				LogFeedback("Connecting...");

				PhotonNetwork.ConnectUsingSettings(_gameVersion);
			}
		}

		void LogFeedback(string message)
		{
			if (feedbackText == null) {
				return;
			}
			feedbackText.text += System.Environment.NewLine+message;
		}

		#endregion


		#region Photon.PunBehaviour CallBacks

		public override void OnConnectedToMaster()
		{

			Debug.Log("Region:"+PhotonNetwork.networkingPeer.CloudRegion);

			if (isConnecting)
			{
				LogFeedback("OnConnectedToMaster: Next -> try to Join Random Room");
				Debug.Log("DemoAnimator/Launcher: OnConnectedToMaster() was called by PUN. Now this client is connected and could join a room.\n Calling: PhotonNetwork.JoinRandomRoom(); Operation will fail if no room found");
				PhotonNetwork.JoinRandomRoom();
			}
		}

		public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
		{
			LogFeedback("<Color=Red>OnPhotonRandomJoinFailed</Color>: Next -> Create a new Room");
			Debug.Log("DemoAnimator/Launcher:OnPhotonRandomJoinFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom(null, new RoomOptions() {maxPlayers = 4}, null);");

			PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = this.maxPlayersPerRoom}, null);
		}

		public override void OnDisconnectedFromPhoton()
		{
			LogFeedback("<Color=Red>OnDisconnectedFromPhoton</Color>");
			Debug.LogError("DemoAnimator/Launcher:Disconnected");

			isConnecting = false;
			controlPanel.SetActive(true);

		}

		public override void OnJoinedRoom()
		{
			LogFeedback("<Color=Green>OnJoinedRoom</Color> with ");
			Debug.Log("DemoAnimator/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");

			PhotonNetwork.LoadLevel(room);

		


		}
		}

		#endregion

	}
