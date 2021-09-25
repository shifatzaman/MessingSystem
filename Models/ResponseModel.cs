using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MessingSystem.Models
{
    public class ResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public ResponseModel CreateSuccessRespone(object data, string message)
        {
            return new ResponseModel
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        public ResponseModel CreatePermissionGrantedResponse()
        {
            return new ResponseModel
            {
                Success = true,
                Message = "Permission Granted"
            };
        }
    }
}
