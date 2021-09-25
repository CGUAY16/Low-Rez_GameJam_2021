using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // References
    public static MainMenuManager instance;
    [SerializeField] GameObject mainMenu_buttons;
    [SerializeField] GameObject creditUi;
    [SerializeField] AudioSource a;
    [SerializeField] AudioClip clip;

    enum Scene
    {
        MainMenu,
        House,
        Testing
    }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void CreditButtonClicked()
    {
        playAudioSound();
        mainMenu_buttons.SetActive(false);
        creditUi.SetActive(true);
    }

    public void returnToMainMenu()
    {
        playAudioSound();
        mainMenu_buttons.SetActive(true);
        creditUi.SetActive(false);
    }

    public void QuitButtonClicked()
    {
        playAudioSound();
        Application.Quit();
    }

    public void playAudioSound()
    {
        a.PlayOneShot(clip);
    }

}
