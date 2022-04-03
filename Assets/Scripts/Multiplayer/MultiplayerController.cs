using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerController : MonoBehaviourPunCallbacks
{
    public GameObject SpawnedPlayerPrefab;
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        SpawnedPlayerPrefab = PhotonNetwork.Instantiate("Character", Position(), transform.rotation);
        SpawnedPlayerPrefab.name = "My Player";
        CharacterController.instance.SetSpawned(SpawnedPlayerPrefab);
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.Destroy(SpawnedPlayerPrefab);
    }
    public Vector3 Position()
    {
        return NetworkManager.instance.Spawns[PhotonNetwork.PlayerList.Length - 1].position;
    }
}
