using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using System.Xml;

namespace MyGame
{
    public class Enemy : SyncScript
    {
        public float HP = 100.0f;
        public Entity Need_to_Delete;

        public override void Update()
        {
            if (HP <= 0)
            {
                DebugText.Print("Enemy Deleted!!!", new Int2(580, 580));
                Entity.Scene.Entities.Remove(Entity);
              //  Need_to_Delete = null;
            }
        }
    }
}
