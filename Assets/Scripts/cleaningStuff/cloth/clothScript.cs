using UnityEngine;

public class clothScript : MonoBehaviour
{

    private bool returning = false;
    private bool holdingCloth = false;
    private Vector3 offset;
    [SerializeField] private int spedd;
    public int efficience = 10;
    public float size = 1f;
    private Vector3 originalScale;

    private void Start()
    {
        transform.position = GameObject.Find("clothSlot").transform.position;
        offset.z = -10f; 
        efficience = GameObject.Find("SceneControl").GetComponent<playerEQ>().clotchEfficience;
        size = GameObject.Find("SceneControl").GetComponent<playerEQ>().clotchSize;
        
        originalScale = transform.localScale;
        transform.localScale = new Vector3(originalScale.x * size, originalScale.y * size, originalScale.z * size);
    }

    private void Update()
    {
        if (holdingCloth)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) - offset ;
        }

        if (returning)
        {
            transform.position = Vector3.MoveTowards(transform.position, GameObject.Find("clothSlot").transform.position, spedd * Time.deltaTime);
            if (Vector3.Distance(transform.position, GameObject.Find("clothSlot").transform.position) < 0.1f)
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
