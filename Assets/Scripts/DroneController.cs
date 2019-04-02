using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneController : MonoBehaviour
{
    public float maxSpeed;
    public Health hp;
    public Transform corpse;

    private Rigidbody2D rb2d;
    private Rigidbody2D player;
    private bool agroed = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (hp.alive)
        {
            Vector2 drone_vec = new Vector2(rb2d.transform.position.x, rb2d.transform.position.y);
            Vector2 player_vec = new Vector2(player.transform.position.x, player.transform.position.y);
            float distance = Mathf.Abs(Vector2.Distance(drone_vec, player_vec));
            RaycastHit2D see = Physics2D.Linecast(drone_vec, player_vec); //check if the enemy can see the player
            if (agroed || (see.transform.tag == "Player" && distance < 30))
            {
                agroed = true;
                drone_vec = player_vec - drone_vec;
                drone_vec.Normalize();
                rb2d.velocity = drone_vec * maxSpeed;
            }
        }
        else
        {
            Instantiate(corpse, new Vector3(rb2d.gameObject.transform.position.x + 1f, rb2d.gameObject.transform.position.y - 0.7f, rb2d.gameObject.transform.position.z), Quaternion.identity);
            Destroy(rb2d.gameObject);
        }
    }

  

}
