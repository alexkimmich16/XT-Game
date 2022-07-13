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
    //public bool HasStarted = false;
    public float CountDownTime;
    public float CountDownTimer;
    public bool CountingDown;
    private bool AwaitingStart = true;
    private void Start()
    {
        CountDownTimer = CountDownTime;
        HealthControl.instance.WaitingForPlayers.SetActive(true);
    }

    public bool BothJoined()
    {
        PhotonView[] Players = FindObjectsOfType<PhotonView>();
        return Players.Length == 2;
    }
    void Update()
    {
        if(BothJoined() && CountingDown == false && AwaitingStart == true)
        {
            CountingDown = true;
            AwaitingStart = false;
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
    public void TryOutCome(int MyHealth, float OtherHealth)
    {
        if(MyHealth < 0 || OtherHealth < 0)
        {
            EndGame();
            if (MyHealth < 0)
            {
                HealthControl.instance.Result.text = "You Lose Dumbass!";
                HealthControl.instance.Result.gameObject.SetActive(true);
            }
            else if (OtherHealth < 0)
            {
                HealthControl.instance.Result.gameObject.SetActive(true);
                HealthControl.instance.Result.text = "You Win!";
            }
        }
    }
    public void EndGame()
    {
        GameActive = false;
        HealthControl.instance.RestartButton.SetActive(true);

        CharacterController.instance.CurrentHealth = CharacterController.instance.MaxHealth;
        SetPlayerInt(PlayerHealth, CharacterController.instance.CurrentHealth, PhotonNetwork.LocalPlayer);

        CharacterController.instance.transform.position = NetworkManager.instance.Spawns[0].position;
    }

    
    public void Restart()
    {
        AwaitingStart = true;
        HealthControl.instance.RestartButton.SetActive(false);
        HealthControl.instance.Result.gameObject.SetActive(false);
        if (PhotonNetwork.PlayerList.Length > 1)
        {
            //normal restart
            //Debug.Log("SentRestart");
            CharacterController.instance.Spawned.GetComponent<PhotonView>().RPC("Respawn", RpcTarget.All);
        }
        else if (PhotonNetwork.PlayerList.Length == 1)
        {
            //one player quit
        }

        //wait time for start and countdown
    }

    //use rbc calls to trigger send restart to other person
}
