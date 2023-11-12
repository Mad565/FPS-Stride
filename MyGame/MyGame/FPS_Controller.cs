using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stride.Core.Mathematics;
using Stride.Input;
using Stride.Engine;
using Stride.Physics;

namespace MyGame
{
    public class FPS_Controller : SyncScript
    {
        public CharacterComponent characterController;
        public CameraComponent camera;

        // Camera settings
        public float MouseSpeed = 70f;
        public float MaxLookUpAngle = -70;
        public float MaxLookDownAngle = 70;
        // Character settings
        //public Vector3 MovementMultiplier = new Vector3(1, 0, 1);
        public float Jump_Velocity = 5;
        public float Speed = 10;
        private bool isActive = false;
        private CharacterComponent character;
        // Crouching Varibles
        private bool isCrouching = false;
        public float StandHeight = 1.9f;
        public float CrouchHeight = 0.4f;

        // Private variables
        private float Mouse_X;
        private float Mouse_Y;

        public override void Start()
        {
            isActive = true;
            Game.IsMouseVisible = false;
            Input.LockMousePosition(true);
            character = Entity.Get<CharacterComponent>();
        }

        public override void Update()
        {
            if (Input.IsKeyPressed(Keys.Escape))
            {
                isActive = !isActive;
                Game.IsMouseVisible = !isActive;
                Input.UnlockMousePosition();
            }

            if (isActive)
            {
                Mouse_X -= Input.MouseDelta.X * MouseSpeed;
                Mouse_Y -= Input.MouseDelta.Y * MouseSpeed;

                Mouse_Y = MathUtil.Clamp(Mouse_Y, -90, 90);
                camera.Entity.Transform.Rotation = Quaternion.RotationYawPitchRoll(0, MathUtil.DegreesToRadians(Mouse_Y), 0);
                characterController.Orientation = Quaternion.RotationYawPitchRoll(MathUtil.DegreesToRadians(Mouse_X), 0, 0);


                var velocity = new Vector3();
                if (Input.IsKeyDown(Keys.W))
                    velocity.Z++;
                if (Input.IsKeyDown(Keys.S))
                    velocity.Z--;
                if (Input.IsKeyDown(Keys.A))
                    velocity.X++;
                if (Input.IsKeyDown(Keys.D))
                    velocity.X--;
                
                velocity.Normalize();
                //   velocity *= MovementMultiplier * Speed;
                velocity.X *= Speed * 2.5f; // Apply speed to horizontal movement
                velocity.Z *= Speed * 2.5f; // Apply half speed to vertical movement


                velocity = Vector3.SmoothStep(velocity, velocity, 0.2f);
                velocity = Vector3.Transform(velocity, characterController.Orientation);
                character.SetVelocity(velocity);

                if (Input.IsKeyDown(Keys.Space) && character.IsGrounded == true && ! Input.IsKeyDown(Keys.LeftCtrl))
                    character.Jump(new Vector3(0, Jump_Velocity, 0));

                var targetHeight = isCrouching ? CrouchHeight : StandHeight;

                if (Input.IsKeyPressed(Keys.LeftCtrl))
                    isCrouching = !isCrouching;

                var colliderShape = character.ColliderShape;
                var targetScale = new Vector3(1, targetHeight, 1);
                var currentScale = colliderShape.Scaling;
                var smoothScale = Vector3.Lerp(currentScale, targetScale, 10f * (float)Game.UpdateTime.Elapsed.TotalSeconds);
                colliderShape.Scaling = smoothScale;
            }
        }
    }
}
