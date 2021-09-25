using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Player player;
    SpriteRenderer sprite;
    BoxCollider2D doorCollider;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        sprite = GetComponent<SpriteRenderer>();
        doorCollider = GetComponent<BoxCollider2D>();
    }

    // Removes the door if the player is close enough
    void Update()
    {
        if (Vector2.Distance(transform.position, player.transform.position) < 2)
        {
            sprite.enabled = false;
            doorCollider.enabled = false;
        }
        else
        {
            sprite.enabled = true;
            doorCollider.enabled = true;
        }
    }
}
