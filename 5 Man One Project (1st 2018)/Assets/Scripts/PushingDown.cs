using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingDown : MonoBehaviour {

    private PlayerController player;
    public bool inside;
    public float downForce;
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (inside)
        {
            //player.transform.position += Vector3.down *Time.deltaTime * downForce;
            player.rb.AddForce(Vector3.down * Time.deltaTime * downForce);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        player = other.GetComponent<PlayerController>();
        inside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        inside = false;
    }
}
