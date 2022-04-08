using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Custom.Cus;
using Photon.Pun;
using TMPro;

public class HealthControl : MonoBehaviour
{
    public static HealthControl instance;
    private void Awake(){ instance = this; }

    public List<Side> Sides;
    public Sprite Win;
    public Sprite Lose;
    public Sprite Empty;

    public GameObject RestartButton;
    public GameObject WaitingForPlayers;
    public TextMeshProUGUI StartingIn;
    [System.Serializable]
    public class Side
    {
        public List<GameObject> Dots;
        public Image Fill;
        public Image Icon;
        public void SetFillSize(float Set)
        {
            Fill.fillAmount = Set;
        }
    }

    [PunRPC]
    void SetPosition(int Spawn)
    {
        transform.position = NetworkManager.instance.Spawns[Spawn].position;
    }

    

    public void UpdateHealth()
    {
        float PlayerHealthBar = (float)CharacterController.instance.CurrentHealth / (float)CharacterController.instance.MaxHealth;
        Sides[0].SetFillSize(PlayerHealthBar);

        if(PhotonNetwork.PlayerList.Length > 1 && Exists("Health", PhotonNetwork.LocalPlayer))
        {
            float EnemyHealth = GetPlayerInt(PlayerHealth, PhotonNetwork.PlayerList[GetOther()]) / CharacterController.instance.MaxHealth;
            Debug.Log("OtherHealth: " + EnemyHealth);
            Sides[1].SetFillSize((float)EnemyHealth);
            WinController.instance.TryOutCome(CharacterController.instance.CurrentHealth, EnemyHealth);
        }
        
    }
    
    public void DisplayWin(int Side)
    {
        for(int i = 0; i > Sides[Side].Dots.Count; i++)
            if (Sides[Side].Dots[i] != Empty)
            {
                Sides[Side].Dots[i].GetComponent<Image>().sprite = Win;
                return;
            }
    }
    private void Start()
    {
        UpdateHealth();
    }


    //set objective to each other
    //on one person leaving restart
    //
}
