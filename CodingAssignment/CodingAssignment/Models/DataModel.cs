using System.Collections.Generic;

namespace CodingAssignment.Models
{
    public class DataModel
    {
        public int Id { get; set; }

        public Dictionary<int, List<string>> Dictionary { get; set; }
    }
}