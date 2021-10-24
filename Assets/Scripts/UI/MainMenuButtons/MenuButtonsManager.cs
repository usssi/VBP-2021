using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;

public class MenuButtonsManager : MonoBehaviour
{
    [SerializeField] private Slider loadingBar;
    [SerializeField] private Text progressText;
    [SerializeField] private GameObject buttonsPanel;
    [SerializeField] private GameObject questionPanel;
    [SerializeField] private AudioSource musicAudioSource;

    [Range(0, 1)]
    [SerializeField] private float menuMusicVolume;

    [SerializeField] private Button playButton;
    [Header("SettingsButton")]
    [SerializeField] private Button settingsButton, audioON, audioOFF, changeAudioState, fixedJoystick, floatingJoystick, changeJoystickType, closeSettings;
    [SerializeField] private Animator settingsAnim, audioSwitchAnim, joystickSwitchAnim;
    public Image audioSwitchImage;
    public Image joystickSwitchImage;
    public float timeToWait = 0.5f;
    [SerializeField] private GameObject settingsMenu;

    [Header("ExitButton")]
    public Button exitButton;
    public Button yesDecition;
    public Button noDecition;
    public Image buttonFinalPart;
    public TextMeshProUGUI exitText;
    public Animator exitButtonAnim;
    public GameObject exitMenu;
    
    
    public int audioMuted;             //0 for yes 1 for no
    private int joystickType = 0;      //0 para el normal y 1 para el flotante.
    private int hasPlayedTutorial = 1; // 0 para sí 1 para no.
    public AudioMixer masterMixer;  

