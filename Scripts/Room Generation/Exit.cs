using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (!player.dog)
            {
                Debug.Log("Don't leave your dog behind!!");
            }

            else
            {
                GameController.Instance.Win();

                Debug.Log("Congratulations! You're free!");
            }
        }
    }

}
