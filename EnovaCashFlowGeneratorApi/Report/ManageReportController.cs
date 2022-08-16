using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace EnovaCashFlowGeneratorApi
{
    [Authorize]
    [ApiController]
    public class ManageReportController : ControllerBase
    {
        private readonly IMediator mediator;

        public ManageReportController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("/api/Report")]
        public async Task<IActionResult> Post(ReportDto command)
        {
            return await mediator.Send(command).Process();
        }

        public class ReportDto : IRequest<Result<Guid>>
        {
            [JsonIgnore]
            public Guid Id { get; set; }
            public DateTime CreateDate { get; set; }
            public int TenantId { get; set; }
            public int ReportId { get; set; }
            public string Data { get; set; }
        }

        public class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<ReportDto, ReportModel>();
            }
        }
        public class ManageTenantQueryHandler : IRequestHandler<ReportDto, Result<Guid>>
        {
            private readonly IMapper mapper;
            private readonly IReportRepository repository;

            public ManageTenantQueryHandler(IMapper mapper, IReportRepository repository)
            {
                this.mapper = mapper;
                this.repository = repository;
            }

            public async Task<Result<Guid>> Handle(ReportDto request, CancellationToken cancellationToken)
            {
                ReportModel report;

                bool isAdding = request.Id == Guid.Empty;

                if (isAdding)
                {
                    report = new ReportModel();
                }
                else
                {
                    report = await repository.GetByCreateDate(request.CreateDate, cancellationToken);

                    if (report == null)
                    {
                        return Result.NotFound<Guid>(request.CreateDate);
                    }
                }

                report.Data = JsonSerializer.Deserialize<string>(report.Data)!;

                report = mapper.Map(request, report);

                if (isAdding)
                {
                    await repository.Create(report, cancellationToken);
                }
                else
                {
                    await repository.Update(report, cancellationToken);
                }

                return Result.Ok(report.Id);
            }
        }
    }
}