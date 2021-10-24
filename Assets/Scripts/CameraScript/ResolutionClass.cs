using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResolutionClass
{
    [SerializeField] private string resName;

    [SerializeField] private int screenWidth;
    public int ScreenWidth { get => screenWidth; }

    [SerializeField] private int screenHeight;
    public int ScreenHeigth { get => screenHeight; }

    [SerializeField] private int refResX;
    public int RefResX { get => refResX; }

    [SerializeField] private int refResY;
    public int RefResY { get => refResY; }
}
