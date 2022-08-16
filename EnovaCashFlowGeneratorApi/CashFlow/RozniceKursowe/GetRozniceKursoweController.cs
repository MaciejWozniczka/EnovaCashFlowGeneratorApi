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

    public class GetRozniceKursoweController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GetRozniceKursoweController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("api/RozniceKursowe")]
        public async Task<string> Get([FromQuery] GetRozniceKursoweCommand dataAccessBody)
        {
            return (string)await _mediator.Send(dataAccessBody);
        }

        public class GetRozniceKursoweCommand : IRequest<string>
        {
            public string DbName { get; set; }
            public DateTime ToDate { get; set; }
        }

        public class GetRozniceKursoweQueryHandler : IRequestHandler<GetRozniceKursoweCommand, string>
        {
            private readonly IConnectionStringBuilder _builder;
            private readonly IGetRozniceKursoweService _service;

            public GetRozniceKursoweQueryHandler(IConnectionStringBuilder builder, IGetRozniceKursoweService service)
            {
                _builder = builder;
                _service = service;
            }

            public Task<string> Handle(GetRozniceKursoweCommand request, CancellationToken cancellationToken)
            {
                var connectionString = _builder.BuildConnectionEnova(request.DbName);
                var output = _service.GetRozniceKursowe(request, connectionString);

                return output;
            }
        }
    }
}