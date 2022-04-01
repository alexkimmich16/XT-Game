using UnityEngine;
using Photon.Pun;
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
    //private bool Ac
    void Start()
    {
        //PhotonNetwork.OfflineMode = false;
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
        roomOptions.MaxPlayers = 10;
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
        if (PhotonNetwork.InRoom == true)
        {
            HealthControl.instance.UpdateHealth();
        }

    }
}