using System.Runtime.InteropServices;

namespace CHISP
{
    namespace Wrapper
    {
        public static class CHISPWrapper
        {
            enum ReturnInfoType
            {
                Success = 0,
                Error = 1,
                Hit = 2,
            }

            enum EventType
            {
                Arrival = 3,
                Remove = 0,
            }

            //ISP下载设备结构信息
            [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
            public struct CHISPDeviceInfo
            {
                public UInt32 Index;         //枚举后的设备序号
                public Byte McuType;       //芯片型号
                public Int32 IsSupportUID;  //MCU是否支持获取UID,较早版本不支持UID获取
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
                public Byte[] IspVer;     //ISP版本
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
                public Byte[] IspMcuUID;  //MCU唯一ID
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
                public String PortName;  //[32],串口设备名，只有串口下载时才有值
                public Int32 DevIsOnline;   //设备是否连接上该串口，只在串口下载时有用
                public Int32 IsColdBoot; //是否是上电进行BOOT。如果不是，则不能修改配置位
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
                public Byte[] MacAddr;
                public Int32 IsPreBTV230;
            };
            public static void InitISPDeviceInfoStruct(ref CHISPDeviceInfo deviceInfo)
            {
                deviceInfo.IspVer = new Byte[4];
                deviceInfo.IspMcuUID = new Byte[8];
                deviceInfo.MacAddr = new Byte[6];
                deviceInfo.PortName = "";
            }

            public delegate void USBNotifyHandler(  // 设备事件通知回调程序
                UInt32 iEventStatus,  // 设备事件和当前状态(在下行定义): 0=设备拔出事件, 3=设备插入事件
                UInt32 DevIndexArray, //插入或移除的设备序号
                UInt32 DevCnt);       //当前设备总数
                                      // 应用层信息输出函数
            public delegate void OutputPrintHandler(Byte InforType, String DataStr);
            // 应用层信息追加输出函数
            public delegate void AppendPrintHandler(UInt32 LineNo, String DataStr);

            //ISP下载设备设置
            [StructLayout(LayoutKind.Sequential, Pack = 1)]
            public struct CHISPOption
            {
                public UInt32 IspInterface;  //下载接口类型 0:USB，1:串口	 2:网口
                public Byte IspMcuType;    //芯片型号,芯片型号的最后两位，如CH563，则写0x63
                public Byte IspMcuSeries;  //芯片系列 0:CH55X;1:CH56X
                public Int32 IsEnableLongRest;    //使能上电复位期间的额外延时复位
                public Int32 IsXtOscStrong;       //启用晶体振荡器增强对外驱动能力,CH554不支持
                public Int32 IsEnableResetPin;    //指定手工复位输入引脚,CH554使用RST引脚,其他型号指定P5.7
                public Int32 IsEnableP0PullUp;    //使能系统复位期间P0端口的内部上拉电阻,CH554不支持
                public Int32 IsMcuResetAfterIsp;  //下载完成后是否复位直接运行目标程序
                public Int32 IsEnableCfgBuf64K;
                public Int32 EnableIce;           //CH55X,CH32FX复用为串口一键下载功能
                public Int32 EnableBootLoader;
                public Int32 IsSetNetDevMac;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
                public Byte[] NetDevMacAddr;    //CH58X时，第一字节用来记录写保护块数
                public Int32 IsCodeProtect;       //启用代码保护
                public Int32 IsFirstRunIAP;       //上电后运行IAP
                public Int32 IsEnableIAP;         //是否启用IAP
                public UInt32 IAPStartAddr;        //IAP起始地址
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
                public String UserFileName; //用户文件名
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
                public String IapFileName;  //IAP文件名	
                [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 260)]
                public String DataFileName; //Dataflash文件名

                public Int32 IsNoKeyDnAtSer;       //界面，开启串口免铵键下载
                public Byte BootPinNum;           //ISP下载配置脚
                public Int32 IsClearDataFlash;     //清空DataFlash

                public Int32 IsEnableUsbPnpNotify; //是否启用USB插拔通知
                public Int32 IsEraseAllCFlash;
                public Int32 IsEraseAllDFlash;
                public USBNotifyHandler UsbPnpNotifyRoutine;     //插拔通知回调函数
                public UInt32 LocalIP;              //本地ISP IP地址
                public UInt32 GatewayIP;
                public UInt32 MaskIP;
                public Int32 IsSetDevIP;
                public UInt32 dwDevIP;

                public OutputPrintHandler AppOutput;       //应用程序信息打印函数
                public AppendPrintHandler AppAppendOutput; //应用程序信息追加打印函数

                public Int32 UILangIsCH;           //中英文
                public Byte LV_RST_VOL;           //门限电压,CH32V003复用为复位引脚功能
                public Int32 IsSimulat;            //两线仿真调试接口使能
                public Int32 IsBootLoader;         //引导程序使能
                public Int32 IsPorCtr;
                public Int32 IsUsbdPu;
                public Int32 IsUsbdMode;
                public Int32 IsStandbyRst;
                public Int32 IsStopRst;
                public Int32 IsIwdgSw;
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
                public Byte[] WPR_DATA;

                public Int32 IsEnLockup;
                public Int32 IsEnOutReset;
                public Int32 IsEnDebug;
                public Byte UserMem;              //CH543作为LDO输出电压参数
                [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
                public Byte[] Baud;
            };

            public static void InitISPOptionStruct(ref CHISPOption option)
            {
                option.NetDevMacAddr = new byte[6];
                option.WPR_DATA = new byte[8];
                option.Baud = new byte[4];
                option.UserFileName = "";
                option.IapFileName = "";
                option.DataFileName = "";

                //下载接口
                option.IspInterface = 0;
                //下载完成后是否复位直接运行目标程序
                option.IsMcuResetAfterIsp = 0;
                //使能上电复位期间的额外延时复位
                option.IsEnableLongRest = 1;
                //指定手工复位输入引脚
                option.IsEnableResetPin = 1;
                //启用晶体振荡器增强对外驱动能力,CH554不支持
                option.IsXtOscStrong = 0;
                //使能系统复位期间P0端口的内部上拉电阻,CH554不支持
                option.IsEnableP0PullUp = 0;
                //是否启用IAP
                option.IsEnableIAP = 0;
                //设备插拔通知
                option.IsEnableUsbPnpNotify = 0;
                //默认下载脚
                option.BootPinNum = 1;
                //串口免按键下载
                option.IsNoKeyDnAtSer = 0;
                //清空DataFlash
                option.IsClearDataFlash = 0;
                //从BOOT启动必须开启
                option.IsBootLoader = 1;
                //代码读写保护必须开启
                option.IsCodeProtect = 1;
                //其他参数
                option.IsEnableCfgBuf64K = 0;
                option.EnableIce = 0;
                option.IsSetDevIP = 0;
                //门限电压
                option.LV_RST_VOL = 0;
                option.IsEraseAllCFlash = 1;
                option.Baud[0] = 0x00;
                option.Baud[1] = 0xC2;
                option.Baud[2] = 0x01;
                option.Baud[3] = 0x00;

                option.UsbPnpNotifyRoutine = null;

            }

            //option->IspInterface = 0:USB下载方式时,此函数将执行通过USB搜索ISP下载设备，返回枚举的设备数
            //option->IspInterface = 1:串口下载方式时,此函数将执行枚举电脑上所有的串口，返回枚举的串口数

            /// <summary>
            /// 获取指定序号设备信息
            /// </summary>
            /// <param name="iIndex">指定序号设备信息</param>
            /// <param name="IspDevInfor">设备信息，out传出，需提前实例化</param>
            /// <returns>成功返回非0，失败返回0</returns>
            [DllImport("WCHISPAPI.dll", EntryPoint = "WCH55x_GetIspDeviceInfor", CallingConvention = CallingConvention.Winapi)]
            public static extern Int32 GetISPDevInfo(UInt32 iIndex,      //
                                                     out CHISPDeviceInfo IspDevInfor);

            /// <summary>
            /// 设置ISP下载设置
            /// </summary>
            /// <param name="option">ISP选项</param>
            /// <returns>成功返回非0，失败返回0</returns>

            [DllImport("WCHISPAPI.dll", EntryPoint = "WCH55x_SetIspOption", CallingConvention = CallingConvention.Winapi)]
            public static extern Int32 SetISPOption(in CHISPOption option);

            /// <summary>
            /// 获取ISP下载设置
            /// </summary>
            /// <param name="option">ISP选项，out传出，需提前实例化</param>
            /// <returns>成功返回非0，失败返回0</returns>
            [DllImport("WCHISPAPI.dll", EntryPoint = "WCH55x_GetIspOption", CallingConvention = CallingConvention.Winapi)]
            public static extern Int32 GetISPOption(out CHISPOption option);

            //写EEPROM数据
            [DllImport("WCHISPAPI.dll", EntryPoint = "WCH55x_WriteDataFlash")]
            public static extern Int32 WriteDataFlash(UInt32 iIndex,            //设备序号
            UInt32 StartAddr,         //起始地址
                                         ref UInt32 oWriteLen,        //写入长度
                                         [In] Byte[] DataBuf,           //数据缓冲区
                                         Int32 bIsErase);

            //读EEPROM数据
            [DllImport("WCHISPAPI.dll", EntryPoint = "WCH55x_ReadDataFlash")]
            public static extern Int32 ReadDataFlash(UInt32 iIndex,            //设备序号
            UInt32 StartAddr,         //起始地址
                                        ref UInt32 oReadLen,        //写入长度
                                        [Out] Byte[] DataBuf);           //数据缓冲区)

            [DllImport("WCHISPAPI.dll", EntryPoint = "WCH55xIsp_ReadConfig", CallingConvention = CallingConvention.Winapi)]
            public static extern Int32 ReadConfig(UInt32 DevI,
                                     [Out] Byte[] IapCfgVal, //4字节
                                     [Out] Byte[] IspCfgVal, //4字节
                                     [Out] Byte[] CFlashCfgVal, //4字节
                                     [Out] UInt32 BootVer,
                                     [Out] Byte[] UUID);
            //取消操作
            [DllImport("WCHISPAPI.dll", EntryPoint = "WCH55x_StopOp")]
            public static extern void StopOpr();

            //手动关闭串口
            [DllImport("WCHISPAPI.dll", EntryPoint = "WCH55x_CloseDevice")]
            public static extern void CloseDev(UInt32 DevI);

            /// <summary>
            /// 通过USB搜索ISP下载设备，返回枚举的设备数。旧版BTV230，只支持一个设备枚举
            /// </summary>
            /// <param name="IspDevInfor">设备信息数组，[Out]传出，需提前实例化</param>
            /// <param name="MaxDevCnt">最大扫描设备数，需<=16</param>
            /// <param name="BtChipSeries">搜索到的最后一个设备芯片系列</param>
            /// <param name="BtChipType">搜索到的最后一个设备芯片型号</param>
            /// <param name="IsPreBTV230">（？）设备Bootloader低于BTV230</param>
            /// <returns>搜索到的设备数量</returns>
            [DllImport("WCHISPAPI.dll", EntryPoint = "WCH55x_EnumDevices", CallingConvention = CallingConvention.Winapi)]
            public static extern UInt32 EnumDev([Out] CHISPDeviceInfo[] IspDevInfor, Byte MaxDevCnt, out Byte BtChipSeries, out Byte BtChipType, out Int32 IsPreBTV230);

            /// <summary>
            /// 对片内固件与目标文件以及当前配置进行校验
            /// </summary>
            /// <param name="iIndex">设备序号</param>
            /// <param name="UserFileDataBuf">用户下载文件数据</param>
            /// <param name="UserFileDataLen">用户下载文件数据长度</param>
            /// <param name="UserFileDateType">用户下载文件格式 0:HEX格式,1:BIN格式</param>
            /// <param name="IAPFileDataBuf">IAP下载文件数据</param>
            /// <param name="IAPFileDataLen">IAP下载文件数据长度</param>
            /// <param name="IAPFileDateType">IAP文件格式 0:HEX格式,1:BIN格式</param>
            /// <returns>成功返回非0，失败返回0</returns>
            [DllImport("WCHISPAPI.dll", EntryPoint = "WCH55x_FlashVerifyB", CallingConvention = CallingConvention.Winapi)]
            public static extern Int32 FlashVerify(UInt32 iIndex, [In] Byte[] UserFileDataBuf, UInt32 UserFileDataLen, Byte UserFileDateType, [In] Byte[]? IAPFileDataBuf, UInt32 IAPFileDataLen, Byte IAPFileDateType);

            /// <summary>
            /// 烧写固件和配置选项
            /// </summary>
            /// <param name="iIndex">设备序号</param>
            /// <param name="UserFileDataBuf">用户下载文件数据</param>
            /// <param name="UserFileDataLen">用户下载文件数据长度</param>
            /// <param name="UserFileDateType">用户下载文件格式 0:HEX格式,1:BIN格式</param>
            /// <param name="IAPFileDataBuf">IAP下载文件数据</param>
            /// <param name="IAPFileDataLen">IAP下载文件数据长度</param>
            /// <param name="IAPFileDateType">IAP文件格式 0:HEX格式,1:BIN格式</param>
            /// <returns>成功返回非0，失败返回0</returns>
            [DllImport("WCHISPAPI.dll", EntryPoint = "WCH55x_FlashProgramB", CallingConvention = CallingConvention.Winapi)]
            public static extern Int32 FlashProgram(UInt32 iIndex, [In] Byte[] UserFileDataBuf, UInt32 UserFileDataLen, Byte UserFileDateType, [In] Byte[]? IAPFileDataBuf, UInt32 IAPFileDataLen, Byte IAPFileDateType);

            //OTP读函数
            [DllImport("WCHISPAPI.dll", EntryPoint = "WCH55x_ReadOTP", CallingConvention = CallingConvention.Winapi)]
            public static extern Int32 ReadOTP(UInt32 iIndex,        //设备序号
                                          Byte OffSet,        //OTP偏移值
                                          [Out] Byte[] DataBuf);  //OTP数据

            //OTP写函数
            [DllImport("WCHISPAPI.dll", EntryPoint = "WCH55x_WriteOTP", CallingConvention = CallingConvention.Winapi)]
            public static extern Int32 WriteOTP(UInt32 iIndex,        //设备序号
                                          Byte OffSet,        //OTP偏移值
                                          [In] Byte[] DataBuf);  //OTP数据

            //手动发送结束包
            [DllImport("WCHISPAPI.dll", EntryPoint = "WCH55x_IspEnd", CallingConvention = CallingConvention.Winapi)]
            public static extern Int32 ISPEnd(UInt32 iIndex,        //设备序号
                                          Int32 bIsDlReset);   //是否运行用户程序

            //CH57X启动仿真
            [DllImport("WCHISPAPI.dll", EntryPoint = "WCH55x_IspSumilat", CallingConvention = CallingConvention.Winapi)]
            public static extern Int32 EnableSimulation(UInt32 iIndex);       //设备序号


            //CH32Fx解除代码读保护
            [DllImport("WCHISPAPI.dll", EntryPoint = "WCH55x_IspRemoveProtect", CallingConvention = CallingConvention.Winapi)]
            public static extern Int32 RemoveProtect(UInt32 iIndex);       //设备序号


            //获取下载、校验操作的进度
            [DllImport("WCHISPAPI.dll", EntryPoint = "WCH55x_IspGetOptPrograss", CallingConvention = CallingConvention.Winapi)]
            public static extern UInt32 GetOprPrograss(UInt32 iIndex);      //设备序号

        }
    }
}
