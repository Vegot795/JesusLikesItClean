using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class whereDirt : MonoBehaviour
{
    private GameObject magicEye;
    private GameUI gameUI;
    private playerEQ _playerEQ;

    public WindowScript[] windows;

    public Image image;
    public GameObject previewCellPrefab;
    public List<GameObject> previewCells = new List<GameObject>();
    public GameObject previewCell;

    private float cellX;
    private float cellY;

    private bool isButtonHeld = false;

    void Start()
    {
        image = gameObject.GetComponent<Image>();
        _playerEQ = GameObject.Find("SceneControl").GetComponent<playerEQ>();
        gameUI = GameObject.Find("UI").GetComponent<GameUI>();
        magicEye = GameObject.Find("whereDirt");
        image.enabled = false;
    }

    void Update()
    {
        int lvl = GameObject.Find("Game").GetComponent<GameLoad>().licznik;
        if (_playerEQ.firstTry)
        {
            image.enabled = false;
        }
        else
        {
            if (gameUI.currentWindow.clearingProgress >= 0.9f)
            {
                image.enabled = true;
            }
            else if (gameUI.currentWindow.clearingProgress < 0.9f)
            {
                image.enabled = false;
            }
        }
    }

    public void SwitchDirtVision()
    {
        if (!isButtonHeld && Input.GetKeyDown(KeyCode.Mouse0))
        {
            isButtonHeld = true;
            RevealDirt();
        }
        if (isButtonHeld && !Input.GetKeyUp(KeyCode.Mouse0))
        {
            isButtonHeld = false;
            HideDirt();
        }
    }

    private void RevealDirt()
    { 
        foreach (GameObject cell in gameUI.currentWindow.stainedCells)
        {
            cellX = cell.transform.position.x;
            cellY = cell.transform.position.y;

            Vector3 spawnPosition = new Vector3(cellX, cellY, 0f);

            previewCell = Instantiate(previewCellPrefab, spawnPosition, Quaternion.identity);
            previewCell.transform.SetParent(gameObject.transform);
            previewCells.Add(previewCell);
        }
    }

    private void HideDirt()
    {
        
        foreach (GameObject cell in previewCells)
        {
            Destroy(cell);
        }
    }
}
