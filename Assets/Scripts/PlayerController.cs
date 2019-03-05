using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed;
    public Transform gunPivot;
    public Transform gun;
    public DashAbility dashLogic;
    public Health hp;

    public float swordDmg, colDmg;


    public Transform bullet;

    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // rotate gun
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDelta = mouseWorld - gunPivot.position;
        float angle = Mathf.Atan2(mouseDelta.y, mouseDelta.x) * 180 / Mathf.PI;
        gunPivot.rotation = Quaternion.Euler(0, 0, angle);

        if (Input.GetButtonDown("Fire1"))
        {
            Instantiate(bullet, gun.position, gun.rotation);
        }
    }

    void FixedUpdate()
    {
        if (!dashLogic.dashing)
        {
            float hSpeed = Input.GetAxis("Horizontal");
            float vSpeed = Input.GetAxis("Vertical");
            Vector2 v = new Vector2(hSpeed, vSpeed);
            rb2d.velocity = v * maxSpeed;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision detected");
        if (collision.tag == "Killable" && dashLogic.frame == DashAbility.Frames.Damage)
        {
            if (collision.gameObject.GetComponent<Health>().takeDamage(swordDmg))
                dashLogic.setKilled();
            Debug.Log("Contact");
        }
        else if (collision.tag == "Killable")
        {
            rb2d.gameObject.GetComponent<Health>().takeDamage(colDmg);
            // Take damage if contact in vulnerable 
        }
        else if (collision.tag == "teleporter")
        {
            transform.position = new Vector3(0f, -1.5f, 0.0f);
        
        }
    }
}