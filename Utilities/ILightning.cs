using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace NoxiumMod.Utilities
{
    interface ILightning
    {
        bool IsComplete { get; }

        void Update();
        void Draw(SpriteBatch spriteBatch);
    }
}
