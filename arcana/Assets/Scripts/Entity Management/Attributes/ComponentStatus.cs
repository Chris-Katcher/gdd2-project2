using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana.Entities.Attributes
{

    /// <summary>
    /// Represents a status that is given to a component.
    /// </summary>
    public enum ComponentStatus
    {
        NULL,
        Init,
        Start,
        Run,
        Pause,
        Resume,
        Reset,
        Dead,
        Destroy
    }

    /// <summary>
    /// Status represents a component's current status, and things that can be polled from it.
    /// </summary>
    public class Status : MonoBehaviour
    {

        #region // Data members.

        /// <summary>
        /// Represents the last status.
        /// </summary>
        private ComponentStatus m_previousStatus = ComponentStatus.NULL;

        /// <summary>
        /// Represents the current status.
        /// </summary>
        private ComponentStatus m_currentStatus = ComponentStatus.NULL;

        /// <summary>
        /// Represents the frames that have passed since the last change in status.
        /// </summary>
        private int m_framesSincePrevious = 0;

        /// <summary>
        /// Represents the time in seconds that have passed since the last change in status.
        /// </summary>
        private float m_timeSincePrevious = 0.0f;
        
        // Properties.

        /// <summary>
        /// The current status.
        /// </summary>
        public ComponentStatus Current
        {
            get { return this.m_currentStatus; }
        }

        /// <summary>
        /// The previous status.
        /// </summary>
        public ComponentStatus Previous
        {
            get { return this.m_previousStatus; }
        }

        /// <summary>
        /// Frames since the previous status ended.
        /// </summary>
        public int FramesSincePrevious
        {
            get { return this.m_framesSincePrevious; }
        }

        /// <summary>
        /// Seconds since the previous status ended.
        /// </summary>
        public float TimeSincePrevious
        {
            get { return this.m_timeSincePrevious; }
        }

        /// <summary>
        /// Returns true if the current status is null.
        /// </summary>
        public bool IsNull
        {
            get { return (this.m_currentStatus == ComponentStatus.NULL); }
        }

        #endregion

        #region // Service Methods.

        #region // // UnityEngine methods.

        /// <summary>
        /// Initializes the status values.
        /// </summary>
        public void Start()
        {
            this.m_previousStatus = ComponentStatus.NULL; // There was no previous status before starting.
            this.m_currentStatus = ComponentStatus.NULL; // This has just started; we don't know what the OTHER component, using this one, will set this value to!

            this.m_framesSincePrevious = 0;
            this.m_timeSincePrevious = 0.0f;
        }

        public void Update()
        {
            UpdateCounters();
        }

        #endregion

        #region // // Helper methods.

        /// <summary>
        /// Update the frame counter and timer for the current tick.
        /// </summary>
        private void UpdateCounters()
        {
            this.m_framesSincePrevious += 1;
            this.m_timeSincePrevious += Time.deltaTime;
        }

        /// <summary>
        /// Reset the frame counter and timer for the current tick.
        /// </summary>
        private void ResetCounters()
        {
            this.m_framesSincePrevious = 0;
            this.m_timeSincePrevious = 0.0f;
        }

        #endregion

        #region // // Status Checks.

        /// <summary>
        /// Returns true if frame counter is set to zero.
        /// </summary>
        /// <returns>Returns true whenever a status is changed.</returns>
        public bool StatusChanged()
        {
            return (this.m_previousStatus != this.m_currentStatus) && (this.m_framesSincePrevious == 0);
        }

        /// <summary>
        /// Returns true if in the Init state.
        /// </summary>
        /// <returns>Returns flag current status is init.</returns>
        public bool IsInitializing()
        {
            return (this.m_currentStatus == ComponentStatus.Init);
        }

        /// <summary>
        /// Returns true if current status is set to pause.
        /// </summary>
        /// <returns>Returns flag current status is pause.</returns>
        public bool IsPaused()
        {
            return (this.m_currentStatus == ComponentStatus.Pause);
        }
        
        /// <summary>
        /// Returns true if current status was just set to pause, this frame.
        /// </summary>
        /// <returns>Returns flag current status is pause.</returns>
        public bool JustPaused()
        {
            return (IsPaused() && StatusChanged());
        }

        /// <summary>
        /// Returns true if current status is set to running.
        /// </summary>
        /// <returns>Returns flag current status is running.</returns>
        public bool IsRunning()
        {
            return (this.m_currentStatus == ComponentStatus.Run);
        }

        /// <summary>
        /// Returns true if current status is set to start.
        /// </summary>
        /// <returns>Returns flag current status is start.</returns>
        public bool IsStarting()
        {
            return (this.m_currentStatus == ComponentStatus.Start);
        }

        /// <summary>
        /// Returns true if current status is set to reset.
        /// </summary>
        /// <returns>Returns flag current status is reset.</returns>
        public bool IsResetting()
        {
            return (this.m_currentStatus == ComponentStatus.Reset);
        }

        /// <summary>
        /// Returns true if the current status is set to resume.
        /// </summary>
        /// <returns>Returns flag current status is resume.</returns>
        public bool IsResuming()
        {
            return (this.m_currentStatus == ComponentStatus.Resume);
        }

        /// <summary>
        /// Returns true if current status was just set to resume, this frame.
        /// </summary>
        /// <returns>Returns flag current status is resume.</returns>
        public bool JustResumed()
        {
            return (IsResuming() && StatusChanged());
        }

        /// <summary>
        /// Returns false if the current status is set to dead.
        /// </summary>
        /// <returns>Returns opposite flag current status is dead.</returns>
        public bool IsAlive()
        {
            return !IsDead();
        }

        /// <summary>
        /// Returns true if the current status is set to dead.
        /// </summary>
        /// <returns>Returns flag current status is dead.</returns>
        public bool IsDead()
        {
            return (this.m_currentStatus == ComponentStatus.Dead);
        }

        /// <summary>
        /// Returns true if current status was just set to dead, this frame.
        /// </summary>
        /// <returns>Returns flag current status is dead.</returns>
        public bool JustKilled()
        {
            return (IsDead() && StatusChanged());
        }

        /// <summary>
        /// Returns true if the current status is set to destroy.
        /// </summary>
        /// <returns>Returns flag current status is destroy.</returns>
        public bool IsReleasing()
        {
            return (this.m_currentStatus == ComponentStatus.Destroy);
        }

        /// <summary>
        /// Returns true if current status was just set to destroy, this frame.
        /// </summary>
        /// <returns>Returns flag current status is destroy.</returns>
        public bool JustReleasing()
        {
            return (IsReleasing() && StatusChanged());
        }


        #endregion

        #endregion

        #region // Mutator Methods.

        /// <summary>
        /// Calls the initialization state.
        /// </summary>
        public void Initialize()
        {
            ChangeStatus(ComponentStatus.Init);
        }

        /// <summary>
        /// If not currently paused, set the state to pause.
        /// </summary>
        public void Pause()
        {
            ChangeStatus(ComponentStatus.Pause);
        }

        /// <summary>
        /// Reset the component's status.
        /// </summary>
        public void TriggerReset()
        {
            ChangeStatus(ComponentStatus.NULL); // Get rid of previous status record.
            ChangeStatus(ComponentStatus.Reset); // Set to the reset state.
        }

        /// <summary>
        /// If currently paused, set state to resume, and, get ready to run.
        /// </summary>
        public void Resume()
        {
            if (this.m_currentStatus == ComponentStatus.Pause)
            {
                this.m_currentStatus = ComponentStatus.Resume;
            }
            else if (this.m_currentStatus == ComponentStatus.Resume)
            {
                if (this.m_previousStatus != ComponentStatus.Pause)
                {
                    ChangeStatus(this.m_previousStatus);
                } else
                {
                    Run();
                }
            }
        }

        /// <summary>
        /// Set the status to destroy.
        /// </summary>
        public void Release()
        {
            ChangeStatus(ComponentStatus.Destroy);
        }

        /// <summary>
        /// Set the status to dead.
        /// </summary>
        public void Kill()
        {
            ChangeStatus(ComponentStatus.Dead);
        }

        /// <summary>
        /// Set the status to run.
        /// </summary>
        public void Run()
        {
            ChangeStatus(ComponentStatus.Run);
        }

        /// <summary>
        /// Change status to input, if different.
        /// </summary>
        /// <param name="_status">Status to change to.</param>
        private void ChangeStatus(ComponentStatus _status)
        {
            // Only change if current status is different from input status.
            if (_status != this.m_currentStatus)
            {
                this.m_previousStatus = this.m_currentStatus; // Keep track of the last status.
                this.m_currentStatus = _status; // Set the current status.
                ResetCounters(); // Reset the counters.
            }
        }

        #endregion

    }
}
