using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class BossController : MonoBehaviour
{
    public Health hp;
    public Transform corpse;
    public Transform bullet;
    public Transform gunPivot;
    public Transform gun;

    public Transform playerGhost;
    public TargetReached targetReached;

    public AIDestinationSetter moveTo;

    private Rigidbody2D rb2d;
    private Rigidbody2D player;

    public RoutineController routine;
    public Transform[] PatrolRoute;
    public int PatrolSize;
    private int patrolState;

    private float pistolCooldown = 0;
    private float machineGunCooldown = 0;
    private float maxSpeed = 2;
    private float shootingTime = 0;
    private bool agroed = false;
    private bool shooting = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").GetComponent<Rigidbody2D>();
        routine.SetPatrol();
        patrolState = 0;
        if (PatrolSize > 0)
        {
            SetMoveTo(PatrolRoute[0]);
            targetReached.SetNextTarget(PatrolRoute[1]);
        }
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        if (hp.alive)
        {
            Vector2 enemy_vec = new Vector2(rb2d.transform.position.x, rb2d.transform.position.y);
            Vector2 player_vec = new Vector2(player.transform.position.x, player.transform.position.y);
            Vector2 enemy_to_player = player_vec - enemy_vec;
            if (!agroed)
            {
                float distance = Mathf.Abs(Vector2.Distance(enemy_vec, player_vec));
                RaycastHit2D see = Physics2D.Linecast(enemy_vec, player_vec);
                if(see.transform.tag == "Player")
                {
                    agroed = true;
                }
            }
            if (routine.GetPatrol())
            {
                if (PatrolSize > 0 && targetReached.SetNextTarget(PatrolRoute[patrolState])) patrolState = (patrolState + 1) % PatrolSize;
            }
            if (agroed)
            {
                aimGun(enemy_to_player, gunPivot); 
                SetMoveTo(player.transform);
                RaycastHit2D hit = Physics2D.Linecast(gun.position, player_vec); //check if the enemy can see the player
                if (hit.transform.tag == "Player") //If it can see the player, then shoot at it 
                { //probably able to get rid of this if statement but not sure yet
                    SetMoveTo(player.transform);
                    handleShooting();
                }
            }
        }
        else
        {
            Instantiate(corpse, new Vector3(rb2d.gameObject.transform.position.x + 1f, rb2d.gameObject.transform.position.y - 0.7f, rb2d.gameObject.transform.position.z), Quaternion.identity);
            Destroy(rb2d.gameObject);
        }
    }

    private void SetMoveTo(Transform target)
    {
        if (moveTo.target != target) moveTo.target = target;
    }

    private void shootGun(Transform bullet, Transform gun)
    {
        var shooting = Instantiate(bullet, gun.position, gun.rotation);
        shooting.tag = "EnemyAttack";
    }

    private void aimGun(Vector2 enemy_to_player, Transform gunPivot)
    {
        float angle = Mathf.Atan2(enemy_to_player.y, enemy_to_player.x);
        gunPivot.rotation = Quaternion.Euler(0, 0, angle * 180 / Mathf.PI);
    }

    private void handleShooting()
    {
        pistolCooldown += 1;
        if (pistolCooldown > 50)
        {
            pistolCooldown = 0;
            shootGun(bullet, gun);
        }
        shootingTime += 1;
        if (shooting)
        {
            shootGun(bullet, gun);
            if(shootingTime > 150)
            {
                shooting = false;
                shootingTime = 0;
            }
        } else
        {
        }
    }
}