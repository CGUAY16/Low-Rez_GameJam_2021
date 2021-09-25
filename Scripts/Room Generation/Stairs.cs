using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stairs : MonoBehaviour
{
    [SerializeField]
    private Vector2 teleportLocation;



    public void SetTeleport(Vector2 coordinates)
    {
        teleportLocation.x = coordinates.x;
        teleportLocation.y = coordinates.y - 5;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Sets access to player script
            Player player = collision.gameObject.GetComponent<Player>();

            if (!this.gameObject.CompareTag("StairsA"))
            {
                //Checks if the player has the key
                if (player.key)
                {
                    //Teleports to second floor/level/space
                    Debug.Log("Teleported");
                    player.TeleportTo(teleportLocation);
                }
            }

            else
            {
                //Checks if the player has the key
                if (player.atticKey)
                {
                    //Teleports to second floor/level/space
                    Debug.Log("Teleported");
                    player.TeleportTo(teleportLocation);
                }
            }

        }
    }
}
