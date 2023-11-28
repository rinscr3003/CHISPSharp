using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CHISP
{
    public class ISPException
    {
        /// <summary>
        /// ISP参数无效异常
        /// </summary>
        public class InvalidISPParamException : System.Exception
        {
            public InvalidISPParamException() { }
            public InvalidISPParamException(string message)
                : base(message) { throw new Exception(message); }
            public InvalidISPParamException(string message, Exception innerException)
                : base(message, innerException) { }
        }

        /// <summary>
        /// 传入参数无效异常
        /// </summary>
        public class InvalidFuncParamException : System.Exception
        {
            public InvalidFuncParamException() { }
            public InvalidFuncParamException(string message)
                : base(message) { throw new Exception(message); }
            public InvalidFuncParamException(string message, Exception innerException)
                : base(message, innerException) { }
        }

        /// <summary>
        /// 文件内容无效异常
        /// </summary>
        public class InvalidFileContentException : System.Exception
        {
            public InvalidFileContentException() { }
            public InvalidFileContentException(string message)
                : base(message) { }
            public InvalidFileContentException(string message, Exception innerException)
                : base(message, innerException) { }
        }
    }
}
