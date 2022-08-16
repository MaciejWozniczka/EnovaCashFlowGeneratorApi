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
    public class GetWplatyWyplatyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GetWplatyWyplatyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("api/WplatyWyplaty")]
        public async Task<string> Get([FromQuery] GetWplatyWyplatyCommand dataAccessBody)
        {
            return (string)await _mediator.Send(dataAccessBody);
        }

        public class GetWplatyWyplatyCommand : IRequest<string>
        {
            public string DbName { get; set; }
            public DateTime ToDate { get; set; }
        }

        public class GetWplatyWyplatyQueryHandler : IRequestHandler<GetWplatyWyplatyCommand, string>
        {
            private readonly IConnectionStringBuilder _builder;
            private readonly IGetWplatyWyplatyService _service;

            public GetWplatyWyplatyQueryHandler(IConnectionStringBuilder builder, IGetWplatyWyplatyService service)
            {
                _builder = builder;
                _service = service;
            }

            public Task<string> Handle(GetWplatyWyplatyCommand request, CancellationToken cancellationToken)
            {
                var connectionString = _builder.BuildConnectionEnova(request.DbName);
                var output = _service.GetWplatyWyplaty(request, connectionString);

                return output;
            }
        }
    }
}