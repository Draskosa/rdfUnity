using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    private Vector3 moveDirection;
    private Vector3 velocity;

    [SerializeField] private bool isGrounded;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity;

    [SerializeField] private float jumpHeight;

    //REFERENCES
    private CharacterController controller;
    private Animator anim;
    public HUD Hud;
    private bool showinfo = false;
    private string oeuvre = "";
    // Start is called before the first frame update
    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
        if (Input.GetKeyDown(KeyCode.M) && showinfo)
        {
            if (!Hud.InfoPanel.activeSelf)
            {
                Hud.OpenInfoPanel(oeuvre);
            }
            else
            {
                Hud.CloseInfoPanel();
            }
        }
    }

    private void Move()
    {
        isGrounded = Physics.CheckSphere(transform.position,groundCheckDistance, groundMask);

        if(isGrounded && velocity.y <0 )
        {
            velocity.y = -2f;
        }
        float moveZ = Input.GetAxis("Vertical");

        moveDirection = new Vector3(0,0,moveZ);
        moveDirection = transform.TransformDirection(moveDirection); 

        if (isGrounded)
        {

            if (moveDirection != Vector3.zero && !Input.GetKey(KeyCode.LeftShift))
            {
                //Walk
                Walk();

            }
            else if (moveDirection != Vector3.zero && Input.GetKey(KeyCode.LeftShift))
            {
                //Run
                Run();
            }
            else if (moveDirection == Vector3.zero)
            {
                //Idle
                Idle();
            }

            moveDirection *= moveSpeed;

            if(Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

        }
        controller.Move(moveDirection * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }


    private void Idle() 
    {
        anim.SetFloat("AnimState", 0);
    }

    private void Walk()
    {
        moveSpeed = walkSpeed;
        anim.SetFloat("AnimState", 1);

    }

    private void Run()
    {
        moveSpeed = runSpeed;
        anim.SetFloat("AnimState", 2);
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        anim.SetFloat("AnimState", 3);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "paint")
        {
            Hud.OpenMessagePanel("");
            oeuvre = other.name;
            showinfo = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        Hud.CloseMessagePanel();
        showinfo = false;
        if (Hud.InfoPanel.activeSelf)
        {
            Hud.CloseInfoPanel();
        }
    }

}
