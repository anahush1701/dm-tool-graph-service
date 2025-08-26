using GraphService.Models;

namespace GraphService.Interfaces
{
    public interface IGraphRepository
    {
        public Task<IEnumerable<Graph>> GetGraphsAsync();
        public Task<IEnumerable<Graph>> GetGraphsPagedAsync(int pageNumber, int pageSize);
        public Task<Graph> GetGraphAsync(int id);
        public void AddGraph(Graph graph);
        public void DeleteGraph(int id);
        public Task UpdateGraphAsync(Graph graph);
        public Task SaveChangesAsync();
    }
}
