using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    float hInput, vInput, mouseX, mouseY, rotY ;
    public float movementSpeed, rotationSpeed;
    Rigidbody rBody;
    GameObject cam;
	RaycastHit hit;
	public float damageValue;
    // Use this for initialization
    private void Awake()
    {
        rBody = GetComponent<Rigidbody>();
        cam = transform.Find("Camera").gameObject;
    }
    void Start()
    {
        rotY = 0;
		Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        hInput = Input.GetAxis("Horizontal");
        vInput = Input.GetAxis("Vertical");
        mouseX = Input.GetAxis("Mouse X");
        rotY += Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        rotY = Mathf.Clamp(rotY, -90, 90);
        transform.Translate(Vector3.forward * vInput * Time.deltaTime * movementSpeed);
        transform.Translate(Vector3.right* hInput * Time.deltaTime * movementSpeed);
        transform.Rotate(Vector3.up * rotationSpeed * mouseX*Time.deltaTime);
        cam.transform.localRotation = Quaternion.Euler(-rotY, 0, 0);
    
 
		if(Input.GetButtonDown("Fire1"))
		{
			Shoot();
		}
	}	
     
	void Shoot()
	{
		if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, 50))
		{
			Debug.Log("Hit " + hit.transform.gameObject.name);
			if(hit.transform.gameObject.tag == "Enemy")
			{
				hit.transform.gameObject.SendMessageUpwards("ApplyDamage", damageValue);
			}
		}
	}
}
