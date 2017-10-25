/************************************************
 * StatTracker.cs
 * 
 * This file contains:
 * - The StatTracker struct.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arcana.Utilities
{

    #region Struct: StatTracker structure.

    /// <summary>
    /// Keeps track of a current value, between a minimum and maximum, while also providing service functions.
    /// </summary>
    public struct StatTracker
    {

        #region Data Members.

        #region Fields.

        /////////////////////
        // Fields.
        /////////////////////

        /// <summary>
        /// Name of the status.
        /// </summary>
        private string m_stat;

        /// <summary>
        /// Maximum value of the stat.
        /// </summary>
        private float m_maximum;

        /// <summary>
        /// Minimum value of the stat.
        /// </summary>
        private float m_minimum;

        /// <summary>
        /// Current value of the stat.
        /// </summary>
        private float m_value;

        /// <summary>
        /// Maximum value that a stat can change by at a given time. (-1 indicates there is no limit).
        /// </summary>
        private float m_maxChange;

        /// <summary>
        /// Minimum value that a stat can change by at a given time. (-1 indicates there is no limit).
        /// </summary>
        private float m_minChange;

        #endregion

        #region Properties.

        /// <summary>
        /// Returns the name of the tracker.
        /// </summary>
        public string Name
        {
            get { return this.m_stat; }
        }

        /// <summary>
        /// Returns the minimum.
        /// </summary>
        public float Min
        {
            get { return this.m_minimum; }
            set { SetMinimum(value); }
        }

        /// <summary>
        /// Returns the maximum.
        /// </summary>
        public float Max
        {
            get { return this.m_maximum; }
            set { SetMaximum(value); }
        }

        /// <summary>
        /// Value of the StatTracker.
        /// </summary>
        public float Value
        {
            get { return this.m_value; }
            set { SetValue(this.m_value); }
        }

        /// <summary>
        /// Returns calculated percentage value, on a [0, 1] range (inclusive).
        /// </summary>
        public float Percent
        {
            get { return this.m_value.MapValue(this.m_minimum, this.m_maximum, 0.0f, 1.0f); }
            set { SetValueByPercentage(value); }
        }

        #endregion

        #endregion

        #region Struct Constructor 

        /// <summary>
        /// Creates a StatTracker.
        /// </summary>
        /// <param name="_stat">Name of the stat being tracked.</param>
        /// <param name="_value">Current value of the stat.</param>
        /// <param name="_min">Minimum value of the stat.</param>
        /// <param name="_max">Maximum value of the stat.</param>
        /// <param name="_minInc">Minimum increment value of the stat.</param>
        /// <param name="_maxInc">Maximum increment value of the stat.</param>
        public StatTracker(string _stat = "Untitled Stat", float _value = 100.0f, float _min = 0.0f, float _max = 100.0f, float _minInc = -1.0f, float _maxInc = -1.0f)
        {
            // Set name.
            if (_stat.Trim().Length > 0)
            {
                this.m_stat = _stat;
            }
            else
            {
                this.m_stat = "Untitled Stat";
            }           
            
            // Set and verify the other members.
            this.m_minimum = Services.Min(_min, _max);
            this.m_maximum = Services.Max(_min, _max);

            // Set the delta limiters.
            this.m_minChange = Services.Min(_minInc, _maxInc);
            this.m_maxChange = Services.Max(_minInc, _maxInc);

            // Set the value.
            this.m_value = _value;

            // Validate the values.
            SetName(this.m_stat);
            SetValue(this.m_value);
        }

        /// <summary>
        /// Copy values from one stat tracker over to this one.
        /// </summary>
        /// <param name="stats">Tracker to copy stats from.</param>
        /// <returns>Returns this.</returns>
        public StatTracker Copy(StatTracker stats)
        {
            this.m_stat = stats.m_stat;
            this.m_minimum = stats.m_minimum;
            this.m_maximum = stats.m_maximum;
            this.m_maxChange = stats.m_maxChange;
            this.m_minChange = stats.m_minChange;
            this.m_value = stats.m_value;

            return this;
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Returns true if the range lengths are the same for both trackers.
        /// </summary>
        /// <param name="other">StatTracker to compare.</param>
        /// <returns>Returns true if range lengths are the same.</returns>
        public bool IsSimilar(StatTracker other)
        {
            return ((this % other) == 0);
        }

        /// <summary>
        /// If a negative value, there is no minimum delta input limit.
        /// </summary>
        /// <returns>Returns true if minimum increment value is less than zero.</returns>
        public bool HasMinimumIncrement()
        {
            return (this.m_minChange < 0.0f);
        }

        /// <summary>
        /// If a negative value, there is no maximum delta input limit.
        /// </summary>
        /// <returns>Returns true if maximum increment value is less than zero.</returns>
        public bool HasMaximumIncrement()
        {
            return (this.m_maxChange < 0.0f);
        }
        
        /// <summary>
        /// At maximum stat value.
        /// </summary>
        /// <returns>Return true if value equal to maximum.</returns>
        public bool IsMaximum()
        {
            return (this.m_value == this.m_maximum);
        }

        /// <summary>
        /// At minimum stat value.
        /// </summary>
        /// <returns>Return true if value equal to minimum.</returns>
        public bool IsMinimum()
        {
            return (this.m_value == this.m_minimum);
        }

        /// <summary>
        /// Returns true if value less than maximum.
        /// </summary>
        /// <returns>Returns true if value is less than (and not equal to) the maximum.</returns>
        public bool LessThanMax() {
            return (this.m_value < this.m_maximum);
        }

        /// <summary>
        /// Returns true if value greater than minimum.
        /// </summary>
        /// <returns>Returns true if value is greater than (and not equal to) the minimum.</returns>
        public bool GreaterThanMin()
        {
            return (this.m_value > this.m_minimum);
        }

        /// <summary>
        /// Returns true if the value is between the minimum and maximum, and, is not equal to either.
        /// </summary>
        /// <returns></returns>
        public bool IsBetween()
        {
            return (!IsMinimum() && !IsMaximum());
        }

        /// <summary>
        /// Returns the difference between the minimum and maximum cap values.
        /// </summary>
        /// <returns>Returns a float.</returns>
        public float GetRangeLength()
        {
            return (this.m_maximum - this.m_minimum);
        }

        /// <summary>
        /// Return this StatTracker's value.
        /// </summary>
        /// <returns>Returns float value.</returns>
        public float GetValue()
        {
            return this.Value;
        }

        /// <summary>
        /// Returns the percentage value, calculated from current value and range, as value between [0, 1] (inclusive).
        /// </summary>
        /// <returns>Returns float on inclusive range from zero to one.</returns>
        public float GetPercent()
        {
            return this.Percent;
        }

        /// <summary>
        /// Return the current value, but, mapped to a different range. Useful for representing on UI bars (stat value to pixels!).
        /// </summary>
        /// <param name="_start">Start of new range.</param>
        /// <param name="_end">End of new range.</param>
        /// <returns>Returns float that is proportional to this stat tracker's internal value.</returns>
        public float GetValueOnRange(float _start, float _end)
        {
            float value = this.m_value;
            return value.MapValue(this.m_minimum, this.m_maximum, _start, _end);
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Set the maximum value of the tracker.
        /// </summary>
        /// <param name="_max">Maximum value.</param>
        public void SetMaximum(float _max)
        {
            this.m_maximum = Services.Max(this.m_minimum, _max);
        }

        /// <summary>
        /// Set the minimum value of the tracker.
        /// </summary>
        /// <param name="_min">Minimum value</param>
        public void SetMinimum(float _min)
        {
            this.m_minimum = Services.Min(_min, this.m_maximum);
        }

        /// <summary>
        /// Set the stat name.
        /// </summary>
        /// <param name="_stat">Name to attempt setting stat to.</param>
        public void SetName(string _stat)
        {
            if (_stat.Trim().Length > 0)
            {
                this.m_stat = _stat;
            }
            else
            {
                this.m_stat = "Untitled Stat";
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="_value">Value to set.</param>
        public void SetValue(float _value)
        {
            // Make sure input is above the minimum...
            this.m_value = Services.Max(this.m_minimum, _value);

            // ...and below the maximum.
            this.m_value = Services.Min(this.m_maximum, _value);
        }

        /// <summary>
        /// Set the stat tracker to a value based on the percentage. Percentage is on a [0, 1] range.
        /// </summary>
        /// <param name="_percent">Value between 0 and 1.</param>
        public void SetValueByPercentage(float _percent)
        {
            // Clamp percent value.
            float percent = Services.Clamp(_percent, 0.0f, 1.0f);

            // Map the percentage (which is a range value) to the minimum and maximum range of the Stat Tracker.
            SetValue(percent.MapValue(0.0f, 1.0f, this.m_minimum, this.m_maximum));
        }
        
        /// <summary>
        /// Clamp the minimum limit.
        /// </summary>
        /// <param name="_limit">Limit to assign.</param>
        public void LimitMinimumChange(float _limit)
        {
            // Clamp the limit value.
            float limit = Services.Abs(_limit);

            // Set limit.
            this.m_maxChange = limit;
        }
        
        /// <summary>
        /// Clamp the maximum limit.
        /// </summary>
        /// <param name="_limit">Limit to assign.</param>
        public void LimitMaximumChange(float _limit)
        {
            // Clamp the limit value.
            float limit = Services.Abs(_limit);

            // Set limit.
            this.m_maxChange = limit;
        }

        /// <summary>
        /// Reset the stat tracker values to maximum.
        /// </summary>
        public void Reset()
        {
            SetValue(this.m_maximum);
        }

        /// <summary>
        /// Set the stat tracker values to minimum.
        /// </summary>
        public void End()
        {
            SetValue(this.m_minimum);
        }

        /// <summary>
        /// Amount to increase the value by.
        /// </summary>
        /// <param name="_delta">Change in delta.</param>
        public void Increment(float _delta)
        {
            float delta = Services.Abs(_delta);

            // If there is a limit on the maximum amount of change possible.
            if (HasMaximumIncrement())
            {
                // Clamp value.
                delta = Services.Min(_delta, this.m_maxChange);
            }

            // If there is a limit on the minimum amount of change possible.
            if (HasMinimumIncrement())
            {
                // Clamp value.
                delta = Services.Max(_delta, this.m_minChange);
            }

            // Delta has been clamped and can be used on the health value.
            SetValue(this.m_value + delta);

        }

        /// <summary>
        /// Increment by a percentage value.
        /// </summary>
        public void IncrementByPercent(float _percent)
        {
            // Clamp percentage amount.
            float percent = Services.Clamp(_percent, 0.0f, 1.0f);

            // Get the value that would be incremented by.
            float delta = percent.MapValue(0.0f, 1.0f, this.m_minimum, this.m_maximum);

            // Increment by that amount.
            Increment(delta);
        }

        /// <summary>
        /// Amount to decrease the value by.
        /// </summary>
        /// <param name="_delta">Change in delta.</param>
        public void Decrement(float _delta)
        {
            float delta = Services.Abs(_delta);

            // If there is a limit on the maximum amount of change possible.
            if (HasMaximumIncrement())
            {
                // Clamp value.
                delta = Services.Min(_delta, this.m_maxChange);
            }

            // If there is a limit on the minimum amount of change possible.
            if (HasMinimumIncrement())
            {
                // Clamp value.
                delta = Services.Max(_delta, this.m_minChange);
            }

            // Delta has been clamped and can be used on the health value.
            SetValue(this.m_value - delta);

        }

        /// <summary>
        /// Decrement by a percentage value.
        /// </summary>
        public void DecrementByPercent(float _percent)
        {
            // Clamp percentage amount.
            float percent = Services.Clamp(_percent, 0.0f, 1.0f);

            // Get the value that would be incremented by.
            float delta = percent.MapValue(0.0f, 1.0f, this.m_minimum, this.m_maximum);

            // Increment by that amount.
            Decrement(delta);
        }

        #endregion

        #region Service Methods.

        /// <summary>
        /// Check if equal to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>Returns true if equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj.GetType() == typeof(int)
                || obj.GetType() == typeof(float)
                || obj.GetType() == typeof(long)
                || obj.GetType() == typeof(double)
                || obj.GetType() == typeof(short))
            {
                return this == (float)obj;
            }
            else if (obj.GetType() == typeof(StatTracker))
            {
                return this == (StatTracker)obj;
            }

            // Default case, not equal.
            return false;
        }

        /// <summary>
        /// Returns hash code for the tracker.
        /// </summary>
        /// <returns>Returns a hash code for this struct.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        
        /// <summary>
        /// Return the StatTracker as a string.
        /// </summary>
        public override string ToString()
        {
            return (this.m_stat + ": (" + this.m_value + " / " + this.m_maximum + ") (Minimum: " + this.m_minimum + ", Percentage: " + (this.Percent * 100.0f).ToString() + "%)");
        }

        /// <summary>
        /// Returns the size difference between the range lengths for both trackers. Zero, means the difference between the minimum and maximum values the trackers can have, are the same.
        /// </summary>
        /// <param name="a">Lefthand term.</param>
        /// <param name="b">Righthand term.</param>
        /// <returns>Returns float value indicating deviation between ranges.</returns>
        public static float operator% (StatTracker a, StatTracker b)
        {
            // Get the two lengths.
            float left = a.GetRangeLength();
            float right = b.GetRangeLength();

            // If the difference is zero, then the range lengths can be assumed to be the same, at least when projecting percentage changes between two trackers.
            // If the right hand side is greater, the result will be negative.
            return left - right;
        }

        /// <summary>
        /// Checks if the values are the same between trackers, percentage-wise.
        /// </summary>
        /// <param name="a">Lefthand term.</param>
        /// <param name="b">Righthand term.</param>
        /// <returns>Returns true if percents are the same.</returns>
        public static bool operator== (StatTracker a, StatTracker b)
        {
            return a.Percent == b.Percent;
        }
        
        /// <summary>
        /// Checks if the values are different between trackers, percentage-wise.
        /// </summary>
        /// <param name="a">Lefthand term.</param>
        /// <param name="b">Righthand term.</param>
        /// <returns>Returns true if percents are different.</returns>
        public static bool operator!= (StatTracker a, StatTracker b)
        {
            return a.Percent != b.Percent;
        }

        /// <summary>
        /// Checks if the scalar value directly matches the value of the StatTracker.
        /// </summary>
        /// <param name="a">Lefthand term.</param>
        /// <param name="b">Righthand, scalar term.</param>
        /// <returns>Returns true if values match directly.</returns>
        public static bool operator== (StatTracker a, float b)
        {
            return a.Value == b;
        }
        
        /// <summary>
        /// Checks if the scalar value doesn't matches the value of the StatTracker.
        /// </summary>
        /// <param name="a">Lefthand term.</param>
        /// <param name="b">Righthand, scalar term.</param>
        /// <returns>Returns true if values don't match directly.</returns>
        public static bool operator!= (StatTracker a, float b)
        {
            return a.Value != b;
        }

        /// <summary>
        /// Compares percentage values.
        /// </summary>
        /// <param name="a">Lefthand term.</param>
        /// <param name="b">Righthand term.</param>
        /// <returns>Returns true if comparison is appropriate.</returns>
        public static bool operator> (StatTracker a, StatTracker b)
        {
            return a.Percent > b.Percent;
        }
        
        /// <summary>
        /// Compares percentage values.
        /// </summary>
        /// <param name="a">Lefthand term.</param>
        /// <param name="b">Righthand term.</param>
        /// <returns>Returns true if comparison is appropriate.</returns>
        public static bool operator< (StatTracker a, StatTracker b)
        {
            return a.Percent < b.Percent;
        }

        /// <summary>
        /// Compares percentage values.
        /// </summary>
        /// <param name="a">Lefthand term.</param>
        /// <param name="b">Righthand term.</param>
        /// <returns>Returns true if comparison is appropriate.</returns>
        public static bool operator>= (StatTracker a, StatTracker b)
        {
            return a.Percent > b.Percent;
        }

        /// <summary>
        /// Compares percentage values.
        /// </summary>
        /// <param name="a">Lefthand term.</param>
        /// <param name="b">Righthand term.</param>
        /// <returns>Returns true if comparison is appropriate.</returns>
        public static bool operator<= (StatTracker a, StatTracker b)
        {
            return a.Percent < b.Percent;
        }

        /// <summary>
        /// If multiplied by a value on the [0, 1] (inclusive) range, it will modulate the value by the input percentage.
        /// </summary>
        /// <param name="a">StatTracker to modify.</param>
        /// <param name="value">Percentage to affect stat tracker by.</param>
        /// <returns>Returns StatTracker with modifications.</returns>
        public static StatTracker operator* (StatTracker a, float value)
        {
            if (Services.InRange(value, 0.0f, 1.0f))
            {
                if (value == 0.0)
                {
                    // Bring to minimum.
                    a.End();
                    return a;
                }

                if (value == 1.0f)
                {
                    // Reset the value.
                    a.Reset();
                    return a;
                }

                // If between zero and one, treat value like its a percentage value.
                a.SetValueByPercentage(value);
                return a;
            }

            // In every other case, do nothing.
            return a;
        }


        /// <summary>
        /// If multiplied by a value on the [0, 1] (inclusive) range, it will modulate the value by the difference between the input percentage and 100%.
        /// </summary>
        /// <param name="a">StatTracker to modify.</param>
        /// <param name="value">Percentage to affect stat tracker by.</param>
        /// <returns>Returns StatTracker with modifications.</returns>
        public static StatTracker operator/ (StatTracker a, float value)
        {
            if (Services.InRange(value, 0.0f, 1.0f))
            {
                if (value == 0.0)
                {
                    // Reset the value.
                    a.Reset();
                    return a;
                }

                if (value == 1.0f)
                {
                    // Bring to minimum.
                    a.End();
                    return a;
                }

                // If between zero and one, treat value like its a percentage value.
                a.SetValueByPercentage(1.0f - value);
                return a;
            }

            // In every other case, do nothing.
            return a;
        }

        /// <summary>
        /// Increments the StatTracker by a strictly scalar value.
        /// </summary>
        /// <param name="a">StatTracker to be incremented.</param>
        /// <param name="value">Value to increment by</param>
        /// <returns>Returns StatTracker being incremented. </returns>
        public static StatTracker operator+ (StatTracker a, float value)
        {
            // If value is less than zero, do the opposite operation.
            if (value < 0.0f)
            {
                a.Decrement(Services.Abs(value));
                return a;
            }

            // Increment a.
            a.Increment(value);
            return a;
        }

        /// <summary>
        /// Adds two stat trackers together by percentage, keeping the range of the tracker on the lefthand side.
        /// </summary>
        /// <param name="a">Lefthand side statement.</param>
        /// <param name="b">Righthand side statement.</param>
        /// <returns>Returns StatTracker value, keeping the range values from the term on the left hand side.</returns>
        public static StatTracker operator+ (StatTracker a, StatTracker b)
        {
            // Return a after setting its value to the sum of the percents.
            a.SetValueByPercentage(a.Percent + b.Percent);
            return a;
        }
        
        /// <summary>
        /// Increments the StatTracker by 1 percentage point.
        /// </summary>
        /// <param name="a">StatTracker to increment.</param>
        /// <returns>Returns same StatTracker but incremented by one percentage point.</returns>
        public static StatTracker operator++ (StatTracker a)
        {
            // Map 1 out of 100% to the [0, 1] range.
            a.IncrementByPercent((1f).MapValue(0.0f, 100.0f, 0.0f, 1.0f));
            return a;
        }

        /// <summary>
        /// Decrements the StatTracker by a strictly scalar value.
        /// </summary>
        /// <param name="a">StatTracker to be decremented.</param>
        /// <param name="value">Value to decrement by</param>
        /// <returns>Returns StatTracker being decremented. </returns>
        public static StatTracker operator- (StatTracker a, float value)
        {
            // If value is less than zero, do the opposite operation.
            if (value < 0.0f)
            {
                a.Increment(Services.Abs(value));
                return a;
            }

            // Decrement a.
            a.Decrement(value);
            return a;
        }

        /// <summary>
        /// Subtracts two stat trackers from each other by percentage, keeping the range of the tracker on the lefthand side.
        /// </summary>
        /// <param name="a">Lefthand side statement.</param>
        /// <param name="b">Righthand side statement.</param>
        /// <returns>Returns StatTracker value, keeping the range values from the term on the left hand side.</returns>
        public static StatTracker operator- (StatTracker a, StatTracker b)
        {
            // Return a after setting its value to the difference of the percents.
            a.SetValueByPercentage(a.Percent - b.Percent);
            return a;
        }

        /// <summary>
        /// Decrements the StatTracker by 1 percentage point.
        /// </summary>
        /// <param name="a">StatTracker to decrement.</param>
        /// <returns>Returns same StatTracker but decremented by one percentage point.</returns>
        public static StatTracker operator-- (StatTracker a)
        {
            // Map 1 out of 100% to the [0, 1] range.
            a.DecrementByPercent((1f).MapValue(0.0f, 100.0f, 0.0f, 1.0f));
            return a;
        }

        #endregion

    }

    #endregion

}
