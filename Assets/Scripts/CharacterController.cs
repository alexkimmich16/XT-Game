using System.Collections;
using UnityEngine;
using static Custom.Cus;
using Photon.Pun;
using System.Collections.Generic;

public class CharacterController : MonoBehaviour
{
    public static CharacterController instance;
    private void Awake() { instance = this; }

    [SerializeField] float runspeed = 10f;
    [SerializeField] float walkForwardSpeed = 10f;
    [SerializeField] float walkBackSpeed = 10f;
    [SerializeField] float ArialSpeed = 10f;
    [SerializeField] float jumpspeed = 10f;
    [SerializeField] float StartSpeed = 10f;
    [SerializeField] float CapSpeed = 10f;

    //[SerializeField] float JumpWaitTime = 2f;
    public Vector2 TrueSpeed;
    Rigidbody2D myyRigidbody;
    CapsuleCollider2D mycapsuleCollider2D;
    bool isalive = true;
    //public Transform Facing;
    private bool Jumping = false;
    private bool Crouching = false;
    private bool DetectingJumpLand = false;

    private float Size;

    private int CheckCount;
    private int CheckInterval = 3;

    public float XDrag;

    private bool CanTakeDamage = true;
    public float Invincibility;

    private bool TouchedA;
    private bool TouchedD;

    public int CurrentHealth;
    public int MaxHealth = 120;

    public GameObject Spawned;
    public GameObject Other;

    public List<Animator> anims;
    public void SetOther(GameObject other)
    {
        Other = other;
    }
    public void SetSpawned(GameObject SpawnedOBJ)
    {
        Spawned = SpawnedOBJ;
        if (NetworkManager.instance.ViewPlayer == false)
            Spawned.transform.GetChild(0).gameObject.SetActive(false);

        anims[1] = Spawned.transform.GetChild(0).GetComponent<Animator>();
    }
    public void TakeDamage(DamageInfo DamageStat)
    {
        Debug.Log("takedamage");
        string anim = "";
        if (DamageStat.HitType == AttackType.Low)
            anim = "MidHit";
        else if (DamageStat.HitType == AttackType.High)
            anim = "HighHit";
        Debug.Log("TakeDamage");
        foreach (Animator A in anims)
            A.Play(anim);

        CurrentHealth -= DamageStat.Damage;
        SetPlayerInt(PlayerHealth, CurrentHealth, PhotonNetwork.LocalPlayer);
        HealthControl.instance.UpdateHealth();
        if (CurrentHealth < 0)
            Die();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool HitBox = collision.transform.GetComponent<HitBoxControl>();
        Debug.Log("TakeDamage: " + CanTakeDamage + "  HitBox: " + HitBox);
        if (CanTakeDamage == true && collision.transform.GetComponent<HitBoxControl>() && collision.transform.parent.parent.GetComponent<PhotonView>().IsMine == true)
        {
            Debug.Log("hit");
            HitBoxControl control = collision.transform.GetComponent<HitBoxControl>();
            DamageInfo DamageStat = new DamageInfo();
            DamageStat.HitType = control.type;
            DamageStat.Damage = control.Damage;
            TakeDamage(DamageStat);
            StartCoroutine(InvincibilityWait());
        }
    }
    IEnumerator InvincibilityWait()
    {
        CanTakeDamage = false;
        yield return new WaitForSeconds(Invincibility);
        CanTakeDamage = true;
    }
    void Start()
    {
        Size = transform.localScale.x;
        myyRigidbody = GetComponent<Rigidbody2D>();
        anims.Add(GetComponent<Animator>());
        anims.Add(null);
        mycapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        CurrentHealth = MaxHealth;
    }
    Vector2 GetInput()
    {
        Vector2 input = Vector2.zero;
        if (Input.GetKey(KeyCode.A))
        {
            input.x = -1;
            if (DoubleClickControl.instance.LeftDoubleClick == true && FacingRight() == false)
                input.x = -2;
        }
            
        if (Input.GetKey(KeyCode.D))
        {
            input.x = 1;
            if (DoubleClickControl.instance.RightDoubleClick == true && FacingRight() == true)
                input.x = 2;
        }
            
        if (Input.GetKey(KeyCode.S))
            input.y = -1;
        if (Input.GetKey(KeyCode.W))
            input.y = 1;
         return input;
    }
    bool FacingRight()
    {
        float me = transform.position.x;
        float them = Other.transform.position.x;
        float Check = me - them;
        if (Check < 0)
            return true;
        else
            return false;
    }
    private void FixedUpdate()
    {
        if (CanMove() == false) { return; }

        Walk();
    }
    public bool CanMove()
    {
        bool HasSpawned = (Other != null);
        //Debug.Log(" Spawned: " + HasSpawned + " Spawned: " + WinController.instance.GameActive);
        if (HasSpawned && WinController.instance.GameActive == true)
            return true;
        else
            return false;
    }
    void Update()
    {
        if (CanMove() == false) { return; }

        Color tmp = GetComponent<SpriteRenderer>().color;
        if (CanTakeDamage == true)
            tmp.a = 1f;
        else
            tmp.a = 0.5f;

        GetComponent<SpriteRenderer>().color = tmp;
        if (Input.GetKeyDown(KeyCode.A))
            TouchedA = true;
        else if (Input.GetKeyDown(KeyCode.D))
            TouchedD = true;
        ConvertToAnimation(GetInput());
        FaceDirection(FacingRight());
        Die();

        if (Grounded() == false && DetectingJumpLand == false && Jumping == true)
            DetectingJumpLand = true;
        if (DetectingJumpLand = true && Grounded() == true && Jumping == true)
            JumpLand();
    }
    public bool AttacksActive()
    {
        if (anims[0].GetBool("LightPunch") == true || anims[0].GetBool("LightKick") == true || anims[0].GetBool("HeavyPunch") == true || anims[0].GetBool("HeavyKick") == true)
            return true;
        else
            return false;
    }
    void ConvertToAnimation(Vector2 Keys)
    {
        int KeyX = (int)Keys.x;
        if (FacingRight() == false)
        {
            foreach (Animator A in anims)
                A.SetFloat("Move", -KeyX);
        } 
        else
        {
            foreach (Animator A in anims)
                A.SetFloat("Move", KeyX);
        }
        foreach (Animator A in anims)
            A.SetBool("Grounded", Grounded());

        if(Input.GetKeyDown(KeyCode.UpArrow) && AttacksActive() == false)
        {
            foreach (Animator A in anims)
            {
                A.SetTrigger("LightPunch");
                A.SetTrigger("Attack");
            }
        }
            
        if (Input.GetKeyDown(KeyCode.DownArrow) && AttacksActive() == false)
        {
            foreach (Animator A in anims)
            {
                A.SetTrigger("LightKick");
                A.SetTrigger("Attack");
            }
        }
            
        if (Input.GetKeyDown(KeyCode.RightArrow) && AttacksActive() == false)
        {
            foreach (Animator A in anims)
            {
                A.SetTrigger("HeavyPunch");
                A.SetTrigger("Attack");
            }
        }
            
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (anims[0].GetBool("HeavyKick") == true || anims[0].GetCurrentAnimatorStateInfo(0).IsName("HeavyKick"))
            {
                foreach (Animator A in anims)
                    A.SetTrigger("HeavyKick2");
            }
            else if(AttacksActive() == false)
            {
                foreach (Animator A in anims)
                {
                    A.SetTrigger("HeavyKick");
                    A.SetTrigger("Attack");
                }
            }
                
        }
        CheckCount += 1;
        if(CheckCount > CheckInterval)
        {
            CheckCount = 0;
            if (Keys.y == 1 && Jumping == false && Grounded() == true)
                Jump();
        }

