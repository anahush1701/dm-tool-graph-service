namespace GraphService.Models
{
    public class Graph
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Node> Nodes { get; set; }
        public List<Link> Links { get; set; }
        public Guid UserId { get; set; }
    }
}
