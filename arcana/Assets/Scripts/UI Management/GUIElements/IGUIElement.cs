/************************************************
 * IGUIElement.cs
 * 
 * Contains implementation for the IGUIElement interface.
 ************************************************/

/////////////////////
// Using statements.
/////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Arcana.UI.Elements
{

    #region Abstract Class: GUIElement class.

    /// <summary>
    /// GUIElement represents HUD elements that can be drawn to the screen.
    /// </summary>
    public abstract class GUIElement : MonoBehaviour, IGUIElement
    {

        #region Data Members

        #region Fields.

        /////////////////////
        // Private data fields.
        /////////////////////

        /// <summary>
        /// Children that may be GUI elements.
        /// </summary>
        private List<GUIElement> m_elements;

        /// <summary>
        /// Name of the element.
        /// </summary>
        private string m_elementName;

        /// <summary>
        /// Offset position of the element.
        /// </summary>
        private Vector2 m_offset;

        /// <summary>
        /// Position of the element.
        /// </summary>
        private Vector2 m_position;

        /// <summary>
        /// Draw order value.
        /// </summary>
        private int m_depth;

        /// <summary>
        /// Element visibility.
        /// </summary>
        private bool m_visible;

        /// <summary>
        /// Enable state for the element.
        /// </summary>
        private bool m_enabled;

        /// <summary>
        /// Focus state for the element.
        /// </summary>
        private bool m_focus;
        
        /// <summary>
        /// Selected state for the element.
        /// </summary>
        private bool m_selected;

        /// <summary>
        /// Hover state for the element.
        /// </summary>
        private bool m_hover;

        /// <summary>
        /// Size of the element.
        /// </summary>
        private Rect m_dimensions;

        #endregion

        #region Properties.

        /////////////////////
        // Public data fields.
        /////////////////////

        /// <summary>
        /// Name of the element.
        /// </summary>
        public string Name
        {
            get { return this.m_elementName; }
            set
            {
                this.m_elementName = value;
                this.gameObject.name = this.m_elementName;        
            }
        }

        /// <summary>
        /// Reference to its children.
        /// </summary>
        public List<GUIElement> Children
        {
            get
            {
                if (this.m_elements == null)
                {
                    this.m_elements = new List<GUIElement>();
                }
                return this.m_elements;
            }
        }

        /// <summary>
        /// Flag determining if element has children.
        /// </summary>
        public bool HasChildren
        {
            get { return this.Children.Count != 0; }
        }

        /// <summary>
        /// Offset of the element.
        /// </summary>
        public Vector2 Offset
        {
            get { return this.m_offset; }
            protected set { this.m_offset = value; }
        }
        
        /// <summary>
        /// Element position.
        /// </summary>
        public Vector2 Position
        {
            get { return this.m_position; }
            protected set { this.m_position = value; }
        }

        /// <summary>
        /// Hover state for the element.
        /// </summary>
        public bool Hover
        {
            get { return this.m_hover; }
            protected set { this.m_hover = value; }
        }

        /// <summary>
        /// Focus state for the element.
        /// </summary>
        public bool Focus
        {
            get { return this.m_focus; }
            protected set { this.m_focus = value; }
        }

        /// <summary>
        /// Selected state for the element.
        /// </summary>
        public bool Selected
        {
            get { return this.m_selected; }
            protected set { this.m_selected = value; }
        }

        /// <summary>
        /// Visibility of the element.
        /// </summary>
        public bool Visible
        {
            get { return this.m_visible; }
            protected set { this.m_visible = value; }
        }

        /// <summary>
        /// The enabled flag reference. 
        /// </summary>
        public bool Enabled
        {
            get { return this.m_enabled; }
            set { this.m_enabled = value; }
        }

        /// <summary>
        /// Size of the image.
        /// </summary>
        public Vector2 Dimensions
        {
            get { return this.m_dimensions.max; }
            set { SetSize(value); }
        }

        /// <summary>
        /// Returns the boundaries in world-space.
        /// </summary>
        public Rect Bounds
        {
            get { return new Rect(this.Position.x, this.Position.y, this.Position.x + this.Dimensions.x, this.Position.y + this.Dimensions.y); }
            set
            {
                this.Position = value.min;
                this.Dimensions = value.max - value.min;
            }
        }

        #endregion

        #endregion

        #region Service Methods

        /// <summary>
        /// Builds a GUIElement, initializes it, and returns a reference to it.
        /// </summary>
        /// <typeparam name="T">Type of element to build.</typeparam>
        /// <param name="_parent">Parent that the element will be attached to.</param>
        /// <returns>Returns a built and initialized GUIElement.</returns>
        public T Build<T>(GameObject _parent = null) where T : GUIElement
        {
            GameObject parent = _parent;

            // If parent is null, create a new parent.
            if (parent == null)
            {
                parent = Services.CreateEmptyObject("GUIElement");
            }

            // Get reference to the input element.
            T element = parent.GetComponent<T>();

            // If the element doesn't exist, make a new one.
            if (element == null)
            {
                parent.AddComponent<T>();
            }

            // Initialize the element.
            return Initialize<T>(element);
        }

        /// <summary>
        /// Generic GUIElement constructor.
        /// </summary>
        /// <param name="_parent">Parent that the element will be attached to.</param>
        /// <returns>Returns a built and initialized GUIElement.</returns>
        public GUIElement Build(GameObject _parent = null)
        {
            return Build<GUIElement>(_parent);
        }

        /// <summary>
        /// Initialize the GUIElement.
        /// </summary>
        /// <returns>Returns an initialized GUIElement.</returns>
        private T Initialize<T>(T _element) where T : GUIElement
        {
            _element.Initialize();
            return _element;
        }

        /// <summary>
        /// Initialize the GUIElement. Defined by child classes.
        /// </summary>
        protected abstract void Initialize();

        /// <summary>
        /// Destroy the element.
        /// </summary>
        public void Destroy()
        {
            // Destroying the element.
            Debugger.Print("Destroying GUIElement.", this.Name);

            // Destroy each of the children.
            foreach (GUIElement child in this.Children)
            {
                child.Destroy();
            }

            // Destroy the game object.
            Destroy(this.gameObject);
        }

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Set the boundaries of the image.
        /// </summary>
        /// <param name="_size">Bounds.</param>
        public void SetSize(Vector2 _size)
        {
            this.transform.localScale = new Vector3(_size.x, _size.y, 1.0f);
            this.m_dimensions.Set(0.0f, 0.0f, _size.x, _size.y);
        }
        
        /// <summary>
        /// Set the offset of this element.
        /// </summary>
        /// <param name="_offset">Set the offset of this element</param>
        public virtual void SetOffset(Vector2 _offset)
        {
            this.m_offset = _offset;
            
            foreach (GUIElement element in this.Children)
            {
                element.SetOffset(element.Offset + this.Offset);
            }
        }

        /// <summary>
        /// Set the position of this element.
        /// </summary>
        /// <param name="_position">Set the position of this element.</param>
        public virtual void SetPosition(Vector2 _position)
        {
            this.m_position = _position;

            foreach (GUIElement element in this.Children)
            {
                element.SetOffset(element.Position + this.Position);
            }
        }

        /// <summary>
        /// Set the depth of this element and any of its children.
        /// </summary>
        /// <param name="_depth">Depth to set.</param>
        public virtual void SetDepth(int _depth)
        {
            this.m_depth = _depth;

            foreach (GUIElement element in this.Children)
            {
                element.SetDepth(element.m_depth + this.m_depth);
            }
        }

        /// <summary>
        /// Set the visibility.
        /// </summary>
        /// <param name="_visible">Visibility flag.</param>
        public virtual void SetVisible(bool _visible)
        {
            this.m_visible = _visible;

            foreach (GUIElement element in this.Children)
            {
                element.SetVisible(this.m_visible);
            }
        }

        /// <summary>
        /// Set the focus.
        /// </summary>
        /// <param name="_focus">Focus flag.</param>
        public virtual void SetFocus(bool _focus)
        {
            this.m_focus = _focus;

            foreach (GUIElement element in this.Children)
            {
                element.SetFocus(this.m_focus);
            }
        }

        /// <summary>
        /// Set the hover.
        /// </summary>
        /// <param name="_hover">Hover flag.</param>
        public virtual void SetHover(bool _hover)
        {
            this.m_hover = _hover;

            foreach (GUIElement element in this.Children)
            {
                element.SetHover(this.m_hover);
            }
        }

        /// <summary>
        /// Enable/Disable.
        /// </summary>
        /// <param name="_flag">Enable flag.</param>
        public virtual void Enable(bool _flag)
        {
            this.m_enabled = _flag;

            foreach (GUIElement element in this.Children)
            {
                element.Enable(this.m_enabled);
            }
        }

        /// <summary>
        /// Select a GUI element.
        /// </summary>
        /// <param name="_select">Selection flag.</param>
        public virtual void Select(bool _select)
        {
            this.m_selected = _select;

            foreach (GUIElement element in this.Children)
            {
                element.Select(this.m_selected);
            }
        }

        #endregion

    }

    #endregion

    #region Interface: IGUIElement.

    /// <summary>
    /// Interface of the GUI objects that screens will be creating during gameplay.
    /// </summary>
    public interface IGUIElement
    {

        #region Data Members
        
        /// <summary>
        /// Hover state for the element.
        /// </summary>
        bool Hover
        { get; }

        /// <summary>
        /// Focus state for the element.
        /// </summary>
        bool Focus
        { get; }
        
        /// <summary>
        /// Selected state for the element.
        /// </summary>
        bool Selected
        { get; }

        /// <summary>
        /// Visibility of the element.
        /// </summary>
        bool Visible
        { get; }

        #endregion

    }

    #endregion

}
