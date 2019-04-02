using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class EnemyController : MonoBehaviour
{
    public float maxSpeed;

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

    private Vector2 LastPlayerPos;

    private float agroDistance = 15;
    private float alertDistance = 20;
    private float gunCooldown = 15;

    public RoutineController routine;
    public Transform[] PatrolRoute;
    public int PatrolSize;
    private int patrolState;
    private bool markPlayer;

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
            float distance = Mathf.Abs(Vector2.Distance(enemy_vec, player_vec));
            RaycastHit2D see = Physics2D.Linecast(enemy_vec, player_vec); //check if the enemy can see the player
            if (routine.GetPatrol())
            {
                if(PatrolSize > 0 && targetReached.SetNextTarget(PatrolRoute[patrolState])) patrolState = (patrolState + 1) % PatrolSize;
            }
            if (routine.GetAlert() || (distance < alertDistance && see.transform.tag == "Player"))
            {
                routine.SetAlert();
                float angle = Mathf.Atan2(enemy_to_player.y, enemy_to_player.x);
                gunPivot.rotation = Quaternion.Euler(0, 0, angle * 180 / Mathf.PI); //aim gun at player
                if (see.transform.tag == "Player")
                {
                    LastPlayerPos = player_vec;
                    markPlayer = true;
                    playerGhost.position = new Vector3(LastPlayerPos.x, LastPlayerPos.y, 0);
                }
                else if (markPlayer)
                {
                    SetMoveTo(playerGhost);
                    markPlayer = false;
                }
                else
                {
                    routine.SetForget();
                    markPlayer = false;
                }
            }

            if (routine.GetAggro() || distance < agroDistance)
            {
                float angle = Mathf.Atan2(enemy_to_player.y, enemy_to_player.x);
                gunPivot.rotation = Quaternion.Euler(0, 0, angle * 180 / Mathf.PI); //aim gun at player
                RaycastHit2D hit = Physics2D.Linecast(gun.position, player_vec); //check if the enemy can see the player
                if (hit.transform.tag == "Player") //If it can see the player, then shoot at it 
                {
                    routine.SetAggro();
                    if (distance > 3)
                    {
                        enemy_to_player.Normalize();
                        SetMoveTo(player.transform);
                        //rb2d.velocity = enemy_to_player * maxSpeed;
                    }
                    gunCooldown += 1;
                    if (gunCooldown > 50)
                    {
                        gunCooldown = 0;
                        shootGun(bullet, gun);
                    }
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
}