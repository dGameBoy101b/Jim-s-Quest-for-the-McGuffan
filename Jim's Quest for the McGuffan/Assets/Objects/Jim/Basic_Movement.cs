using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Movement : MonoBehaviour
{
    public float walk_speed;
    public float jump;
    public float air_walk_fac;
    public bool falling = true;

    private Rigidbody2D rb;
    private Transform tran;
    private float move_hori;
    private Vector2 velocity;
    private bool left_face;

    void Start()
    {
        Physics2D.IgnoreLayerCollision(9, 10, true);
        rb = GetComponent<Rigidbody2D>();
        tran = GetComponent<Transform>();
        left_face = tran.localScale.x < 0;
    }

    void FixedUpdate()
    {
        //calculate falling condition of player
        velocity = rb.velocity;
        falling = ! ((velocity.normalized.x == tran.right.x
        && velocity.normalized.y == tran.right.y)
        || (velocity.normalized.x == tran.right.x * -1
        && velocity.normalized.y == tran.right.y * -1)
        || velocity.normalized == Vector2.zero);

        //attempt jump
        if (Input.GetButtonDown("Jump") && ! falling)
        {
            rb.AddForce(new Vector2(0f, jump), ForceMode2D.Impulse);
        }

        move_hori = Input.GetAxisRaw("Horizontal");
        //correct facing direction
        if (move_hori != 0.0f)
        {
            left_face = move_hori < 0;
        }
        if ((left_face && tran.localEulerAngles.y < 90f) || ((! left_face) && tran.localEulerAngles.y > 90f))
        {
            tran.Rotate(0, 180, 0);
        }

        //attempt horizontal movement
        if (falling)
        {
            rb.velocity = new Vector2(move_hori * air_walk_fac * walk_speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(move_hori * walk_speed, rb.velocity.y);
        }
    }
}
