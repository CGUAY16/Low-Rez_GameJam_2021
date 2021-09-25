using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    [SerializeField]
    GameObject enemies;
    [SerializeField]
    bool activeEnemies;
    [SerializeField]
    GameObject normalHouse;
    [SerializeField]
    GameObject mutatedHouse;


    // Start is called before the first frame update
    void Start()
    {
        //Makes sure there are enemies in the room
        if (enemies != null)
        {
            //RNG to check if enemies spawn at the start
            int spawnChance = 2;
            //Random.Range(1, 8);
            
            if (spawnChance == 1)
            {
                //Signals the enemies should be spawned
                activeEnemies = true;
            }

            //Allows for enemies to be hard set to spawn, on top of RNG
            if (activeEnemies == true)
            {
                //Triggers spawns
                ActivateEnemies();
            }
            else
            {
                //removes enemies, to be spawned later
                DeactivateEnemies();
            }
        }
    }

    //Spawns enemies and flips tilemaps. Public to allow it to be triggered externally
    public void ActivateEnemies()
    {
        //Makes sure it won't return error for empty room
        if (enemies != null && !activeEnemies)
        {
            enemies.SetActive(true);
            //Signals they should be spawned
            activeEnemies = true;
        }

    }

    void DeactivateEnemies()
    {
        //Deactivates for rooms that start empty
        if (enemies != null)
        {
            enemies.SetActive(false);
            //Signals they should be spawned
            activeEnemies = false;
        }
    }

    public void ChangeTiles()
    {
        //If mutated tiles available, activates them
        if (mutatedHouse != null)
        {
            normalHouse.SetActive(false);
            mutatedHouse.SetActive(true);
        }
    }
}
