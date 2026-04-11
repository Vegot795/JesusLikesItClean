using UnityEngine;

public class clothScript : MonoBehaviour
{
    private bool returning = false;
    public bool holdingCloth = false;
    private Vector3 offset;
    private GameUI gameUI;
    private playerEQ playerEQ;

    [SerializeField] private int spedd;
    public int efficience = 10;
    public float size = 1f;
    private Vector3 originalScale;

    private void Start()
    {
        playerEQ = GameObject.Find("SceneControl").GetComponent<playerEQ>();
        gameUI = GameObject.Find("UI").GetComponent<GameUI>();
        transform.position = GameObject.Find("clothSlot").transform.position;
        offset.z = -10f; 
        efficience = playerEQ.clotchEfficience;
        size = playerEQ.clotchSize;
        gameObject.GetComponent<SpriteRenderer>().sprite = playerEQ.clothSprites[playerEQ.clothLvlEf];

        originalScale = transform.localScale;
        transform.localScale = new Vector3(originalScale.x * size, originalScale.y * size, originalScale.z * size);
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
    }

    private void FixedUpdate()
    {
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
