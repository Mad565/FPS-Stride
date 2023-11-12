using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;

namespace MyGame
{
    public class Weapon_sway : SyncScript
    {
        // The amount of sway
        public float amount = 0.1f;

        // The maximum amount of sway
        public float maxAmount = 0.3f;

        // The smoothness of the sway
        public float smoothAmount = 6f;

        private Vector3 initialPosition;

        public override void Start()
        {
            initialPosition = Entity.Transform.Position;
        }

        public override void Update()
        {
            float movementX = -Input.MouseDelta.X * amount;
            float movementY = -Input.MouseDelta.Y * amount;

            movementX = Math.Clamp(movementX, -maxAmount, maxAmount);
            movementY = Math.Clamp(movementY, -maxAmount, maxAmount);

            Vector3 finalPosition = new Vector3(movementX, movementY, 0);
            Entity.Transform.Position = Vector3.Lerp(Entity.Transform.Position, finalPosition + initialPosition, (float)Game.UpdateTime.Elapsed.TotalSeconds * smoothAmount);
        }
    }
}
