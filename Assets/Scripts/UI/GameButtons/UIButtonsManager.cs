using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIButtonsManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject buttonsCanvas;
    [SerializeField] private GameObject panchitosOKCanvas, animalScapedCanvas;

    [SerializeField] private Joystick fixedJoystick;
    [SerializeField] private Joystick floatingJoystick;
    [SerializeField] private GameObject fixedJoystickGameObject, floatingJoystickGameObject;
    [SerializeField] private Animator joystickSwitchAnimator;
    private int joystickType = 0;       //0 para el fixed, 1 para el flotante

    [Header("VolumeSettings")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource soundsAudioSource;
    [SerializeField] private Button muteButton;
    [SerializeField] private float gameMusicVolume;
    [SerializeField] private float gameSoundsVolume;
    private int audioMuted;

    [SerializeField] private Image joystickImage;
    [SerializeField] private Sprite switchIzquierda, switchDerecha;



    private ChadMovement movementScript;


    private void Start()
    {
        movementScript = GameObject.FindObjectOfType<ChadMovement>();
        joystickType = PlayerPrefs.GetInt("joystickType");
        Debug.LogWarning("JoystickType is: " + joystickType);
        //if (joystickType == 0)
        //{
        //    joystickSwitchAnimator.SetBool("on", true);
        //    Debug.LogWarning("Switch a true");
        //}
        //else
        //{
        //    joystickSwitchAnimator.SetBool("on", false);
        //    Debug.LogWarning("Switch a false");
        //}

        SetAudio();
    }

    bool toggleSetted = false;
    public void PauseGame()
    {
        FindObjectOfType<AudioManager>().Play("botonPress");

        pauseButton.SetActive(false);
        pauseMenu.SetActive(true);
        buttonsCanvas.SetActive(false);
        Debug.LogWarning("Pausando juego");
        Debug.LogWarning("Joystic type is: " + joystickType);
        if (joystickType == 1)
            joystickImage.sprite = switchDerecha;
        else
            joystickImage.sprite = switchIzquierda;
            
           
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        FindObjectOfType<AudioManager>().Play("botonPress");

        Time.timeScale = 1;
        buttonsCanvas.SetActive(true);
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void RestartGame()
    {
        FindObjectOfType<AudioManager>().Play("botonPress");

        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
        FindObjectOfType<AudioManager>().Play("botonPress");

        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("MainMenuScene");
    }

    public void CloseCanvas()
    {
        if (panchitosOKCanvas.activeSelf == true)
        {
            panchitosOKCanvas.SetActive(false);

        }
        else if (animalScapedCanvas.activeSelf == true)
            animalScapedCanvas.SetActive(false);

        if (buttonsCanvas.activeSelf == false)
            buttonsCanvas.SetActive(true);

        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }

    public void SetFixedJoystick()
    {
        fixedJoystickGameObject.SetActive(true);
        floatingJoystickGameObject.SetActive(false);
        movementScript.SetJoystick(fixedJoystick);
        PlayerPrefs.SetInt("joystickType", 0);
        joystickType = PlayerPrefs.GetInt("joystickType");
        joystickImage.sprite = switchIzquierda;
        Debug.LogError("Seteando a Fixeado");
    }

    public void SetFloatingJoystick()
    {
        floatingJoystickGameObject.SetActive(true);
        fixedJoystickGameObject.SetActive(false);
        movementScript.SetJoystick(floatingJoystick);
        PlayerPrefs.SetInt("joystickType", 1);
        joystickType = PlayerPrefs.GetInt("joystickType");
        joystickImage.sprite = switchDerecha;
        Debug.LogError("Seteando flotante");
    }

    public void ChangeJoystickState()
    {
        FindObjectOfType<AudioManager>().Play("switchToggle");

        int n = PlayerPrefs.GetInt("joystickType");
        Debug.Log("N is: " + n);
        if (n == 1)
        {

            SetFixedJoystick();
        }
        else
        {

            SetFloatingJoystick();
        }
            
        Debug.Log("Cambiand oestado");
    }

    private void HandleMuteVolumeButton()
    {
        if (audioMuted == 0)
            audioMuted = 1;
        else if (audioMuted == 1)
            audioMuted = 0;

        if (audioMuted == 0)
        {
            musicAudioSource.volume = gameMusicVolume;
            soundsAudioSource.volume = gameSoundsVolume;
        }
        else if (audioMuted == 1)
        {
            musicAudioSource.volume = 0;
            soundsAudioSource.volume = 0;
        }

    }

    void SetAudio()
    {
        muteButton.onClick.AddListener(HandleMuteVolumeButton);
        movementScript = GameObject.FindGameObjectWithTag("Player").GetComponent<ChadMovement>();
        audioMuted = PlayerPrefs.GetInt("audioMuted");


        if (audioMuted == 0)
        {
            musicAudioSource.volume = gameMusicVolume;
            soundsAudioSource.volume = gameSoundsVolume;
        }
        else
        {
            musicAudioSource.volume = 0;
            soundsAudioSource.volume = 0;

        }
    }

    private void OnDestroy()
    {
      
        PlayerPrefs.SetInt("audioMuted", audioMuted);
    }

}
