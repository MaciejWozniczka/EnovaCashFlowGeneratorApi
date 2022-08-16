using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EnovaCashFlowGeneratorApi
{
    [Authorize]
    [ApiController]
    public class GetWplatyWypatyCheckController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GetWplatyWypatyCheckController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("api/WplatyWyplatyCheck")]
        public async Task<string> Get([FromQuery] GetWplatyWypatyCheckCommand dataAccessBody)
        {
            return (string)await _mediator.Send(dataAccessBody);
        }

        public class GetWplatyWypatyCheckCommand : IRequest<string>
        {
            public string DbName { get; set; }
            public DateTime FromDate { get; set; }
            public DateTime ToDate { get; set; }
            public DateTime YearToDate { get; set; }
        }

        public class GetWplatyWypatyCheckQueryHandler : IRequestHandler<GetWplatyWypatyCheckCommand, string>
        {
            private readonly IConnectionStringBuilder _builder;
            private readonly IGetWplatyWyplatyCheckService _service;

            public GetWplatyWypatyCheckQueryHandler(IConnectionStringBuilder builder, IGetWplatyWyplatyCheckService service)
            {
                _builder = builder;
                _service = service;
            }

            public Task<string> Handle(GetWplatyWypatyCheckCommand request, CancellationToken cancellationToken)
            {
                var connectionString = _builder.BuildConnectionEnova(request.DbName);
                var output = _service.GetWplatyWyplatyCheck(request, connectionString);

                return output;
            }
        }
    }
}