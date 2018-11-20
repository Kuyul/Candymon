using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthScript : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Jelly") {
            GameControl.Instance.RemoveCandy(collision.gameObject);
            //Destroy(collision.gameObject);
        }
    }
}
