using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public GameObject cloth;
    public GameObject sprinkle;
    public GameObject clothSlot;
    public GameObject sprinkleSlot;
    public GameObject Tools;
    public GameObject Inventory;
    public bool isCameraMoving = false;

    private int windowNumber = 3;
    private Vector3 CurrentCamPos;

    [Tooltip("Wygładzanie ruchu kamery. Mniej = szybszciej.")]
    public float cameraSmoothTime = 0.35f;
    private Vector3 cameraVelocity = Vector3.zero;
    private Vector3 targetCamPos;
    private const float arriveThresholdSqr = 0.0009f;


    void Start()
    {
        windowNumber = 3;
        Tools = GameObject.Find("Object");
        cloth = GameObject.Find("cloth");
        sprinkle = GameObject.Find("sprinkle");
        clothSlot = GameObject.Find("clothSlot");
        sprinkleSlot = GameObject.Find("sprinkleSlot");
        Inventory = GameObject.Find("Inventory");

        Windows.Clear();
        WindowScript[] found = GameObject.FindObjectsByType<WindowScript>(FindObjectsSortMode.InstanceID);
        int ExtractTrailingNumber(string s)
        {
            int i = s.Length - 1;
            while (i >= 0 && char.IsDigit(s[i])) i--;
            string num = s.Substring(i + 1);
            if (int.TryParse(num, out int n)) return n;
            return int.MaxValue;
        }
        var ordered = found
            .OrderBy(w => ExtractTrailingNumber(w.gameObject.name))
            .ThenBy(w => w.gameObject.name)
            .ToList();
        Windows.AddRange(ordered);

        MainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
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
        sprinkle.transform.position = sprinkleSlot.transform.position;

        if (currentWindow = Windows[3])
        {
            currentWindow.GetComponent<WindowScript>().OnStart();
        }
        Vector3 startWindowPos = Windows[3].transform.position;
        MainCam.transform.position = new Vector3(startWindowPos.x, startWindowPos.y - 0.5f, 0f);
    }

    public void Update()
    {
        Inventory.transform.position = new Vector2(MainCam.gameObject.transform.position.x, MainCam.transform.position.y -8f);
        Tools.transform.position = new Vector2(MainCam.gameObject.transform.position.x, MainCam.transform.position.y -3f);
        cloth.transform.position = clothSlot.transform.position;

        CurrentLevelController();
        HandleCameraMovement();

        if (currentWindow.clearingProgress == 1f)
        {
            currentWindow.isCleaned = true;
        }

        if (currentWindow.name == "Glass4")
        {
            if (currentWindow.clearingProgress >= 0.5f && currentWindow.firstTry)
            {
                currentWindow.JumpScare();
                Debug.Log("JumpScare");
                MoveToNextLvl();
                Windows[0].GetComponent<WindowScript>().OnStart();
                cloth.GetComponent<clothScript>().holdingCloth = false;
                Debug.Log("Bouta start coroutine");
                StartCoroutine(DelayedCleanupAfterJumpscare(3f));
            }
        }
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

        Tools.SetActive(true);
        Inventory.SetActive(true);
    }

    public void MoveToNextLvl()
    {
        Tools.SetActive(false);
        Inventory.SetActive(false);

        if (windowNumber >= 3 && currentWindow.firstTry)
        {
            currentWindow.firstTry = false;
            windowNumber = 0;
            Debug.Log($"First try, moving to next level. windowNumber: {windowNumber}");
        }
        else if (windowNumber == 3 && !currentWindow.firstTry)
        {
            Debug.Log("returning");
            return;
        }
        else
        {
            windowNumber++;
            Debug.Log($"Moving to next level. windowNumber: {windowNumber}");
        }

        currentWindow = Windows[windowNumber];

        if(windowNumber >= 3 && !currentWindow.firstTry)
        {
            currentWindow.SpawnDirtOnWindows();
        }
        else
        {
            currentWindow.OnStart();
        }
        Debug.Log($"Current window: {currentWindow.gameObject.name}, current camera: {MainCam.name}");


        if (MainCam != null && currentWindow != null)
        {
            targetCamPos = new Vector3(currentWindow.transform.position.x, currentWindow.transform.position.y, MainCam.transform.position.z);
            isCameraMoving = true;
            Debug.Log($"Moving camera to {currentWindow.gameObject.name} at position {targetCamPos}");
        }
    }

    private void CharacterDeath()
    {

    }

    private IEnumerator DelayedCleanupAfterJumpscare(float delay)
    {
        yield return new WaitForSeconds(delay);
        CleanupAfterJumpscare();
    }

    private void CleanupAfterJumpscare()
    {
        WindowScript window = Windows[3];
        window.ResetGrid();
        Destroy(window.Jessy);
        window.WindowLvl = 4;
        Debug.Log("Cleanup after jumpscare completed.");

    }

}
