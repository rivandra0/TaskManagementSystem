using System;

namespace TaskManagementSystem.Models
{
    public class AppHttpException : Exception
    {
        public int Code { get; }

        public AppHttpException(int code, string message) : base(message)
        {
            Code = code;
        }
    }
}
