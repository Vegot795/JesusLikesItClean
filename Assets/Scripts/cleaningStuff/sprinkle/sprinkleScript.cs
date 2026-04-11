using NUnit.Framework;
using UnityEngine;

public class sprinkleScript : MonoBehaviour
{
    private bool returning = false;
    private bool holdingSprinkle = false;
    private Vector3 offset;
    private GameUI gameUI;
    private playerEQ playerEQ;

    [SerializeField] private int spedd;

    [SerializeField] private GameObject waterPrefab;

    private void Start()
    {
        playerEQ = GameObject.Find("SceneControl").GetComponent<playerEQ>();
        gameUI = GameObject.Find("UI").GetComponent<GameUI>();
        transform.position = GameObject.Find("sprinkleSlot").transform.position;
        offset.z = -10f;
        gameObject.GetComponent<SpriteRenderer>().sprite = playerEQ.sprinkleSprites[playerEQ.sprinkleLvlEf];
    }

    private void Update()
    {


        if (holdingSprinkle)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset;

            if (Input.GetMouseButtonDown(0))
            {
                Instantiate(waterPrefab, transform.position, Quaternion.identity);
            }
        }

        if (gameUI.isCameraMoving)
        {
            returning = true;
            holdingSprinkle = false;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;

        }

        
    }

    private void FixedUpdate()
    {
        if (returning)
        {
            transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("sprinkleSlot").transform.position, spedd * Time.deltaTime);
            if (Vector3.Distance(transform.position, GameObject.Find("sprinkleSlot").transform.position) < 0.1f)
            {
                returning = false;
                transform.position = GameObject.Find("sprinkleSlot").transform.position;
            }
        }
    }


    private void OnMouseDown()
    {
        if (holdingSprinkle)
        {
            Instantiate(waterPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (holdingSprinkle)
            {
                holdingSprinkle = false;
                returning = true;
            }
            else
            {
                holdingSprinkle = true;
                returning = false;
            }
        }
    }
}
