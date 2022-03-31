using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static Custom.Cus;
public class NetworkManager : MonoBehaviourPunCallbacks
{
    #region Singleton + classes
    public static NetworkManager instance;
    void Awake() { instance = this; }

    #endregion
    public bool DebugScript = false;

    void Start()
    {
        PhotonNetwork.OfflineMode = false;
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
        
        if (DebugScript == true)
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
        
        //Debug.Log(PhotonNetwork.OfflineMode);

        SetPlayerInt("Kills", 5, PhotonNetwork.LocalPlayer);
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Kills", out object temp2))
        {
            int Num = (int)temp2;
            Debug.Log(Num);
        }
        //int Num = PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Kills", out object temp2);
        
        
        CharacterController.instance.Initialize(PhotonNetwork.PlayerList.Length - 1);
        if (Exists("try", null) == false)
        {
            SetGameBool("try", false);
            Debug.Log("Health0: " + GetGameBool("try"));
        }



        //Hashtable setPlayerKills = new Hashtable() { { "Kills", 0 } };
        
        //PhotonNetwork.LocalPlayer.SetCustomProperties(setPlayerKills);




        //Debug.Log((int)PhotonNetwork.LocalPlayer.CustomProperties["Kills"]);

        //Debug.Log(PhotonNetwork.PlayerList.Length);
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
        //Debug.Log(this.RoomReference.IsOffline);
    }
}
