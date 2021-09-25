using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathEnemy : MonoBehaviour
{
    Animator eAnimator;

    //Variables to control the enemies movement
    public float speed = 1f;
    //Sets how long the enemy waits at a waypoint before moving on
    public float waitTime = .3f;
    public bool moving = true;
    private bool startMovement;

    //Values for the path of the enemy
    public Transform pathHolder;
    Vector3[] waypoints;
    Vector3 targetWaypoint;

    Music music;

    private void OnEnable()
    {
        //Build the waypoints for the enemy
        if (pathHolder != null)
        {
            waypoints = new Vector3[pathHolder.childCount];
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypoints[i] = pathHolder.GetChild(i).position;
            }
            transform.position = waypoints[0];
        }

        music = GetComponent<Music>();
        eAnimator = GetComponent<Animator>();

        StartCoroutine(FollowPath(waypoints));
    }


    IEnumerator FollowPath(Vector3[] waypoints)
    {
        yield return new WaitForSeconds(2);
        //Sets the enemy at its inital position
        //transform.position = waypoints[0];

        //Moves the position in the waypoint list
        int targetWaypointIndex = 1;
        targetWaypoint = waypoints[targetWaypointIndex];

        //Plays SFX
        music.PlayTrack(0);

        while (true)
        {
            if (!GameController.Instance.paused && moving)
            {
                //Moves the enemy
                transform.position = Vector3.MoveTowards(transform.position, targetWaypoint, speed * Time.deltaTime);
                eAnimator.SetFloat("Speed", (targetWaypoint.x - transform.position.x));

                //Enemy has hit waypoint
                if (transform.position == targetWaypoint)
                {
                    //Changes the waypoint when reached
                    targetWaypointIndex = (targetWaypointIndex + 1) % waypoints.Length;

                    //Pauses before moving again
                    yield return new WaitForSeconds(waitTime);

                    targetWaypoint = waypoints[targetWaypointIndex];
                }
            }

            yield return null;
        }

    }


        //This draws a visual line of the waypoints of an enemy in the editor, to help move waypoints easily
        private void OnDrawGizmos()
    {
        Vector3 startPosition = pathHolder.GetChild(0).position;
        Vector3 previousPosition = startPosition;

        foreach (Transform waypoint in pathHolder)
        {
            Gizmos.DrawSphere(waypoint.position, 0.3f);
            Gizmos.DrawLine(previousPosition, waypoint.position);
            previousPosition = waypoint.position;
        }
        Gizmos.DrawLine(previousPosition, startPosition);
    }

}
