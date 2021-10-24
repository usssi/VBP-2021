using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TUT_UIButtonsManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject pauseButton;
    [SerializeField] private GameObject buttonsCanvas;
    [SerializeField] private GameObject panchitosOKCanvas, animalScapedCanvas;

    [SerializeField] private Joystick fixedJoystick;
    [SerializeField] private Joystick floatingJoystick;
    [SerializeField] private GameObject fixedJoystickGameObject, floatingJoystickGameObject;
    private int joystickType = 0;       //0 para el fixed, 1 para el flotante

    [Header("VolumeSettings")]
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource soundsAudioSource;
    [SerializeField] private Button muteButton;
    [SerializeField] private float gameMusicVolume;
    [SerializeField] private float gameSoundsVolume;
    private int audioMuted;



    private TUT_ChadMovement movementScript;


    private void Start()
    {
        movementScript = GameObject.FindObjectOfType<TUT_ChadMovement>();
        joystickType = PlayerPrefs.GetInt("joystickType");
        if (joystickType == 0)
            SetFixedJoystick();
        else
            SetFloatingJoystick();

        SetAudio();


    }


    public void PauseGame()
    {
        pauseButton.SetActive(false);
        pauseMenu.SetActive(true);
        buttonsCanvas.SetActive(false);
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        buttonsCanvas.SetActive(true);
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoToMainMenu()
    {
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
    }

    public void SetFloatingJoystick()
    {
        floatingJoystickGameObject.SetActive(true);
        fixedJoystickGameObject.SetActive(false);
        movementScript.SetJoystick(floatingJoystick);
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
