using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRooms : MonoBehaviour
{
    private RoomTemplates templates;

    private void Start()
    {
        if (transform.position.x < 500)
        {
            templates = GameObject.FindGameObjectWithTag("Rooms1f").GetComponent<RoomTemplates>();
            templates.rooms.Add(this.gameObject);
        }

        else if (transform.position.x > 500)
        {
            templates = GameObject.FindGameObjectWithTag("Rooms2f").GetComponent<RoomTemplates>();
            templates.rooms.Add(this.gameObject);
        }

    }
}
