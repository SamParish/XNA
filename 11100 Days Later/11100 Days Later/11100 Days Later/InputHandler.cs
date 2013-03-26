using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace _11100_Days_Later
{
    class InputHandler
    {
        KeyboardState currentKBState, previousKBState;
        MouseState currentMouseState, previousMouseState;

        const Keys up = Keys.Up,
                    down = Keys.Down,
                    left = Keys.Left,
                    right = Keys.Right,
                    enter = Keys.Enter;

        public void UpdateInput()
        {
            previousKBState = currentKBState;
            currentKBState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }

        public bool IsLeftClick()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed;
        }

        public bool IsLeftPressed()
        {
            return currentKBState.IsKeyDown(left) || currentKBState.IsKeyDown(Keys.A);
        }

        public bool IsRightPressed()
        {
            return currentKBState.IsKeyDown(right) || currentKBState.IsKeyDown(Keys.D);
        }

        public bool IsUpPressed()
        {
            return currentKBState.IsKeyDown(up) || currentKBState.IsKeyDown(Keys.W);
        }

        public bool IsDownPressed()
        {
            return currentKBState.IsKeyDown(down) || currentKBState.IsKeyDown(Keys.S);
        }
    }
}
