using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static Custom.Cus;
public class WinController : MonoBehaviour
{
    public static WinController instance;
    private void Awake() { instance = this; }
    public bool GameActive = false;
    public bool HasStarted = false;
    public float CountDownTime;
    public float CountDownTimer;
    public bool CountingDown;
    private void Start()
    {
        CountDownTimer = CountDownTime;
        HealthControl.instance.WaitingForPlayers.SetActive(true);
    }
    void Update()
    {
        if(PhotonNetwork.PlayerList.Length > 1 && CountingDown == false && HasStarted == false)
        {
            CountingDown = true;
            HasStarted = true;
            //could set position here, first frame active
        }
        if(CountingDown == true)
        {
            HealthControl.instance.StartingIn.text = "Starting In: " + CountDownTimer.ToString("f2");
            HealthControl.instance.StartingIn.gameObject.SetActive(true);
            HealthControl.instance.WaitingForPlayers.SetActive(false);
            CountDownTimer -= Time.deltaTime;
            if(CountDownTimer < 0)
            {
                StartGame();
            }
        }
    }
    public void StartGame()
    {
        HealthControl.instance.StartingIn.gameObject.SetActive(false);
        CountDownTimer = CountDownTime;
        GameActive = true;
        CountingDown = false;
    }
    public void TryOutCome(int MyHealth, int OtherHealth)
    {
        if(MyHealth < 0)
        {
            //i lose
            GameActive = false;
            HealthControl.instance.RestartButton.SetActive(true);
        }
        else if(OtherHealth < 0)
        {
            //i win
            GameActive = false;
            HealthControl.instance.RestartButton.SetActive(true);
        }

    }

    public void Restart()
    {
        GameActive = false;
        HasStarted = false;
        HealthControl.instance.RestartButton.SetActive(false);
        if (PhotonNetwork.PlayerList.Length > 1)
        {
            //normal restart
            SetPlayerInt(PlayerHealth, CharacterController.instance.MaxHealth, PhotonNetwork.PlayerList[0]);
            SetPlayerInt(PlayerHealth, CharacterController.instance.MaxHealth, PhotonNetwork.PlayerList[1]);
            CharacterController.instance.transform.position = NetworkManager.instance.Spawns[0].position;
            gameObject.GetComponent<PhotonView>().RPC("SetPosition", RpcTarget.Others, 1);
        }
        else if (PhotonNetwork.PlayerList.Length == 1)
        {
            //one player quit
        }

        //each player moves to spawn
        //set max health
        //wait time for start and countdown
    }

    //use rbc calls to trigger send restart to other person
}
