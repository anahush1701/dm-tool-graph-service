using GraphService.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphService.Context
{
    public class GraphServiceDbContext : DbContext
    {
        public GraphServiceDbContext(DbContextOptions<GraphServiceDbContext> options)
            : base(options)
        {
        }

        public DbSet<Graph> Graphs { get; set; }
        public DbSet<Node> Nodes { get; set; }
        public DbSet<Link> Links { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Define relationships and other schema rules here
            modelBuilder.Entity<Graph>().HasKey(g => g.Id);
            modelBuilder.Entity<Node>().HasKey(n => n.Id);
            modelBuilder.Entity<Link>().HasKey(l => l.Id);

            // This is important to handle the lists of nodes and links
            modelBuilder.Entity<Graph>()
                .HasMany(g => g.Nodes)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Graph>()
                .HasMany(g => g.Links)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
