using System.Runtime.Serialization;
using System.Text;

namespace EYChallenge.Domain.Product.ViewModels.Response
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class SuccessResponseViewModel<T> : SuccessResponseViewModel where T : class
    {


        /// <summary>
        /// Response any object necessary
        /// </summary>
        /// <value>Response any object necessary</value>
        [DataMember(Name = "data")]
        public T Data { get; set; }


        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class SuccessResponse {\n");
            sb.Append("  Message: ").Append(Message).Append("\n");
            sb.Append("  Data: ").Append(Data).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
    }
    public class SuccessResponseViewModel
    {
        /// <summary>
        /// Success message of application
        /// </summary>
        /// <value>Success message of application</value>
        [DataMember(Name = "message")]
        public string Message { get; set; }
    }
}
