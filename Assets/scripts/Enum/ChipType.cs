using System;

[Flags]
public enum ChipType
{
    None = 0,
    Horizontal = 1,
    Vertical = 2,
    LeftUp = 4,
    LeftDown = 8,
    RightDown = 16,
    UpRight = 32,
    Block = 64,
    Empty = 128,
}