using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;
using static Custom.Cus;
using Hashtable = ExitGames.Client.Photon.Hashtable;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Singleton + classes
    public static NetworkManager instance;
    void Awake() { instance = this; }

    #endregion
    public bool DebugScript = false;
    private bool FirstRoomFrame = false;
    private bool AlreadyInRoom = false;
    public bool ViewPlayer;
    public List<Transform> Spawns;
    void Start()
    {
        ConnectToServer();
    }
    void ConnectToServer()
    {
        PhotonNetwork.ConnectUsingSettings();
        if (DebugScript == true)
        {
            //Debug.Log("try connect to server");
        }
    }
    public override void OnConnectedToMaster()
    {
        //if (DebugScript == true)
            //Debug.Log("connected to server");
        base.OnConnectedToMaster();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;

        PhotonNetwork.JoinOrCreateRoom("Room 1", roomOptions, TypedLobby.Default);
    }
    public override void OnJoinedRoom()
    {
        if (DebugScript == true)
            Debug.Log("joined a room");

        //SetPlayerInt(PlayerHealth, );

        CharacterController.instance.Initialize(PhotonNetwork.PlayerList.Length - 1);
        base.OnJoinedRoom();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        if (DebugScript == true)
            Debug.Log("a new player joined");
        
        base.OnPlayerEnteredRoom(newPlayer);
    }

    private void Update()
    {
        
        if (PhotonNetwork.PlayerList.Length == 1)
        {
            WinController.instance.GameActive = false;
            WinController.instance.HasStarted = false;
        }
        else
        {
            //CharacterController.instance.Other = GetEnemyPlayer();
            CharacterController.instance.SetOther(GetEnemyPlayer());
            HealthControl.instance.UpdateHealth();
        }
        
        if (FirstRoomFrame == true)
        {
            OnFirstMultiplayerFrame();
            FirstRoomFrame = false;
        }
        
        if (PhotonNetwork.InRoom == true && FirstRoomFrame == false && AlreadyInRoom == false)
        {
            FirstRoomFrame = true;
            AlreadyInRoom = true;
        }
        
        
        if (FirstRoomFrame == true && AlreadyInRoom == false)
        {
            FirstRoomFrame = true;
            AlreadyInRoom = false;
        }
    }
    public void OnFirstMultiplayerFrame()
    {
        CharacterController.instance.SetOther(GetEnemyPlayer());
        SetPlayerInt(PlayerHealth, CharacterController.instance.CurrentHealth, PhotonNetwork.LocalPlayer);
        if (PhotonNetwork.PlayerList.Length == 1)
        {
            CharacterController.instance.transform.position = Spawns[0].position;
        }
            
        else
        {
            CharacterController.instance.transform.position = Spawns[1].position;
            //CharacterController.instance.Spawned.GetComponent<PhotonView>().RPC("SetObjective", RpcTarget.Others, CharacterController.instance.Spawned);
        }
            
        HealthControl.instance.UpdateHealth();
        if(GetPlayer() != null)
        {

        }
        GetPlayer().transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
    }

    public GameObject GetEnemyPlayer()
    {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        //Debug.Log(Players.Length);
        for (int i = 0; i < Players.Length; i++)
        {
            Debug.Log(Players.Length);
            if (Players[i].GetComponent<PhotonView>().IsMine == false)
            {
                return Players[i];
            }
        }
        
        return null;
    }
    public GameObject GetPlayer()
    {
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");
        //Debug.Log(Players.Length);
        for (int i = 0; i < Players.Length; i++)
        {
            Debug.Log(Players.Length);
            if (Players[i].GetComponent<PhotonView>().IsMine == true)
            {
                return Players[i];
            }
        }

        return null;
    }
}