using GraphService.Context;
using GraphService.Interfaces;
using GraphService.Models;
using Microsoft.EntityFrameworkCore;

namespace GraphService.Repositories
{
    public class GraphRepository : IGraphRepository
    {
        private GraphServiceDbContext _context;

        public GraphRepository(GraphServiceDbContext context) => _context = context;

        public void AddGraph(Graph graph)
        {
            _context.Graphs.Add(graph);
        }

        public void DeleteGraph(int id)
        {
            _context.Graphs.Remove(new Graph { Id = id });
        }

        public async Task<Graph> GetGraphAsync(int id)
        {
            return await _context.Graphs
                .Include(g => g.Nodes)
                .Include(g => g.Links)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Graph>> GetGraphsAsync()
        {
            return await _context.Graphs
                .Include(g => g.Nodes)
                .Include(g => g.Links)
                .ToListAsync();
        }

        public async Task<IEnumerable<Graph>> GetGraphsPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.Graphs
                .Include(g => g.Nodes)
                .Include(g => g.Links)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGraphAsync(Graph updatedGraph)
        {
            // 1. Fetch the existing graph with all related data
            var existingGraph = await _context.Graphs
                .Include(g => g.Nodes)
                .Include(g => g.Links)
                .FirstOrDefaultAsync(g => g.Id == updatedGraph.Id);

            if (existingGraph == null)
                throw new KeyNotFoundException($"Graph with Id {updatedGraph.Id} not found.");

            // 2. Update scalar properties of the root entity
            _context.Entry(existingGraph).CurrentValues.SetValues(updatedGraph);

            // 3. Clear existing collections to remove old relationships
            // This is the key step. It will delete all child entities that aren't re-added.
            _context.Links.RemoveRange(existingGraph.Links);
            _context.Nodes.RemoveRange(existingGraph.Nodes);

            // 4. Re-add new collections from the updated graph
            // The new nodes and links will be added with their correct relationships
            foreach (var node in updatedGraph.Nodes)
            {
                existingGraph.Nodes.Add(node);
            }

            foreach (var link in updatedGraph.Links)
            {
                existingGraph.Links.Add(link);
            }

            // 5. Save all changes at once
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Handle the concurrency exception gracefully
                // e.g., log it, re-load the entity, and retry the update
                // You can get more details from ex.Entries
                throw new Exception("Concurrency conflict detected. The graph was updated by another process.", ex);
            }
        }
    }
}
