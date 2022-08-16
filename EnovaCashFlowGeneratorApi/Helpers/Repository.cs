using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EnovaCashFlowGeneratorApi
{
    public interface IRepository
    {
    }

    public class Repository<T> : IRepository where T : BaseModel, new()
    {
        private readonly DataContextSql db;
        private readonly DbSet<T> dbSet;

        public Repository(DataContextSql db)
        {
            this.db = db;
            this.dbSet = db.Set<T>();
        }

        public async Task<T> Create(T model, CancellationToken cancellationToken)
        {
            await dbSet.AddAsync(model, cancellationToken);
            await db.SaveChangesAsync(cancellationToken);

            return model;
        }

        public async Task<T> Update(T model, CancellationToken cancellationToken)
        {
            await db.SaveChangesAsync(cancellationToken);

            return model;
        }

        public async Task<T> GetByCreateDate(DateTime createDate, CancellationToken cancellationToken)
        {
            return await dbSet.FirstOrDefaultAsync(m => m.CreateDate == createDate, cancellationToken);
        }
    }
}
