using UnityEngine;
using System.Collections;

public class SCCarRespawn : MonoBehaviour {

    void OnTriggerExit(Collider car)
    {
        Vector2 randPos = Random.insideUnitCircle * 50;
        Vector3 spawnPos = new Vector3(randPos.x, 1f, randPos.y);
        car.transform.root.position = spawnPos;
        car.transform.root.GetComponent<SCCarController>().currentMotorTorque = 0;
        car.transform.root.GetComponent<SCCarController>().currentSteeringAngle = 0;
        car.transform.root.GetComponent<Rigidbody>().velocity = Vector3.zero;
        car.transform.root.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;

    }
}
