using System.ComponentModel.DataAnnotations.Schema;

namespace EnovaCashFlowGeneratorApi
{
    public class Feature
    {
        [ForeignKey("DbItem")]
        public int Parent { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
        public string DataKey { get; set; }
        public string ParentType { get; set; }
        public DbItem DbItem { get; set; }
    }

    public class DbItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}