using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heart_controller : MonoBehaviour
{
    public Sprite FULL_HEART;
    public Sprite EMPTY_HEART;

    public int min_health;
    public Health_Tracker health_script;

    private SpriteRenderer renderer;

    void Start(){
        renderer = gameObject.GetComponent<SpriteRenderer>();
    }

    void Update(){
        //empty health
        if (health_script.health < min_health
        && renderer.sprite == FULL_HEART){
            renderer.sprite = EMPTY_HEART;
        }
        //fill health
        if (health_script.health >= min_health
        && renderer.sprite == EMPTY_HEART){
            renderer.sprite = FULL_HEART;
        }
    }
}
