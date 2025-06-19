using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class NetworkConnectionManager : MonoBehaviourPunCallbacks
    {

        public Button BtnConnectRandomRoom;
        public Button BtnCreateRoom;
        public Button BtnConnectRoom;

        protected bool TriesToConnectToMaster;
        protected bool TriesToConnectToRoom;

        private string _createCode;
        private string _joinCode;

        void Start()
        {
            DontDestroyOnLoad(this);
            TriesToConnectToMaster = false;
            TriesToConnectToRoom = false;

            BtnConnectRandomRoom.interactable = false;
            BtnCreateRoom.interactable = false;
            BtnConnectRoom.interactable = false;

            BtnConnectRandomRoom.onClick.AddListener(OnClickConnectToRandomRoom);
            BtnConnectRoom.onClick.AddListener(OnClickConnectToRoom);
            BtnCreateRoom.onClick.AddListener(OnClickCreateARoom);
            ConnectToMaster();
        }

        public void ConnectToMaster()
        {
            TriesToConnectToMaster = true;

            //Settings (all optional and only for tutorial purpose)
            PhotonNetwork.OfflineMode = false;           //true would "fake" an online connection
            PhotonNetwork.NickName = "PlayerName";       //to set a player name
            PhotonNetwork.AutomaticallySyncScene = true; //to call PhotonNetwork.LoadLevel()
            PhotonNetwork.GameVersion = "v1";            //only people with the same game version can play together

            //PhotonNetwork.ConnectToMaster(ip,port,appid); //manual connection
            if (!PhotonNetwork.OfflineMode)
                PhotonNetwork.ConnectUsingSettings();           //automatic connection based on the config file in Photon/PhotonUnityNetworking/Resources/PhotonServerSettings.asset

        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            TriesToConnectToMaster = false;
            Debug.Log("Connected to Master!");

            BtnConnectRandomRoom.interactable = true;
            BtnCreateRoom.interactable = true;
            BtnConnectRoom.interactable = true;
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            base.OnDisconnected(cause);
            TriesToConnectToMaster = false;
            TriesToConnectToRoom = false;
            Debug.Log(cause);
        }

        public void OnClickConnectToRandomRoom()
        {
            if (!PhotonNetwork.IsConnected)
                return;

            TriesToConnectToRoom = true;
            PhotonNetwork.JoinRandomRoom();               //Join a random Room     - Error: OnJoinRandomRoomFailed  
        }
        public void OnClickCreateARoom()
        {
            if (!PhotonNetwork.IsConnected)
                return;

            TriesToConnectToRoom = true;
            PhotonNetwork.CreateRoom(_createCode, new RoomOptions { MaxPlayers = 20 }); //Create a specific Room - Error: OnCreateRoomFailed
        }

        public void OnClickConnectToRoom()
        {
            if (!PhotonNetwork.IsConnected)
                return;

            TriesToConnectToRoom = true;
            PhotonNetwork.JoinRoom(_joinCode);   //Join a specific Room   - Error: OnJoinRoomFailed  
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            base.OnJoinRandomFailed(returnCode, message);
            //no room available
            //create a room (null as a name means "does not matter")
            PhotonNetwork.CreateRoom("NewRoom", new RoomOptions { MaxPlayers = 20 });
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            base.OnCreateRoomFailed(returnCode, message);
            Debug.Log(message);
            base.OnCreateRoomFailed(returnCode, message);
            TriesToConnectToRoom = false;
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            TriesToConnectToRoom = false;


            Debug.Log("Master: " + PhotonNetwork.IsMasterClient + " | Players In Room: " + PhotonNetwork.CurrentRoom.PlayerCount + " | RoomName: " + PhotonNetwork.CurrentRoom.Name + " Region: " + PhotonNetwork.CloudRegion);
            if (PhotonNetwork.IsMasterClient && SceneManager.GetActiveScene().name != "Main")
                PhotonNetwork.LoadLevel("Main");
        }

        public void SetRoomCreateCode(string code)
        {
            _createCode = code;
        }
        public void SetRoomJoinCode(string code)
        {
            _joinCode = code;
        }
}