        if (Keys.y == -1)
            Crouching = true;
        else
            Crouching = false;
        foreach (Animator A in anims)
            A.SetBool("Crouch", Crouching);
    }
    void Jump()
    {
        Jumping = true;
        foreach (Animator A in anims)
            A.SetTrigger("IsJump");
        myyRigidbody.velocity += new Vector2(0f, jumpspeed);
    }
    void JumpLand()
    {
        foreach (Animator A in anims)
            A.SetBool("Jump", false);
        Jumping = false;
    }
    bool Grounded()
    {
        if (mycapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")) && myyRigidbody.velocity.y < 0.01f)
            return true;
        else
            return false;
    }
    void Walk()
    {
        TrueSpeed = myyRigidbody.velocity;
        if (Crouching) { return; }

        if(GetInput().x == 0)
        {
            AddDrag();
            return;
        }
        if (TouchedA == true)
        {
            TouchedA = false;
            myyRigidbody.velocity = new Vector2(-StartSpeed, myyRigidbody.velocity.y);
        }
            
        if (TouchedD == true)
        {
            TouchedD = false;
            myyRigidbody.velocity = new Vector2(StartSpeed, myyRigidbody.velocity.y);
        }



        float Speed = 0;
        if (Grounded() == false)
            Speed = ArialSpeed;
        else if (GetInput().x == 1)
            Speed = walkForwardSpeed;
        else if (GetInput().x == -1)
            Speed = walkBackSpeed;
        else if (GetInput().x == 2 || GetInput().x == -2)
            Speed = runspeed;
        Speed = Speed / 10;

        

        float NewVelocity = myyRigidbody.velocity.x + GetInput().x * Speed;
        Vector2 Pvelocity = new Vector2(NewVelocity, myyRigidbody.velocity.y);
        if (Pvelocity.x > CapSpeed)
            Pvelocity = new Vector2(CapSpeed, myyRigidbody.velocity.y);
        else if(Pvelocity.x < -CapSpeed)
            Pvelocity = new Vector2(-CapSpeed, myyRigidbody.velocity.y);
        myyRigidbody.velocity = Pvelocity;

    }
    void AddDrag()
    {
        float multiplier = 1.0f - XDrag * Time.fixedDeltaTime;
        if (multiplier < 0.0f)
            multiplier = 0.0f;
        myyRigidbody.velocity = new Vector2(myyRigidbody.velocity.x * multiplier, myyRigidbody.velocity.y);
    }
    void FaceDirection(bool IsLeft)
    {
        if (IsLeft)
            transform.localScale = new Vector2(Size, Size);
        else
            transform.localScale = new Vector2(-Size, Size);
        //bool playerhorizatanlspeed = Mathf.Abs(myyRigidbody.velocity.x) > Mathf.Epsilon;
    }
    void Die()
    {
        if (mycapsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("enemies")))
        {
            isalive = false;
            GetComponent<SpriteRenderer>().flipX = true;
            foreach (Animator A in anims)
                A.SetTrigger("dying");

            GetComponent<CapsuleCollider2D>().enabled = false;
            myyRigidbody.velocity = new Vector2(-6, 10f);
        }
    }
}
