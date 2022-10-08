using System.Runtime.Serialization;

namespace EYChallenge.Domain.Product.ViewModels.Response
{
    [DataContract]
    public class ErrorResponseViewModel
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "data")]
        public string Message { get; set; }

    }
}
