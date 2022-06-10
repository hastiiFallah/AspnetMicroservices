using Microsoft.EntityFrameworkCore;
using OrderingDomain.Common;
using OrderingDomain.Moddels;

namespace OrderingInfrastructure.Persistence
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> opt):base(opt)
        {

        }
        public DbSet<Order> orders { get; set; }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<ModelBase>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedDate = DateTime.Now;
                        entry.Entity.CreatedBy = "swn";
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModifiedDate = DateTime.Now;
                        entry.Entity.LastModified = "swn";
                        break;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
