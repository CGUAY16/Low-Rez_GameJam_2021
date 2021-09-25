using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemy : MonoBehaviour
{
    private Vector3[] movementDirections = new Vector3[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    //Optional bool to set if you want the enemy to walk around
    [SerializeField]
    bool canWander;
    bool wandering;

    [SerializeField]
    float minAttackDistance = 3;
    [SerializeField]
    float dashSpeed = 10;
    bool canAttack;
    Vector2 followVector;

    //Track 0 = wander, track 1 = dash
    Music music;
    Rigidbody2D rb;

    private void OnEnable()
    {
        //Makes sure the enemy is set to attack
        canAttack = true;
        //If it's preset to wonder, it starts the loop
        if (canWander)
        {
            StartCoroutine(Wander());
        }

        music = GetComponent<Music>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //checks how far away the player is
        float distanceToPlayer = Vector2.Distance(transform.position, Player.Instance.transform.position);

        //If the player is close enough and hasn't attacked recently
        if (distanceToPlayer < minAttackDistance && canAttack)
        {
            //Changes the bool to stop multiple attacks
            canAttack = false;
            StartCoroutine(DashAttack());
        }
    }

    private void FixedUpdate()
    {
        if (GameController.Instance.paused)
        {
            followVector = Vector2.zero;
        }

        rb.velocity = followVector;
    }

    //Loop movement to dash toward player's position when they entered the range
    IEnumerator DashAttack()
    {
        //stops wandering
        wandering = false;
        //Sets player position when they entered into attack range
        Vector3 targetPosition = Player.Instance.transform.position;
        float breakTime = 2f;
        //short wait to let player dodge
        yield return new WaitForSeconds(.8f);

        music.StopTrack();
        music.PlayTrack(1);

        //Add visual indicator attack happening here

        //While moving
        while (Vector2.Distance(targetPosition, this.transform.position) > 0.1 )
        {
            if (!GameController.Instance.paused)
            {
                followVector = (targetPosition - this.transform.position).normalized;
                followVector.x *= dashSpeed;
                followVector.y *= dashSpeed;
                breakTime -= Time.deltaTime;
            }
            yield return null;
        }
        followVector = Vector2.zero;
        //music.StopTrack();
        //Short wait to prevent multiple attacks
        yield return new WaitForSeconds(1.5f);

        //Resets attack state and wandering (if allowed)
        canAttack = true;
        if (canWander)
        {
            StartCoroutine(Wander());
        }
    }

    //Controls enemy wandering around
    IEnumerator Wander()
    {
        //Indicates the enemy is wandering
        wandering = true;

        while (wandering)
        {
            //Sets delay before move and direction
            int waitTime = Random.Range(2, 5);
            int roll = Random.Range(0, 4);
            float breakTime = 1f;
            Vector3 destination = transform.position + movementDirections[roll];
            yield return new WaitForSeconds(waitTime);

            if (!GameController.Instance.paused && wandering)
            {
                music.StopTrack();
                music.PlayTrack(0);
            }

            //Moves towards direction, assuming not attacking
            while (transform.position != destination && wandering)
            {
                if (!GameController.Instance.paused)
                {
                    Vector3 currentPosition = transform.position;
                    followVector = (destination - currentPosition).normalized;
                    yield return null;

                    breakTime -= Time.deltaTime;

                    if (Vector2.Distance(destination, transform.position) < 0.1 || breakTime <= 0)
                    {
                        //Stops movement if close to target position, or takes too long (running into wall))
                        break;
                    }
                }
                yield return null;
            }
            followVector = Vector2.zero;
            music.StopTrack();
        }
    }

}
