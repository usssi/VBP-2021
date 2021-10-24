using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChadAnimationsManager : MonoBehaviour
{
    [Header("PlayerComponents")]
    private Transform chadTransform;
    private Animator anim;
    private Renderer chadRenderer;
    private ChadMovement movementScript;

    [Header("AnimatorValues")]
    private float lookingDir;
    private bool isRunning;
    private bool isJumping;
    private bool isFalling;

    [Header("BlinkVars")]
    [SerializeField] private float timeToChangeAlpha = 0.05f;
    [SerializeField] private float alphaVariation = 0.05f;
    private bool blinkStarted = false;

    [Header("CorralCollider")]
    [SerializeField] private BoxCollider2D corralCollider;

    

    private void Start()
    {
        chadTransform = GetComponent<Transform>();
        anim = GetComponent<Animator>();
        chadRenderer = GetComponent<Renderer>();
        movementScript = GetComponent<ChadMovement>();
    }

    private void Update()
    {
        //Set lookingDir var and asign it to lookingDir param of the animator
        lookingDir = SetLookingDir();
        anim.SetFloat("lookingDir", lookingDir);

        //Set isRunning var and asign it to isRunning param of the animator
        isRunning = SetIsRunning();
        anim.SetBool("isRunning", isRunning);

        //Set isJumping var and asign it to isJumping param of the animator
        isJumping = SetIsJumping();
        anim.SetBool("isJumping", isJumping);

        //Set isFalling var and asign it to isFalling param of the animator
        isFalling = SetIsFalling();
        anim.SetBool("isFalling", isFalling);

        //If player pos is inside corralCollider, the blink
        if (corralCollider.bounds.Contains(chadTransform.position) && movementScript.InAir == false)
            if(blinkStarted == false)
                StartCoroutine(ChadBlink());
    }

    float SetLookingDir()
    {
        if (movementScript.HMovement != 0)
            return movementScript.HMovement;
        else
            return lookingDir;
    }

    bool SetIsRunning()
    {
        if (movementScript.HMovement != 0 || movementScript.VMovement != 0)
            return true;
        else
            return false;
    }

    bool SetIsJumping() => movementScript.Jumping;



    bool SetIsFalling() => movementScript.Falling;

    //Decrease and increase, in that order, chad opacity.
    IEnumerator ChadBlink()
    {
        blinkStarted = true;

        Color rendererColor = chadRenderer.material.color;

        for(int i = 0; i < 3; i++)
        {
            while (rendererColor.a > 0.2f)
            {
                rendererColor.a -= 0.05f;
                chadRenderer.material.color = rendererColor;

                yield return new WaitForSeconds(timeToChangeAlpha);
            }

            while (rendererColor.a < 1)
            {
                rendererColor.a += 0.05f;
                chadRenderer.material.color = rendererColor;

                yield return new WaitForSeconds(timeToChangeAlpha);
            }
        }
       
        blinkStarted = false;
    }



}
