using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiplayerSync : MonoBehaviourPunCallbacks, IPunObservable
{
    public PhotonView photonView;
    private Transform NetworkPlayer;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //if (stream.IsWriting)
            //stream.SendNext(Health);
        //else
            //Health = (int)stream.ReceiveNext();
    }
    private void Start()
    {
        NetworkPlayer = GameObject.Find("MyCharacter").transform;
        
    }
    void Update()
    {
        if (photonView.IsMine)
        {
            //gameObject.SetActive(false);
            transform.position = NetworkPlayer.position;
        }
    }

    
}
