using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EnovaCashFlowGeneratorApi
{
    [Authorize]
    [ApiController]
    public class GetTenantsController : ControllerBase
    {
        private IMediator _mediator;

        public GetTenantsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("api/GetTenants")]
        public async Task<List<string>> Get()
        {
            return await _mediator.Send(new QueryTenants());
        }
        public class QueryTenants : IRequest<List<string>>
        {
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Feature, string>();
            }
        }

        public class GetTenantsHandler : IRequestHandler<QueryTenants, List<string>>
        {
            private readonly IConnectionStringBuilder _builder;

            public GetTenantsHandler(IConnectionStringBuilder builder)
            {
                _builder = builder;
            }

            public async Task<List<string>> Handle(QueryTenants request, CancellationToken cancellationToken)
            {
                var connectionString = _builder.BuildConnectionEnova("Baza_Master");
                var dataContext = new DataContextEnova(connectionString);

                var result = await dataContext.Features // Client name instead of 'Client' below
                    .Where(d => d.DataKey == "Client" && d.ParentType == "DBItems" && d.Name == "Intercompany")
                    .Select(d => d.DbItem.Name)
                    .ToListAsync(cancellationToken);

                return result;
            }
        }
    }
}