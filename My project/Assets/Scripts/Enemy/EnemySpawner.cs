using Photon.Pun;
using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;
    [SerializeField]
    private float cooldown;

    private void Start()
    {
        StartCoroutine(SpawnCooldown());
    }

    private IEnumerator SpawnCooldown()
    {
        yield return new WaitForSeconds(cooldown);

        PhotonNetwork.InstantiateRoomObject(enemyPrefab.name, gameObject.transform.position, Quaternion.identity);
    }
}
