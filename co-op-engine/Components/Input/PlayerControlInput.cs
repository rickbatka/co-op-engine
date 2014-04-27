using co_op_engine.Utility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace co_op_engine.Components.Input
{
    enum Device { Mouse, Joystick, Keyboard }
    class PlayerControlInput
    {
        private Device currentAimingDevice = Device.Mouse;
        private Device previousAimingDevice = Device.Mouse;
        private Device currentMovingDevice = Device.Keyboard;
        private Device previousMovingDevice = Device.Keyboard;
        private Vector2 currentMouseVector;
        private Vector2 previousMouseVector = Vector2.Zero;
        private Vector2 currentRightStickVector;
        private Vector2 previousRightStickVector = Vector2.Zero;
        private const float MouseDeadZone = 5f;
        private const float JoystickDeadZone = 0.65f;
        private const float TriggerDeadZone = 0.65f;

        public void BeforeUpdate() 
        {
            currentMouseVector = InputHandler.MousePositionVectorCameraAdjusted();
            if (JoystickIsPastDeadZone(InputHandler.GamePadStates[(int)PlayerIndex.One].ThumbSticks.Right))
            {
                currentRightStickVector = InputHandler.GamePadStates[(int)PlayerIndex.One].ThumbSticks.Right;
            }
            
            SetAimingDevice();
            SetMovingDevice();
        }

        private void SetAimingDevice() 
        {
            if (currentMouseVector != previousMouseVector &&
                (currentMouseVector - previousMouseVector).Length() > MouseDeadZone)
            {
                currentAimingDevice = Device.Mouse;
            }
            else if (JoystickIsPastDeadZone(InputHandler.GamePadStates[(int)PlayerIndex.One].ThumbSticks.Right))
            {
                // if they are using both the mouse and joystick, welp, the joystick wins I guess
                currentAimingDevice = Device.Joystick;
            }
            else
            {
                currentAimingDevice = previousAimingDevice;
            }
        }
        private void SetMovingDevice() 
        {
            if(KeyBindings.KeyboardMovementKeys.Any(k => k.IsDown()))
            {
                currentMovingDevice = Device.Keyboard;
            }
            else if (JoystickIsPastDeadZone(InputHandler.GamePadStates[(int)PlayerIndex.One].ThumbSticks.Left))
            {
                currentMovingDevice = Device.Joystick;
            }
            else
            {
                currentMovingDevice = previousMovingDevice;
            }
        }

        public void Update(GameTime gameTime)
        {

        }

        public void AfterUpdate()
        {
            previousMouseVector = currentMouseVector;
            previousRightStickVector = currentRightStickVector;
            previousAimingDevice = currentAimingDevice;
            previousMovingDevice = currentMovingDevice;
        }

        public Vector2 GetFacingDirection(Vector2 ownerPosition)
        {
            var facingDirectionRaw = Vector2.Zero;
            if (currentAimingDevice == Device.Mouse)
            {
                facingDirectionRaw = currentMouseVector - ownerPosition;
            }
            else if (currentAimingDevice == Device.Joystick)
            {
                if (JoystickIsPastDeadZone(InputHandler.GamePadStates[(int)PlayerIndex.One].ThumbSticks.Right))
                {
                    facingDirectionRaw = new Vector2(
                        x: currentRightStickVector.X,
                        y: -currentRightStickVector.Y
                    );
                }
                else
                {
                    facingDirectionRaw = new Vector2(
                        x: previousRightStickVector.X,
                        y: -previousRightStickVector.Y
                    );
                }
            }
            else
            {
                // No aiming device, let it default to last frame's facing direction
            }

            facingDirectionRaw.Normalize();
            return facingDirectionRaw;
        }

        private bool JoystickIsPastDeadZone(Vector2 thumbstick)
        {
            return (Math.Abs(thumbstick.X) > JoystickDeadZone
                  || Math.Abs(thumbstick.Y) > JoystickDeadZone);
        }

        public bool IsPressingAttackButton()
        {
            return KeyBindings.AttackInputs.Any(k => k.IsBeingPressed());
        }

        public Vector2 GetMovement()
        {
            if(currentMovingDevice == Device.Keyboard)
            {
                return GetKeyboardMovement();
            }
            else if(currentMovingDevice == Device.Joystick)
            {
                return GetJoystickMovement();
            }
            else
            {
                return Vector2.Zero;
            }
        }

        private Vector2 GetKeyboardMovement()
        {
            Vector2 inputVector = new Vector2();
            if (KeyBindings.KEYBOARD_MOVE_UP.IsDown())
            {
                inputVector.Y = -1;
            }
            else if (KeyBindings.KEYBOARD_MOVE_DOWN.IsDown())
            {
                inputVector.Y = 1;
            }
            else
            {
                inputVector.Y = 0;
            }

            if (KeyBindings.KEYBOARD_MOVE_LEFT.IsDown())
            {
                inputVector.X = -1;
            }
            else if (KeyBindings.KEYBOARD_MOVE_RIGHT.IsDown())
            {
                inputVector.X = 1;
            }
            else
            {
                inputVector.X = 0;
            }
            return inputVector;
        }

        private Vector2 GetJoystickMovement()
        {
            if (JoystickIsPastDeadZone(InputHandler.GamePadStates[(int)PlayerIndex.One].ThumbSticks.Left))
            {
                return new Vector2(
                    x: InputHandler.GamePadStates[(int)PlayerIndex.One].ThumbSticks.Left.X,
                    y: -InputHandler.GamePadStates[(int)PlayerIndex.One].ThumbSticks.Left.Y
                );
            }

            return Vector2.Zero;  
        }

    }
}
