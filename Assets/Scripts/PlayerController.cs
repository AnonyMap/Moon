﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed;
    public Transform gunPivot;
    public Transform gun;
    public Transform corpse;
    public GameObject gunGameObject;

    public Sprite shotgun;
    public Sprite pistol;
    private string[] guns = new string[2];
    int currentGun = 0;

    public float camDistance;
    public Transform cam;

    public float swordDmg, colDmg;

    public Health hp;


    public Transform bullet;

    private Rigidbody2D rb2d;
    private DashAbility dashLogic;

    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        dashLogic = GetComponent<DashAbility>();
        gunGameObject = gun.gameObject;
        shotgun = Resources.Load<Sprite>("shotgun") as Sprite;
        pistol = Resources.Load<Sprite>("gun") as Sprite;
        guns[0] = "pistol";
        guns[1] = "shotgun";


    }

    void Update()
    {
        // rotate gun
        if (hp.alive)
        {
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDelta = mouseWorld - gunPivot.position;
            float angle = Mathf.Atan2(mouseDelta.y, mouseDelta.x);
            gunPivot.rotation = Quaternion.Euler(0, 0, angle * 180 / Mathf.PI);

            // translate camera
            Vector3 camTarget = new Vector3(camDistance * Mathf.Cos(angle), camDistance * Mathf.Sin(angle), -10);
            cam.localPosition = Vector3.Lerp(cam.localPosition, camTarget, 0.1f);

            if (Input.GetButtonDown("Fire1"))
            {
                switch(guns[currentGun]){
                    case "pistol":
                        shootBullet(bullet, gun.position, gun.rotation);
                        break;
                    case "shotgun":
                        Quaternion bullet2Rotation = Quaternion.Euler(gun.rotation.eulerAngles.x, gun.rotation.eulerAngles.y, gun.rotation.eulerAngles.z+10);
                        Quaternion bullet3Rotation = Quaternion.Euler(gun.rotation.eulerAngles.x, gun.rotation.eulerAngles.y, gun.rotation.eulerAngles.z - 10);
                        break;
                    default:
                        shootBullet(bullet, gun.position, gun.rotation);
                        break;
                }
            }

            if (Input.GetKeyDown(KeyCode.Q)){

                currentGun = (currentGun + 1) % guns.Length; 
                switch(guns[currentGun]){
                    case "pistol":
                        gunGameObject.GetComponent<SpriteRenderer>().sprite = pistol;
                        break;
                    case "shotgun":
                        gunGameObject.GetComponent<SpriteRenderer>().sprite = shotgun;
                        break;
                    default: 
                        gunGameObject.GetComponent<SpriteRenderer>().sprite = pistol;
                        break;

                }
            }
        }
    }

    void FixedUpdate()
    {
        if (!dashLogic.dashing && hp.alive)
        {
            float hSpeed = Input.GetAxis("Horizontal");
            float vSpeed = Input.GetAxis("Vertical");
            Vector2 v = new Vector2(hSpeed, vSpeed);
            rb2d.velocity = v * maxSpeed;
        }
        if (!hp.alive)
        {
            GetComponent<SpriteRenderer>().sprite = corpse.gameObject.GetComponent<SpriteRenderer>().sprite;
            //           Instantiate(corpse, new Vector3(rb2d.gameObject.transform.position.x + 1f, rb2d.gameObject.transform.position.y - 0.7f, rb2d.gameObject.transform.position.z), Quaternion.identity);
            //   Die.exe;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (hp.alive)
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
                GetComponent<Health>().takeDamage(colDmg);
                // Take damage if contact in vulnerable 
            }
            else if (collision.tag == "interact")
            {


            }
        }
    }

    void shootBullet(Transform bulletType, Vector3 position, Quaternion rotation){
        var shooting = Instantiate(bulletType, position, rotation);
        shooting.tag = "PlayerAttack";
    }
}