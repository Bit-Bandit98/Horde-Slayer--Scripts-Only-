using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;
    public GameObject ControlsText, TipsText, StartText;


    private void Start()
    {
        Instance = this;
    }
}
