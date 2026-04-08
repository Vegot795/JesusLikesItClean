using UnityEngine;

public class sprinkleScript : MonoBehaviour
{

    private bool returning = false;
    private bool holdingSprinkle = false;
    private Vector3 offset;
    [SerializeField] private int spedd;

    [SerializeField] private GameObject waterPrefab;

    private void Start()
    {
        transform.position = GameObject.Find("sprinkleSlot").transform.position;
        offset.z = -10f;
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
