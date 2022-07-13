using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager instance;
    void Awake() { instance = this; }

    public TMP_InputField CreateInput;
    public TMP_InputField JoinInput;
    public GameObject LobbyItemPrefab;
    public Transform Content;
    public List<LobbyButton> LobbyList;

    public string CurrentRoom;

    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
        
        //PhotonNetwork.Connect
        Debug.Log("try connect to server");
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("connected to server");
        PhotonNetwork.JoinLobby();
    }
    private void Update()
    {
        if(PhotonNetwork.CurrentRoom != null)
            CurrentRoom = PhotonNetwork.CurrentRoom.Name;
    }

    
    //public override On
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("UpdateRoom: " + roomList.Count);
        UpdateRoomList(roomList);
        //base.OnRoomListUpdate(roomList);
    }
    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        foreach(LobbyButton item in LobbyList)
        {
            Destroy(item.gameObject);
        }
        LobbyList.Clear();

        foreach (RoomInfo room in roomList)
        {
            GameObject LobbyButton = Instantiate(LobbyItemPrefab, Content);
            LobbyButton.GetComponent<LobbyButton>().RoomName.text = room.Name;
            LobbyList.Add(LobbyButton.GetComponent<LobbyButton>());
        }
    }
    public void CreateRoom()
    {
        string RoomName = CreateInput.text;
        if(RoomName.Length >= 1)
        {
            PhotonNetwork.CreateRoom(RoomName, NetworkManager.RoomSettings());
            //SceneManager.LoadScene("BattleScene");
        }
        
    }

    public void JoinRoom(string Name)
    {
        PhotonNetwork.JoinRoom(Name);
    }

    public void JoinRoomAsText()
    {
        //PhotonNetwork.Get

        //if()
        JoinRoom(JoinInput.text);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("FailedToJoin");
        base.OnJoinRoomFailed(returnCode, message);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Successfully Joined Room");
        PhotonNetwork.LoadLevel("BattleScene");
        //SceneManager.LoadScene("BattleScene");
        base.OnJoinedRoom();
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("Successfully Created Room");
        PhotonNetwork.LoadLevel("BattleScene");
        //SceneManager.LoadScene("BattleScene");
        base.OnCreatedRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed To Created Room");
        base.OnCreateRoomFailed(returnCode, message);
    }
}
