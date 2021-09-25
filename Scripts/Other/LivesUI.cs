using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesUI : MonoBehaviour
{
    [SerializeField]
    GameObject[] lives;

    public void ReduceLife(int health)
    {
        if (health > -1)
        {
            lives[health].SetActive(false);
        }
    } 
}
