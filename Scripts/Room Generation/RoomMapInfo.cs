using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomMapInfo : MonoBehaviour
{
    //details for the map generation
    public Vector2 mapCoordinates;
    //Details for world space bounds
    public float maxX, minX, maxY, minY;

    public bool upDoor = false;
    public bool downDoor = false;
    public bool leftDoor = false;
    public bool rightDoor = false;

    public bool hasKey;
    public bool hasStairs;

    //Indicator for whether it is a long room (0 = normal, 1 = long up (spawn from room below), 2 = long down (spawn from room above),
    //3 = long left (spawn from right), 4 = long right (spawn from left))
    public int longRoomInt;

    public GameObject previousRoom;

    // Start is called before the first frame update
    void Start()
    {
        RoomSpawner[] spawners = transform.GetComponentsInChildren<RoomSpawner>();
        //Updates the bools to indicate door direction
        //To be used if we add doors to map, though depends on time and pixel space
        foreach (var item in spawners)
        {
            if (item.openingDirection == 1)
            {
                upDoor = true;
            }
            else if (item.openingDirection == 2)
            {
                downDoor = true;
            }
            else if (item.openingDirection == 3)
            {
                leftDoor = true;
            }
            else if (item.openingDirection == 4)
            {
                rightDoor = true;
            }
        }
    }

    public void SetRoomBounds(GameObject newRoom)
    {
        this.maxX = newRoom.transform.position.x + 10;
        this.minX = newRoom.transform.position.x - 10;
        this.maxY = newRoom.transform.position.y + 10;
        this.minY = newRoom.transform.position.y - 10;
    }

}
