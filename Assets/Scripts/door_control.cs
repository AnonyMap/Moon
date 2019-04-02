using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door_control : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public Rigidbody2D player;
    public SpriteRenderer sprite;
    private Color orig_color;
    public bool switched;
    // Start is called before the first frame update
    void Start()
    {
        orig_color = sprite.color;
        switched = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 control_vec = new Vector2(rb2d.transform.position.x, rb2d.transform.position.y);
        Vector2 player_vec = new Vector2(player.transform.position.x, player.transform.position.y);
        Vector2 controls_to_player = player_vec - control_vec;
        float distance = Mathf.Abs(Vector2.Distance(control_vec, player_vec));

        if (distance <= 3)
        {
            sprite.color = new Color(0.8f, 0.5f, 1f, 0.9f);
            if (Input.GetKeyDown("e"))
            {
                switched = true;
            }
        }
        else
        {
            sprite.color = orig_color;
        }
    }
}