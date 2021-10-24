
using UnityEngine;
using UnityEngine.UI;

public class ManageCongratsImage : MonoBehaviour
{
    private int spriteIndex = 0;
    private bool imageUpdated;
    [SerializeField] private Sprite[] congratsSprites;
    [SerializeField] private Image congratsImage;
    


    private void OnDisable()
    {
        if (spriteIndex < congratsSprites.Length)
        {
            congratsImage.sprite = congratsSprites[spriteIndex];
            spriteIndex++;
        }
    }
}
