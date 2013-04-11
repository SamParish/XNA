using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
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
                    shoot = Keys.Space,
                    enter = Keys.Enter,
                    exit = Keys.Escape;

        public void UpdateInput()
        {
            previousKBState = currentKBState;
            currentKBState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();
        }

        public bool IsEnterPressed()
        {
            return currentKBState.IsKeyDown(enter);
        }

        public bool IsEscPressed()
        {
            return currentKBState.IsKeyDown(exit);
        }

        public bool IsLeftClick()
        {
            return currentMouseState.LeftButton == ButtonState.Pressed;
        }

        public bool IsUpPressed()
        {
            return currentKBState.IsKeyDown(up);
        }

        public bool IsDownPressed()
        {
            return currentKBState.IsKeyDown(down);
        }

        public bool IsLeftPressed()
        {
            return currentKBState.IsKeyDown(left);
        }

        public bool IsRightPressed()
        {
            return currentKBState.IsKeyDown(right);
        }

        public bool IsSpacePressed()
        {
            return currentKBState.IsKeyDown(shoot);
        }
    }
}
