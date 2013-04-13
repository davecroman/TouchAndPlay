using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TouchAndPlay.screens
{
    public enum StagePreparationStates
    {
        CHECK_KINECT_CONNECTION,
        CHECK_SKELETON,
        CHECK_DEPTH,
        ADJUST_ANGLE,
        MEASURE_SEGMENTS,
        READY,
        FINISHED,
    }
}
