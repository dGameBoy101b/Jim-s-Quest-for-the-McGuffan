using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Camera_Controller : MonoBehaviour
{
    public GameObject player;
    public float x_min = Mathf.NegativeInfinity;
    public float x_max = Mathf.Infinity;
    public float y_min = Mathf.NegativeInfinity;
    public float y_max = Mathf.Infinity;
    public bool user_restart;
    public bool auto_restart;

    private Vector3 offset;
    private float size;
    private float width_ratio;
    private Camera cam;
    private Vector3 adjust_pos;
    private Scene current_scene;
    private Vector3 player_pos;

    void Start()
    {
        //calculate variables
        offset = transform.position - player.transform.position;
        cam = GetComponent<Camera>();
        size = cam.orthographicSize;
        width_ratio = cam.aspect;

        //check public arguments
        if (x_max - x_min < 2 * size * width_ratio)
        {
             throw new System.ArgumentException("x_max and x_min must define a range large enough to fit the view port", "x_max - x_min");
        }
        if (y_max - y_min < 2 * size)
        {
             throw new System.ArgumentException("y_max and y_min must define a range large enough to fit the view port", "y_max - y_min");
        }
        current_scene = SceneManager.GetActiveScene();
    }
    void Update()
    {
        //quit game
        if (Input.GetButtonDown("Quit")){
            Application.Quit();
        }
        //restart current scene
        if (current_scene.isLoaded
        && (user_restart && Input.GetButtonDown("Restart")
        || auto_restart && restart()))
        {
            auto_restart = false;
            user_restart = false;
            SceneManager.LoadScene(current_scene.path);
        }
    }

    void LateUpdate()
    {
        //maintain camera distance from player
        transform.position = player.transform.position + offset;

        adjust_pos = new Vector3(0, 0, transform.position.z);

        //correct camera x position to within bounds
        if (transform.position.x < x_min + size * width_ratio)
        {
            adjust_pos.x = x_min + size * width_ratio;
        }
        else
        {
            if (transform.position.x > x_max - size * width_ratio)
            {
                adjust_pos.x = x_max - size * width_ratio;
            }
            else
            {
                adjust_pos.x = transform.position.x;
            }
        }

        //correct camera y position to within bounds
        if (transform.position.y < y_min + size)
        {
            adjust_pos.y = y_min + size;
        }
        else
        {
            if (transform.position.y > y_max - size)
            {
                adjust_pos.y = y_max - size;
            }
            else
            {
                adjust_pos.y = transform.position.y;
            }
        }

        transform.position = adjust_pos;
    }
    bool restart()
    {
        player_pos = player.GetComponent<Transform>().position;
        bool pit = player_pos.x > x_max || player_pos.x < x_min || player_pos.y > y_max || player_pos.y < y_min;
        bool killed = player.GetComponent<Health_Tracker>().health == 0;
        return pit || killed;
    }
}
