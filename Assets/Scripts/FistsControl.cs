using UnityEngine;

public class FistsControl : MonoBehaviour
{
    public GameObject leftFist;
    public GameObject rightFist;
    public float punchDistance = 1.0f;

    void Update()
    {
        Vector3 cursorPos = Input.mousePosition;
        cursorPos = Camera.main.ScreenToWorldPoint(new Vector3(cursorPos.x, cursorPos.y, Camera.main.transform.position.y));

        Vector3 direction = (cursorPos - transform.position).normalized;

        leftFist.transform.position = transform.position + direction * punchDistance;
        rightFist.transform.position = transform.position + direction * punchDistance;
    }
}
