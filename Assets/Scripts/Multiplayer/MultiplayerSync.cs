using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiplayerSync : MonoBehaviour
{
    public PhotonView photonView;
    public Transform NetworkPlayer;
    private float Size;
    private float CheckView;
    private void Start()
    {
        Size = transform.localScale.x;
        NetworkPlayer = CharacterController.instance.transform;
        
    }
    void Update()
    {
        if (photonView.IsMine)
        {
            //gameObject.SetActive(false);
            transform.position = NetworkPlayer.position;
        }
        FaceDirection(FacingRight());
    }
    void FaceDirection(bool IsLeft)
    {
        if (IsLeft)
            transform.localScale = new Vector2(Size, Size);
        else
            transform.localScale = new Vector2(-Size, Size);
        //bool playerhorizatanlspeed = Mathf.Abs(myyRigidbody.velocity.x) > Mathf.Epsilon;
    }
    bool FacingRight()
    {
        float me = transform.position.x;
        float them = CharacterController.instance.Spawned.transform.position.x;
        float Check = me - them;
        CheckView = Check;
        if (Check < 0)
            return true;
        else
            return false;
    }

}
