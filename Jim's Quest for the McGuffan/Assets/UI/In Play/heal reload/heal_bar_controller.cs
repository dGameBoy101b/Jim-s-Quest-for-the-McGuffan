using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heal_bar_controller : MonoBehaviour
{
    public GameObject FRONT;
    public GameObject BACK;
    public int LENGTH;

    public Heal_Gun_Controller gun_script;

    private GameObject front;
    private GameObject back;

    void Start(){
        back = Instantiate(BACK, transform);
        back.transform.localPosition = new Vector3(0, BACK.GetComponent<SpriteRenderer>().bounds.extents.y - Screen.height / 2, 2);
        back.transform.localScale = new Vector3(LENGTH, 1, 1);
        front = Instantiate(FRONT, transform);
        front.transform.localPosition = new Vector3(0, BACK.GetComponent<SpriteRenderer>().bounds.extents.y - Screen.height / 2, 3);
        front.transform.localScale = new Vector3(LENGTH, 1, 1);
    }

    void LateUpdate(){
        front.GetComponent<Transform>().localScale = new Vector3(LENGTH * gun_script.reload_time / gun_script.fire_rate, 1, 1);
    }
}
