using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick_Up_Glow_Controller : MonoBehaviour
{
    public GameObject item;
    private Heal_Gun_Controller heal_script;
    private Hurt_Gun_Controller hurt_script;
    private Dual_Gun_Controller dual_script;

    void Start()
    {
        heal_script = item.GetComponent<Heal_Gun_Controller>();
        hurt_script = item.GetComponent<Hurt_Gun_Controller>();
        dual_script = item.GetComponent<Dual_Gun_Controller>();
    }

    void Update()
    {
        if((heal_script != null && heal_script.held)
        || (dual_script != null && dual_script.held)
        || (hurt_script != null && hurt_script.held))
        {
            Destroy(gameObject);
        }
    }
}
