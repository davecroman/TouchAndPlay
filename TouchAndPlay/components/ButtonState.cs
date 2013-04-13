using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TouchAndPlay.components
{
    enum ButtonState
    {
        //mouse is away from button
        MOUSE_OUT,

        //when mouse hovers with the button
        HOVERED,

        //when left mouse button is pressed while being hovered
        PRESSED,
    }
}
