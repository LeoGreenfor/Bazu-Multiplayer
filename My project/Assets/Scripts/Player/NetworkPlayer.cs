using Photon.Pun;
using Photon.Pun.Demo.SlotRacer;
using Photon.Realtime;
using System;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    private PhotonView photonView;
    private GameObject Player;
    private Vector3 RemotePlayerPosition;
    private Quaternion RemotePlayerRotation;
    private float RemoteLookX;
    private float RemoteLookZ;
    private float LookXVel;
    private float LookZVel;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();

        if (!photonView.IsMine)
        {
            Destroy(gameObject.GetComponent<PlayerController>());
        }
    }

    public void Update()
    {
        if (photonView.IsMine)
            return;

        var LagDistance = RemotePlayerPosition - transform.position;

        //High distance => sync is to much off => send to position
        if (LagDistance.magnitude > 5f)
        {
            transform.position = RemotePlayerPosition;
            LagDistance = Vector3.zero;
        }

        //ignore the y distance
        LagDistance.y = 0;

        bool isJumping = RemotePlayerPosition.y - transform.position.y > 0.2f;

        if (LagDistance.magnitude < 0.11f)
        {
            //Player is nearly at the point
            //Player.Input.RunX = 0;
            //Player.Input.RunZ = 0;
        }
        else
        {
            //Player has to go to the point
            //Player.Input.RunX = LagDistance.normalized.x;
            //Player.Input.RunZ = LagDistance.normalized.z;
        }

        //Look Smooth
        //Player.transform.eulerAngles = new Vector3(
        //    Mathf.SmoothDampAngle(Player.transform.eulerAngles.x, RemoteLookX, ref LookXVel, 0.2f),
        //    Player.transform.eulerAngles.y,
        //    Mathf.SmoothDamp(Player.Input.LookZ, RemoteLookZ, ref LookZVel, 0.2f););

        Player.transform.rotation = RemotePlayerRotation;

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(Convert.ToSingle(Input.GetKey(KeyCode.S)) * -1f + Convert.ToSingle(Input.GetKey(KeyCode.W)));
            stream.SendNext(Convert.ToSingle(Input.GetKey(KeyCode.A)) * -1f + Convert.ToSingle(Input.GetKey(KeyCode.D)));
            
        }
        else
        {
            RemotePlayerPosition = (Vector3)stream.ReceiveNext();
            RemotePlayerRotation = (Quaternion)stream.ReceiveNext();
            RemoteLookX = (float)stream.ReceiveNext();
            RemoteLookZ = (float)stream.ReceiveNext();
        }
    }
}
