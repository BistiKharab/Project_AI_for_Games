using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectionScript : MonoBehaviour
{
    public float time;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
            time = Time.time;
    }
}
