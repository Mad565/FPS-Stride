using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Physics;

namespace MyGame.Code
{
    public class Weapon_Script : SyncScript
    {

        public Prefab BulletHolePrefab;
        public override void Update()
        {
            if (Input.IsMouseButtonPressed(MouseButton.Left))
            {
                Fire();
            }
        }
        private void Fire()
        {
            var raycastStart = Entity.Transform.WorldMatrix.TranslationVector;
            var forward = Entity.Transform.WorldMatrix.Forward;
            var raycastEnd = raycastStart + forward * 100f;
            var result = this.GetSimulation().Raycast(raycastStart, raycastEnd);

            // Create a ray from the center of the screen
            if (result.Succeeded && result.Collider != null)
            {
                var rigidBody = result.Collider as RigidbodyComponent;

                var instance = BulletHolePrefab.Instantiate();
                var entity = instance[0];
                var entityWorldPos = result.Collider.Entity.Transform.WorldMatrix.TranslationVector;
                entity.Transform.Position = result.Point - entityWorldPos;
                entity.Transform.Rotation = Quaternion.BetweenDirections(Vector3.UnitY, result.Normal);
                result.Collider.Entity.AddChild(entity);
                
                if (rigidBody != null)
                {
                 rigidBody.Activate();
                 rigidBody.ApplyImpulse(forward * 5f);
                 rigidBody.ApplyTorqueImpulse(forward * 5f + new Vector3(0, 1, 0));
                }

            }

        }
        
    }
}
