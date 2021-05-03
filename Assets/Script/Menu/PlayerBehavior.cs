using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float sensitivity = 10f;
    public float maxYAngle = 80f;
    private Vector2 currentRotation;

    void Update()
    {
        if (Input.GetMouseButton(1) && this.name == "Camera")
        {
            currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
            currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
            currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
            currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
            transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
            if (Input.GetMouseButtonDown(0))
                Cursor.lockState = CursorLockMode.Locked;
        }
        else if(Input.GetKey(KeyCode.LeftShift) && this.name == "LeftHand")
        {
            if (Input.GetAxis("Mouse X") > 0 && !Input.GetMouseButton(2))
            {
                transform.position += new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity,
                                           Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity, 0.0f);
            }

            else if (Input.GetAxis("Mouse X") < 0 && !Input.GetMouseButton(2))
            {
                transform.position += new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity,
                                           Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity, 0.0f);
            }

            else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                transform.position += new Vector3(0.0f, 0.0f, 0.5f);
            }

            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                transform.position += new Vector3(0.0f, 0.0f, -0.5f);
            }

            else if(Input.GetMouseButton(2))
            {
                currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
                currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
                currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
                currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
                transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
                if (Input.GetMouseButtonDown(0))
                    Cursor.lockState = CursorLockMode.Locked;
            }
        }
        else if (Input.GetKey(KeyCode.Space) && this.name == "RightHand")
        {
            if (Input.GetAxis("Mouse X") > 0 && !Input.GetMouseButton(2))
            {
                transform.position += new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity,
                                           Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity, 0.0f);
            }

            else if (Input.GetAxis("Mouse X") < 0 && !Input.GetMouseButton(2))
            {
                transform.position += new Vector3(Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity,
                                           Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity, 0.0f);
            }

            else if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                transform.position += new Vector3(0.0f, 0.0f, 0.5f);
            }

            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                transform.position += new Vector3(0.0f, 0.0f, -0.5f);
            }

            else if (Input.GetMouseButton(2))
            {
                currentRotation.x += Input.GetAxis("Mouse X") * sensitivity;
                currentRotation.y -= Input.GetAxis("Mouse Y") * sensitivity;
                currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
                currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
                transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
                if (Input.GetMouseButtonDown(0))
                    Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}
