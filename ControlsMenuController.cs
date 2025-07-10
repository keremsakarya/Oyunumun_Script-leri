using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlsMenuController : MonoBehaviour
{
    public void OnBackClick()
    {
        SceneManager.LoadScene("StartScene");
    }
}
