using ProjectGT3.Input;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//////////////////////////
//   Kristofer Ledoux   //
// Copyright &copy 2022 //
//////////////////////////

namespace ProjectGT3
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarController : MonoBehaviour
    {
        private InputManager inputManager;

        [SerializeField]
        private float torque;
        [SerializeField]
        private float brakeSmooth = 500f;
        [SerializeField]
        private float brakeForce = 25000f;

        [SerializeField]
        private float maxSteer = 10f;

        [SerializeField]
        private float acceleration;

        [SerializeField]
        private Wheel[] wheels;

        [SerializeField]
        private Vector3 centerOfMass;

        private Rigidbody rb;

        private float axisValue;
        private bool drift = false;

        private void Awake()
        {
            inputManager = new InputManager();
            inputManager.CarControl.MoveForward.performed += MoveCarInput;
            inputManager.CarControl.TurnRight.performed += TurnCar;
            inputManager.CarControl.Drift.performed += DriftCar;

            rb = GetComponent<Rigidbody>();
            rb.centerOfMass = centerOfMass;
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            MoveCar();
        }

        private void FixedUpdate()
        {

            
        }

        void MoveCar()
        {
            bool forward = axisValue > 0 ? true : false;
            bool backward = axisValue < 0 ? true : false;
            
            foreach (Wheel wheel in wheels)
            {
                if (drift)
                {
                    wheel.WheelBrake(brakeForce * acceleration * Time.deltaTime * 1000f);
                }
                else
                {
                    if (forward || backward)
                    {
                   
                       wheel.WheelTorque(torque * axisValue * acceleration * Time.deltaTime * 2000f);
                    }
                    else
                    {
                        wheel.WheelTorque(0);
                    }
                }
               
            }
        }

        void MoveCarInput(InputAction.CallbackContext ctx)
        {
            axisValue = ctx.ReadValue<float>(); 
        }

        void TurnCar(InputAction.CallbackContext ctx)
        {
            float axisValue = ctx.ReadValue<float>();

            foreach(Wheel wheel in wheels)
            {
                wheel.WheelSteer(axisValue * maxSteer);
            }
        }

        void DriftCar(InputAction.CallbackContext ctx)
        {
            drift = ctx.ReadValue<float>()==1?true:false;
        }

        private void OnEnable()
        {
            inputManager.CarControl.Enable();
        }

        private void OnDisable()
        {
            inputManager.CarControl.Disable();
        }
    }
}
