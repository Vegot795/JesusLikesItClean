using UnityEngine;

public class sprinkleScript : MonoBehaviour
{

    private bool returning = false;
    private bool holdingSprinkle = false;
    private Vector3 offset;
    [SerializeField] private int spedd;

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
        holdingSprinkle = true;
    }

    private void OnMouseUp()
    {
        holdingSprinkle = false;
        returning = true;
    }
}
