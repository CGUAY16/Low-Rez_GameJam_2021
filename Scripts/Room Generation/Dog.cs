using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public Animator dAnimator;
    public Music music;

    private void OnEnable()
    {
        dAnimator = GetComponent<Animator>();
        music = GetComponent<Music>();
    }


    //When player picks it up
    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.Instance.dog = true;
            Debug.Log("DOG GET");
            //Starts mini cutscene and changes house
            music.PlayTrack(0);
            GameController.Instance.MutateHouse(this.gameObject);
        }
    }

}
