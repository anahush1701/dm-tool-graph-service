using GraphService.Interfaces;
using GraphService.Models;
using Microsoft.AspNetCore.Mvc;

namespace GraphService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GraphController : ControllerBase
    {
        private readonly IGraphRepository _repository;
        public GraphController(IGraphRepository repository) => _repository = repository;

        [HttpGet("get", Name = "getAllGraphs")]
        public async Task<IEnumerable<Graph>> GetAllGraphs()
        {
            return await _repository.GetGraphsAsync();
        }

        [HttpGet("getPaged", Name = "getAllGraphsPaged")]
        public async Task<IEnumerable<Graph>> GetAllGraphs(int pageNumber, int pageSize)
        {
            return await _repository.GetGraphsPagedAsync(pageNumber, pageSize);
        }

        [HttpPost("create", Name = "createGraph")]
        public async Task<ActionResult<Graph>> CreateGraph([FromBody] Graph graph)
        {
            if (graph == null)
            {
                return BadRequest("Graph cannot be null");
            }

            _repository.AddGraph(graph);
            await _repository.SaveChangesAsync();

            return new ActionResult<Graph>(graph);
        }

        [HttpGet("{id}", Name = "getGraph")]
        public async Task<Graph> GetGraph(int id)
        {
            return await _repository.GetGraphAsync(id);
        }

        [HttpDelete("{id}", Name = "deleteGraph")]
        public async Task DeleteAsync(int id)
        {
            _repository.DeleteGraph(id);
            await _repository.SaveChangesAsync();
        }

        [HttpPut("{id}", Name = "updateGraph")]
        public async Task<ActionResult<Graph>> UpdateGraph(int id, [FromBody] Graph graph)
        {
            if (graph == null || graph.Id != id)
            {
                return BadRequest("Graph is null or ID mismatch.");
            }

            try
            {
                await _repository.UpdateGraphAsync(graph);
                await _repository.SaveChangesAsync();
                return new ActionResult<Graph>(graph);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}
