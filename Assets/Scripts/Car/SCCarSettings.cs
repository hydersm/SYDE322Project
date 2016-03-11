using UnityEngine;
using System.Collections;

public class SCCarSettings : MonoBehaviour {

	public WheelCollider[] wheelColliders;

	public float forwardFrictionExtremeSlip;
	public float forwardFrictionExtremeVal;
	public float forwardFrictionAsympSlip;
	public float forwardFrictionAsympVal;
	public float forwardFrictionStiffness;

	public float sidewaysFrictionExtremeSlip;
	public float sidewaysFrictionExtremeVal;
	public float sidewaysFrictionAsympSlip;
	public float sidewaysFrictionAsympVal;
	public float sidewaysFrictionStiffness;

	public WheelFrictionCurve sidewaysFriction;

	// Use this for initialization
	void Start () {

		foreach (WheelCollider wheelCollider in wheelColliders) {
			WheelFrictionCurve wfc = new WheelFrictionCurve ();
			wfc.extremumSlip = forwardFrictionExtremeSlip;
			wfc.extremumValue = forwardFrictionExtremeVal;
			wfc.asymptoteSlip = forwardFrictionAsympSlip;
			wfc.asymptoteValue = forwardFrictionAsympVal;
			wfc.stiffness = forwardFrictionStiffness;
			wheelCollider.forwardFriction = wfc;

			WheelFrictionCurve wfc2 = new WheelFrictionCurve ();
			wfc2.extremumSlip = sidewaysFrictionExtremeSlip;
			wfc2.extremumValue = sidewaysFrictionExtremeVal;
			wfc2.asymptoteSlip = sidewaysFrictionAsympSlip;
			wfc2.asymptoteValue = sidewaysFrictionAsympVal;
			wfc2.stiffness = sidewaysFrictionStiffness;
			wheelCollider.sidewaysFriction = wfc2;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
