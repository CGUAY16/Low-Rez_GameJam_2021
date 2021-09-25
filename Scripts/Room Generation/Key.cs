using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (!collision.gameObject.GetComponent<Player>().key)
            {
                collision.gameObject.GetComponent<Player>().key = true;
                GameController.Instance.keyUI.SetActive(true);
                this.transform.parent.GetComponent<RoomMapInfo>().hasKey = false;
                Debug.Log("Key picked up");
                Destroy(this.gameObject);
            }
            else
            {
                collision.gameObject.GetComponent<Player>().atticKey = true;
                GameController.Instance.keyUI.SetActive(true);
                this.transform.parent.GetComponent<RoomMapInfo>().hasKey = false;
                Debug.Log("Attic key picked up");
                Destroy(this.gameObject);
            }

        }
    }

}
