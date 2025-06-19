using Photon.Pun;
using Photon.Pun.Demo.SlotRacer;
using Photon.Realtime;
using System;
using UnityEngine;

[Photon.Pun.PunRPC]
public class NetworkPlayer : MonoBehaviour, IPunObservable
{
    private PhotonView photonView;
    private CharacterController characterController;
    private Vector3 RemotePlayerPosition;
    private Quaternion RemotePlayerRotation;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        characterController = GetComponent<CharacterController>();

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
        if (LagDistance.magnitude > 1f)
        {
            transform.position = RemotePlayerPosition;
            LagDistance = Vector3.zero;
        }

        //ignore the y distance
        LagDistance.y = 0;

        if (LagDistance.magnitude < 0.11f)
        {
            characterController.Move(Vector3.zero);
        }
        else
        {
            Vector3 moveDir = new Vector3(LagDistance.normalized.x, 0f, LagDistance.normalized.z);

        }

        //Look Smooth
        characterController.transform.rotation = RemotePlayerRotation;
        characterController.transform.position = Vector3.MoveTowards(transform.position, RemotePlayerPosition, 1f);

    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            Vector3 pos = transform.position;
            stream.SendNext(pos);
            stream.SendNext(transform.rotation);
        }
        else
        {
            RemotePlayerPosition = (Vector3)stream.ReceiveNext();
            RemotePlayerRotation = (Quaternion)stream.ReceiveNext();
        }
    }
}
