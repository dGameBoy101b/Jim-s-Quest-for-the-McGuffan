using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal_Gun_Controller : MonoBehaviour
{
    private Vector3 pos;
    private Quaternion rot;
    private Quaternion holder_rot;
    private GameObject bolt;
    private Heal_Bolt_Controller bolt_script;
    private Pick_Up_Glow_Controller effect_script;
    private GameObject holder;
    private GameObject effect_object;
    private Transform bolt_transform;
    private GameObject reload_bar;

    public bool held;
    public Vector3 offset;
    public Vector3 bolt_offset;
    public Vector3 effect_offset;
    public string fire_axis = "";
    public float fire_rate;
    public GameObject bolt_manager;
    public GameObject bolt_prefab;
    public GameObject effect_prefab;
    public float bolt_speed;
    public int bolt_damage = 1;
    public string team_tag;
    public string team_heal_tag;
    public string team_hurt_tag;
    public string enemy_heal_tag;
    public string enemy_hurt_tag;
    public GameObject reload_bar_prefab;

    public float reload_time;

    void Start()
    {
        held = transform.parent != null;
        if (held)
        {
            holder = transform.parent.gameObject;
            gameObject.tag = holder.tag;
            CalcHeldTag();
            transform.localPosition = offset;
            reload_time = 0f;
        }
        else
        {
            reload_time = fire_rate;
            pos = transform.position + effect_offset;
            effect_object=Instantiate(effect_prefab, pos, transform.rotation);
            effect_script = effect_object.GetComponent<Pick_Up_Glow_Controller>();
            effect_script.item = gameObject;
        }
    }

    void Update()
    {
        //detect holder destruction
        if (held)
        {
            if (! holder)
            {
                Destroy(gameObject);
            }
            //attempt fire
            if (reload_time <= 0f)
            {
                if (fire_axis == "" || Input.GetButton(fire_axis))
                {
                    reload_time = fire_rate;
                    fire();
                }
            }
            else
            {
                reload_time -= Time.deltaTime;
            }
        }
    }

    void fire()
    {
        ///fire a bolt
        //create bolt
        pos = transform.position;
        rot = transform.rotation;
        bolt = Instantiate(bolt_prefab, pos, rot, bolt_manager.GetComponent<Transform>());

        //get bolt information
        bolt_script = bolt.GetComponent<Heal_Bolt_Controller>();
        bolt_transform = bolt.GetComponent<Transform>();

        //configue bolt
        bolt.tag = team_heal_tag;
        bolt_transform.localPosition += new Vector3(bolt_offset.x * bolt_transform.right.x, bolt_offset.y * bolt_transform.up.y, bolt_offset.z * bolt_transform.forward.z);
        bolt_script.damage = bolt_damage;
        bolt_script.speed = bolt_speed;
        bolt_script.coll_ignore_tags.Add(team_tag);
        bolt_script.coll_ignore_tags.Add(team_hurt_tag);
        bolt_script.coll_ignore_tags.Add(team_heal_tag);
        bolt_script.enemy_hurt_tag = enemy_hurt_tag;
        bolt_script.coll_ignore_tags.Add(enemy_heal_tag);
        bolt_script.bolt_manager = bolt_manager;
    }

    void OnTriggerEnter2D(Collider2D coll)
    {
        //attempt pickup
        if ((! held) && coll.gameObject.tag == "Player Team")
        {
            pickup(coll);
        }
    }
    void pickup(Collider2D coll)
    {
        held = true;
        holder = coll.gameObject;
        transform.SetParent(holder.GetComponent<Transform>());
        CalcHeldTag();
        holder_rot = holder.GetComponent<Transform>().rotation;
        transform.localPosition = offset;
        transform.rotation = holder_rot;
    }

    private void CalcHeldTag(){
        switch (holder.tag){
            case "Player Team":
                gameObject.tag = "Player Gun";
                break;
            case "Enemy Team":
                gameObject.tag = "Enemy Gun";
                break;
            default:
                gameObject.tag = holder.tag;
                break;
        }
        reload_bar = Instantiate(reload_bar_prefab, Camera.main.transform);
        reload_bar.GetComponent<heal_bar_controller>().gun_script = this;
        reload_bar.GetComponent<Canvas>().worldCamera = Camera.main;
    }
}
