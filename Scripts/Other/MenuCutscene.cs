using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuCutscene : MonoBehaviour
{

    [SerializeField]
    GameObject player;
    [SerializeField]
    GameObject menuButtons;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Cutscene());
    }

    
    // REMOVED UPDATE FUNCTION


    IEnumerator Cutscene()
    {
        Animator playerAni = player.GetComponent<Animator>();
        playerAni.SetFloat("Horizontal_input", -1f);
        playerAni.SetFloat("Last Horizontal", -1f);
        playerAni.SetFloat("Speed", 1f);
        Vector3 destination = new Vector3(0f, -2.56f, 0f);
        while (player.transform.position != destination)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, destination, 5f * Time.deltaTime);
            yield return null;
        }
        playerAni.SetFloat("Speed", 0f);
        yield return new WaitForSeconds(1f);
        playerAni.SetFloat("Last Vertical", 1f);
        playerAni.SetFloat("Last Horizontal", 0);
        yield return new WaitForSeconds(1f);
        menuButtons.SetActive(true);
    }

    public void StartGame()
    {
        menuButtons.SetActive(false);
        StartCoroutine(EnterHouse());
    }

    IEnumerator EnterHouse()
    {
        player.transform.localScale = new Vector3(.9f, .9f, 1);
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + .2f, 0);
        yield return new WaitForSeconds(0.5f);
        player.transform.localScale = new Vector3(.8f, .8f, 1);
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + .2f, 0);
        yield return new WaitForSeconds(0.5f);
        player.transform.localScale = new Vector3(.7f, .7f, 1);
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + .2f, 0);
        yield return new WaitForSeconds(0.5f);
        player.transform.localScale = new Vector3(.6f, .6f, 1);
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + .2f, 0);
        yield return new WaitForSeconds(0.5f);
        player.transform.localScale = new Vector3(.5f, .5f, 1);
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + .2f, 0);
        yield return new WaitForSeconds(0.5f);
        player.SetActive(false);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("House");
    }

    // ADDED QUIT FUNCTION
    


}

