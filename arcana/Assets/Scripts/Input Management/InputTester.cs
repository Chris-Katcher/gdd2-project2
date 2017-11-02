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
        public Vector3 targetPosition = Vector3.zero;
        private Vector3 basePosition = Vector3.zero;
        public float maxDistance = 5f;

        public bool leftStick = true;

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

                this.Debug = false;
                this.Status.Activate();

                if (this.leftStick) { this.Name = "Left Stick"; }
                else { this.Name = "Right Stick"; }

                this.basePosition = this.transform.position;
                this.currentPosition = this.basePosition;
                this.targetPosition = this.basePosition;

                // Add the controls.
                BuildControlScheme();
                InitializeControls();
            }
        }

        protected override void HandleInput()
        {
            base.HandleInput();

            this.Debug = true;

            Vector3 delta = Vector3.zero;

            if (this.Controls.IsActivated(GetAction("Move Sphere X")))
            {
                // Debugger.Print("Mouse Delta X: " + this.Controls.GetValue(GetAction("Move Sphere X")), this.Self.name, this.Debug);
                // mouseDelta.x = this.Controls.GetValue(GetAction("Move Sphere X"));

                float axisValue = this.Controls.GetValue(GetAction("Move Sphere X"));
                Debugger.Print("X: " + axisValue, this.Self.name, this.Debug);
                delta.x = axisValue;
            }

            if (this.Controls.IsActivated(GetAction("Move Sphere Y")))
            {
                // Debugger.Print("Mouse Delta Y: " + this.Controls.GetValue(GetAction("Move Sphere Y")), this.Self.name, this.Debug);
                // mouseDelta.y = this.Controls.GetValue(GetAction("Move Sphere Y"));

                float axisValue = -this.Controls.GetValue(GetAction("Move Sphere Y"));
                Debugger.Print("Y: " + axisValue, this.Self.name, this.Debug);
                delta.y = axisValue;
            }

            float distance = Vector3.Distance(targetPosition, basePosition);

            if (delta != Vector3.zero)
            {
                delta = delta.normalized;
                if (distance < maxDistance)
                {
                    targetPosition = basePosition + (delta * maxDistance * Time.deltaTime);
                }

                // Lerp the current position.
                currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * 2.0f * distance);
            }
            else
            {
                targetPosition = basePosition;

                // Lerp the current position.
                currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.deltaTime * 4.0f);
            }
            

            // currentPosition.z = basePosition.z;
            // targetPosition.z = basePosition.z;

            if (this.Controls.IsActivated(GetAction("A Button Press")))
            {
                Debugger.Print("A Button has been pressed on a controller.", this.Self.name, this.Debug);
            }

            if (this.Controls.IsActivated(GetAction("A Button Release")))
            {
                Debugger.Print("A Button has been released on a controller.", this.Self.name, true);
            }
            
            if (this.Controls.IsActivated(GetAction("B Button Press")))
            {
                Debugger.Print("B Button has been pressed on a controller.", this.Self.name, this.Debug);
            }

            if (this.Controls.IsActivated(GetAction("B Button Release")))
            {
                Debugger.Print("B Button has been released on a controller.", this.Self.name, true);
            }
            
            if (this.Controls.IsActivated(GetAction("LTP")))
            {
                float axisValue = this.Controls.GetValue(GetAction("LTP"));

              if (axisValue != 0.0f)
              {
                    Debugger.Print("Left trigger value: " + axisValue, this.Self.name, this.Debug);
              }
            }

            if (this.Controls.IsActivated(GetAction("RTP")))
            {
                float axisValue = this.Controls.GetValue(GetAction("RTP"));

              if (axisValue != 0.0f)
              {
                    Debugger.Print("Right trigger value: " + axisValue, this.Self.name, this.Debug);
              }
            }
           
            this.Debug = false;
        }

        protected override void BuildControlScheme()
        {
            this.Director = Director.System;
            base.BuildControlScheme();
        }

        protected override ControlScheme InitializeControls()
        {
            this.m_scheme = base.InitializeControls();

            // Move the sphere.
            // this.RegisterControl(ControlScheme.CreateAction("Move Sphere X"), ControlScheme.CreateTrigger(Control.MouseX()));
            // this.RegisterControl(ControlScheme.CreateAction("Move Sphere X"), ControlScheme.CreateTrigger(Control.MouseX()));

            if (leftStick)
            {
                this.RegisterControl(ControlScheme.CreateAction("Move Sphere X"), ControlScheme.CreateTrigger(Control.LeftStickHorizontal(1)));
                this.RegisterControl(ControlScheme.CreateAction("Move Sphere Y"), ControlScheme.CreateTrigger(Control.LeftStickVertical(1)));
            }
            else
            {
                this.RegisterControl(ControlScheme.CreateAction("Move Sphere X"), ControlScheme.CreateTrigger(Control.RightStickHorizontal(1)));
                this.RegisterControl(ControlScheme.CreateAction("Move Sphere Y"), ControlScheme.CreateTrigger(Control.RightStickVertical(1)));
            }
                      
            this.RegisterControl(ControlScheme.CreateAction("A Button Press"), ControlScheme.CreateTrigger(Control.AButton(1), ResponseMode.Pressed));
            this.RegisterControl(ControlScheme.CreateAction("A Button Release"), ControlScheme.CreateTrigger(Control.AButton(1), ResponseMode.Released));

            this.RegisterControl(ControlScheme.CreateAction("B Button Press"), ControlScheme.CreateTrigger(Control.BButton(1), ResponseMode.Pressed));
            this.RegisterControl(ControlScheme.CreateAction("B Button Release"), ControlScheme.CreateTrigger(Control.BButton(1), ResponseMode.Released));

            this.RegisterControl(ControlScheme.CreateAction("Triggers"), ControlScheme.CreateTrigger(Control.BothTriggers(1)));
            this.RegisterControl(ControlScheme.CreateAction("LTP"), ControlScheme.CreateTrigger(Control.LeftTrigger(1)));
            this.RegisterControl(ControlScheme.CreateAction("RTP"), ControlScheme.CreateTrigger(Control.RightTrigger(1)));

            return this.m_scheme;
        }

    }
}
