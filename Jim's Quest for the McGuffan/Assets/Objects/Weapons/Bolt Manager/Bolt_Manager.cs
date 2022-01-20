using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bolt_Manager : MonoBehaviour
{
    public IDictionary<int, int> bolt_repos = new Dictionary<int, int>();

    public void addBolt(int id, int damage)
    {
        //add id and damage to list of bolts as key-value pair
        bolt_repos.Add(id, damage);
    }

    public int readBolt(int id)
    {
        //return damage of given bolt
        foreach (int key in bolt_repos.Keys)
        {
            Debug.Log("an existing key: "+key);
        }
        Debug.Log("attempting to read bolt: "+id);
        return bolt_repos[id];
    }

    public void deleteBolt(int id)
    {
        //remove key-value pair given key
        bolt_repos.Remove(id);
    }

    public void changeBolt(int id, int damage)
    {
        Debug.Log("trying to change bolt: "+id);
        deleteBolt(id);
        addBolt(id, damage);
    }
}
