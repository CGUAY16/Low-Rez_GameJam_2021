using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    //Lists of the premade rooms
    //The up rooms are rooms with a door at the top, down with door at the bottom and so on.
    [SerializeField]
    public GameObject[] upRooms;
    public GameObject[] downRooms;
    public GameObject[] leftRooms;
    public GameObject[] rightRooms;

    //Single block room, used to wall up doors
    public GameObject closedRoom;

    public GameObject startRoom;
    private GameObject exitRoom;
    private GameObject keyRoom;
    [SerializeField]
    private List<RoomSpawner> startSpawnPoints;
    [SerializeField]
    private GameObject nextStairs;

    //List of all rooms, used to add exits and create a map
    public List<GameObject> rooms;
    public GameObject roomHolder;

    //Used to spawn exit to floor
    [SerializeField]
    private float waitTime;
    private bool exitSpawned = false;
    [SerializeField]
    private GameObject exit;
    [SerializeField]
    private GameObject key;
    [SerializeField]
    private GameObject dog;
    private bool walledRoom;

    private void Start()
    {
        //Sets the room spawners in the start room, to allow for reseting the floor
        foreach (Transform item in startRoom.transform)
        {
            if (item.GetComponent<RoomSpawner>() != null)
            {
                RoomSpawner spawnPoint = item.GetComponent<RoomSpawner>();
                if (spawnPoint.openingDirection != 0)
                {
                    startSpawnPoints.Add(spawnPoint);
                }
            }
        }
    }

    private void Update()
    {
        //Waits until all the rooms are spawned, then adds the exit to the last/furthest room from the start
        if (waitTime <= 0 && exitSpawned == false)
        {
            //Checks if the floor is too big or small
            if (this.rooms.Count - 1 < 6 || this.rooms.Count > 12)
            {
                //Deletes the rooms from the game and resets the list
                foreach (var item in this.rooms)
                {
                    Destroy(item.gameObject);
                }
                this.rooms.Clear();
                
                //triggers the spawners in the Start room again
                foreach (var spawn in startSpawnPoints)
                {
                    spawn.spawned = false;
                    spawn.Invoke("SpawnFloor", 0.02f);
                }

                //resets the timer for spawning
                waitTime = 1f;
            }

            else
            {
                //Spawns important exits and items (using while loops to prevent walled rooms, so in a coroutine))
                StartCoroutine(FinishGeneration());

                //Lets player move once floor is generated
                GameController.Instance.paused = false;

            }

        }

        //'if' statement stops the timer running indefinitely
        else if (waitTime > 0)
        {
            waitTime -= Time.deltaTime;
        }
    }

    IEnumerator FinishGeneration()
    {
        //Checks to make sure a walled room hasn't been placed over a built room
        //This can occur when two triggers occur at the same time while spawning

        //Building temp Lists to prevent interating or deleting over the same list
        List<GameObject> roomList = this.rooms;
        List<GameObject> toDelete = new List<GameObject>();
        foreach (var room in roomList)
        {
            //Checks if the room is a walled room
            if (room.CompareTag("Wall"))
            {
                foreach (var item in this.rooms)
                {
                    //check if they are in the same position, and if the other is an actual room
                    if(item.transform.position == room.transform.position && !item.CompareTag("Wall"))
                    {
                        //Adds to local list for deletion
                        toDelete.Add(room);
                    }
                }
            }
        }

        //Deletes the walled room, leaving only the actual room. This ensures the player has a path to each room
        foreach (var item in toDelete)
        {
            Debug.Log("Deleting walled room at" + item.transform.position);
            Destroy(item.gameObject);
        }

        //Decides on location for exit or dog
        exitRoom = this.rooms[this.rooms.Count - 1];

        //Makes sure it's not a walled-up room
        int nextRoom = 1;
        while (exitRoom.CompareTag("Wall") || exitRoom.GetComponent<RoomMapInfo>().longRoomInt != 0)
        {
            nextRoom += 1;
            exitRoom = this.rooms[this.rooms.Count - nextRoom];
        }

        //Spawns the stairs
        Instantiate(exit, exitRoom.transform.position, Quaternion.identity);
        exitRoom.GetComponent<RoomMapInfo>().hasStairs = true;

        //Sets this location for next floor stairs
        if (nextStairs != null)
        {
                nextStairs.GetComponentInChildren<Stairs>().SetTeleport(exitRoom.transform.position);
        }
        exitSpawned = true;

        //Decides on location for key
        keyRoom = this.rooms[Random.Range(0, this.rooms.Count - 1)];
        //Checks to make sure it's not a walled-up room
        while (keyRoom.CompareTag("Wall") || exitRoom.GetComponent<RoomMapInfo>().longRoomInt != 0)
        {
                keyRoom = this.rooms[Random.Range(0, this.rooms.Count - 1)];
        }

        //Adds the key
        Instantiate(key, keyRoom.transform.position, Quaternion.identity, keyRoom.transform);
        keyRoom.GetComponent<RoomMapInfo>().hasKey = true;

        yield return null;

    }

}


