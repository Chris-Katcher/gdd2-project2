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
        /// Enable flag.
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
        
        #endregion

        #region Properties.

        /////////////////////
        // Public data fields.
        /////////////////////

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
        }
        
        /// <summary>
        /// Element position.
        /// </summary>
        public Vector2 Position
        {
            get { return this.m_position; }
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

        #endregion

        #endregion

        #region Mutator Methods.

        /// <summary>
        /// Set the offset of this element.
        /// </summary>
        /// <param name="_offset">Set the offset of this element</param>
        public void SetOffset(Vector2 _offset)
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
        public void SetPosition(Vector2 _position)
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
        public void SetDepth(int _depth)
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
        public void SetVisible(bool _visible)
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
        public void SetFocus(bool _focus)
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
        public void SetHover(bool _hover)
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
        public void Enable(bool _flag)
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
        public void Select(bool _select)
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
