using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;

namespace ChaseCameraSample
{
    public class CollisionManager:IManager
    {
        public List<Collidable> m_Collidables;
        private HashSet<Collision> m_Collisions;

        public CollisionManager()
        {
            //Initialise();
        }

        public void Initialise()
        {
            m_Collidables = new List<Collidable>();
            m_Collisions = new HashSet<Collision>(new CollisionComparer());
        }

        public void Update(GameTime gameTime)
        {
            UpdateCollisions();
            ResolveCollisions();
        }

        public void AddCollidable(Collidable c)
        {
            m_Collidables.Add(c);
        }

        public bool RemoveCollidable(Collidable c)
        {
            return m_Collidables.Remove(c);
        }


        private void UpdateCollisions()
        {
            if (m_Collisions.Count > 0)
            {
                m_Collisions.Clear();
            }

            for (int i = 0; i < m_Collidables.Count; i++)
            {
                for (int j = 0; j < m_Collidables.Count; j++)
                {
                    Collidable collidable1 = m_Collidables[i];
                    Collidable collidable2 = m_Collidables[j];

                    // Make sure we're not checking an object with itself
                    if (!collidable1.Equals(collidable2))
                    {
                        // If the two objects are colliding then add them to the set
                        if (collidable1.CollisionTest(collidable2))
                        {
                            m_Collisions.Add(new Collision(collidable1, collidable2));
                        }
                    }
                }
            }
        }

        private void ResolveCollisions()
        {
            foreach (Collision collision in m_Collisions)
            {
                collision.Resolve();
            }
        }

        public void Clear()
        {
            m_Collidables.Clear();
            m_Collisions.Clear();
        }
    }
}
