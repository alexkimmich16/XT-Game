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

    public TextMeshProUGUI Result;
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
    void PlayAnimation(AttackType attack)
    {
        Debug.Log("Anim: " + attack);
        
        if (attack == AttackType.High)
            CharacterController.instance.Other.transform.GetChild(0).GetComponent<Animator>().Play("HighHit");
        else if(attack == AttackType.Low)
            CharacterController.instance.Other.transform.GetChild(0).GetComponent<Animator>().Play("MidHit");
        //transform.position = NetworkManager.instance.Spawns[Spawn].position;
    }

    public void UpdateHealth()
    {
        
        float Max = CharacterController.instance.MaxHealth;
        int MyHealth = GetPlayerInt(PlayerHealth, PhotonNetwork.PlayerList[GetLocal()]);
        float PlayerHealthBar = MyHealth / Max;
        Sides[0].SetFillSize(PlayerHealthBar);
        if (PhotonNetwork.PlayerList.Length > 1 && Exists("Health", PhotonNetwork.LocalPlayer))
        {
            int EnemyHealth = GetPlayerInt(PlayerHealth, PhotonNetwork.PlayerList[GetOther()]);
            Debug.Log("MyHealth:  " + MyHealth + "  EnemyHealth:  " + EnemyHealth);
            float EnemyHealthBar = EnemyHealth / Max;
            Sides[1].SetFillSize(EnemyHealthBar);
            WinController.instance.TryOutCome(MyHealth, EnemyHealth);
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
        //UpdateHealth();
    }


    //set objective to each other
    //on one person leaving restart
    //
}
