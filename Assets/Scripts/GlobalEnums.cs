﻿public struct GlobalEnums
{
    public enum Options { Rotate3x3Left90Degrees, Rotate3x3Right90Degrees, Rotate3x3Left180Degrees, Rotate3x3Right180Degrees, Destroy, DestroyWithColors, ThreeByThreeSwitch, SwitchColorOfOne, TranslateOneTile, SwitchAdjacentRows, SwitchAdjacentColumns, HorizontalFlip2x2, VerticalFlip2x2, Rotate2x2Left90Degrees, Rotate2x2Left180Degrees, HorizontalFlip3x3, VerticalFlip3x3, SwitchColorOfTwo, SwitchColorOfThree, SwitchAdjacent2x2, Move3ToTop, Move2x2ToTop, FlipHorizontal, FlipVertical, SwitchEdgeRows, SwitchEdgeColumns, Move1x3Left, Move1x3Right, Move1x2LeftOrRight };
    public enum SelectionMode { ThreeByThree, TwoByTwo, Single, SaveSelection, OneByThree };

    public enum FlipOptions { Vertical, Horizontal };
}
