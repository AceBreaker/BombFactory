using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    [SerializeField]
    Camera mainCam = null;

    GameObject bombHeld = null;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach(RaycastHit hit in hits)
            {
                if (hit.collider.tag == "Bomb")
                {
                    bombHeld = hit.collider.gameObject;
                    bombHeld.transform.position = new Vector3(bombHeld.transform.position.x, 3.0f, bombHeld.transform.position.z);
                    bombHeld.GetComponent<Bomb>().ToggleGravity();
                }

            }
        }
        else if(bombHeld != null && Input.GetMouseButtonUp(0))
        {
            bombHeld.GetComponent<Bomb>().ToggleGravity();
            bombHeld = null;
        }
        else if(bombHeld != null && Input.GetMouseButton(0))
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.tag == "MovePlane")
                {
                    Debug.Log("raycast hit MovePlane");
                    bombHeld.GetComponent<Rigidbody>().MovePosition(hit.point);
                    break;
                }

            }
        }
	}
}
