using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DimensionCollapse;

//武器范围测试
public class Temp_Scarecrow : MonoBehaviour
{

    public float health;
    // Use this for initialization
    void Start()
    {
        health = 200;
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0)
        {
            health = 0;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            this.health = 200;
        }
    }
    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
		 Debug.Log("OnCollisionEnter 调用！");
        if (other.gameObject.GetComponent<Bullet>() != null)
        {
            Debug.Log("伤害值" + other.gameObject.GetComponent<Bullet>().getDamage());
            this.health -= other.gameObject.GetComponent<Bullet>().getDamage();
        }
    }
    /// <summary>
    /// OnGUI is called for rendering and handling GUI events.
    /// This function can be called multiple times per frame (one call per event).
    /// </summary>
    void OnGUI()
    {
        Rect rect = new Rect(50, 0, 100, 50);
        GUI.TextField(rect, "剩余" + health + "血量");
    }
}
