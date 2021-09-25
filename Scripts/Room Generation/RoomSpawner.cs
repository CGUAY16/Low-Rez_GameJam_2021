using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    //Setting direction for room to spawn/direction the door is (1=Up, 2=Down, 3=Left, 4=Right)
    public int openingDirection;

    //Holds the premade room prefabs
    private RoomTemplates templates;
    //Used to randomly chose a room from options
    private int rand;
    //Indicates whether a rooms has been spawned in a position when building the rooms
    public bool spawned = false;

    [SerializeField]
    private GameObject[] doors;
    [SerializeField]
    private GameObject[] eWalls;

    private GameObject closedRoom;

    private void Start()
    {
        //Grabs the room templates
        RoomTemplates[] templatesOptions = FindObjectsOfType<RoomTemplates>();
        //Selects the correct one for each floor
        if (transform.position.x < 400)
        {
            templates = templatesOptions[0];
        }
        else if (transform.position.x < 1200)
        {
            templates = templatesOptions[1];
        }
        //Builds the rooms, with a small delay to give time to detect other  rooms after built
        Invoke("SpawnFloor", 0.02f);
    }


    public void SpawnFloor()
    {
        //Checks if there is already a room in the position it is trying to spawn
        if (!spawned)
        {
            //Sets spawned to true, to indicate a room now exists here
            spawned = true;

            //placeholders to prevent errors
            GameObject[] roomType = templates.upRooms;
            Vector2 coordinateAdjustment = new Vector2 (0,0);

            //Sets variables for the type of room
            if (openingDirection == 1)
            {
                roomType = templates.downRooms;
                coordinateAdjustment = new Vector2 (0, 3);
            }
            else if (openingDirection == 2)
            {
                roomType = templates.upRooms;
                coordinateAdjustment = new Vector2(0, -3);
            }
            else if (openingDirection == 3)
            {
                roomType = templates.rightRooms;
                coordinateAdjustment = new Vector2(-3, 0);
            }
            else if (openingDirection == 4)
            {
                roomType = templates.leftRooms;
                coordinateAdjustment = new Vector2(3, 0);
            }

            //Creates room, as long as there is a direction for the spawner
            if (openingDirection != 0)
            {
                //Randomly choses a room from the correct layout
                rand = Random.Range(0, roomType.Length);
                GameObject newRoom = Instantiate(roomType[rand], transform.position, Quaternion.identity, templates.roomHolder.transform);

                //Set the parent room to this new room variable, for generation checks
                newRoom.GetComponent<RoomMapInfo>().previousRoom = this.transform.parent.gameObject;

                //Sets variables for setting room info
                RoomMapInfo lastRoomMapInfo = transform.parent.GetComponent<RoomMapInfo>();
                RoomMapInfo newRoomMapInfo = newRoom.GetComponent<RoomMapInfo>();

                //Sets room position bounds
                newRoomMapInfo.SetRoomBounds(newRoom);


                //Adjusts coordinates and bounds for long rooms
                if (lastRoomMapInfo.longRoomInt == 1)
                {
                    coordinateAdjustment.y += 3;
                    lastRoomMapInfo.maxY += 20;
                }
                else if (lastRoomMapInfo.longRoomInt == 2)
                {
                    coordinateAdjustment.y -= 3;
                    lastRoomMapInfo.minY -= 20;
                }
                else if (lastRoomMapInfo.longRoomInt == 3)
                {
                    coordinateAdjustment.x -= 3;
                    lastRoomMapInfo.minX -= 20;
                }
                else if (lastRoomMapInfo.longRoomInt == 4)
                {
                    coordinateAdjustment.x += 3;
                    lastRoomMapInfo.maxX += 20;
                }

                //Sets map coordinates for new room
                newRoomMapInfo.mapCoordinates = lastRoomMapInfo.mapCoordinates + coordinateAdjustment;

                //Activates door and deletes wall
                if (doors != null)
                {
                    foreach (var item in doors)
                    {
                        item.SetActive(true);
                    }
                }
                if (eWalls != null)
                {
                    foreach (var item in eWalls)
                    {
                        item.SetActive(false);
                    }
                }
            }
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Grabs the room templates, as this fires before Start function
        RoomTemplates[] templatesOptions = FindObjectsOfType<RoomTemplates>();
        if (transform.position.x < 400)
        {
            templates = templatesOptions[0];
        }
        else if (transform.position.x < 1200)
        {
            templates = templatesOptions[1];
        }

        //Checks if the spawn point is colliding with another rooms spawn point
        //(ie, on a right angle with another room trying to spawn)
        if (collision.CompareTag("SpawnPoint"))
        {
            //Checks if neither opening has built a room in this position
            if (!collision.GetComponent<RoomSpawner>().spawned && !spawned)
            {
                //Spawns a wall to prevent door opening into no room
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity, templates.roomHolder.transform);

                //Deactivates door indicator, to remove it from the UI map
                DeactivateDoorBool(this.gameObject);
                DeactivateDoorBool(collision.gameObject);

                //Destroys the spawn point to prevent another room appearing
                //Destroy(this.gameObject);
            }
            else if (collision.GetComponent<RoomSpawner>().spawned)
            {
                GameObject lastRoom = transform.parent.GetComponent<RoomMapInfo>().previousRoom;
                if (lastRoom != null)
                {
                    if (this.transform.position == lastRoom.transform.position ||
                        collision.gameObject.transform.parent.gameObject == lastRoom)
                    {
                        RoomMapInfo lastRoomInfo = lastRoom.GetComponent<RoomMapInfo>();

                        if ((openingDirection == 1 && lastRoomInfo.downDoor) || (openingDirection == 2 && lastRoomInfo.upDoor) ||
                            (openingDirection == 3 && lastRoomInfo.rightDoor) || (openingDirection == 4 && lastRoomInfo.leftDoor))
                        {
                            //Activates door and deletes wall
                            //if (doors != null)
                            //{
                            //    foreach (var item in doors)
                            //    {
                            //        item.SetActive(true);
                            //    }
                            //}
                            if (eWalls != null)
                            {
                                foreach (var item in eWalls)
                                {
                                    item.SetActive(false);
                                }
                            }
                        }
                    }
                }
                
            }

            //Sets spawned as true, to indicate it shouldn't build anything here
            spawned = true;

        }
    }

    void DeactivateDoorBool(GameObject spawner)
    {
        //Deactivates door indicator, to remove it from the UI map
        RoomSpawner spawnerInfo = spawner.GetComponent<RoomSpawner>();
        RoomMapInfo parentRoom = spawner.transform.parent.GetComponent<RoomMapInfo>();

        if (spawnerInfo.openingDirection == 1)
        {
            parentRoom.upDoor = false;
        }
        else if (spawnerInfo.openingDirection == 2)
        {
            parentRoom.downDoor = false;
        }
        else if (spawnerInfo.openingDirection == 3)
        {
            parentRoom.leftDoor = false;
        }
        else if (spawnerInfo.openingDirection == 4)
        {
            parentRoom.rightDoor = false;
        }

        //Deactivates door and deletes wall
        if (spawnerInfo.doors != null)
        {
            foreach (var item in doors)
            {
                item.SetActive(false);
            }
        }
        if (spawnerInfo.eWalls != null)
        {
            foreach (var item in eWalls)
            {
                item.SetActive(true);
            }
        }
    }

}
