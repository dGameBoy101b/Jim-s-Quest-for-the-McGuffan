using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie_Melee : MonoBehaviour
{

    public int damage = 1;
    public List<string> targ_tag;
    public float cooldown = 0.2f;

    private float timer = 0f;

    void FixedUpdate(){
        if (timer > 0f){
            timer -= Time.fixedDeltaTime;
        }
        if (timer < 0f){
            timer = 0f;
        }
    }

    void OnCollisionStay2D(Collision2D coll){
        string tag = coll.gameObject.tag;
        for (int i = 0; i < targ_tag.Count; i++){
            if (tag == targ_tag[i]
            && timer <= 0f){
                damageOther(coll.gameObject);
            }
        }
    }

    void damageOther(GameObject other){
        timer = cooldown;
        Health_Tracker health_track = other.GetComponent<Health_Tracker>();
        health_track.health -= damage;
    }
}
