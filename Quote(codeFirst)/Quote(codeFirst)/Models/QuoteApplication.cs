namespace Quote_codeFirst_
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class QuoteApplication : DbContext
    {
        public QuoteApplication()
            : base("name=QuoteApplication")
        {
        }

        public virtual DbSet<Quote> Quotes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Quote>()
                .Property(e => e.FirstName)
                .IsUnicode(false);

            modelBuilder.Entity<Quote>()
                .Property(e => e.LastName)
                .IsUnicode(false);

            modelBuilder.Entity<Quote>()
                .Property(e => e.EmailAddress)
                .IsUnicode(false);

            modelBuilder.Entity<Quote>()
                .Property(e => e.DateOfBirth)
                .IsUnicode(false);

            modelBuilder.Entity<Quote>()
                .Property(e => e.CarMake)
                .IsUnicode(false);

            modelBuilder.Entity<Quote>()
                .Property(e => e.CarModel)
                .IsUnicode(false);

            modelBuilder.Entity<Quote>()
                .Property(e => e.DUI)
                .IsUnicode(false);

            modelBuilder.Entity<Quote>()
                .Property(e => e.Coverage)
                .IsUnicode(false);
        }
    }
}
