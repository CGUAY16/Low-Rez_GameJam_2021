using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StareEnemy : MonoBehaviour
{
    private Vector3[] movementDirections = new Vector3[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left };
    //Optional bool to set if you want the enemy to walk around freely
    [SerializeField]
    bool canWander;
    bool wandering;

    [SerializeField]
    float minFollowDistance = 8;
    [SerializeField]
    float followSpeed = 3;
    bool following;
    Vector3 startPosition;
    Vector2 followVector;

    Music music;
    Rigidbody2D rb;
    Animator anim;

    private void OnEnable()
    {
        music = GetComponent<Music>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        startPosition = transform.position;
        //If it's preset to wonder, it starts the loop
        if (canWander)
        {
            StartCoroutine(Wander());
        }
    }

    // Update is called once per frame
    void Update()
    {
        //checks how far away the player is
        float distanceToPlayer = Vector2.Distance(transform.position, Player.Instance.transform.position);

        //If the player is close enough and hasn't attacked recently
        if (distanceToPlayer < minFollowDistance && !following)
        {
            //Changes the bool to stop multiple attacks
            following = true;
            StartCoroutine(FollowAttack());
        }
    }

    private void FixedUpdate()
    {
        if (GameController.Instance.paused)
        {
            followVector = Vector2.zero;
        }
        rb.velocity = new Vector2(followVector.x * followSpeed, followVector.y * followSpeed);
    }

    //Loop movement to dash toward player's position when they entered the range
    IEnumerator FollowAttack()
    {
        //stops wandering
        wandering = false;

        music.PlayTrack(0);

        //Add visual indicator attack happening here

        //While moving
        while (Vector2.Distance(transform.position, Player.Instance.transform.position) < minFollowDistance)
        {
            if (!GameController.Instance.paused)
            {
                //transform.position = Vector3.MoveTowards(transform.position, Player.Instance.transform.position, followSpeed * Time.deltaTime);
                followVector = (Player.Instance.transform.position - transform.position).normalized;
                yield return null;
            }
            yield return null;
        }

        followVector = Vector2.zero;
        following = false;

        music.StopTrack();

        //Short wait before moving back to start position
        yield return new WaitForSeconds(1.5f);

        while (transform.position != startPosition && !following)
        {
            if (!GameController.Instance.paused)
            {
                Vector3 currentPosition = transform.position;
                transform.position = Vector3.MoveTowards(transform.position, startPosition, Time.deltaTime);
                yield return null;
            }
            yield return null;
        }

        if (canWander && !following)
        {
            yield return new WaitForSeconds(1);
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
            int waitTime = Random.Range(2, 4);
            int roll = Random.Range(0, 4);
            float breakTime = 1f;
            Vector3 destination = transform.position + (movementDirections[roll]);
            yield return new WaitForSeconds(waitTime);

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
        }
    }
}
