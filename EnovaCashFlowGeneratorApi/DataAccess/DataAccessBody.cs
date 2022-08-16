using MediatR;
using System;

namespace EnovaCashFlowGeneratorApi
{
    public class DataAccessBody : IRequest<string>
    {
        public string DbName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime YearToDate { get; set; }
    }
}
