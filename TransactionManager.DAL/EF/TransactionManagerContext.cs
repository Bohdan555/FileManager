using TransactionManager.DAL.Entities;
using TransactionManager.DAL.Extensions;
using Microsoft.EntityFrameworkCore;

namespace TransactionManager.DAL.EF
{
    public class TransactionManagerContext : DbContext
    {        
        public DbSet<Status> Statuses { get; set; }
        public DbSet<Transaction> Transactions { get; set; }        

        public TransactionManagerContext(DbContextOptions<TransactionManagerContext> options)
           : base(options)
        {            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.Property(p => p.Id)                    
                    .UseIdentityColumn()
                    .IsRequired()
                    .HasColumnName("Id");

                entity.Property(p => p.TransactionId)
                    .HasMaxLength(50)
                    .IsRequired()
                    .HasColumnName("TransactionId");

                entity.Property(p => p.Amount)
                    .IsRequired()
                    .HasColumnName("Amount");

                entity.Property(p => p.CurrencyCode)
                    .IsRequired()
                    .HasColumnName("CurrencyCode");

                entity.Property(p => p.TransactionDate)
                    .IsRequired()
                    .HasColumnName("TransactionDate");                
            });

            modelBuilder.Entity<Transaction>()
                .HasOne(t => t.Status)
                .WithMany()
                .HasForeignKey(k => k.StatusId);                           

            modelBuilder.Entity<Status>()
                .HasData(new Status 
                {
                    Id = 1,
                    Name = Enums.Status.Approved.GetDescription()
                },
                new Status 
                {
                    Id = 2,
                    Name = Enums.Status.Done.GetDescription()
                },
                new Status
                { 
                    Id = 3,
                    Name = Enums.Status.Failed.GetDescription()
                },
                new Status
                { 
                    Id = 4,
                    Name = Enums.Status.Finished.GetDescription()
                },
                new Status
                { 
                    Id = 5,
                    Name = Enums.Status.Rejected.GetDescription()
                });            
        }      
    }
}
