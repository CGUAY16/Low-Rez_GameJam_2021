using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairGate : MonoBehaviour
{
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
                    //Unlocks gate
                    GameController.Instance.keyUI.SetActive(false);
                    Debug.Log("Gate unlocked");
                    this.gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("You don't have a key");
                }
            }
            else
            {
                //Checks if the player has the key
                if (player.atticKey)
                {
                    //Unlocks gate
                    GameController.Instance.keyUI.SetActive(false);
                    Debug.Log("Gate unlocked");
                    this.gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("You don't have a key");
                }
            }

        }
    }
}
