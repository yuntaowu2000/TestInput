using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 1f;
    [SerializeField] GameObject screen = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // simulate the rotation of the head camera
        float horizontalSpeed = Input.GetAxis("Mouse X");
        transform.RotateAround(transform.position, transform.up, horizontalSpeed * rotationSpeed);
    }

    void LateUpdate() {
        if (screen == null) return;
        // set the screen location to be in front of the user
        Vector3 newScreenLocation = transform.position + transform.forward * 150f;
        screen.transform.position = newScreenLocation;
        // set the screen rotation, 
        // so that the screen always faces the user at the correct direction.
        screen.transform.LookAt(this.transform.position);
        screen.transform.forward = -screen.transform.forward;
    }
}
