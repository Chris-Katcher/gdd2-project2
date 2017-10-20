using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arcana
{
    /// <summary>
    /// The Constraints class allows rules to be passed into Component factories.
    /// </summary>
    public class Constraints
    {

        #region Static Members.

        // Static Fields.

        /// <summary>
        /// Collection of keys that can exist across all constraints.
        /// </summary>
        private static List<string> s_keys = null;


        // Static Properties.

        /// <summary>
        /// Return the collection of keys, publicly.
        /// </summary>
        public static List<string> Keys
        {
            get { return s_keys; }
        }

        /// <summary>
        /// Return the number of keys held within the collection.
        /// </summary>
        public static int Size
        {
            get { return Constraints.Keys.Count; }
        }

        /// <summary>
        /// Determine if static member has been initialized.
        /// </summary>
        public static bool Initialized
        {
            get { return (Constraints.Keys != null); }
        }

        /// <summary>
        /// Return true, if the list is null or empty.
        /// </summary>
        public static bool NoKeys
        {
            get { return (Constraints.Initialized || Constraints.Size == 0); }
        }

        #endregion

        #region  Static Methods.

        /// <summary>
        /// Initialize the static members.
        /// </summary>
        public static void Initialize()
        {
            Constraints.s_keys = new List<string>();
        }

        /// <summary>
        /// Check if the key is contained within the static collection member.
        /// </summary>
        /// <param name="key">Key to check for.</param>
        /// <returns>Return true if it exists.</returns>
        public static bool Contains(string key)
        {
            return (Constraints.Keys.Contains(key));
        }

        /// <summary>
        /// Add key to the static collection.
        /// </summary>
        /// <param name="key">Key to add.</param>
        /// <param name="uid">Unique index value associated with key. Optional, to allow insertion.</param>
        public static void AddKey(string key, int uid = -1)
        {
            if (!Constraints.Contains(key))
            {
                if (uid < 0)
                {
                    Constraints.Keys.Add(key);
                }
                else
                {
                    int index = Services.Clamp<int>(uid, 0, Constraints.Size - 1);
                    Constraints.Keys.Insert(index, key);
                }
            }
        }

        #endregion

        #region Data Members
        
        /// <summary>
        /// Collection of constraints and their titles in this constraint object.
        /// </summary>
        private Dictionary<string, Constraint> m_constraints;

        // Properties.

        /// <summary>
        /// All the rule keys.
        /// </summary>
        public List<string> RuleIDs
        {
            get { return Keys; }
        }

        /// <summary>
        /// Map of rule ID's to their settings.
        /// </summary>
        public Dictionary<string, Constraint> Rules
        {
            get { return this.m_constraints; }
        }

        /// <summary>
        /// Determine if there are any entries in the object's collection.
        /// </summary>
        public int Count
        {
            get { return this.m_constraints.Count; }
        }

        /// <summary>
        /// Determine if this is empty.
        /// </summary>
        public bool IsEmpty 
        {
            get { return (this.m_constraints == null || this.Count == 0); }
        }

        #endregion

        #region Constructor.

        /// <summary>
        /// Create collection to hold onto the constraints.
        /// </summary>
        public Constraints()
        {
            if(!Constraints.Initialized)
            {
                Constraints.Initialize();
            }

            this.m_constraints = new Dictionary<string, Constraint>();
        }

        #endregion

        #region Service Methods.

        /// <summary>
        /// Check if there is a rule in the <see cref="s_keys"/> collection.
        /// </summary>
        /// <param name="key">Key to check for.</param>
        /// <returns>Return true if it exists.</returns>
        public bool HasKey(string key)
        {
            return Constraints.Contains(key);
        }

        /// <summary>
        /// Checks to see if there is a non-empty value associated with the key.
        /// </summary>
        /// <param name="key">Key to check entry for.</param>
        /// <returns>Returns true if it exists.</returns>
        public bool HasEntry(string key)
        {
            return (this.HasKey(key) && this.m_constraints.ContainsKey(key));
        }

        /// <summary>
        /// Check if there is a value, associated with a particular entry.
        /// </summary>
        /// <param name="key">Rule to check entry value for.</param>
        /// <returns>Returns true if it exists and is not empty.</returns>
        public bool HasValue(string key)
        {
            return (this.HasEntry(key) && this.m_constraints[key].HasValue());
        }

        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Attempt to get the key from a particular entry.
        /// </summary>
        /// <param name="uid">Unique index associated with a rule.</param>
        /// <param name="key">The key value that is returned by this method.</param>
        /// <returns>Returns true if a valid key is found.</returns>
        public bool TryGetKey(int uid, out string key)
        {
            key = GetKey(uid);
            return (key != "");
        }

        /// <summary>
        /// Get the key associated with its particular UID.
        /// </summary>
        /// <param name="uid">Unique index associated with a rule.</param>
        /// <returns>Returns the key as a string.</returns>
        public string GetKey(int uid)
        {
            string key = "";

            // Check size.
            if (Services.InRange<int>(uid, 0, this.Count - 1))
            {
                key = Constraints.s_keys[uid];
            }

            return key;
        }

        /// <summary>
        /// Return true if the entry has value and isn't null.
        /// </summary>
        /// <param name="uid">Unique index associated with a rule.</param>
        /// <param name="entry">Constraint containing entry's value.</param>
        /// <returns>Returns the entry as a Constraint.</returns>
        public bool TryGetEntry(int uid, out Constraint entry)
        {
            entry = GetEntry(uid);
            return (entry != null && entry.HasValue() && !entry.Equals(Constraint.Empty));
        }

        /// <summary>
        /// Return true if the entry has value and isn't null.
        /// </summary>
        /// <param name="key">Key associated with an entry.</param>
        /// <param name="entry">Constraint containing entry's value.</param>
        /// <returns>Returns the entry as a Constraint.</returns>
        public bool TryGetEntry(string key, out Constraint entry)
        {
            entry = GetEntry(key);
            return (entry != null && entry.HasValue() && !entry.Equals(Constraint.Empty));
        }

        /// <summary>
        /// Get the key entry associated with a particular UID.
        /// </summary>
        /// <param name="uid">Unique index associated with a rule.</param>
        /// <returns>Returns entry as a Constraint.</returns>
        public Constraint GetEntry(int uid)
        {
            // Create an empty constraint.
            Constraint entry = Constraint.Empty;

            string key = "";
            if (TryGetKey(uid, out key))
            {
                return GetEntry(key);
            }

            return entry;
        }

        /// <summary>
        /// Get the entry associated with a key.
        /// </summary>
        /// <param name="key">Key associated with an entry.</param>
        /// <returns>Returns entry as a Constraint.</returns>
        public Constraint GetEntry(string key) {

            // Create an empty constraint.
            Constraint entry = Constraint.Empty;
            
            if (HasEntry(key))
            {
                entry = this.m_constraints[key];
            }

            return entry;
        }

        /// <summary>
        /// Returns true if a value is successfully returned.
        /// </summary>
        /// <typeparam name="T">Generic type to grab value as.</typeparam>
        /// <param name="uid">Unique index associated with a rule.</param>
        /// <param name="value">Value to return.</param>
        /// <returns>Returns true if operation is successful.</returns>
        public bool TryGetValue<T>(int uid, out T value)
        {
            value = default(T);
            return (GetValue<T>(uid).Equals(default(T)));
        }

        /// <summary>
        /// Returns true if a value is successfully returned.
        /// </summary>
        /// <typeparam name="T">Generic type to grab value as.</typeparam>
        /// <param name="key">Key associated with an entry.</param>
        /// <param name="value">Value to return.</param>
        /// <returns>Returns true if operation is successful.</returns>
        public bool TryGetValue<T>(string key, out T value)
        {
            value = default(T);
            return (GetValue<T>(key).Equals(default(T)));
        }

        /// <summary>
        /// Return a cast value from the selected entry, if it exists.
        /// </summary>
        /// <typeparam name="T">Cast value as this type.</typeparam>
        /// <param name="uid">Unique index associated with a rule.</param>
        /// <returns>Returns stored value.</returns>
        public T GetValue<T>(int uid)
        {
            string key = "";
            TryGetKey(uid, out key);
            return GetValue<T>(key);
        }

        /// <summary>
        /// Return a cast value from the selected entry, if it exists.
        /// </summary>
        /// <typeparam name="T">Cast value as this type.</typeparam>
        /// <param name="key">Key associated with an entry.</param>
        /// <returns>Returns stored value.</returns>
        public T GetValue<T>(string key)
        {
            T value = default(T);

            if (HasValue(key))
            {
                this.m_constraints[key].TryGetValue<T>(out value);
            }

            return value;
        }

        #endregion

        #region Mutator Methods

        /// <summary>
        /// Add a new key to the static collection.
        /// </summary>
        /// <param name="key">Key to add.</param>
        /// <param name="uid">Index of added key.</param>
        private void AddNewKey(string key, int uid = -1)
        {
            Constraints.AddKey(key, uid);
        }

        /// <summary>
        /// Add an entry to the collection.
        /// </summary>
        /// <param name="key">Key that will be assigned to the entry.</param>
        /// <param name="entry">Entry with value to add.</param>
        /// <param name="uid">Index of added key.</param>
        private void AddEntry(string key, Constraint entry = null, int uid = -1)
        {
            // Check if the key exists.
            if (!HasKey(key))
            {
                Constraints.AddKey(key, uid);
            }

            if (!HasEntry(key))
            {   // If entry isn't null, add it to the collection.
                if (entry != null)
                {
                    this.Rules.Add(key, entry);
                }
                else
                {
                    this.Rules.Add(key, Constraint.Empty); // Add an empty constraint.
                }
            }
            else
            {   // If the key exists, overwrite.
                // If entry isn't null, add it to the collection.
                if (entry != null)
                {
                    this.Rules[GetKey(uid)] = entry;
                }
                else
                {
                    this.Rules[GetKey(uid)] = Constraint.Empty; // Add an empty constraint.
                }
            }
        }
    
        /// <summary>
        /// Give an entry a value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void AddValue<T>(string key, T value)
        {
            // Assign the value to the entry.
            Constraint entry = Constraint.CreateConstraint(value);

            // If the entry already exists, overwrite the value.
            if (HasEntry(key))
            {
                this.Rules[key] = entry;
                return;
            }

            // Else, add a new rule and entry.
            if (!HasKey(key))
            {
                AddNewKey(key);
            }

            if (!HasEntry(key))
            {
                AddEntry(key, entry);
            }
        }

        #endregion

    }
    
    /// <summary>
    /// Represents a constraint object. Used to pass a setting into component factories.
    /// </summary>
    public class Constraint
    {

        #region Static Members.

        /// <summary>
        /// Instance of the empty, defualt constraint.
        /// </summary>
        private static Constraint s_empty = null;

        /// <summary>
        /// Get the empty constraint instance, or create it if it doesn't already exist.
        /// </summary>
        public static Constraint Empty {
            get
            {
                if (s_empty == null)
                {
                    s_empty = CreateConstraint(null);
                }

                return s_empty;
            }
        }

        #endregion

        #region Static Constructors.

        /// <summary>
        /// Construct a new constraint.
        /// </summary>
        /// <param name="input">Input value.</param>
        /// <returns>Return the created Constraint object.</returns>
        public static Constraint CreateConstraint(object input)
        {
            return new Constraint(input);
        }

        /// <summary>
        /// Construct a new constraint with the specified type.
        /// </summary>
        /// <param name="_input">Input value.</param>
        /// <param name="_type">Type of input value.</param>
        /// <returns>Return the created Constraint object.</returns>
        public static Constraint CreateConstraint(object _input, Type _type)
        {
            return new Constraint(_input, _type);
        }

        #endregion

        #region Static Methods.

        /// <summary>
        /// Attempt to parse out a value from the Constraint, given the Constraint's rule.
        /// </summary>
        /// <typeparam name="T">Generic type to parse out.</typeparam>
        /// <param name="input">Input constraint.</param>
        /// <param name="output">Output value.</param>
        /// <returns>Return cast value.</returns>
        public static bool TryGetValue<T>(Constraint input, out T output)
        {
            // Default value.
            output = default(T);

            // Parse out the value.
            if (input != null && input.HasValue())
            {
                input.TryGetValue<T>(out output);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Try (and catch exceptions) when parsing object as a string.
        /// </summary>
        /// <param name="input">Input object.</param>
        /// <param name="output">Output value.</param>
        /// <returns>Returns cast output.</returns>
        public static bool TryParseString(object input, out string output)
        {
            output = "";

            try
            {
                output = (string)input;
                return true;
            }
            catch (Exception e)
            {
                Debugger.Print("The object cannot be cast this way. " + e.StackTrace, "Cast Failure.");
                return false;
            }
        }

        /// <summary>
        /// Try (and catch exceptions) when parsing object as an integer.
        /// </summary>
        /// <param name="input">Input object.</param>
        /// <param name="output">Output value.</param>
        /// <returns>Returns cast output.</returns>
        public static bool TryParseInteger(object input, out int output)
        {
            output = 0;

            try
            {
                output = (int)input;
                return true;
            }
            catch (Exception e)
            {
                Debugger.Print("The object cannot be cast this way. " + e.StackTrace, "Cast Failure.");
                return false;
            }
        }

        /// <summary>
        /// Try (and catch exceptions) when parsing object as a double.
        /// </summary>
        /// <param name="input">Input object.</param>
        /// <param name="output">Output value.</param>
        /// <returns>Returns cast output.</returns>
        public static bool TryParseDouble(object input, out double output)
        {
            output = 0;

            try
            {
                output = (double)input;
                return true;
            }
            catch (Exception e)
            {
                Debugger.Print("The object cannot be cast this way. " + e.StackTrace, "Cast Failure.");
                return false;
            }
        }

        /// <summary>
        /// Try (and catch exceptions) when parsing object as a float.
        /// </summary>
        /// <param name="input">Input object.</param>
        /// <param name="output">Output value.</param>
        /// <returns>Returns cast output.</returns>
        public static bool TryParseFloat(object input, out float output)
        {
            output = 0;

            try
            {
                output = (float)input;
                return true;
            }
            catch (Exception e)
            {
                Debugger.Print("The object cannot be cast this way. " + e.StackTrace, "Cast Failure.");
                return false;
            }
        }

        /// <summary>
        /// Try (and catch exceptions) when parsing object as a boolean.
        /// </summary>
        /// <param name="input">Input object.</param>
        /// <param name="output">Output value.</param>
        /// <returns>Returns cast output.</returns>
        public static bool TryParseFloat(object input, out bool output)
        {
            output = false;

            try
            {
                output = (bool)input;
                return true;
            }
            catch (Exception e)
            {
                Debugger.Print("The object cannot be cast this way. " + e.StackTrace, "Cast Failure.");
                return false;
            }
        }

        /// <summary>
        /// Returns an input, cast back as the generic type if casting is possible.
        /// </summary>
        /// <typeparam name="T">Generic, castable type.</typeparam>
        /// <param name="input">Input object.</param>
        /// <param name="output">Output value.</param>
        /// <returns>Return the cast value.</returns>
        public static bool TryParse<T>(object input, out T output)
        {
            output = default(T);

            try
            {
                output = (T)input;
                return true;
            }
            catch (Exception e)
            {
                Debugger.Print("The object cannot be cast this way. " + e.StackTrace, "Cast Failure.");
                return false;
            }
        }

        #endregion

        #region Data Members.

        // Fields.

        /// <summary>
        /// The value associated with a given constraint.
        /// </summary>
        private object m_value = null;

        /// <summary>
        /// Type of object.
        /// </summary>
        private Type m_type;
        
        // Properties.

        /// <summary>
        /// Value to be held in the constraint.
        /// </summary>
        public object Value
        {
            get { return this.m_value; }
        }

        /// <summary>
        /// The type of the input value.
        /// </summary>
        public Type Type
        {
            get { return this.m_type; }
        }

        /// <summary>
        /// If there is any value stored.
        /// </summary>
        public bool IsEmpty
        {
            get { return (this.m_value == null); }
        }

        #endregion

        #region Constructors.

        /// <summary>
        /// Constructor that creates an object and stores its type.
        /// </summary>
        /// <param name="value"></param>
        public Constraint(object _value)
        {
            this.m_value = _value;
            if (!this.IsEmpty) { this.m_type = _value.GetType(); }
        }
                
        /// <summary>
        /// Constructor that creates an object and stores its type.
        /// </summary>
        /// <param name="value"></param>
        public Constraint(object _value, Type _type)
        {
            this.m_value = _value;
            this.m_type = _type;
        }
        
        #endregion

        #region Accessor Methods.

        /// <summary>
        /// Will return true if value matches input type, and can be retrieved.
        /// </summary>
        /// <typeparam name="T">Type to request object as.</typeparam>
        /// <param name="value">Value of the object.</param>
        /// <returns>Return true. Outputs value.</returns>
        public bool TryGetValue<T>(out T value)
        {
            value = default(T);

            try
            {
                if (this.HasValue())
                {
                    // Check if the requested type matches.
                    if (this.m_type == typeof(T))
                    {
                        value = GetValue<T>((object)value);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Debugger.Print("Cannot get value of this type. " + e.StackTrace, "Casting Failure.");
                return false;
            }

            return false;
        }

        /// <summary>
        /// Returns true if value assign is successful.
        /// </summary>
        /// <typeparam name="T">Type to assign value as.</typeparam>
        /// <param name="value">Value to assign.</param>
        /// <returns>Returns boolean flag determining success of operation.</returns>
        public bool TryAssignValue<T>(object value)
        {
            try
            {
                AssignValue<T>(value);
                return true;
            }
            catch (Exception e)
            {
                Debugger.Print("Cannot get value of this type. " + e.StackTrace, "Casting Failure.");
                return false;
            }
        }

        /// <summary>
        /// Assign a value, cast to the input generic type. Doesn't work if the constraint already has a type.
        /// </summary>
        /// <typeparam name="T">Generic type to input value as.</typeparam>
        /// <param name="value">Value to assign.</param>
        private void AssignValue<T>(object value)
        {
            if (this.m_type == typeof(T))
            {
                this.m_value = value;
            }
            else
            {
                throw new Exception("The type of generic " + typeof(T).ToString() + " does not match the stored type of " + this.m_type.ToString() + ".");
            }
        }

        /// <summary>
        /// Return value, cast as a particular type.
        /// </summary>
        /// <typeparam name="T">Type to request object as.</typeparam>
        /// <param name="value">Value of the object.</param>
        /// <returns>Return object, cast as input type.</returns>
        private T GetValue<T>(object value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        /// <summary>
        /// Returns true if it has a value.
        /// </summary>
        /// <returns>Returns boolean.</returns>
        public bool HasValue()
        {
           return (!IsEmpty);
        }

        #endregion

    }

}
