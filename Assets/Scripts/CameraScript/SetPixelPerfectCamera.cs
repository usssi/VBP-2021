using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class SetPixelPerfectCamera : MonoBehaviour
{
    private PixelPerfectCamera ppCamera;
    Camera cam;
    [SerializeField] private int screenWidth; //Ancho
    [SerializeField] private int screenHeight; //Alto


    [Header("ResolutionSettings")]
    [SerializeField] private ResolutionClass[] resAndRefRes;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        ppCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<PixelPerfectCamera>();
        screenWidth = Screen.width;
        screenHeight = Screen.height;
        SetCamera();
    }

    void Start()
    {


        //Debug.Log("screen width: " + screenWidth);
        //Debug.Log("screen height: " + screenHeight);
    }


    void SetCamera()
    {
       if(cam.aspect == 16f / 9f)
        {
            ppCamera.cropFrameX = true;
            ppCamera.cropFrameY = true;
        }
        //if((float)screenWidth/(float)screenHeight == 16f / 9f)
        //{
        //    ppCamera.cropFrameX = true;
        //    ppCamera.cropFrameY = true;
        //}






        //for(int i = 0; i < resAndRefRes.Length; i++)
        //{
        //    if(resAndRefRes[i].ScreenWidth != 0 && resAndRefRes[i].ScreenHeigth != 0)
        //    {
        //        if (resAndRefRes[i].ScreenWidth == screenWidth && resAndRefRes[i].ScreenHeigth == screenHeight)
        //        {
        //            if(resAndRefRes[i].RefResX != 0 && resAndRefRes[i].RefResY != 0)
        //            {
        //                ppCamera.refResolutionX = resAndRefRes[i].RefResX;
        //                ppCamera.refResolutionY = resAndRefRes[i].RefResY;
        //                Debug.Log(ppCamera.refResolutionX);
        //                Debug.Log(ppCamera.refResolutionY);
        //                canSetValues = true;
        //                break;
        //            }
        //        }
        //    }
        //}

        //if(canSetValues == false)
        //{
        //    //Debug.LogError("Screen Resolution not managed");
        //    Debug.Log("ScreenWidth: " + screenWidth);
        //    Debug.Log("ScreenHeigth: " + screenHeight);
        //    if (screenWidth / screenHeight == 2)
        //    {
        //        Debug.Log("Sceen size is arround 2");
        //        ppCamera.refResolutionX = 640;
        //        ppCamera.refResolutionY = 320;
        //    }
        //    else
        //    {
        //        Debug.Log("Screen size is less tha 2");
        //        ppCamera.refResolutionX = 568;
        //        ppCamera.refResolutionY = 320;
        //    }

        //    //ppCamera.refResolutionX = 630;
        //    //ppCamera.refResolutionY = 314;
        //}
    }

    //void SetPPCamera()
    //{
    //    if (screenWidth / screenHeight == 2)
    //    {
    //        ppCamera.refResolutionX = 630;
    //        ppCamera.refResolutionY = 314;
    //    }
    //    else if (screenWidth == 1684 && screenHeight == 947)
    //    {
    //        ppCamera.refResolutionX = 560;
    //        ppCamera.refResolutionY = 314;
    //    }
    //}

  
}
