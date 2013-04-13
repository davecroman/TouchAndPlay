using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TouchAndPlay.screens
{
    enum ScreenState
    {
        //shows the games logo and other affiliated parties
        //this can also be used as the loading screen
        SPLASH_SCREEN,

        //shows all startup interactions: start game, create profile, add profile
        MENU_SCREEN,

        //this is the game proper screen that handles the appearance of bubbles
        STAGE_SCREEN,

        TEST_SCREEN,

        CARTESIAN_TEST_SCREEN,

        CREATE_PROFILE_SCREEN,
        SELECT_GAME_SCREEN,
        OPTION_SCREEN,

    }
}
