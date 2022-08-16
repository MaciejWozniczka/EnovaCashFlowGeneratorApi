using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace EnovaCashFlowGeneratorApi
{
    public class ReportRepository : Repository<ReportModel>, IReportRepository
    {
        private readonly DataContextSql db;
        private readonly DbSet<ReportModel> dbSet;

        public ReportRepository(DataContextSql db) : base(db)
        {
            this.db = db;
            this.dbSet = db.Set<ReportModel>();
        }

        public async Task<List<ReportModel>> GetAllReports(CancellationToken cancellationToken)
        {
            return await dbSet.ToListAsync(cancellationToken);
        }
    }
    public interface IReportRepository
    {
        Task<ReportModel> Create(ReportModel model, CancellationToken cancellationToken);
        Task<ReportModel> Update(ReportModel model, CancellationToken cancellationToken);
        Task<ReportModel> GetByCreateDate(DateTime createDate, CancellationToken cancellationToken);
        Task<List<ReportModel>> GetAllReports(CancellationToken cancellationToken);
    }
}
