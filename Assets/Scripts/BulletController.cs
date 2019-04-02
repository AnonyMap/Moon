using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    public float dmg;

    void Start()
    {

    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime *2);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player" && collision.tag != "Ethereal" && transform.parent.tag == "PlayerAttack" && collision.tag!="Untagged") 
        {
            if (collision.tag == "Killable")
            {
                collision.gameObject.GetComponent<Health>().takeDamage(dmg);
            }
            Destroy(transform.parent.gameObject);
        }
        else if (collision.tag != "Killable" && transform.parent.tag == "EnemyAttack")
        {
            if(collision.tag == "Player")
            {
                collision.gameObject.GetComponent<Health>().takeDamage(dmg);
            }
            Destroy(transform.parent.gameObject);
        }
    }
}
