using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningCutscene : MonoBehaviour
{
    [SerializeField]
    GameObject dog;
    [SerializeField]
    GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(IntroCutscene());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator IntroCutscene()
    {
        Music dogSFX = dog.GetComponent<Music>();
        Animator playerAni = player.GetComponent<Animator>();
        playerAni.SetFloat("Horizontal_input", -1f);
        playerAni.SetFloat("Speed", 1f);
        yield return new WaitForSeconds(1);
        dogSFX.PlayTrack(0);
        yield return new WaitForSeconds(1.3f);
        dogSFX.StopTrack();
        Vector3 destination = new Vector3(-1f, dog.transform.position.y, 1f);
        while (dog.transform.position != destination)
        {
            dog.transform.position = Vector3.MoveTowards(dog.transform.position, destination, 2f * Time.deltaTime);
            
            player.transform.position = Vector3.MoveTowards(player.transform.position, destination, 2f * Time.deltaTime);
            yield return null;
        }

        destination = new Vector3(-9f, dog.transform.position.y, 1f);
        //playerAni.SetFloat("Last Horizontal", -1f);
        //playerAni.SetFloat("Speed", 0f);
        dogSFX.PlayTrack(0);
        while (dog.transform.position != destination)
        {
            dog.transform.position = Vector3.MoveTowards(dog.transform.position, destination, 2f * Time.deltaTime);

            player.transform.position = Vector3.MoveTowards(player.transform.position, destination, .5f * Time.deltaTime);
            yield return null;
        }
        playerAni.SetFloat("Speed", 0f);
        yield return new WaitForSeconds(0.5f);
        playerAni.SetFloat("Speed", 1f);
        destination = new Vector3(-6f, dog.transform.position.y, 1f);
        while (player.transform.position != destination)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, destination, 6f * Time.deltaTime);
            yield return null;
        }
        SceneManager.LoadScene("MainMenu");
        yield return null;
    }
}

