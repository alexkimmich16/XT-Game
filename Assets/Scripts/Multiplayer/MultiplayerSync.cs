using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MultiplayerSync : MonoBehaviourPunCallbacks, IPunObservable
{
    public Transform NetworkPlayer;
    private float Size;
    private float CheckView;
    public Animator animator;
    public int Move;
    
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {

        //Debug.Log("111");
        if(animator != null)
        {
            //Debug.Log("222");
            if (stream.IsWriting)
            {
                //This is our player, we need to send our actual position to network
                stream.SendNext(animator.GetBool("IsJump"));
                stream.SendNext(animator.GetBool("LightPunch"));
                stream.SendNext(animator.GetBool("LightKick"));
                stream.SendNext(animator.GetBool("HeavyPunch"));
                stream.SendNext(animator.GetBool("HeavyKick"));
                stream.SendNext(animator.GetBool("HeavyKick2"));
                stream.SendNext(animator.GetBool("Attack"));

            }
            else
            {
                //Here we make sure other players receive the sent animations of my player, through the network.
                animator.SetBool("IsJump", (bool)stream.ReceiveNext());
                animator.SetBool("LightPunch", (bool)stream.ReceiveNext());
                animator.SetBool("LightKick", (bool)stream.ReceiveNext());
                animator.SetBool("HeavyPunch", (bool)stream.ReceiveNext());
                animator.SetBool("HeavyKick", (bool)stream.ReceiveNext());
                animator.SetBool("HeavyKick2", (bool)stream.ReceiveNext());
                animator.SetBool("Attack", (bool)stream.ReceiveNext());
            }
        }
    }
   
 
  
    private void Start()
    {
        Size = transform.localScale.x;
        NetworkPlayer = CharacterController.instance.transform;
        animator = transform.GetChild(0).GetComponent<Animator>();
    }
    void Update()
    {
        if (photonView.IsMine)
        {
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
