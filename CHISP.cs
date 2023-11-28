using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHISP
{
    /// <summary>
    /// 芯片系列
    /// </summary>
    public enum ChipSeries
    {
        CH55X = 0,
        CH56X = 1,
        CH54X = 2,
        CH57X = 3,
        CH32F10X = 4,
        CH32V10X = 5,
        CH58X = 6,
        CH32V30X = 7,
        CH32F20X = 8,
        CH32V20X = 9,
        CH32V00X = 0x0b,
        CH59X = 0x0c,
        CH32X035 = 0x0d, // not supported
        CH643 = 0x0e, // not supported
    }

    /// <summary>
    /// CH55X系列的芯片型号
    /// </summary>
    public enum CH55X_ChipType
    {
        CH551 = 0x51,
        CH552 = 0x52,
        CH554 = 0x54,
        CH555 = 0x55,
        CH556 = 0x56,
        CH557 = 0x57,
        CH558 = 0x58,
        CH559 = 0x59,
    }

    /// <summary>
    /// CH56X系列的芯片型号
    /// </summary>
    public enum CH56X_ChipType
    {
        CH561 = 0x61,
        CH563 = 0x63,
        CH565 = 0x65,
        CH566 = 0x66,
        CH567 = 0x67,
        CH568 = 0x68,
        CH569 = 0x69,
    }

    /// <summary>
    /// CH54X系列的芯片型号
    /// </summary>
    public enum CH54X_ChipType
    {
        CH543 = 0x43,
        CH545 = 0x45,
        CH546 = 0x46,
        CH547 = 0x47,
        CH548 = 0x48,
        CH549 = 0x49,
    }

    /// <summary>
    /// CH57X系列的芯片型号
    /// </summary>
    public enum CH57X_ChipType
    {
        CH571 = 0x71,
        CH573 = 0x73,
        CH577 = 0x77,
        CH578 = 0x78,
        CH579 = 0x79,
    }

    /// <summary>
    /// CH32F10X系列的芯片型号
    /// </summary>
    public enum CH32F10X_ChipType
    {
        CH32F103 = 0x3F,
    }

    /// <summary>
    /// CH32F20X系列的芯片型号
    /// </summary>
    public enum CH32F20X_ChipType
    {
        CH32F203 = 0x30,
    }

    /// <summary>
    /// CH32V10X系列的芯片型号
    /// </summary>
    public enum CH32V10X_ChipType
    {
        CH32V103 = 0x3F,
    }

    /// <summary>
    /// CH32V30X系列的芯片型号
    /// </summary>
    public enum CH32V30X_ChipType
    {
        CH32V303 = 0x30,
        CH32V305 = 0x50,
        CH32V307 = 0x70,
    }

    /// <summary>
    /// CH32V20X系列的芯片型号
    /// </summary>
    public enum CH32V20X_ChipType
    {
        CH32V203 = 0x30,
        CH32V208 = 0x80,
    }

    /// <summary>
    /// CH32V00X系列的芯片型号
    /// </summary>
    public enum CH32V00X_ChipType
    {
        CH32V003 = 0x30,
    }

    /// <summary>
    /// CH58X系列的芯片型号
    /// </summary>
    public enum CH58X_ChipType
    {
        CH581 = 0x81,
        CH582 = 0x82,
        CH583 = 0x83,
    }

    /// <summary>
    /// CH59X系列的芯片型号
    /// </summary>
    public enum CH59X_ChipType
    {
        CH591 = 0x91,
        CH592 = 0x92,
    }
}
