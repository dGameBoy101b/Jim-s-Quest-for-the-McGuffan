using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal_Bolt_Controller : MonoBehaviour
{
    public float speed;
    public int damage;
    public string enemy_hurt_tag;
    public GameObject bolt_manager;
    public List<string> coll_ignore_tags;

    private Rigidbody2D rb;
    private Transform tran;
    private Vector2 move;
    private Vector2 pos;
    private int id;
    private int other_id;
    private Bolt_Manager bolt_repo;

    void Start()
    {
        coll_ignore_tags.Add(enemy_hurt_tag);
        rb = GetComponent<Rigidbody2D>();
        tran = GetComponent<Transform>();
        move = tran.right * speed;
        id = gameObject.GetInstanceID();
        bolt_repo = bolt_manager.GetComponent<Bolt_Manager>();
        bolt_repo.addBolt(id, damage);
    }

    void FixedUpdate()
    {
        //destroy self if damage < 1
        if (damage < 1)
        {
            Debug.Log("heal bolt destroying self: "+id);
            bolt_repo.deleteBolt(id);
            Destroy(gameObject);
        }

        // move to new position
        pos = tran.position;
        rb.MovePosition(move * Time.deltaTime + pos);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        //reduce damage if collide with enemy hurt bolt
        if (coll.gameObject.tag == enemy_hurt_tag)
        {
            other_id = coll.gameObject.GetInstanceID();
            Debug.Log("collided with enemy hurt bolt: "+other_id);
            Debug.Log("read other bolt damage: "+bolt_repo.readBolt(other_id));
            damage -= bolt_repo.readBolt(other_id);
            bolt_repo.changeBolt(id, damage);
        }

        //destroy gameObject if collide with non-team member
        bool ignore = false;
        for (int i=0; i<coll_ignore_tags.Count; i++)
        {
            if (coll.gameObject.tag == coll_ignore_tags[i])
            {
                ignore = true;
                break;
            }
        }
        if (!ignore)
        {
            bolt_repo.deleteBolt(id);
            Destroy(gameObject);
        }
    }
}
