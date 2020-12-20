using System.Collections.Generic;

#nullable disable

namespace Mindbox.Database.Sqlite.Data
{
    public partial class Figure
    {
        public long Id { get; set; }
        public long Type { get; set; }
        public double Area { get; set; }

        public virtual Circle Circle { get; set; }
        public virtual Triangle Triangle { get; set; }
    }
}
