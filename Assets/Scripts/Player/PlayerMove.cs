using UnityEngine;

public class PlayerMove : MonoBehaviour
{
	public float moveSpeed = 6.0f;
	public float rotateSpeed = 10.0f;
	//public float jumpVelocity = 5.0f;

	private float minMouseRotateX = -45.0f;
    private float maxMouseRotateX = 45.0f;
    private float mouseRotateX;

	public GameObject keyboard;
	
	Camera myCamera;
    //Rigidbody rigid;
    //bool isGrounded;
    //CapsuleCollider capsuleCollider;

    void Start()
    {
		myCamera = Camera.main;
		mouseRotateX = myCamera.transform.localEulerAngles.x;
		//rigid = GetComponent<Rigidbody> ();
		//capsuleCollider = GetComponent<CapsuleCollider> ();
	}
    
	void Update()
	{
        if (GetComponent<Photon.Pun.PhotonView>().IsMine == true)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Move(h, v);

			if(keyboard.activeSelf == false){
				float rv = Input.GetAxisRaw("Mouse X");
				float rh = Input.GetAxisRaw("Mouse Y");
				Rotate(rh, rv);
			}
        }
	}
    
	void Move(float h,float v)
    {
		transform.Translate ((Vector3.forward * v + Vector3.right * h) * moveSpeed * Time.deltaTime);
	}

	void Rotate(float rh,float rv)
    {
		transform.Rotate (0, rv * rotateSpeed, 0);
		mouseRotateX -= rh * rotateSpeed;
		mouseRotateX = Mathf.Clamp (mouseRotateX, minMouseRotateX, maxMouseRotateX);
		myCamera.transform.localEulerAngles = new Vector3 (mouseRotateX, 0.0f, 0.0f);
	}
}