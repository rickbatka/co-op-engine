using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace co_op_engine.Utility
{
	/// <summary>
	/// input helper class, handles states for keyboard and gamepads
	/// </summary>
	public class InputHandler : GameComponent
	{
		#region Fields & Properties

		//keyboard tracking
		static KeyboardState keyboardState;
		public static KeyboardState KeyboardState
		{
			get { return keyboardState; }
		}

		static KeyboardState lastKeyboardState;
		public static KeyboardState LastKeyboardState
		{
			get { return lastKeyboardState; }
		}

		static MouseState mouseState;
		public static MouseState MouseState
		{
			get { return mouseState; }
		}

		static MouseState lastMouseState;
		public static MouseState LastMouseState
		{
			get { return lastMouseState; }
		}

		static GamePadState[] gamePadStates;
		public static GamePadState[] GamePadStates
		{
			get { return gamePadStates; }
		}

		static GamePadState[] lastGamePadStates;
		public static GamePadState[] LastGamePadStates
		{
			get { return lastGamePadStates; }
		}

		#endregion

		#region Constructors

		public InputHandler(Game game)
			: base(game)
		{
			keyboardState = Keyboard.GetState();

			gamePadStates = new GamePadState[Enum.GetValues(typeof(PlayerIndex)).Length];

			mouseState = Mouse.GetState();

			foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
			{
				gamePadStates[(int)index] = GamePad.GetState(index);
			}
		}

		#endregion

		#region Methods

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void Update(GameTime gameTime)
		{
			lastKeyboardState = keyboardState;
			keyboardState = Keyboard.GetState();

			lastMouseState = mouseState;
			mouseState = Mouse.GetState();

			lastGamePadStates = (GamePadState[])gamePadStates.Clone();
			foreach (PlayerIndex index in Enum.GetValues(typeof(PlayerIndex)))
			{
				gamePadStates[(int)index] = GamePad.GetState(index);
			}

			base.Update(gameTime);
		}

		/// <summary>
		/// clears keyboard input buffer, all current step functions cleared
		/// </summary>
		public static void Flush()
		{
			lastKeyboardState = keyboardState;
		}

		/// <summary>
		/// checks if the given key was released this frame
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool KeyReleased(Keys key)
		{
			return keyboardState.IsKeyUp(key) && lastKeyboardState.IsKeyDown(key);
		}

		/// <summary>
		/// checks if the given key was pressed this frame
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool KeyPressed(Keys key)
		{
			return keyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
		}

		/// <summary>
		/// checks if a given key is in the down position
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static bool KeyDown(Keys key)
		{
			return keyboardState.IsKeyDown(key);
		}

		/// <summary>
		/// checks if a gamepad's button was released this frame
		/// </summary>
		/// <param name="button"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static bool ButtonReleased(Buttons button, PlayerIndex index)
		{
			return gamePadStates[(int)index].IsButtonUp(button) && lastGamePadStates[(int)index].IsButtonDown(button);
		}

		/// <summary>
		/// checks if a gamepad's button was pressed this frame
		/// </summary>
		/// <param name="button"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static bool ButtonPressed(Buttons button, PlayerIndex index)
		{
			return gamePadStates[(int)index].IsButtonDown(button) && lastGamePadStates[(int)index].IsButtonUp(button);
		}

		/// <summary>
		/// checks if a gamepad's button is in the down state
		/// </summary>
		/// <param name="button"></param>
		/// <param name="index"></param>
		/// <returns></returns>
		public static bool ButtonDown(Buttons button, PlayerIndex index)
		{
			return gamePadStates[(int)index].IsButtonDown(button);
		}

		//left up
		public static bool MouseLeftReleased()
		{
			return mouseState.LeftButton == ButtonState.Released && lastMouseState.LeftButton == ButtonState.Pressed;
		}

		//left down
		public static bool MouseLeftPressed()
		{
			return mouseState.LeftButton == ButtonState.Pressed && lastMouseState.LeftButton == ButtonState.Released;
		}

		//left pressed
		public static bool MouseLeftDown()
		{
			return mouseState.LeftButton == ButtonState.Pressed;
		}

		//right up
		public static bool MouseRightReleased()
		{
			return mouseState.RightButton == ButtonState.Released && lastMouseState.RightButton == ButtonState.Pressed;
		}

		//right down
		public static bool MouseRightPressed()
		{
			return mouseState.RightButton == ButtonState.Pressed && lastMouseState.RightButton == ButtonState.Released;
		}

		//right pressed
		public static bool MouseRightDown()
		{
			return mouseState.RightButton == ButtonState.Pressed;
		}

		//middle up
		public static bool MouseMiddleReleased()
		{
			return mouseState.MiddleButton == ButtonState.Released && lastMouseState.MiddleButton == ButtonState.Pressed;
		}

		//middle down
		public static bool MouseMiddlePressed()
		{
			return mouseState.MiddleButton == ButtonState.Pressed && lastMouseState.MiddleButton == ButtonState.Released;
		}

		//middle pressed
		public static bool MouseMiddleDown()
		{
			return mouseState.MiddleButton == ButtonState.Pressed;
		}

		//position of mouse
		public static Point MousePositionPoint()
		{
			return new Point(Mouse.GetState().X, Mouse.GetState().Y);
		}

		public static Vector2 MousePositionVector()
		{
			return new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
		}

		public static Vector2 MousePositionVectorCameraAdjusted()
		{
			return new Vector2(Mouse.GetState().X, Mouse.GetState().Y) + Camera.Instance.Position;
		}

		#endregion
	}
}
