using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace EnovaCashFlowGeneratorApi
{
    [Authorize]
    [ApiController]
    public class GetReport : ControllerBase
    {
        private readonly IMediator _mediator;

        public GetReport(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("/api/GetReport")]
        public async Task<IActionResult> Get(DateTime createDate, int reportId) => await _mediator.Send(new GetReportQuery(createDate, reportId)).Process();

        public class GetReportQuery : IRequest<Result<List<ReportDto>>>
        {
            public GetReportQuery(DateTime createDate, int reportId)
            {
                CreateDate = createDate;
                ReportId = reportId;
            }

            public int ReportId { get; set; }
            public DateTime CreateDate { get; set; }
        }
        public class ReportDto
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
            public MappingProfile() => CreateMap<ReportDto, ReportModel>();
        }

        public class GetReportQueryHandler : IRequestHandler<GetReportQuery, Result<List<ReportDto>>>
        {
            private readonly DataContextSql _db;

            public GetReportQueryHandler(DataContextSql db)
            {
                _db = db;
            }

            public async Task<Result<List<ReportDto>>> Handle(GetReportQuery request, CancellationToken cancellationToken)
            {
                var result = await _db.Reports
                    .Where(r => r.CreateDate == request.CreateDate && r.ReportId == request.ReportId)
                    .Select(r => new ReportDto { CreateDate = r.CreateDate, Data = r.Data, Id = r.Id, ReportId = r.ReportId, TenantId = r.TenantId })
                    .ToListAsync(cancellationToken);

                if (result == null)
                    return Result.NotFound<List<ReportDto>>(request.CreateDate);

                return Result.Ok(result);
            }
        }
    }
}