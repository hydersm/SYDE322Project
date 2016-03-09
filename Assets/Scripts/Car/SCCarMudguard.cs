using System;
using UnityEngine;

public class SCCarMudguard : MonoBehaviour
{

    private Quaternion m_OriginalRotation;
	public SCCarController carController;


    private void Start()
    {
        m_OriginalRotation = transform.localRotation;
    }


    private void Update()
    {
		transform.localRotation = m_OriginalRotation*Quaternion.Euler(0, carController.currentSteeringAngle, 0);
    }
}
