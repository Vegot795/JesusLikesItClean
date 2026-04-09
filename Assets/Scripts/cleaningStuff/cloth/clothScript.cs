using UnityEngine;

public class clothScript : MonoBehaviour
{

    private bool returning = false;
    private bool holdingCloth = false;
    private Vector3 offset;
    private GameUI gameUI;

    [SerializeField] private int spedd;
    public int efficience = 50;

    private void Start()
    {
        gameUI = GameObject.Find("UI").GetComponent<GameUI>();
        transform.position = GameObject.Find("clothSlot").transform.position;
        offset.z = -10f; 
    }

    private void Update()
    {
        if (holdingCloth)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset ;
        }

        if(gameUI.isCameraMoving)
        {
            returning = true;
            holdingCloth = false;
            GetComponent<Rigidbody2D>().simulated = false;
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
        }
        else
        {
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;

        }

        if (returning)
        {
            transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("clothSlot").transform.position, spedd * Time.deltaTime);
            if (Vector3.Distance(transform.position, GameObject.Find("clothSlot").transform.position) < 0.5f)
            {
                returning = false;
                transform.position = GameObject.Find("clothSlot").transform.position;
                GetComponent<Rigidbody2D>().simulated = true;
            }
        }
    }


    private void OnMouseDown()
    {
        holdingCloth = true;
        
    }

    private void OnMouseUp()
    {
        holdingCloth = false;
        returning = true;
        GetComponent<Rigidbody2D>().simulated = false;
    }
}
