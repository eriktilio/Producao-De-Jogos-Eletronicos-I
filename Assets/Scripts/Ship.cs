using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : GameEntity
{
	[SerializeField] private Transform cannon;
	[SerializeField] private GameObject[] lasers;

	public Spawn spawn { get; set; }
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
    		new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight)
		);
		
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
    
    void Shot (int currentLaser) 
    {
    	if (shooting) return;
    	shooting = true;
    
    	GameObject newShot = Instantiate(lasers[currentLaser], cannon.position, cannon.rotation);
    	newShot.TryGetComponent(out Shot shot);
    	StartCoroutine(Cooldown(shot.cooldown));
    }
    
    IEnumerator Cooldown (float time) 
    {
    	yield return new WaitForSeconds(time);
    	shooting = false;
    }

  protected override void OnTriggerEnter2D(Collider2D collision)
  {
		spawn.SpawnShip();
    base.OnTriggerEnter2D(collision);
  }
}