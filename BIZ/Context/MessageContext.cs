using Microsoft.EntityFrameworkCore;

namespace BIZ.Context
{
    public class MessageContext : DbContext
    {
        public MessageContext(DbContextOptions options) : base(options) { }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<Message>(entity =>
                {
                    entity.HasKey(key => key.MessageID);
                    entity.Property(p => p.Text).IsRequired();
                    entity.Property(p => p.Created).HasDefaultValueSql("GetUtcDate()");
                });
        }
    }
}
