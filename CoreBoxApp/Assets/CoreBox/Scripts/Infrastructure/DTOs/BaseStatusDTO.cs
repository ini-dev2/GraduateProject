using System;

namespace CoreBox
{
    internal class BaseStatusDTO
    {
        public bool IsSuccess = true;
        public string StatusMessage;

        public BaseStatusDTO()
        {
        }

        public BaseStatusDTO(bool isSuccess, string statusMessage)
        {
            IsSuccess = isSuccess;
            StatusMessage = statusMessage ?? throw new ArgumentNullException(nameof(statusMessage));
        }
    }
}