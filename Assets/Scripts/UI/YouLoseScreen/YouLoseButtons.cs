using UnityEngine;
using UnityEngine.SceneManagement;

public class YouLoseButtons : MonoBehaviour
{
   public void LoadNewScene(string sceneName)
    {
        FindObjectOfType<AudioManager>().Play("botonPress");

        SceneManager.LoadSceneAsync(sceneName);
    }
}
