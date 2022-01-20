using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health_monitor : MonoBehaviour
{

    public GameObject HEART;

    public Health_Tracker health_script;

    private float heart_width;
    private float heart_height;
    private Vector3 top_left;

    void Start()
    {
        //initialise privates
        heart_width = HEART.GetComponent<SpriteRenderer>().bounds.size.x;
        heart_height = HEART.GetComponent<SpriteRenderer>().bounds.size.y;
        Vector3[] corners = new Vector3[4];
        gameObject.GetComponent<RectTransform>().GetLocalCorners(corners);
        for (int i = 0; i < 4; i++){
            if (corners[i].x < 0f
            && corners[i].y > 0f){
                top_left = corners[i];
                break;
            }
        }
        //initialise full health bar
        GameObject new_heart;
        for (int i = 0; i < health_script.MAX_HEALTH; i++){
            new_heart = Instantiate(HEART, transform);
            new_heart.transform.localPosition = new Vector3((i + 0.5f) * heart_width + top_left.x, top_left.y - (heart_height / 2f), 0f);
            new_heart.GetComponent<heart_controller>().min_health = i + 1;
            new_heart.GetComponent<heart_controller>().health_script = health_script;
        }
    }
}
