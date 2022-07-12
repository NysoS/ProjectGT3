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

        [SerializeField]
        public wheelConfiguration wheelConfiguration;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            UpdatePos();
        }

        public void WheelTorque(float torqueValue)
        {
            if(wheelConfiguration == wheelConfiguration.Motor || wheelConfiguration == wheelConfiguration.Both)
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

        public void WheelBrake(float brakeValue)
        {
            wheelCollider.motorTorque = 0;
            wheelCollider.brakeTorque = brakeValue;
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
