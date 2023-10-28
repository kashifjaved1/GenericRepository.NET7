using GenericRepository.Demo.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GenericRepository.Demo.Context
{
    /// <summary>
    /// Sample Database Context
    /// </summary>
    public class ApiDbContext : DbContext
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="options">
        /// The options to be used by a Microsoft.EntityFrameworkCore.DbContext. You normally
        ///     override Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)
        ///     or use a Microsoft.EntityFrameworkCore.DbContextOptionsBuilder to create instances
        ///     of this class and it is not designed to be directly constructed in your application
        ///     code.
        /// </param>
        public ApiDbContext(DbContextOptions options) : base(options)
        {
        }

        protected ApiDbContext()
        {
        }

        public virtual DbSet<Author> Authors { get; set; }

        public virtual DbSet<Book> Books { get; set; }

        public virtual DbSet<Article> Articles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Author>(entity =>
            {
                entity
                    .HasMany(m => m.Books)
                    .WithOne(o => o.Author)
                    .HasForeignKey(fk => fk.AuthorId);

                entity
                    .HasMany(a => a.Articles)
                    .WithOne(a => a.Author)
                    .HasForeignKey(fk => fk.AuthorId);
            });

            modelBuilder.Entity<Book>(entity =>
            {
                entity
                    .HasOne(o => o.Author)
                    .WithMany(m => m.Books)
                    .HasForeignKey(fk => fk.AuthorId);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
