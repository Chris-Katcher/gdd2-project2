using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arcana.Entities
{

    public class Entity : MonoBehaviour
    {

        /// <summary>
        /// Universal Variables
        /// </summary>
        private float height;
        public float Height
        {

            get { return height; }

            set { this.height = value; }

        }

        private float width;
        public float Width
        {

            get { return width; }

            set { this.width = value; }

        }

        private float x;

        public float X
        {

            get { return x; }

            set { this.x = value; }

        }

        private float y;

        public float Y
        {

            get { return y; }

            set { this.y = value; }

        }

        private float health;

        public float Health
        {

            get { return health; }

            set { this.health = value; }

        }


        // Use this for initialization
        void Start()
        {



        }

        // Update is called once per frame
        void Update()
        {



        }
    }

}
