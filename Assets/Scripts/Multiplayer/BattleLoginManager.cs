using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class BattleLoginManager : MonoBehaviourPunCallbacks
{
    public GameObject SpawnedPlayerPrefab;
    public GameObject JoinButton;
    public static bool Spawned;
    void Start()
    {
        
    }
    public void SpawnButton()
    {
        SpawnedPlayerPrefab = PhotonNetwork.Instantiate("Character", Position(), transform.rotation);
        SpawnedPlayerPrefab.name = "My Player";
        CharacterController.instance.SetSpawned(SpawnedPlayerPrefab);
        JoinButton.SetActive(false);
        Spawned = true;
    }
    // Update is called once per frame
    void Update()
    {
        //CheckGameReady();
        HealthControl.instance.UpdateHealth();
    }
    public void CheckGameReady()
    {
        //PhotonView[] players = FindObjectsOfType<PhotonView>();
        //if(players)
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
