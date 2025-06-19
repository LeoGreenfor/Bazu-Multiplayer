using Photon.Pun;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private float health;

    private void Start()
    {
        health = 100f;
    }

    public void SetHealth(float Health)
    {
        health = Health;
    }

    public void TakeDamage(float Damage)
    {
        health -= Damage;
        if (health <= 0) 
        {
            GetComponent<PhotonView>().RPC("OnLowHealth", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    private void OnLowHealth()
    {
        gameObject.SetActive(false);
    }
}
