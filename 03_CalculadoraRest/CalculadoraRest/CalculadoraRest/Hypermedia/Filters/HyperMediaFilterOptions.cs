using CalculadoraRest.Hypermedia.Abstract;
using System.Collections.Generic;

namespace CalculadoraRest.Hypermedia.Filters
{
    public class HyperMediaFilterOptions
    {
        public List<IResponseEnricher> ContentRespondeEnricherList { get; set; } = new List<IResponseEnricher>();
    }
}
