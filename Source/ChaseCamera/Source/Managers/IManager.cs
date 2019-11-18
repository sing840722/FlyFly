using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ChaseCameraSample
{
    public interface IManager
    {
        void Initialise();
        void Update(GameTime gameTime);
        void Clear();
        //bool Finished();
        //bool Destroy();

    }
}
