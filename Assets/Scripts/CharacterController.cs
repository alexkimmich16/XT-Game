using System.Collections;
using UnityEngine;
using static Custom.Cus;
using Photon.Pun;

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
    Animator myanimator;
    CapsuleCollider2D mycapsuleCollider2D;
    bool isalive = true;
    public Transform Facing;
    private bool Jumping = false;
    private bool Crouching = false;
    private bool DetectingJumpLand = false;

    private float Size;

    private int CheckCount;
    private int CheckInterval = 3;

    public float XDrag;

    private bool CanTakeDamage = true;
    public float Invincibility;

    //public int MaxHealth;
    private int CurrentHealth;

    private bool FacingLeftSave;

    private bool TouchedA;
    private bool TouchedD;

    public static int MaxHealth = 120;

    public void Initialize(int Count)
    {
        //Debug.Log(PhotonNetwork.LocalPlayer.);
        //SetPlayerInt(PlayerHealth, MaxHealth, PhotonNetwork.LocalPlayer);
        
        
        //Debug.Log("Exists: " + Exists(PlayerHealth, PhotonNetwork.LocalPlayer));
        //Debug.Log("Health0: " + GetPlayerInt(PlayerHealth, PhotonNetwork.LocalPlayer));
        


        
        //Debug.Log("Team0: " + GetPlayerInt(PlayerTeam, PhotonNetwork.LocalPlayer));
        if (Count == 0)
        {
            //im first
            //Debug.Log("Health0: " + GetPlayerInt(PlayerHealth, PhotonNetwork.PlayerList[0]));
            //Debug.Log("Team0: " + GetPlayerInt(PlayerTeam, PhotonNetwork.PlayerList[0]));
        }
        else if(Count == 1)
        {
            //i'm secpond
            Debug.Log("Health0: " + GetPlayerInt(PlayerHealth, PhotonNetwork.PlayerList[0]));
            Debug.Log("Team0: " + GetPlayerInt(PlayerTeam, PhotonNetwork.PlayerList[0]));
            Debug.Log("Health1: " + GetPlayerInt(PlayerHealth, PhotonNetwork.PlayerList[1]));
            Debug.Log("Team1: " + GetPlayerInt(PlayerTeam, PhotonNetwork.PlayerList[1]));
        }
        
    }
    public void TakeDamage(int Take)
    {
        CurrentHealth -= Take;

        SetPlayerInt(PlayerHealth, CurrentHealth, PhotonNetwork.LocalPlayer);
        if (CurrentHealth < 0)
            Die();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (CanTakeDamage == true)
        {
            if (collision.transform.tag == "MidHit")
            {
                myanimator.Play("MidHit");
                StartCoroutine(InvincibilityWait());
            }
            else if (collision.transform.tag == "HighHit")
            {
                StartCoroutine(InvincibilityWait());
                myanimator.Play("HighHit");
            }
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
        myanimator = GetComponent<Animator>();
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
        float them = Facing.position.x;
        float Check = me - them;
        if (Check < 0)
            return true;
        else
            return false;
    }
    private void FixedUpdate()
    {
        Walk();
    }
    void Update()
    {

        

        if (!isalive) { return; }
        FacingLeftSave = FacingRight();
        //if (FacingLeftSave != FacingLeft())
        //DoubleClickControl.instance.DoubleClick = false;
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
        if (myanimator.GetBool("LightPunch") == true || myanimator.GetBool("LightKick") == true || myanimator.GetBool("HeavyPunch") == true || myanimator.GetBool("HeavyKick") == true)
            return true;
        else
            return false;
    }
    void ConvertToAnimation(Vector2 Keys)
    {
        int KeyX = (int)Keys.x;
        if (FacingRight() == false)
            myanimator.SetFloat("Move", -KeyX);
        else
            myanimator.SetFloat("Move", KeyX);

        myanimator.SetBool("Grounded", Grounded());

        if(Input.GetKeyDown(KeyCode.UpArrow) && AttacksActive() == false)
        {
            myanimator.SetTrigger("LightPunch");
            myanimator.SetTrigger("Attack");
        }
            
        if (Input.GetKeyDown(KeyCode.DownArrow) && AttacksActive() == false)
        {
            myanimator.SetTrigger("LightKick");
            myanimator.SetTrigger("Attack");
        }
            
        if (Input.GetKeyDown(KeyCode.RightArrow) && AttacksActive() == false)
        {
            myanimator.SetTrigger("HeavyPunch"); 
            myanimator.SetTrigger("Attack");
        }
            
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (myanimator.GetBool("HeavyKick") == true || myanimator.GetCurrentAnimatorStateInfo(0).IsName("HeavyKick"))
                myanimator.SetTrigger("HeavyKick2");
            else if(AttacksActive() == false)
            {
                myanimator.SetTrigger("HeavyKick");
                myanimator.SetTrigger("Attack");
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
        myanimator.SetBool("Crouch", Crouching);
    }
    void Jump()
    {
        Jumping = true;
        myanimator.SetTrigger("IsJump");
        myyRigidbody.velocity += new Vector2(0f, jumpspeed);
    }
    void JumpLand()
    {
        myanimator.SetBool("Jump", false);
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
            myanimator.SetTrigger("dying");

            //GetComponent<CapsuleCollider2D>().transform.Rotate(0, 0, 90);
            //myyRigidbody.bodyType = RigidbodyType2D.Kinematic;
            GetComponent<CapsuleCollider2D>().enabled = false;
            myyRigidbody.velocity = new Vector2(-6, 10f);

            //triger off and box colider ofset change -0.12 if used as feet checker
            //triger off and box colider ofset change -0.12 if used as feet checker
        }
    }
}
