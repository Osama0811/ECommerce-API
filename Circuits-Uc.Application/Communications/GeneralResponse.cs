using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CircuitsUc.Application.Communications
{
    public class GeneralResponse<T>
    {
        //[JsonProperty("ID")]
        //public Guid ID { get; set; }
        [JsonProperty("success")]
        public bool Success { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("status")]
        public HttpStatusCode Status { get; set; }
        [JsonProperty("resource")]
        public T Resource { get; set; }
        public int ResourceCount { get; set; }


        public GeneralResponse(T resource, string message = "", int Count = 0)
        {
            Success = true;
            Message = message;
            Resource = resource;
            ResourceCount = Count;
            Status = HttpStatusCode.OK;
        }

        public GeneralResponse(string message, HttpStatusCode statusCode)
        {
            Success = false;
            Message = message;
            Resource = default;
            Status = statusCode;
        }


    }
}
