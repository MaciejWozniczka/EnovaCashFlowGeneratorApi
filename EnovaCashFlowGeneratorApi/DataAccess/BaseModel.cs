using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnovaCashFlowGeneratorApi
{
    public class BaseModel
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
