using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_Tracker : MonoBehaviour
{
    public int MAX_HEALTH;
    public int health;
    public List<string> hurt_tags;
    public List<string> heal_tags;

    void Start()
    {
        health = MAX_HEALTH;
    }

    void OnCollisionEnter2D(Collision2D coll){
        OnTriggerEnter2D(coll.collider);
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        //detect damage source
        bool hurt = false;
        for (int i=0; i<hurt_tags.Count; i++)
        {
            if (hurt_tags[i] == coll.gameObject.tag)
            {
                hurt = true;
                break;
            }
        }
        //detect heal source
        bool heal = false;
        for (int i=0; i<heal_tags.Count; i++)
        {
            if (heal_tags[i] == coll.gameObject.tag)
            {
                heal = true;
                break;
            }
        }
        if (hurt && heal){
            //nothing
            hurt = false;
            heal = false;
        }
        if (hurt)
        {
            //take damage
            health -= coll.gameObject.GetComponent<Hurt_Bolt_Controller>().damage;
            if (health < 0)
            {
                health = 0;
            }
        }
        if (heal)
        {
            //heal damage
            health += coll.gameObject.GetComponent<Heal_Bolt_Controller>().damage;
            if (health > MAX_HEALTH)
            {
                health = MAX_HEALTH;
            }
        }
    }

    void Update(){
        if (health <= 0){
            //death
        }
    }
}
