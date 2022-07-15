using ProjectGT3.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGT3
{
    public class RacingCarController : MonoBehaviour
    {
        private InputManager inputManager;

        [SerializeField]
        private float moteur;
        [SerializeField]
        private float regimeMoteur;

        [SerializeField]
        private float vitesseValue;

        private float vertical;

        private void Awake()
        {
            inputManager = new InputManager();
            inputManager.CarControl.MoveForward.performed += MoveCarInput;
        }

        private void MoveCarInput(UnityEngine.InputSystem.InputAction.CallbackContext ctx)
        {
            vertical = ctx.ReadValue<float>();
        }

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
           //rpm = 
           //torque = vitesse[].evaluate(rpm)*vertical
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