    private void Start()
    {
        if(PlayerPrefs.HasKey("hasPlayedTutorial"))
            hasPlayedTutorial = PlayerPrefs.GetInt("hasPlayedTutorial");

        string sceneToLoad = (hasPlayedTutorial == 0) ? "GameScene" : "SchoolScene";

        playButton.onClick.AddListener(() => PlayButton("ComicScene"));
        settingsButton.onClick.AddListener(OpenSettings);
        audioON.onClick.AddListener(TurnAudioOn);
        audioOFF.onClick.AddListener(TurnAudioOff);
        changeAudioState.onClick.AddListener(ChangeAudioState);
        fixedJoystick.onClick.AddListener(SetJoystickToFixed);
        floatingJoystick.onClick.AddListener(SetJoystickToFloating);
        changeJoystickType.onClick.AddListener(ChangeJoystickType);
        closeSettings.onClick.AddListener(CloseSettings);
        exitButton.onClick.AddListener(() =>ManageExitMenu(true));
        noDecition.onClick.AddListener(() => ManageExitMenu(false));
        yesDecition.onClick.AddListener(ExitGame);

        audioMuted = PlayerPrefs.GetInt("audioMuted");
        Debug.Log("AudioMuted: " + audioMuted);
        if (audioMuted == 0)
            MasterVolume();
        else
            MasterVolumeMute();


        joystickType = PlayerPrefs.GetInt("joystickType");
    }


    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    PlayerPrefs.DeleteAll();
        //    Debug.Log("Data deleted");
        //}
    }



    //public void QuitButton()
    //{
    //    buttonsPanel.SetActive(false);
    //    questionPanel.gameObject.SetActive(true);
    //    //Application.Quit();
    //}

    //public void NoButton()
    //{
    //    buttonsPanel.SetActive(true);
    //    questionPanel.gameObject.SetActive(false);
    //}

    //public void YesButton()
    //{
    //    Application.Quit();
    //}

    public void PlayButton(string sceneName)
    {
        //play sound jugar
        FindObjectOfType<AudioManager>().Play("botonPress");

        LoadSceneAsync(sceneName);
    }

    #region SettingsMenu Functions
    private void OpenSettings()
    {
        if (settingsAnim.GetBool("isOpen"))
        {
            CloseSettings();
            return;
        }

        FindObjectOfType<AudioManager>().Play("settingsOpen");

        settingsAnim.SetBool("isOpen", true);
        settingsMenu.gameObject.SetActive(true);
        SetSwitchAnim(audioMuted, audioSwitchAnim);
        SetSwitchAnim(joystickType, joystickSwitchAnim);
        StartCoroutine(EnableSwitches(timeToWait));
    }

    private void CloseSettings()
    {
        FindObjectOfType<AudioManager>().Play("settingsClose");

        settingsAnim.SetBool("isOpen", false);
        settingsMenu.gameObject.SetActive(false);
        SetImageColor(0);
    }
    private void TurnAudioOff()
    {
        //FindObjectOfType<AudioManager>().Play("switchToggle");
        MasterVolumeMute();

        audioMuted = 1;
        //musicAudioSource.volume = 0;
        PlayerPrefs.SetInt("audioMuted", audioMuted);
        SetSwitchAnim(audioMuted, audioSwitchAnim);
    }
    private void TurnAudioOn()
    {
        //FindObjectOfType<AudioManager>().Play("switchToggle");
        MasterVolume();

        audioMuted = 0;

        //musicAudioSource.volume = menuMusicVolume;
        PlayerPrefs.SetInt("audioMuted", audioMuted);
        SetSwitchAnim(audioMuted, audioSwitchAnim);
    }

    private void ChangeAudioState()
    {
        FindObjectOfType<AudioManager>().Play("switchToggle");

        if (audioMuted == 0)
        {
            MasterVolumeMute();
            audioMuted = 1;
        }
        else if (audioMuted == 1)
        {
            MasterVolume();
            audioMuted = 0; 
        }

        if (audioMuted == 0)
            musicAudioSource.volume = menuMusicVolume;
        else if (audioMuted == 1)
            musicAudioSource.volume = 0;
        PlayerPrefs.SetInt("audioMuted", audioMuted);
        SetSwitchAnim(audioMuted, audioSwitchAnim);
    }

    private void SetJoystickToFixed()
    {
        //FindObjectOfType<AudioManager>().Play("pip");

        joystickType = 0;
        PlayerPrefs.SetInt("joystickType", joystickType);
        SetSwitchAnim(joystickType, joystickSwitchAnim);
    }

    private void SetJoystickToFloating()
    {
        //FindObjectOfType<AudioManager>().Play("pip");

        joystickType = 1;
        PlayerPrefs.SetInt("joystickType", joystickType);
        SetSwitchAnim(joystickType, joystickSwitchAnim);
    }

    private void ChangeJoystickType()
    {
        //play sound switch
        FindObjectOfType<AudioManager>().Play("switchToggle");

        if (joystickType == 1)
            joystickType = 0;
        else
            joystickType = 1;

        PlayerPrefs.SetInt("joystickType", joystickType);
        SetSwitchAnim(joystickType, joystickSwitchAnim);
    }

    private void SetSwitchAnim(int animState, Animator anim)
    {
        bool state = (animState == 0) ? true : false;
        anim.SetBool("on", state);
    }

    private IEnumerator EnableSwitches(float _time)
    {
        yield return new WaitForSeconds(_time);
        SetImageColor(1);
    }

    private void SetImageColor(int _value)
    {
        Color c = audioSwitchImage.color;
        c.a = _value;
        audioSwitchImage.color = c;
        c = joystickSwitchImage.color;
        c.a = _value;
        joystickSwitchImage.color = c;
    }

    #endregion

    #region ExitMenu Functions
    private void ManageExitMenu(bool _menuState)
    {
        exitButtonAnim.SetBool("isOpen", _menuState);
        settingsClosed = false;
        if (_menuState == true)
        {
            StartCoroutine(EnableExitMenu(0.45f));
            exitButton.gameObject.SetActive(false);
        }
        else
        {
            Color c = buttonFinalPart.color;
            c.a = 0;
            buttonFinalPart.color = c;
            c = exitText.color;
            c.a = 0;
            exitText.color = c;
            exitButton.gameObject.SetActive(true);
            exitMenu.SetActive(false);
            FindObjectOfType<AudioManager>().Play("exitClose");

        }
    }

    private void ExitGame() => Application.Quit();

    private IEnumerator EnableExitMenu(float _time)
    {
        //sonido de tabla desliza
        FindObjectOfType<AudioManager>().Play("exitOpen");

        yield return new WaitForSeconds(_time);
        exitMenu.SetActive(true);
        Color c = buttonFinalPart.color;
        c.a = 1;
        buttonFinalPart.color = c;
        c = exitText.color;
        c.a = 1;
        exitText.color = c;
    }
    #endregion


    private void OnDestroy()
    {
        PlayerPrefs.SetInt("audioMuted", audioMuted);
    }


    private void LoadSceneAsync(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    [SerializeField] private Button creditsButton;
    [SerializeField] private GameObject credits;
    bool settingsClosed = false;
    public void HandleCredits()
    {
        if (credits.activeSelf)
        {
            FindObjectOfType<AudioManager>().Play("botonPress");

            credits.SetActive(false);
            if (settingsClosed)
                settingsMenu.SetActive(true);
        }
        else
        {
            FindObjectOfType<AudioManager>().Play("botonPress");

            credits.SetActive(true);
            if (settingsMenu.activeSelf)
            {
                settingsMenu.SetActive(false);
                settingsClosed = true;
            }
        }
    }



    public void MasterVolumeMute()
    {
        masterMixer.SetFloat("masterVol", -80);
        Debug.Log("master volume mute");
    }

    public void MasterVolume()
    {
        masterMixer.SetFloat("masterVol", 0);
        Debug.Log("master volume sonando");
    }
}
