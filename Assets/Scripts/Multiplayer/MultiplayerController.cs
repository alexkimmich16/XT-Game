using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerController : MonoBehaviourPunCallbacks
{
    private GameObject SpawnedPlayerPrefab;
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        SpawnedPlayerPrefab = PhotonNetwork.Instantiate("Character", transform.position, transform.rotation);
        SpawnedPlayerPrefab.name = "My Player";
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(SpawnedPlayerPrefab);
    }

}
