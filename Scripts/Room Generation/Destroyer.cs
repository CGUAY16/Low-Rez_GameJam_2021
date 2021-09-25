using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    //Used for the start room of the floor. It stops other rooms spawning another room over this one
    //by deleting any room spawn point it hits

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Checks to make sure it is only deleting room spawners, not enemies, etc
        if (collision.CompareTag("SpawnPoint"))
        {
            Destroy(collision.gameObject);
        }
    }
}
