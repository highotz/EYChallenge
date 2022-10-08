using EYChallenge.Domain.Product.Constants;
using System.Runtime.Serialization;

namespace EYChallenge.Domain.Product.ViewModels.Common
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class PagedResponseViewModel<T> where T : class
    {
        /// <summary>
        /// A lista de equipamentos para a pesquisa, conforme parâmetros informados.
        /// </summary>
        /// <value>A lista de equipamentos para a pesquisa, conforme parâmetros informados.</value>
        [DataMember(Name = "data")]
        public IEnumerable<T> Data { get; set; }


        /// <summary>
        /// Tamanho da página de dados para a pesquisa, conforme parâmetros informados. O tamanho de página é configurado na instância no serviço. O tamanho de página não é parametrizável. 
        /// </summary>
        /// <value>Tamanho da página de dados para a pesquisa, conforme parâmetros informados. O tamanho de página é configurado na instância no serviço. O tamanho de página não é parametrizável. </value>
        [DataMember(Name = "pageSize")]
        public int PageSize { get; set; } = PagingSettings.PAGE_SIZE;

        /// <summary>
        /// Tamanho máximo da paginação
        /// </summary>
        /// <value>Tamanho da página de dados para a pesquisa, conforme parâmetros informados. O tamanho de página é configurado na instância no serviço. O tamanho de página não é parametrizável. </value>
        [DataMember(Name = "maxPageSize")]
        public int MaxPageSize
        {
            get
            {
                return PagingSettings.PAGE_SIZE;
            }
        }


        /// <summary>
        /// Quantidade total de recursos para a pesquisa, conforme parâmetros informados.
        /// </summary>
        /// <value>Quantidade total de recursos para a pesquisa, conforme parâmetros informados.</value>
        [DataMember(Name = "total")]
        public int Total { get; set; }

        [DataMember(Name = "totalPages")]
        public int TotalPages
        {
            get
            {
                return (int)Math.Ceiling(((decimal)Total / (decimal)PageSize));
            }
        }

    }
}
