using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ComicDisplayManager : MonoBehaviour
{
    [SerializeField] private Sprite[] comicImages;
    [SerializeField] private Image displayImage;
    [SerializeField] private float timeBetweenImages = 0.5f;
    [SerializeField] private float fadeValue = 0.005f;
    [SerializeField] private Animator comicAnim;
    public float timeal; 
    private int hasPlayedTutorial = 1;
    string sceneToLoad = "GameScene";

    private int counter;

    private void Start()
    {
        if (PlayerPrefs.HasKey("hasPlayedTutorial"))
            hasPlayedTutorial = PlayerPrefs.GetInt("hasPlayedTutorial");
        sceneToLoad = (hasPlayedTutorial == 0) ? "GameScene" : "SchoolScene";
        Debug.Log("SceneToLoad is: " + sceneToLoad);
        StartCoroutine(PlayComic(1));
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StopAllCoroutines();
            if (counter + 1 < comicImages.Length)
            {
                displayImage.sprite = comicImages[++counter];
                Color temp = displayImage.color;
                temp.a = 1;
                displayImage.color = temp;
                StartCoroutine(PlayComic(counter + 1, 0.8f));
            }
            else
                SkipComic();
        }
    }

    private IEnumerator PlayComic(int from, float waitTime = 0)
    {
        for (int i = from; i < comicImages.Length; i++)
        {
            comicAnim.SetTrigger("fadeIn");
            while (displayImage.color.a < 1)
                yield return null;
            comicAnim.SetTrigger("fadeOut");
            while (displayImage.color.a > 0.05)
                yield return null;

            Debug.Log("Cambiar imagen");
            displayImage.sprite = comicImages[i];
            counter = i;
        }

        yield return new WaitForSeconds(1.2f);
        comicAnim.enabled = false;

        yield return new WaitForSeconds(1f);
        SkipComic();
    }

    public void SkipComic()
    {

        Debug.Log("Calling skipComic");
        FindObjectOfType<AudioManager>().Play("botonPress");

        SceneManager.LoadSceneAsync(sceneToLoad);
    }
}
