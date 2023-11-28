using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using static System.Net.WebRequestMethods;

namespace CHISP
{
    public static class BinHelper
    {

        public static byte Str2Byte(string c)
        {
            return (byte)Convert.ToInt32(c, 16);
        }

        public static byte[] HexToBin(string[] hexLines)
        {
            if (hexLines == null)
                throw new ISPException.InvalidFuncParamException("传入的Hex行数组是Null。");
            if (hexLines.Length == 0)
                throw new ISPException.InvalidFuncParamException("传入的Hex行数组是空的。");
            MemoryStream stream = new MemoryStream();
             for (int i = 0; i < hexLines.Length; i++)
            {
                string line = hexLines[i];
                if (line.Length < 9) // 一行起码9个字符，这都没有肯定是有问题的
                    throw new ISPException.InvalidFileContentException(string.Format("Hex文件第{0:D}行内容长度不足。", i));
                if (line[0] != ':') //一行必然0x3A开头
                    throw new ISPException.InvalidFileContentException(string.Format("Hex文件第{0:D}行起始标记不存在。", i));

                byte[] lineBuf = new byte[256];
                int len = Str2Byte(line.Substring(1, 2));
                byte checksum = (byte)len;
                for (int ptr = 0; ptr < len + 4; ptr++)
                {
                    // 读入每一行的数据
                    lineBuf[ptr] = Str2Byte(line.Substring(3 + 2 * ptr, 2));
                    checksum += lineBuf[ptr];
                }
                if (checksum != 0)   // 校验和错
                    throw new ISPException.InvalidFileContentException(string.Format("Hex文件第{0:D}行校验和错误。", i));

                int extAddr = 0;          // 扩展地址
                int segAddr = 0;          // 扩展段地址
                int offsetAddr;          // 偏移地址
                int realAddr, startAddr = 0;         // 写入地址
                int firstOffset = -1;
                bool hexEnd = false;

                //数据类型分析
                switch (lineBuf[2])
                {
                    case 0: // 数据记录
                        offsetAddr = lineBuf[0] * 256 + lineBuf[1]; // 拼合当前行的偏移地址
                        realAddr = extAddr * 65536 + segAddr * 16 + offsetAddr; // 拼合实际写入位置的地址
                        if (firstOffset == -1)
                        {
                            firstOffset = realAddr;
                        }
                        if (realAddr >= 0x08000000)
                        {
                            if (firstOffset != 0)
                                realAddr -= firstOffset;
                            else
                                realAddr -= (extAddr * 65536);
                        }
                        if (offsetAddr != 0)
                        {
                            if (realAddr < startAddr)
                                throw new ISPException.InvalidFileContentException(string.Format("Hex文件第{0:D}行写入的地址错误。", i));
                            realAddr -= startAddr;
                            if (realAddr > 800 * 1024)
                                throw new ISPException.InvalidFileContentException(string.Format("Hex文件第{0:D}行写入的地址超出800KB。", i));
                        }
                        stream.Seek(realAddr, SeekOrigin.Begin);
                        stream.Write(lineBuf.Skip(3).ToArray(), 0, len);
                        stream.Flush();
                        break;
                    case 2: // 扩展段地址记录
                        segAddr = lineBuf[3] * 256 + lineBuf[4];
                        break;
                    case 4: // 扩展线性地址记录
                        extAddr = lineBuf[3] * 256 + lineBuf[4];
                        break;
                    case 5: // 开始线性地址记录
                    case 3: // 开始段地址记录
                        break;
                    case 1:  // HEX文件结束标志
                        hexEnd = true;
                        break;
                    default:
                        throw new ISPException.InvalidFileContentException(string.Format("Hex文件第{0:D}行是不能识别的记录类型。", i));
                }
                if (hexEnd) break;
            }
            return stream.ToArray();
        }

