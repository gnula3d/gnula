
#if UNITY_5 && (!UNITY_5_0 && !UNITY_5_1 && !UNITY_5_2 && !UNITY_5_3) || UNITY_6
#define UNITY_MIN_5_4
#endif

using UnityEngine;
using UnityEngine.EventSystems;

namespace Com.Gnula3d.Multiuser
{
	/// <summary>
	/// Player manager.
	/// Handles fire Input and Beams.
	/// </summary>
	public class PlayerManager : Photon.PunBehaviour
	{
		#region Public Variables

		[Tooltip("The Player's UI GameObject Prefab")]
		public GameObject PlayerUiPrefab;




		[Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
		public static GameObject LocalPlayerInstance;

		#endregion



		#region MonoBehaviour CallBacks

		public void Awake()
		{
			if (photonView.isMine)
			{
				LocalPlayerInstance = gameObject;
			}
			DontDestroyOnLoad(gameObject);
		}

		public void Start()
		{
			CameraWork _cameraWork = gameObject.GetComponent<CameraWork>();

			if (_cameraWork != null)
			{
				if (photonView.isMine)
				{
					_cameraWork.OnStartFollowing();
				}
			}
			else
			{
				Debug.LogError("<Color=Red><b>Missing</b></Color> CameraWork Component on player Prefab.", this);
			}

			// Create the UI
			if (this.PlayerUiPrefab != null)
			{
				GameObject _uiGo = Instantiate(this.PlayerUiPrefab) as GameObject;
				_uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
			}
			else
			{
				Debug.LogWarning("<Color=Red><b>Missing</b></Color> PlayerUiPrefab reference on player Prefab.", this);
			}

			#if UNITY_MIN_5_4
			// Unity 5.4 has a new scene management. register a method to call CalledOnLevelWasLoaded.
			UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
			#endif
		}


		public void OnDisable()
		{
			#if UNITY_MIN_5_4
			UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
			#endif
		}


		/// <summary>
		/// MonoBehaviour method called on GameObject by Unity on every frame.
		/// Process Inputs if local player.
		/// Show and hide the beams
		/// Watch for end of game, when local player health is 0.
		/// </summary>
		public void Update()
		{

		}



		#if !UNITY_MIN_5_4
		/// <summary>See CalledOnLevelWasLoaded. Outdated in Unity 5.4.</summary>
		void OnLevelWasLoaded(int level)
		{
		this.CalledOnLevelWasLoaded(level);
		}
		#endif


		/// <summary>
		/// MonoBehaviour method called after a new level of index 'level' was loaded.
		/// We recreate the Player UI because it was destroy when we switched level.
		/// Also reposition the player if outside the current arena.
		/// </summary>
		/// <param name="level">Level index loaded</param>
		void CalledOnLevelWasLoaded(int level)
		{
			// check if we are outside the Arena and if it's the case, spawn around the center of the arena in a safe zone
			if (!Physics.Raycast(transform.position, -Vector3.up, 5f))
			{
				transform.position = new Vector3(0f, 5f, 0f);
			}

			GameObject _uiGo = Instantiate(this.PlayerUiPrefab) as GameObject;
			_uiGo.SendMessage("SetTarget", this, SendMessageOptions.RequireReceiver);
		}

		#endregion

		#region Private Methods


		#if UNITY_MIN_5_4
		void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode loadingMode)
		{

			this.CalledOnLevelWasLoaded(scene.buildIndex);
		}


		/*
        #region IPunObservable implementation

		void IPunObservable.OnPhotonSerializeView (PhotonStream stream, PhotonMessageInfo info)
		{
			if (stream.isWriting)
			{
				// We own this player: send the others our data
				stream.SendNext(IsFiring);
				stream.SendNext(Health);
			}
            else
            {
				// Network player, receive data
				this.IsFiring = (bool)stream.ReceiveNext();
				this.Health = (float)stream.ReceiveNext();
			}
		}

        #endregion
        */

		#endif
		#endregion



	}
}