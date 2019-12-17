using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tunnel : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.layer == 10) {
			collision.gameObject.SetActive(false);
		}
	}

}
