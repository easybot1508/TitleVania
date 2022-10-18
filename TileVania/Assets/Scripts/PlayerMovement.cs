using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D rb2D;
    [SerializeField] float speed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 10f;
    [SerializeField] Vector2 deathKick = new Vector2(20f, 20f);
    [SerializeField] GameObject bullets;
    [SerializeField] Transform gun;
    float gravityScaleStart;
    Animator playerAnimatior;
    CapsuleCollider2D myCapsuleCollider2D;
    BoxCollider2D myBoxcollider2D;
    bool isAlive = true;
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        playerAnimatior = GetComponent<Animator>();
        myCapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        myBoxcollider2D = GetComponent<BoxCollider2D>();
        gravityScaleStart = rb2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            return;//khong lam` bat cu dieu` gi` nua~
        }
        Run();
        FlipSprite();
        Climbing();
        Die();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive)
        {
            return;//khong lam` bat cu dieu` gi` nua~
        }
        moveInput = value.Get<Vector2>();       
    }

    void OnJump(InputValue value)
    {
        if (!isAlive)
        {
            return;//khong lam` bat cu dieu` gi` nua~
        }
        //kiem tra neu nguoi` choi ko cham vao` lop Ground thi` player se~ ko dc nhay , neu player cham dat thi` moi dc nhay?
        if (!myBoxcollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        
        if (value.isPressed)
        {
            rb2D.velocity += new Vector2(0f, jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive)
        {
            return;//khong lam` bat cu dieu` gi` nua~
        }

        Instantiate(bullets, gun.position, transform.rotation);
    }

    void Run()
    {
            Vector2 playerVelocity = new Vector2(moveInput.x * speed, rb2D.velocity.y);//rb2D.velocity.y giu~ nguyen van. toc' hien tai. cua? no tren truc. y
            rb2D.velocity = playerVelocity;
            //kiem tra xem player co di chuyen hay khong
            //neu co thi` hoat. anh? inRunning se hoat. dong.
            bool playerHasHorizontalSpeed = Mathf.Abs(rb2D.velocity.x) > Mathf.Epsilon;
            playerAnimatior.SetBool("isRunning", playerHasHorizontalSpeed);
       
       
    }

    //nguoi` choi quay theo huong di chuyen
    void FlipSprite()
    {
        //kiem? tra playerHasHorizontalSpeed co di chuyen hay khong (neu di chuyen thi` van toc'. se~ > 0)
        //nhun neu di chuyen sang ben trai gia tri. se~ la` gia' tri. am
        //de khac phuc dieu do chung ta can` dung` Mathf.Abs(Mathf.Abs du` co la` gia tri. am hay duong thi` no van~ tra? ve` gia tri. duong)
        //Mathf.Epsilon  giup chung ta lay den gia tri. nho? nhat (vd: 0.00001f)
        bool playerHasHorizontalSpeed = Mathf.Abs(rb2D.velocity.x) > Mathf.Epsilon;
        if (playerHasHorizontalSpeed)
        {
            //neu nguoi` choi di chuyen? chung ta se~ gan gia' tri. cua? localScale bang voi 1 Vector2
            //Mathf.Sign o day neu gia tri. duong hoac. 0 thi` no se~ tra? ve` gia tri. la` 1 neu gia tri. am no se~ tra? la` -1
            //o day chung ta giu~ nguyen gia tri. tren truc y thay doi gia tri. tren truc. x
            transform.localScale = new Vector2(Mathf.Sign(rb2D.velocity.x), 1f);
        }
        
    }

    void Climbing()
    {
        if (!myBoxcollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            rb2D.gravityScale = gravityScaleStart;
            playerAnimatior.SetBool("isClimbing", false);
            return;
        }        
        Vector2 playerClimbing = new Vector2(rb2D.velocity.x, moveInput.y * climbSpeed);
        rb2D.velocity = playerClimbing;
        rb2D.gravityScale = 0f;

        bool playerHasVerticalVeloity = Mathf.Abs(rb2D.velocity.y) > Mathf.Epsilon;
        playerAnimatior.SetBool("isClimbing", playerHasVerticalVeloity);

    }
     void Die()
    {
        if (myBoxcollider2D.IsTouchingLayers(LayerMask.GetMask("Enemy" ,"Hazard")))
        {
            isAlive = false;
            playerAnimatior.SetTrigger("Dying");
            rb2D.velocity = deathKick;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        }
    }

   
}