        public static int GetIapStartAddrFromHex(string[] hexLines, bool isC51)
        {
            if (hexLines == null)
                throw new ISPException.InvalidFuncParamException("传入的Hex行数组是Null。");
            if (hexLines.Length == 0)
                throw new ISPException.InvalidFuncParamException("传入的Hex行数组是空的。");

            int iapStartAddr = 0;
            for (int i = 0; i < hexLines.Length; i++)
            {
                string line = hexLines[i];
                if (line.Length < 9) // 一行起码9个字符，这都没有肯定是有问题的
                    throw new ISPException.InvalidFileContentException(string.Format("Hex文件第{0:D}行内容长度不足。", i));
                if (line[0] != ':') //一行必然0x3A开头
                    throw new ISPException.InvalidFileContentException(string.Format("Hex文件第{0:D}行起始标记不存在。", i));

                byte[] lineBuf = new byte[256];
                int len = Str2Byte(line.Substring(1, 2));
                byte checksum = (byte)len;
                for (int ptr = 0; ptr < len + 4; ptr++)
                {
                    // 读入每一行的数据
                    lineBuf[ptr] = Str2Byte(line.Substring(3 + 2 * ptr, 2));
                    checksum += lineBuf[ptr];
                }
                if (checksum != 0)   // 校验和错
                    throw new ISPException.InvalidFileContentException(string.Format("Hex文件第{0:D}行校验和错误。", i));

                int extAddr = 0;          // 扩展地址
                int segAddr = 0;          // 扩展段地址
                int offsetAddr;          // 偏移地址
                int realAddr;         // 写入地址
                bool hexEnd = false;

                //数据类型分析
                switch (lineBuf[2])
                {
                    case 0: // 数据记录
                        offsetAddr = lineBuf[0] * 256 + lineBuf[1]; // 拼合当前行的偏移地址
                        realAddr = extAddr * 65536 + segAddr * 16 + offsetAddr; // 拼合实际写入位置的地址
                        if (iapStartAddr == 0)
                            iapStartAddr = realAddr;
                        else if ((iapStartAddr > realAddr) && (realAddr > 0))
                            iapStartAddr = realAddr;
                        break;
                    case 2: // 扩展段地址记录
                        segAddr = lineBuf[3] * 256 + lineBuf[4];
                        break;
                    case 4: // 扩展线性地址记录
                        extAddr = lineBuf[3] * 256 + lineBuf[4];
                        break;
                    case 5:  //开始线性地址记录
                    case 3: //开始段地址记录
                        break;
                    case 1:  // HEX文件结束标志
                        if (isC51)
                            iapStartAddr -= 4;
                        hexEnd = true;
                        break;
                    default:
                        throw new ISPException.InvalidFileContentException(string.Format("Hex文件第{0:D}行是不能识别的记录类型。", i));
                }
                if (hexEnd) break;
            }
            return iapStartAddr;
        }

        public static byte[] IAPHexToBin(string[] hexLines, bool isC51, ref int baseAddr)
        {
            if (hexLines == null)
                throw new ISPException.InvalidFuncParamException("传入的Hex行数组是Null。");
            if (hexLines.Length == 0)
                throw new ISPException.InvalidFuncParamException("传入的Hex行数组是空的。");

            int iapStartAddr = GetIapStartAddrFromHex(hexLines, isC51);
            if (iapStartAddr != 0)
                baseAddr = iapStartAddr;
            MemoryStream stream = new MemoryStream();
             for (int i = 0; i < hexLines.Length; i++)
            {
                string line = hexLines[i];
                if (line.Length < 9) // 一行起码9个字符，这都没有肯定是有问题的
                    throw new ISPException.InvalidFileContentException(string.Format("Hex文件第{0:D}行内容长度不足。", i));
                if (line[0] != ':') //一行必然0x3A开头
                    throw new ISPException.InvalidFileContentException(string.Format("Hex文件第{0:D}行起始标记不存在。", i));

                byte[] lineBuf = new byte[256];
                int len = Str2Byte(line.Substring(1, 2));
                byte checksum = (byte)len;
                for (int ptr = 0; ptr < len + 4; ptr++)
                {
                    // 读入每一行的数据
                    lineBuf[ptr] = Str2Byte(line.Substring(3 + 2 * ptr, 2));
                    checksum += lineBuf[ptr];
                }
                if (checksum != 0)   // 校验和错
                    throw new ISPException.InvalidFileContentException(string.Format("Hex文件第{0:D}行校验和错误。", i));

                int extAddr = 0;          // 扩展地址
                int segAddr = 0;          // 扩展段地址
                int offsetAddr;          // 偏移地址
                int realAddr;         // 写入地址
                bool hexEnd = false;

                //数据类型分析
                switch (lineBuf[2])
                {
                    case 0: // 数据记录
                        offsetAddr = lineBuf[0] * 256 + lineBuf[1]; // 拼合当前行的偏移地址
                        realAddr = extAddr * 65536 + segAddr * 16 + offsetAddr; // 拼合实际写入位置的地址
                        if (!isC51 && iapStartAddr == 0)
                            iapStartAddr = realAddr;
                        else if (offsetAddr != 0)
                        {
                            if (realAddr <= baseAddr)
                                throw new ISPException.InvalidFileContentException(string.Format("Hex文件第{0:D}行写入的地址错误。", i));
                            realAddr -= baseAddr;
                        }
                        stream.Seek(realAddr, SeekOrigin.Begin);
                        stream.Write(lineBuf.Skip(3).ToArray(), 0, len);
                        break;
                    case 2: // 扩展段地址记录
                        segAddr = lineBuf[3] * 256 + lineBuf[4];
                        break;
                    case 4: // 扩展线性地址记录
                        extAddr = lineBuf[3] * 256 + lineBuf[4];
                        break;
                    case 5:  //开始线性地址记录
                    case 3: //开始段地址记录
                        break;
                    case 1:  // HEX文件结束标志
                        hexEnd = true;
                        break;
                    default:
                        throw new ISPException.InvalidFileContentException(string.Format("Hex文件第{0:D}行是不能识别的记录类型。", i));
                }
                if (hexEnd) break;
            }
            return stream.ToArray();
        }

/*        public static byte[] MergeFiles()
        {

        }*/
    }
}
