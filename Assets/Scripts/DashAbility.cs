using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashAbility : MonoBehaviour
{
    public DashState currentState = DashState.Ready;
    public Frames frame = Frames.WindUp;
    public float cooldownTime = 0.3f;
    public float windUpTime = 0.01f;
    public float dmgTime = 0.7f;
    public float vlnTime = 0.01f;
    private float dashTime;
    public int dashVelocity;

 //   private Stack<Collider2D> collided;

    public float swordDmg, colDmg;

    private bool kill = false;

    public bool dashing;
    private bool gotDir = false;

    private Vector2 tempV;

    public Vector2 savedVelocity;

    public Rigidbody2D rb2d;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        switch (currentState)
        {
            case DashState.Ready:
                if (Input.GetButtonDown("Dash"))
                {
                    savedVelocity = rb2d.velocity;
                    dashing = true;
                    currentState = DashState.Dash;
                }
                break;
            case DashState.Dash:
                dashTime += Time.deltaTime * 9;
                if (!gotDir)
                {
//                    tempV = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
                    Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector2 vDir = mouseWorld - rb2d.transform.position;
                    tempV = new Vector2(vDir.x, vDir.y);
                    tempV.Normalize();
                    gotDir = true;
                }
                switch (frame)
                {
                    case Frames.WindUp:
                        if (dashTime >= windUpTime)
                            frame = Frames.PreDash;
                        break;
                    case Frames.PreDash:

                        rb2d.velocity = tempV * dashVelocity;

                      //  rb2d.gameObject.GetComponent<Collider2D>().isTrigger = true;
                        frame = Frames.Damage;
                        break;
                    case Frames.Damage:
                        // Damage logic
                        if (dashTime >= dmgTime)
                        {
                            frame = Frames.PreV;
                        }
                        break;
                    case Frames.PreV:
                     //   rb2d.gameObject.GetComponent<Collider2D>().isTrigger = false;
                        frame = Frames.Vulnerable;
                        break;
                    case Frames.Vulnerable:
                        // Vulnerability logic
                        if (dashTime >= vlnTime)
                            frame = Frames.End;
                        break;
                    case Frames.End:
                        dashTime = cooldownTime;
                        if (kill) dashTime = 0f;
                        kill = false;
                        frame = Frames.WindUp;
                        gotDir = false;
                        currentState = DashState.Cooldown;
                        rb2d.velocity = savedVelocity;
                        break;
                }
                break;
            case DashState.Cooldown:
                dashTime -= Time.fixedDeltaTime;
                dashing = false;
                if (dashTime <= 0)
                {
                    dashTime = 0;
                    currentState = DashState.Ready;
                }
                break;
        }   
    }

    public void setKilled()
    {
        kill = true;
    }

    public enum DashState
    {
        Ready, Dash, Cooldown
    }

    public enum Frames
    {
        WindUp, PreDash, Damage, Vulnerable, End, PreV
    }
}
