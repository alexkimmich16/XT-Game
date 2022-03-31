using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiplayerSync : MonoBehaviour
{
    public PhotonView photonView;
    public Hashtable Info;
    private Transform NetworkPlayer;
    private void Start()
    {
        NetworkPlayer = GameObject.Find("MyCharacter").transform;
        
    }
    void Update()
    {
        if (photonView.IsMine)
        {
            gameObject.SetActive(false);
            transform.position = NetworkPlayer.position;
        }
    }
}
