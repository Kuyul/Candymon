using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouthScript : MonoBehaviour {

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Candy") {
            GameControl.Instance.EatCandy(collision.gameObject);
           GameObject temp = Instantiate(GameControl.Instance.eat, transform.position, Quaternion.identity);
            Destroy(temp, 1f);
        }
    }
}
