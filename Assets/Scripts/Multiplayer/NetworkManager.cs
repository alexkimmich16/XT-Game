using UnityEngine;
using Photon.Pun;
using System.Collections.Generic;
using Photon.Realtime;
using static Custom.Cus;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Singleton + classes
    public static NetworkManager instance;
    void Awake() { instance = this; }

    #endregion
    public bool DebugScript = false;
    private bool FirstRoomFrame = true;
    //public bool ViewPlayer;
    public List<Transform> Spawns;
    
    private int LastCount;
    public static RoomOptions RoomSettings()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        roomOptions.IsVisible = true;
        roomOptions.IsOpen = true;
        //roomOptions. = true;
        return roomOptions;
    }
    void Start()
    {

    }
    public override void OnConnectedToMaster()
    {
        if (DebugScript == true)
            Debug.Log("connected to server");
        base.OnConnectedToMaster();
        //RoomSettings()
        //JoinRandomRoom();
        //PhotonNetwork.JoinOrCreateRoom(GetRoomJoin(), roomOptions, TypedLobby.Default);


    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        if(DebugScript == true)
        {
            //Debug.Log(message + returnCode);
            Debug.Log(" failed to join random game");
        }

        string RoomName = "Room " + PhotonNetwork.CountOfRooms + 1;
    }
    public override void OnJoinedRoom()
    {
        if (DebugScript == true)
            Debug.Log("joined a room");

        //SetPlayerInt(PlayerHealth, );

        //CharacterController.instance.Initialize(PhotonNetwork.PlayerList.Length - 1);
        base.OnJoinedRoom();
    }
    
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        //if (DebugScript == true)
           // Debug.Log("a new player joined");
        
        base.OnPlayerEnteredRoom(newPlayer);
    }

    private void Update()
    {
        if (BattleLoginManager.Spawned == false)
            return;

        if (FirstRoomFrame == true)
        {
            OnFirstMultiplayerFrame();
            FirstRoomFrame = false;
        }
        int Players = PlayerCount();
        if (LastCount == 2 && Players == 1)
        {
            //player left
            WinController.instance.EndGame();
        }
        else if (LastCount == 1 && Players == 2)
        {
            //player joined
            Debug.Log(GetEnemyPlayer().name);
            CharacterController.instance.SetOther(GetEnemyPlayer());
            HealthControl.instance.UpdateHealth();
        }
        LastCount = Players;
    }
    public int PlayerCount()
    {
        PhotonView[] Players = FindObjectsOfType<PhotonView>();
        return Players.Length;
    }
    public void OnFirstMultiplayerFrame()
    {
        SetPlayerInt(PlayerHealth, CharacterController.instance.CurrentHealth, PhotonNetwork.LocalPlayer);
        SetPlayerBool(Invincible, true, PhotonNetwork.LocalPlayer);

        CharacterController.instance.transform.position = Spawns[PhotonNetwork.PlayerList.Length - 1].position;
        StartCoroutine(WaitingForOther());
    }

    IEnumerator WaitingForOther()
    {
        while (GetEnemyPlayer() == null)
        {
            yield return new WaitForSeconds(0.1f);
        }
        
        CharacterController.instance.SetOther(GetEnemyPlayer());

        HealthControl.instance.UpdateHealth();
        GetPlayer().transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
    }

    public GameObject GetEnemyPlayer()
    {
        PhotonView[] Players = FindObjectsOfType<PhotonView>();
        //Debug.Log(Players.Length);
        for (int i = 0; i < Players.Length; i++)
        {
            //Debug.Log(Players.Length);
            if (Players[i].IsMine == false)
            {
                return Players[i].gameObject;
            }
        }
        
        return null;
    }
    public GameObject GetPlayer()
    {
        PhotonView[] Players = FindObjectsOfType<PhotonView>();
        //Debug.Log(Players.Length);
        for (int i = 0; i < Players.Length; i++)
        {
            //Debug.Log(Players.Length);
            if (Players[i].IsMine == true)
            {
                return Players[i].gameObject;
            }
        }
        return null;
    }
}