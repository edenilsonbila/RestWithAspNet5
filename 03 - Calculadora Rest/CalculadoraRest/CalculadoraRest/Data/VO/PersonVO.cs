using CalculadoraRest.Hypermedia;
using CalculadoraRest.Hypermedia.Abstract;
using System.Collections.Generic;

namespace CalculadoraRest.Data.VO
{

    public class PersonVO : ISupportHyperMedia
    {
        public long Id { get; set; }
        public string FirtName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string Gender { get; set; }
        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}
