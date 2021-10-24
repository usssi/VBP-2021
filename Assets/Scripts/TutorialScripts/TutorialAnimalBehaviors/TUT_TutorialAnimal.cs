using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TUT_TutorialAnimal : MonoBehaviour
{
    [Header("AnimalVars")]
    [SerializeField] private int lifes;
    private int actualLifes;
    [SerializeField] private float animalSpeed;
    [SerializeField] private float animalGravity;
    [SerializeField] private Transform physicalCorralLimit;
    private Animator anim;


    [SerializeField] private Vector2 corralCenter;

    public void Start()
    {
        actualLifes = lifes;
        anim = GetComponent<Animator>();
        SetDestinyPosition(corralCenter);
    }

    private void Update()
    {
        DetectMapPosition();
        TriggerJumpCorral();
    }

    public void SetDestinyPosition(Vector2 destinyPos)
    {
       StartCoroutine(MoveTowardsDestinyPos(destinyPos));
    }

    private bool isInsideCorral = true;
    private void DetectMapPosition()
    {
        if (this.transform.position.x > physicalCorralLimit.position.x -0.1f)
            isInsideCorral = true;
        else
            isInsideCorral = false;
    }



    private bool isJumping = false;
    private void TriggerJumpCorral()
    {
        if(this.transform.position.x <= physicalCorralLimit.position.x)
        {
            if(isInsideCorral == true && isJumping == false)
            {
                //StartCoroutine(JumpCorral());
            }
        }
    }

    IEnumerator MoveTowardsDestinyPos(Vector2 destinyPos)
    {
        while (Vector2.Distance(this.transform.position, destinyPos) > 0.05f)
        {
            if(isJumping == false)
            {
                this.transform.position = Vector2.MoveTowards(this.transform.position, destinyPos, 0.5f * Time.deltaTime);
            }
           
            yield return 0;
        }
    }

    //IEnumerator JumpCorral()
    //{
    //    isJumping = true;
    //    Vector2 startJumpPos = this.transform.position;
    //    Vector2 endJumpPos = new Vector2(startJumpPos.x - 0.6f, startJumpPos.y);
    //    this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + 0.25f);
    //    animalSpeed *= 2;

    //    while(this.transform.position.x >= endJumpPos.x)
    //    {
    //        transform.Translate(Vector2.left * animalSpeed * Time.deltaTime);

    //        yield return 0;
    //    }

    //    this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - 0.25f);
    //    isJumping = false;
        
    //}

}
