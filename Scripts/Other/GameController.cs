using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    RoomTemplates[] roomTemplates;
    public GameObject attic;

    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject mapPanel;
    [SerializeField]
    private GameObject diedScreen;
    [SerializeField]
    private GameObject winScreen;
    public bool paused;
    public bool menuActive;

    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private GameObject blackScreen;
    public GameObject keyUI;

    public GameObject[] mutatedTileSet;

    private Music music;

    public static GameController Instance { get; set; }


    // Start is called before the first frame update
    void Start()
    {
        //Ensures there is only one game controller in the game at a time.
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        paused = true;
        menuActive = false;
        music = GetComponent<Music>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            ToggleMapView();
        }
        //For debug resets
        //else if (Input.GetKeyDown(KeyCode.R))
        //{
        //    ReloadGame();
        //}
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu();
        }
    }

    //Toggles the map UI window
    public void ToggleMapView()
    {
        if (!mapPanel.activeSelf)
        {
            //Regenerates map, to allow for different floors and regeneration
            mapPanel.GetComponentInChildren<Map>().GenerateCurrentMap();
        }
        mapPanel.SetActive(!mapPanel.activeSelf);
        menuActive = !menuActive;
    }

    //Function to shake screen 
    public IEnumerator ScreenShake(float duration, float magnitude)
    {
        Vector3 cameraPos = mainCamera.transform.localPosition;
        float elapsedTime = 0f;
        float shakeDuration = duration;

        while (elapsedTime < shakeDuration)
        {
            //Amount of shake, changable by magnitude
            float xOffset = Random.Range(-0.5f, 0.5f) * magnitude;
            float yOffset = Random.Range(-0.5f, 0.5f) * magnitude;

            mainCamera.transform.localPosition = new Vector3(xOffset, yOffset, mainCamera.transform.localPosition.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        //resets camera to inital position
        mainCamera.transform.localPosition = cameraPos;
    }

    public void PauseMenu()
    {
        paused = !paused;
        pauseMenu.SetActive(!pauseMenu.activeSelf);
    }

    //For reloading in demo
    public void ReloadGame()
    {
        SceneManager.LoadScene("House");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Died()
    {
        paused = true;
        music.SetVolume(.5f);
        music.PlayTrack(3);
        diedScreen.SetActive(true);
    }

    public void Win()
    {
        paused = true;
        winScreen.SetActive(true);
    }


    public void MutateHouse(GameObject dog)
    {
        paused = true;

        music.PlayTrack(1);
        music.SetVolume(0.4f);

        StartCoroutine(MutateHouseCutscene(dog));
    }


    //Code called to transform house tiles and spawn monters when the dogs is picked up
    public IEnumerator MutateHouseCutscene(GameObject dog)
    {
        Dog fakeDog = dog.GetComponent<Dog>();
        //Animates player looking around (to be made into animation later)
        yield return new WaitForSeconds(2f);
        Player.Instance.pAnimator.SetFloat("Vertical_input", 0);
        Player.Instance.pAnimator.SetFloat("Horizontal_input", 0);
        Player.Instance.pAnimator.SetTrigger("Dog Transform");

        yield return new WaitForSeconds(5);

        fakeDog.dAnimator.SetTrigger("Dog Transform");
        fakeDog.music.PlayTrack(1);
        fakeDog.music.SetVolume(0.6f);

        yield return new WaitForSeconds(4);
        fakeDog.music.StopTrack();
        //yield return new WaitForSeconds(2);

        //Camera shake
        Vector3 cameraPos = mainCamera.transform.localPosition;
        float elapsedTime = 0f;
        float shakeDuration = 4f;

        while (elapsedTime < shakeDuration)
        {
            float xOffset = Random.Range(-0.5f, 0.5f) * 2;
            float yOffset = Random.Range(-0.5f, 0.5f) * 2;

            mainCamera.transform.localPosition = new Vector3(xOffset, yOffset, mainCamera.transform.localPosition.z);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        mainCamera.transform.localPosition = cameraPos;

        //Blacks out torchlight
        blackScreen.SetActive(true);

        //Changes attic
        RoomInfo[] atticRooms = attic.GetComponentsInChildren<RoomInfo>();
        foreach (var item in atticRooms)
        {
            item.ActivateEnemies();
            item.ChangeTiles();
        }

        //Spawns enemies and changes tiles
        foreach (RoomTemplates item in roomTemplates)
        {
            //Changes start room of each floor
            item.startRoom.GetComponent<RoomInfo>().ChangeTiles();

            foreach (var room in item.rooms)
            {
                //Changes all the generated rooms
                if (room != null && !room.gameObject.CompareTag("Wall"))
                {
                    RoomInfo roomInfo = room.GetComponent<RoomInfo>();
                    roomInfo.ActivateEnemies();
                        roomInfo.ChangeTiles();
                }
            }
        }

        //Destroys dog to prevent retriggering
        Destroy(dog);

        yield return new WaitForSeconds(1f);

        //Removes black screen
        blackScreen.SetActive(false);

        yield return new WaitForSeconds(.5f);

        //Unpauses
        paused = false;

        yield return new WaitForSeconds(5f);

        //***PLAY NEW TRACK AND ADD NEW VOLUME***
        music.PlayTrack(2);
        music.SetVolume(0.3f);
    }


}
