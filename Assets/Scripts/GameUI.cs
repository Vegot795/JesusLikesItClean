using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    public List<WindowScript> Windows = new List<WindowScript>();
    public GameObject NextLvlBut;
    public WindowScript currentWindow;
    public Camera MainCam;

    private int windowNumber = 0;
    private Vector3 CurrentCamPos;

    [Tooltip("Wygładzanie ruchu kamery. Mniej = szybszciej.")]
    public float cameraSmoothTime = 0.35f;
    private Vector3 cameraVelocity = Vector3.zero;
    private Vector3 targetCamPos;
    private bool isCameraMoving = false;
    private const float arriveThresholdSqr = 0.0009f;

    void Start()
    {
        foreach (var window in GameObject.FindObjectsByType<WindowScript>(FindObjectsSortMode.None))
        {
            if (!Windows.Contains(window))
            {
                Windows.Add(window);
            }
        }

        MainCam = Camera.FindAnyObjectByType<Camera>();
        if (MainCam != null && MainCam.CompareTag("MainCamera"))
        {
            NextLvlBut.SetActive(false);
            if (Windows.Count > 0)
            {
                currentWindow = Windows[windowNumber];
                CurrentCamPos = currentWindow.transform.position;
                targetCamPos = new Vector3(CurrentCamPos.x, CurrentCamPos.y, MainCam.transform.position.z);
            }
        }
    }

    public void Update()
    {
        CurrentLevelController();
        HandleCameraMovement();
    }
    public void LoadScene(int scena) // #0 menu startowe, #1 gra, #2 sklep
    {
        SceneManager.LoadScene(scena);
    }

    private void CurrentLevelController ()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (currentWindow.isCleaned)
            {
                NextLvlBut.SetActive(true);

                if (currentWindow.isCleaned)
                {
                    currentWindow.isCleaned = false;
                }
            }
            else
            {
                NextLvlBut.SetActive(false);
            }
        }
    }

    private void HandleCameraMovement()
    {
        if (!isCameraMoving || MainCam == null)
            return;

        MainCam.transform.position = Vector3.SmoothDamp(MainCam.transform.position, targetCamPos, ref cameraVelocity, cameraSmoothTime);

        if ((MainCam.transform.position - targetCamPos).sqrMagnitude <= arriveThresholdSqr)
        {
            MainCam.transform.position = targetCamPos;
            isCameraMoving = false;
            cameraVelocity = Vector3.zero;
        }
    }

    public void MoveToNextLvl()
    {
        if (windowNumber + 1 >= Windows.Count)
        {
            return;
        }

        windowNumber++;
        currentWindow = Windows[windowNumber];

        if (MainCam != null && currentWindow != null)
        {
            targetCamPos = new Vector3(currentWindow.transform.position.x, currentWindow.transform.position.y, MainCam.transform.position.z);
            isCameraMoving = true;
        }
    }

}
