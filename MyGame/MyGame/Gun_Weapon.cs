using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Physics;
using Stride.Animations;
using SharpFont.PostScript;

namespace MyGame
{
    public class Gun_Weapon : SyncScript
    {
        public Prefab BulletHolePrefab;
        private AnimationComponent animation_Component;
        public float animation_Speed = 1.0f;
        public Entity Anim_Gun;
        private PlayingAnimation current_anim = null;

        private float vertical_angle = 3.0f;
        private float horizontal_angle = 2.0f;
        private float min_recoil_amount = 0.5f;
        private float max_recoil_amount = 1.5f;
        private float recoil_recovery_speed = 1.05f;
        private CameraComponent camera;
        public float Damage_amount = 10.0f;


        public override void Start()
        {
            Anim_Gun = Entity.FindChild("Gun_animations");
            animation_Component = Anim_Gun.Components.Get<AnimationComponent>();
            camera = Entity.Get<CameraComponent>();
        }
        public override void Update()
        {
            var delta = (float)Game.UpdateTime.Elapsed.TotalSeconds;
            if (Input.IsMouseButtonPressed(MouseButton.Left))
            {
                current_anim = animation_Component.Play("Shoot");
                Fire();
            }
        }
        private void Fire()
        {
            animation_Speed = 1.5f;
            current_anim.TimeFactor = animation_Speed;
            var raycastStart = Entity.Transform.WorldMatrix.TranslationVector;
            var forward = Entity.Transform.WorldMatrix.Forward;
            var raycastEnd = raycastStart + forward * 100f;
            var result = this.GetSimulation().Raycast(raycastStart, raycastEnd);

            // Create a ray from the center of the screen
            if (result.Succeeded && result.Collider != null)
            {
                var rigidBody = result.Collider as RigidbodyComponent;

                Enemy _Enemy = result.Collider.Entity.Components.Get<Enemy>();

                if (_Enemy != null )
                {
                    _Enemy.HP -= Damage_amount;
                }

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
