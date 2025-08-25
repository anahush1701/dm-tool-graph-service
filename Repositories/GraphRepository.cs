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
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        public async Task<IEnumerable<Graph>> GetGraphsAsync()
        {
            return await _context.Graphs.ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGraphAsync(Graph updatedGraph)
        {
            // 1. Load the existing graph with its nodes and links
            var existingGraph = await _context.Graphs
                .Include(g => g.Nodes)
                .Include(g => g.Links)
                .FirstOrDefaultAsync(g => g.Id == updatedGraph.Id);

            if (existingGraph == null)
                throw new KeyNotFoundException($"Graph with Id {updatedGraph.Id} not found.");

            // 2. Update scalar properties
            _context.Entry(existingGraph).CurrentValues.SetValues(updatedGraph);

            // 3. Update Nodes
            UpdateCollection(existingGraph.Nodes, updatedGraph.Nodes);

            // 4. Update Links
            UpdateCollection(existingGraph.Links, updatedGraph.Links);

            // 5. Save changes
            await _context.SaveChangesAsync();
        }

        // Helper method for updating collections
        private void UpdateCollection<T>(ICollection<T> existing, ICollection<T> updated) where T : class
        {
            // Remove items not in updated
            var updatedIds = updated.Select(e => (int)_context.Entry(e).Property("Id").CurrentValue).ToList();
            var toRemove = existing.Where(e => !updatedIds.Contains((int)_context.Entry(e).Property("Id").CurrentValue)).ToList();
            foreach (var entity in toRemove)
                existing.Remove(entity);

            // Add or update items
            foreach (var updatedEntity in updated)
            {
                var id = (int)_context.Entry(updatedEntity).Property("Id").CurrentValue;
                var existingEntity = existing.FirstOrDefault(e => (int)_context.Entry(e).Property("Id").CurrentValue == id);
                if (existingEntity == null)
                {
                    // Attach if not tracked
                    if (_context.Entry(updatedEntity).State == EntityState.Detached)
                        _context.Attach(updatedEntity);
                    existing.Add(updatedEntity);
                }
                else
                {
                    _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
                }
            }
        }
    }
}
