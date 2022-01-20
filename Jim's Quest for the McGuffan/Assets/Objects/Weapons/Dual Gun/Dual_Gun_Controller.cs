using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dual_Gun_Controller : MonoBehaviour
{
    private Vector3 BOLT_OFFSET = new Vector3(0.5f, 0.05f, 0f);
    private Vector3 EFFECT_OFFSET = new Vector3(0f, 0f, 0f);
    private Vector3 OFFSET = new Vector3(0.5f, 0.05f, -2f);
    private Vector3 HELD_SCALE = new Vector3(2f, 2f, 1f);
    private string HURT_FIRE_AXIS = "Fire Hurt";
    private string HEAL_FIRE_AXIS = "Fire Heal";
    private string[] PLAYER_TAGS = {"Player Team", "Player Gun", "Player Heal Bolt", "Player Hurt Bolt"};
    private string[] ENEMY_TAGS = {"Enemy Team", "Enemy Gun", "Enemy Heal Bolt", "Enemy Hurt Bolt"};

    private Hurt_Bolt_Controller hurt_bolt_script;
    private Heal_Bolt_Controller heal_bolt_script;
    private Pick_Up_Glow_Controller effect_script;
    private float reload_time;
    private GameObject holder;
    private GameObject effect_object;
    private string[] tags = null;

    public GameObject HURT_BOLT_PREFAB;
    public GameObject HEAL_BOLT_PREFAB;
    public GameObject EFFECT_PREFAB;

    public bool held;
    public float hurt_fire_rate;
    public float heal_fire_rate;
    public GameObject bolt_manager;
    public float bolt_speed;
    public int hurt_bolt_damage = 1;
    public int heal_bolt_damage = 1;
    public string team_tag;
    public string team_hurt_tag;
    public string team_heal_tag;
    public string enemy_hurt_tag;
    public string enemy_heal_tag;

    void Start()
    {
        held = transform.parent != null;
        if (held)
        {
            holder = transform.parent.gameObject;
            calcHeldTag();
            transform.localPosition = OFFSET;
            reload_time = 0f;
        }
        else
        {
            reload_time = hurt_fire_rate;
            Vector3 pos = transform.position + EFFECT_OFFSET;
            effect_object = Instantiate(EFFECT_PREFAB, pos, transform.rotation);
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
                if (HURT_FIRE_AXIS == "" || Input.GetButton(HURT_FIRE_AXIS))
                {
                    reload_time = hurt_fire_rate;
                    fire_hurt();
                }
                else if (Input.GetButton(HEAL_FIRE_AXIS))
                {
                    reload_time = heal_fire_rate;
                    fire_heal();
                }
            }
            else
            {
                reload_time -= Time.deltaTime;
            }
        }
    }

    void fire_hurt()
    {
        ///fire a hurt bolt
        //create bolt
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        GameObject bolt = Instantiate(HURT_BOLT_PREFAB, pos, rot, bolt_manager.GetComponent<Transform>());

        //get bolt information
        hurt_bolt_script = bolt.GetComponent<Hurt_Bolt_Controller>();

        //configue bolt
        bolt.tag = team_hurt_tag;
        bolt.transform.localPosition += new Vector3(BOLT_OFFSET.x * bolt.transform.right.x, BOLT_OFFSET.y * bolt.transform.up.y, BOLT_OFFSET.z * bolt.transform.forward.z);
        hurt_bolt_script.damage = hurt_bolt_damage;
        hurt_bolt_script.speed = bolt_speed;
        hurt_bolt_script.coll_ignore_tags.Add(team_tag);
        hurt_bolt_script.coll_ignore_tags.Add(team_hurt_tag);
        hurt_bolt_script.coll_ignore_tags.Add(team_heal_tag);
        hurt_bolt_script.coll_ignore_tags.Add(enemy_hurt_tag);
        hurt_bolt_script.enemy_heal_tag = enemy_heal_tag;
    }

    void fire_heal()
    {
        ///fire a heal bolt
        //create bolt
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;
        GameObject bolt = Instantiate(HEAL_BOLT_PREFAB, pos, rot, bolt_manager.GetComponent<Transform>());

        //get bolt information
        heal_bolt_script = bolt.GetComponent<Heal_Bolt_Controller>();

        //configue bolt
        bolt.tag = team_heal_tag;
        bolt.transform.localPosition += new Vector3(BOLT_OFFSET.x * bolt.transform.right.x, BOLT_OFFSET.y * bolt.transform.up.y, BOLT_OFFSET.z * bolt.transform.forward.z);
        heal_bolt_script.damage = heal_bolt_damage;
        heal_bolt_script.speed = bolt_speed;
        heal_bolt_script.coll_ignore_tags.Add(team_tag);
        heal_bolt_script.coll_ignore_tags.Add(team_hurt_tag);
        heal_bolt_script.coll_ignore_tags.Add(team_heal_tag);
        heal_bolt_script.enemy_hurt_tag = enemy_hurt_tag;
        heal_bolt_script.coll_ignore_tags.Add(enemy_heal_tag);
        heal_bolt_script.bolt_manager = bolt_manager;
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
        calcHeldTag();
        Quaternion holder_rot = holder.GetComponent<Transform>().rotation;
        transform.localPosition = OFFSET;
        transform.rotation = holder_rot;
    }

    private void calcHeldTag(){
        const string PLAYER_TAG = "Player Team";
        const string ENEMY_TAG = "Enemy Team";
        switch (holder.tag){
            case PLAYER_TAG:
                tags = PLAYER_TAGS;
                break;
            case ENEMY_TAG:
                tags = ENEMY_TAGS;
                break;
        }
        gameObject.tag = tags[1];
    }
}
