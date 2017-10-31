using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arcana;
using UnityEngine;

namespace Arcana.InputManagement
{
    
    public class InputTester : ArcanaObject
    {

        public Vector3 currentPosition = Vector3.zero;
        public Vector3 basePosition = Vector3.zero;
        public float maxDistance = 5f;

        public override void Update()
        {
            base.Update();

            this.Self.transform.position = currentPosition;
        }

        public override void Initialize()
        {
            if (!this.Initialized)
            {
                base.Initialize();

                // Add the controls.
                BuildControlScheme();
                InitializeControls();
            }
        }

        protected override void HandleInput()
        {
            base.HandleInput();

            Vector3 mouseDelta = new Vector3(0, 0, basePosition.z);

            if (this.Controls.IsActivated(GetAction("Move Sphere X")))
            {
                Debugger.Print("Mouse Delta X: " + this.Controls.GetValue(GetAction("Move Sphere X")));
                mouseDelta.x = this.Controls.GetValue(GetAction("Move Sphere X"));
            }

            if (this.Controls.IsActivated(GetAction("Move Sphere Y")))
            {
                Debugger.Print("Mouse Delta Y: " + this.Controls.GetValue(GetAction("Move Sphere Y")));
                mouseDelta.y = this.Controls.GetValue(GetAction("Move Sphere Y"));
            }
            
            // update the position.
            currentPosition = Vector3.Lerp(currentPosition, (mouseDelta.normalized * maxDistance), Time.deltaTime);


        }

        protected override void BuildControlScheme()
        {
            this.Director = Director.System;
            base.BuildControlScheme();
        }

        protected override void InitializeControls()
        {
            base.InitializeControls();

            // Move the sphere.
            this.RegisterControl(ControlScheme.CreateAction("Move Sphere X"), ControlScheme.CreateTrigger(Control.MouseX()));
            this.RegisterControl(ControlScheme.CreateAction("Move Sphere Y"), ControlScheme.CreateTrigger(Control.MouseY()));
        }

    }
}
