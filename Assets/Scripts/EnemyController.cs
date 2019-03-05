using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float maxSpeed;
    public Health hp;
    public Rigidbody2D player;
    public Transform corpse;

    private Rigidbody2D rb2d;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (hp.alive)
        {
            Vector2 v = new Vector2(rb2d.transform.position.x, rb2d.transform.position.y);
            Vector2 v2 = new Vector2(player.transform.position.x, player.transform.position.y);
            v = v2 - v;
            v.Normalize();
            rb2d.velocity = v * maxSpeed;
        }
        else
        {
            Instantiate(corpse, new Vector3(rb2d.gameObject.transform.position.x + 1f, rb2d.gameObject.transform.position.y - 0.7f, rb2d.gameObject.transform.position.z), Quaternion.identity);
            Destroy(rb2d.gameObject);
        }
    }
}
