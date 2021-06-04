using System.Collections;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField] private Transform cannon;
    [SerializeField] private GameObject[] lasers;


    public float shootingTime = 0.1f;
    public int currentLaser = 0;
    private bool shooting;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var bottomLeft = Camera.main.ScreenToWorldPoint(Vector3.zero);
        var topRight = Camera.main.ScreenToWorldPoint(
            new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight));

        var cameraRect = new Rect(
            bottomLeft.x,
            bottomLeft.y,
            topRight.x - bottomLeft.x,
            topRight.y - bottomLeft.y
        );

        Vector3 position = transform.position;
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        position.x += x * 5.0f * Time.deltaTime;
        position.y += y * 5.0f * Time.deltaTime;

        transform.position = position;

        float clampX = Mathf.Clamp(transform.position.x, cameraRect.xMin, cameraRect.xMax);
        float clampY = Mathf.Clamp(transform.position.y, cameraRect.yMin, cameraRect.yMax);

        transform.position = new Vector3(clampX, clampY, transform.position.z);

        if (currentLaser >= lasers.Length) currentLaser = 0;
        if (Input.GetKeyDown(KeyCode.E))
        {
            ++currentLaser;
        }
        if (Input.GetKeyDown(KeyCode.Space)) Shot(currentLaser);

    }

    void Shot(int currentLaser)
    {
        if (shooting) return;
        shooting = true;

        GameObject newShot = Instantiate(lasers[currentLaser], cannon.position, cannon.rotation);
        newShot.TryGetComponent(out Rigidbody2D rb);
        rb.AddForce(Vector3.up * 5, ForceMode2D.Impulse);
        Destroy(newShot, 2);

        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown ()
    {
        yield return new WaitForSeconds(shootingTime);
        shooting = false;
    }
}
