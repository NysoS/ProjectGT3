using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//////////////////////////
//   Kristofer Ledoux   //
// Copyright &copy 2022 //
//////////////////////////

namespace ProjectGT3
{
    public enum wheelConfiguration
    {
        Motor,
        Steer,
        Both
    }

    public class Wheel : MonoBehaviour
    {

        [SerializeField]
        private Transform wheelMesh;

        [SerializeField]
        private WheelCollider wheelCollider;


        public wheelConfiguration wheelConfiguration;

        private WheelFrictionCurve frictionCurve;

        [SerializeField]
        private float extremumValue = 1f;

        [SerializeField]
        private float asymptoteValue = 1f;

        [SerializeField]
        private float stiffness = 1f;

        [SerializeField]
        private float noDriftForce = 1f;

        [SerializeField]
        private float driftForce = 10f;

        // Start is called before the first frame update
        void Start()
        {
            frictionCurve.extremumSlip = .8f;
            frictionCurve.extremumValue = extremumValue;
            frictionCurve.asymptoteSlip = 1.4f;
            frictionCurve.asymptoteValue = asymptoteValue;
            frictionCurve.stiffness = stiffness;
        }

        private void FixedUpdate()
        {
            UpdatePos();
            wheelCollider.forwardFriction = frictionCurve;

        }


        public void WheelTorque(float torqueValue)
        {
            frictionCurve.asymptoteValue = noDriftForce;
            if (wheelConfiguration == wheelConfiguration.Motor || wheelConfiguration == wheelConfiguration.Both)
            {
                
                wheelCollider.motorTorque = torqueValue;
                wheelCollider.brakeTorque = 0;
            }
            else
            {
                wheelCollider.motorTorque = 0;
                wheelCollider.brakeTorque = 0;
            }
            
        }

        public void WheelBrake(float brakeValue,wheelConfiguration wheelConfiguration)
        {
            if(wheelConfiguration == wheelConfiguration.Motor || wheelConfiguration == wheelConfiguration.Both)
            {
                wheelCollider.motorTorque = 0;
                wheelCollider.brakeTorque = brakeValue;
            }
        }

        public void WheelDrift()
        {
            
            if (wheelConfiguration == wheelConfiguration.Motor || wheelConfiguration == wheelConfiguration.Both)
            {
                frictionCurve.asymptoteValue = driftForce;
            }
            
        }


        public void WheelSteer(float steerValue)
        {
            if(wheelConfiguration == wheelConfiguration.Steer || wheelConfiguration == wheelConfiguration.Both)
            {
                wheelCollider.steerAngle = steerValue;
            }
        }

        public void UpdatePos()
        {
            Vector3 pos;
            Quaternion rot;

            wheelCollider.GetWorldPose(out pos, out rot);
            wheelMesh.position = pos;
            wheelMesh.rotation = rot;
        }
    }
}
