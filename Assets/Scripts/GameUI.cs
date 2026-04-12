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
    public Sprite _wallSprite;
    public Sprite _starSprite;
    public CutsceneManager cutsceneManager;
    public bool isCameraMoving = false;
    public GameObject GameRoot;
    public GameObject SceneControl;
    public GameObject Sciana4;
    public GameObject Decorations4;

    private int windowNumber = 3;
    private Vector3 CurrentCamPos;

    [Tooltip("Wygładzanie ruchu kamery. Mniej = szybszciej.")]
    public float cameraSmoothTime = 0.35f;
    private Vector3 cameraVelocity = Vector3.zero;
    private Vector3 targetCamPos;
    private const float arriveThresholdSqr = 0.0009f;

    private GameObject glass1;
    public bool isShop = false;
    public GameObject tutorial;
    public GameObject loading;
    public GameObject holyBar;

    private float timer = -1;

    private bool tak = true;
    private bool introPlayed = false;

    void Start()
    {
        if (!isShop)
        {
            windowNumber = 3;
            loading.SetActive(true);
            if (GameObject.Find("Object") != null)
            {
                Tools = GameObject.Find("Object");
            }
            if (GameObject.Find("cloth") != null)
            {
                cloth = GameObject.Find("cloth");
            }
            if (GameObject.Find("sprinkle") != null)
            {
                sprinkle = GameObject.Find("sprinkle");
            }
            if (GameObject.Find("clothSlot") != null)
            {
                clothSlot = GameObject.Find("clothSlot");
            }
            if (GameObject.Find("sprinkleSlot") != null)
            {
                sprinkleSlot = GameObject.Find("sprinkleSlot");
            }
            if (GameObject.Find("Inventory") != null)
            {
                Inventory = GameObject.Find("Inventory");
            }
            if (GameObject.Find("Game") != null)
            {
                GameRoot = GameObject.Find("Game");
            }
            if (GameObject.Find("SceneControl") != null)
            {
                SceneControl = GameObject.Find("SceneControl");
            }
            isCameraMoving = true;

            // Znajdź wszystkie obiekty WindowScript i posortuj je według numeru na końcu nazwy
            Windows.Clear();
            WindowScript[] found = GameObject.FindObjectsByType<WindowScript>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID);
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


            // Znajdź kamerę główną
            if (GameObject.Find("Main Camera") != null) ;
            {
                MainCam = GameObject.Find("Main Camera").GetComponent<Camera>();
            }
            if (MainCam != null && MainCam.CompareTag("MainCamera"))
            {
                currentWindow = Windows[windowNumber];
                CurrentCamPos = currentWindow.transform.position;
                targetCamPos = new Vector3(CurrentCamPos.x, CurrentCamPos.y - 0.5f, MainCam.transform.position.z);
            }
            sprinkle.transform.position = sprinkleSlot.transform.position;

            // Pobierz sprite ściany i przypisz go do okna 4
            GameObject glass4 = Windows[3].gameObject;
            glass4.transform.parent.parent.GetComponent<Animator>().enabled = false;
            glass4.transform.parent.parent.GetComponent<SpriteRenderer>().sprite = _wallSprite;
            glass4.transform.Find("Decorations4Wall").gameObject.SetActive(false);
            glass1 = GameObject.Find("Glass1");


            // Ustawienie początkowej pozycji kamery na aktualne okno
            if (GameObject.Find("SceneControl").GetComponent<playerEQ>().firstTry == false)
            {
                tak = false;
                windowNumber = -1;
                MoveToNextLvl();

            }

            StartCoroutine(PlayIntroAfterLoad());
        }
    }

    private IEnumerator PlayIntroAfterLoad()
    {
        if (GameRoot == null || cutsceneManager == null || SceneControl == null)
            yield break;

        GameLoad gameLoad = GameRoot.GetComponent<GameLoad>();
        playerEQ playerEq = SceneControl.GetComponent<playerEQ>();

        while (gameLoad != null && !gameLoad.loaded)
        {
            yield return null;
        }

        if (!introPlayed && playerEq != null && playerEq.firstTry)
        {
            introPlayed = true;
            cutsceneManager.PlayIntro();
        }
    }
    public void Update()
    {

        if (!isShop)
        {
            if (timer >= 0)
            {
                timer += Time.deltaTime;
                GameObject.Find("SceneControl").GetComponent<playerEQ>().time = timer;
            }

            if (GameObject.Find("SceneControl").GetComponent<playerEQ>().firstTry == false && GameObject.Find("Game").GetComponent<GameLoad>().loaded && !tak && GameObject.Find("SceneControl").GetComponent<playerEQ>().time > 10)
            {
                holyBar.GetComponent<HolyPower>().ShowBar();
                holyBar.GetComponent<HolyPower>().working = true;
                timer = 0;
                tak = true;
            }

            if (MainCam == null || Inventory == null || Tools == null || cloth == null || clothSlot == null)
            {
                Debug.LogError($"Missing UI reference. MainCam: {MainCam != null}, Inventory: {Inventory != null}, Tools: {Tools != null}, cloth: {cloth != null}, clothSlot: {clothSlot != null}");
                return;
            }

            Inventory.transform.position = new Vector2(MainCam.gameObject.transform.position.x, MainCam.transform.position.y - 8f);
            Tools.transform.position = new Vector2(MainCam.gameObject.transform.position.x, MainCam.transform.position.y - 3f);
            cloth.transform.position = clothSlot.transform.position;

            HandleCameraMovement();

            //Debug.Log($"CurrentWindow: {currentWindow != null}, GameRoot: {GameRoot != null}, SceneControl: {SceneControl != null}");
            if (currentWindow == Windows[3] && GameRoot.GetComponent<GameLoad>().loaded && SceneControl.GetComponent<playerEQ>().firstTry == true)
            {
                if (currentWindow.clearingProgress >= 0.5f && currentWindow.firstTry)
                {
                    glass1.SetActive(true);
                    //currentWindow.JumpScare();
                    
                    // Subscribe to event before playing cutscene
                    cutsceneManager.player.OnCutsceneFinished += OnMidCutsceneFinished;
                    cutsceneManager.PlayMid();
                    
                    MoveToNextLvl();
                    tutorial.SetActive(true);
                    Debug.Log("JumpScare");
                    GameObject.Find("SceneControl").GetComponent<playerEQ>().points *= .1f;
                    GameObject.Find("HolyPower").GetComponent<HolyPower>().holyPowerPoints = 100;

                    StartCoroutine(DelayedCleanupAfterJumpscare(1.5f));
                }
            }

            if (GameRoot.GetComponent<GameLoad>().loaded && currentWindow.clearingProgress == 1f)
            {
                currentWindow.isCleaned = true;
            }

            if (Windows[3].isCleaned && currentWindow == Windows[3] && GameRoot.GetComponent<GameLoad>().loaded)
            {
                cutsceneManager.PlayOutro();
            }
        }
    }

    public void LoadScene(int scena) // #0 menu startowe, #1 gra, #2 sklep
    {
        //Cursor.visible = true;
        SceneManager.LoadScene(scena);
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
            GameObject.Find("SceneControl").GetComponent<playerEQ>().firstTry = false;
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
        Debug.Log($"Current window: {currentWindow.gameObject.name}, current camera: {MainCam.name}, firstTry: {currentWindow.firstTry}");

        if (MainCam != null && currentWindow != null)
        {
            targetCamPos = new Vector3(currentWindow.transform.position.x, currentWindow.transform.position.y - 0.05f, MainCam.transform.position.z);
            isCameraMoving = true;
            Debug.Log($"Moving camera to {currentWindow.gameObject.name} at position {targetCamPos}");
        }
        else
        {
            Debug.LogError("MainCam or currentWindow is null. Cannot move camera.");
            return;
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
        window.SpawnBloodOnWindow();
        window.SpawnDirtOnWindows();
        GameObject.Find("Glass4").SetActive(false);

        Debug.Log("Cleanup after jumpscare completed.");

    }

    public void Hide(GameObject obj)
    {
        obj.SetActive(false);
        holyBar.GetComponent<HolyPower>().ShowBar();
        holyBar.GetComponent<HolyPower>().working = true;
        timer = 0;
    }

    private void OnMidCutsceneFinished()
    {
        cutsceneManager.player.OnCutsceneFinished -= OnMidCutsceneFinished;
        
        if (Sciana4 != null && Decorations4 != null)
        {
            Sciana4.GetComponent<SpriteRenderer>().sprite = _starSprite;
            Sciana4.GetComponent<Animator>().enabled = true;
            Sciana4.GetComponent<Animator>().Play("SkyAnimation");
            Decorations4.SetActive(true);
            Debug.Log("Mid cutscene finished, scene transformation completed.");
        }
        else
        {
            Debug.LogError("Sciana4 or Decorations4 reference is missing!");
        }
    }
}