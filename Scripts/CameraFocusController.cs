/*using UnityEngine;

public class CameraFocusController : MonoBehaviour
{
    public static CameraFocusController instance;

    public float moveSpeed = 10f;

    private Vector3 targetPosition;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        targetPosition = transform.position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            moveSpeed * Time.deltaTime
        );
    }

    public void FocusOnWord(Transform word)
    {
        targetPosition = new Vector3(
            word.position.x,
            word.position.y,
            word.position.z - 10f
        );

        Debug.Log("Caméra vers : " + targetPosition);
    }
}
*/
using UnityEngine;

public class CameraFocusController : MonoBehaviour
{
    public static CameraFocusController instance;

    public float moveSpeed = 18f;
    public float offsetZ = -10f;

    private Vector3 targetPosition;

    void Awake()
    {
        instance = this;
        targetPosition = transform.position;
    }

    void Update()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            Time.deltaTime * moveSpeed
        );
    }

    public void FocusOnWord(Transform word)
    {
        float targetZ = word.position.z + offsetZ;

        if (targetZ <= transform.position.z)
            return;

        targetPosition = new Vector3(
            word.position.x,
            word.position.y,
            targetZ
        );

        Debug.Log("Caméra vers : " + targetPosition);
    }
}

