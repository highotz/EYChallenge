using System.Runtime.Serialization;

namespace EYChallenge.Utilities.SystemObjects.Entities
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class BatchViewModel<T> where T : class
    {
        /// <summary>
        /// A lista de equipamentos para a pesquisa, conforme parâmetros informados.
        /// </summary>
        /// <value>A lista de equipamentos para a pesquisa, conforme parâmetros informados.</value>
        [DataMember(Name = "data")]
        public IEnumerable<T> Data { get; set; }

    }
}
