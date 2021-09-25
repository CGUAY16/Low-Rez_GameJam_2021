using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Map : MonoBehaviour
{
    [SerializeField]
    private GameObject generatedMap;
    [SerializeField]
    private GameObject mapSquarePrefab;
    [SerializeField]
    private GameObject mapRectanglePrefab;

    //Used to activate map location
    public bool playerLocation = false;
    [SerializeField]
    private GameObject spawnRoomTile;
    private bool playerLocated;


    // Update is called once per frame
    void Update()
    {
        //Potential to add flashing square on player's current location, but let's see how the balancing goes
        //Could be nice to have players work out where they are, add to the disorientation and exploration
    }

    //Generates the map for the UI, depending on the current floor and location
    public void GenerateCurrentMap()
    {
        //Clears previous map, in case the player has changed floors
        foreach (Transform child in generatedMap.transform)
        {
            Destroy(child.gameObject);
        }

        //Resets bool used to indicate if player in spawn room
        playerLocated = false;

        //Generates for first floor
        if (transform.position.x < 500)
        {
            //Goes through each room and creates a UI square for the map
            foreach (var room in GameObject.FindGameObjectWithTag("Rooms1f").GetComponent<RoomTemplates>().rooms)
            {
                //Spawns the tile on the map UI
                SpawnTile(room);
            }
        }

        else if (transform.position.x > 1400)
        {
            for (int i = 0; i < GameController.Instance.attic.transform.childCount; i++)
            {
                GameObject child = GameController.Instance.attic.transform.GetChild(i).gameObject;
                SpawnTile(child);
            }
        }

        //Generates for second floor
        else if (transform.position.x > 500)
        {
            //Goes through each room and creates a UI square for the map
            foreach (var room in GameObject.FindGameObjectWithTag("Rooms2f").GetComponent<RoomTemplates>().rooms)
            {
                //Spawns the tile on the map UI
                SpawnTile(room);
            }
        }

        //Changes color of spawn room if required.
        if (playerLocated)
        {
            spawnRoomTile.GetComponent<Image>().color = Color.white;
        }
        else
        {
            spawnRoomTile.GetComponent<Image>().color = Color.red;
        }

    }

    void SpawnTile(GameObject room)
    {
        if (room != null)
        {
            //Ensures the map doesn't include solid rooms (for blocking generation gaps)
            if (!room.CompareTag("Wall"))
            {
                RoomMapInfo roomMapInfo = room.GetComponent<RoomMapInfo>();
                GameObject newTile;
                //Maps a square room
                if (roomMapInfo.longRoomInt == 0)
                {
                    newTile = Instantiate(mapSquarePrefab, generatedMap.transform);
                }
                else
                {
                    newTile = Instantiate(mapRectanglePrefab, generatedMap.transform);
                }
                //Translates any long map tiles to fit
                Vector2 shiftVariable = new Vector2(0, 0);
                if (roomMapInfo.longRoomInt == 1)
                {
                    shiftVariable.y += 1;
                }
                else if (roomMapInfo.longRoomInt == 2)
                {
                    shiftVariable.y -= 2;
                }
                else if (roomMapInfo.longRoomInt == 3)
                {
                    shiftVariable.x -= 2;
                }
                else if (roomMapInfo.longRoomInt == 4)
                {
                    shiftVariable.x += 1;
                }

                //Moves the square relative to the canvas
                newTile.transform.localPosition = room.GetComponent<RoomMapInfo>().mapCoordinates + shiftVariable;

                //Used for tile door info
                MapUITile tileDoors = newTile.GetComponent<MapUITile>();

                //Rotates if needed
                if (roomMapInfo.longRoomInt == 3 || roomMapInfo.longRoomInt == 4)
                {
                    newTile.transform.Rotate(new Vector3(0, 0, -90));

                    //Adds doors
                    if (roomMapInfo.leftDoor)
                    {
                        tileDoors.upDoor.SetActive(true);
                    }
                    if (roomMapInfo.rightDoor)
                    {
                        tileDoors.downDoor.SetActive(true);
                    }
                }
                else
                {
                    //Adds doors
                    if (roomMapInfo.upDoor)
                    {
                        tileDoors.upDoor.SetActive(true);
                    }
                    if (roomMapInfo.downDoor)
                    {
                        tileDoors.downDoor.SetActive(true);
                    }
                    if (roomMapInfo.leftDoor)
                    {
                        tileDoors.leftDoor.SetActive(true);
                    }
                    if (roomMapInfo.rightDoor)
                    {
                        tileDoors.rightDoor.SetActive(true);
                    }
                }


                //Highlights player location if activated
                if (playerLocation)
                {
                    if (CheckWithinBounds(roomMapInfo))
                    {
                        if (roomMapInfo.hasKey)
                        {
                            //Changes the room to yellow to indicate key in room
                            newTile.GetComponent<Image>().color = Color.yellow;
                        }
                        else if (roomMapInfo.hasStairs)
                        {
                            //Changes the room to green to indicate stairs
                            newTile.GetComponent<Image>().color = Color.green;
                        }
                        else
                        {
                            //Changes the room to red to indicate current position
                            newTile.GetComponent<Image>().color = Color.red;
                        }
                        //Changes bool to prevent spawn room being colored and each room checking
                        playerLocated = true;
                    }
                }
            }
        }
        
    }

    //Checks if player is in a room
    //Could be optimised
    bool CheckWithinBounds(RoomMapInfo roomMapInfo)
    {
        Player player = FindObjectOfType<Player>();
        if (player.transform.position.x > roomMapInfo.minX && player.transform.position.x < roomMapInfo.maxX &&
            player.transform.position.y > roomMapInfo.minY && player.transform.position.y < roomMapInfo.maxY)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
