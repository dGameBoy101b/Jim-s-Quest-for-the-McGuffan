using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baddie_Controller : MonoBehaviour
{
    public int health = 5;
    public float speed;
    public float turn_time;

    private float timer = 0f;
    private Transform tran;
    private Rigidbody2D rb;
    private bool falling;

    void Start()
    {
        tran = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        falling = (rb.velocity.y < 0f);
        //patrol
        if (!falling)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            if (speed == 0f && turn_time > 0f)
            {
                timer += Time.fixedDeltaTime;
                if (timer > turn_time)
                {
                    timer = 0f;
                    tran.Rotate(0, 180, 0);
                    speed *= -1;
                }
            }
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        bool wall = false;
        Vector2 angle;
        for (int i = 0; i < coll.contactCount; i++)
        {
            angle = coll.GetContact(i).normal.normalized;
            //detect wall
            wall = wall
            || (angle == new Vector2(1, 0)
            || angle == new Vector2(-1, 0))
            && (coll.gameObject.tag=="Ground"
            || coll.gameObject.tag=="AI Ground");
        }
        if (wall)
        {
            tran.Rotate(0, 180, 0);
            speed *= -1;
        }
    }
}
