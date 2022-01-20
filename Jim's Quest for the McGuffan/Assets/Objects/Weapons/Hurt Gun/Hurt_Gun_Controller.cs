using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurt_Gun_Controller : MonoBehaviour
{
    private Transform tran;
    private Vector3 pos;
    private Quaternion rot;
    private Quaternion holder_rot;
    private GameObject bolt;
    private Hurt_Bolt_Controller bolt_script;
    private Pick_Up_Glow_Controller effect_script;
    private float reload_time;
    private GameObject holder;
    private GameObject effect_object;
    private Transform bolt_tran;

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
    public string team_hurt_tag;
    public string team_heal_tag;
    public string enemy_hurt_tag;
    public string enemy_heal_tag;

    void Start()
    {
        tran = GetComponent<Transform>();
        held = tran.parent != null;
        holder = tran.parent.gameObject;
        if (held)
        {
            calcHeldTag();
            tran.localPosition = offset;
            reload_time = 0f;
        }
        else
        {
            reload_time = fire_rate;
            pos = tran.position + effect_offset;
            effect_object = Instantiate(effect_prefab, pos, tran.rotation);
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
        pos = tran.position;
        rot = tran.rotation;
        bolt = Instantiate(bolt_prefab, pos, rot, bolt_manager.GetComponent<Transform>());

        //get bolt information
        bolt_script = bolt.GetComponent<Hurt_Bolt_Controller>();
        bolt_tran = bolt.GetComponent<Transform>();

        //configue bolt
        bolt.tag = team_hurt_tag;
        bolt_tran.localPosition += new Vector3(bolt_offset.x * bolt_tran.right.x, bolt_offset.y * bolt_tran.up.y, bolt_offset.z * bolt_tran.forward.z);
        bolt_script.damage = bolt_damage;
        bolt_script.speed = bolt_speed;
        bolt_script.coll_ignore_tags.Add(team_tag);
        bolt_script.coll_ignore_tags.Add(team_hurt_tag);
        bolt_script.coll_ignore_tags.Add(team_heal_tag);
        bolt_script.coll_ignore_tags.Add(enemy_hurt_tag);
        bolt_script.enemy_heal_tag = enemy_heal_tag;
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
        tran.SetParent(holder.GetComponent<Transform>());
        calcHeldTag();
        holder_rot = holder.GetComponent<Transform>().rotation;
        tran.localPosition = offset;
        tran.rotation = holder_rot;
    }

    private void calcHeldTag(){
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
    }
}
