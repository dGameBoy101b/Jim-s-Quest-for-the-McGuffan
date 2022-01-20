using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_Control : MonoBehaviour
{
    private Animator anim;
    private Basic_Movement move;
    private Rigidbody2D rb;
    public bool walking;
    public float walk_speed;
    public bool falling;
    public float fall_speed;

    void Start()
    {
        anim = GetComponent<Animator>();
        move = GetComponent<Basic_Movement>();
        rb = GetComponent<Rigidbody2D>();
        walk_speed = move.walk_speed;
        anim.SetFloat("Walk Speed", walk_speed);
    }

    void Update()
    {
        walking = (! move.falling) && Input.GetAxisRaw("Horizontal") != 0;
        anim.SetBool("Walking", walking);

        if (Input.GetButtonDown("Jump")) anim.SetTrigger("Jumping");

        falling = move.falling;
        anim.SetBool("Falling", falling);

        fall_speed = rb.velocity.y;
        if (fall_speed < 0f) fall_speed *= -1;
        anim.SetFloat("Fall Speed", fall_speed);

    }
}
