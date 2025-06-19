using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Unity.VisualScripting;
using UnityEngine.UI;
using TMPro;

namespace UnderdogCity
{
    public class GameManagerPhoton : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private NetworkCamera PlayerCamera;
        [SerializeField]
        private GameObject playerPrefab;
        [SerializeField]
        private Transform[] playerSpawnPoints;
        [HideInInspector]
        public GameObject LocalPlayer;

        private void Awake()
        {
            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene("ConnectionScene");
                return;
            }
        }

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => PhotonNetwork.InRoom == true);

            SpawnPlayer();
        }

        private void SpawnPlayer()
        {
            int playersOnScene = GameObject.FindObjectsByType<PlayerHealth>(sortMode: FindObjectsSortMode.None).Length;
            var spawnTransform = playerSpawnPoints[playersOnScene];
            LocalPlayer = PhotonNetwork.Instantiate(playerPrefab.name, spawnTransform.position, spawnTransform.rotation);
            PlayerCamera.SetTarget(LocalPlayer);
        }

        public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player target, ExitGames.Client.Photon.Hashtable changedProps)
        {
            foreach (var change in changedProps)
                Debug.Log("Property " + change.Key + " of player " + target.UserId + " changed to " + change.Value);
        }
    }
}