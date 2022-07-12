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
    public enum CarState
    {
        Idle,
        Forward,
        Back
    }

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
        private float coefAcceleration, coefDeceleration;

        [SerializeField]
        private Wheel[] wheels;

        [SerializeField]
        private Vector3 centerOfMass;

        private Rigidbody rb;

        private float axisValue;
        private bool drift = false;

        public float speed;
        public CarState carState = CarState.Idle;
        public CarState nextcarState = CarState.Idle;

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

    

        private void FixedUpdate()
        {
            speed = rb.velocity.magnitude * 3.6f;
            MoveCar();
        }

        void MoveCar()
        {
            bool forward = axisValue > 0 ? true : false;
            bool backward = axisValue < 0 ? true : false;

            foreach (Wheel wheel in wheels)
            {

                if(forward && speed > 0.1f)
                {
                    nextcarState = CarState.Forward;
                }
                else if(backward && speed > 0.1f)
                {
                    nextcarState = CarState.Back;
                }else if(speed < 0.1f)
                {
                    nextcarState = CarState.Idle;
                }

                if (drift)
                {
                    //wheel.WheelBrake(brakeForce  * Time.deltaTime);
                    wheel.WheelDrift();
                }
                else
                {

                    if (forward || backward)
                    {
                       if (carState != nextcarState && carState != CarState.Idle)
                        {
                            wheel.WheelBrake(brakeForce, wheelConfiguration.Motor);
                            
                            if(speed <= 0.1f)
                            {
                                carState = nextcarState;
                            }
                        }
                        else
                        {
                            carState = nextcarState;
                            wheel.WheelTorque(axisValue * torque * coefAcceleration * Time.deltaTime);
                        }
                    }
                    else
                    {
                        wheel.WheelBrake(brakeSmooth * coefDeceleration * Time.deltaTime,wheelConfiguration.Motor);
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
