#nullable disable

namespace Mindbox.Database.Sqlite.Data
{
    public partial class Triangle
    {
        public long Id { get; set; }
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        public virtual Figure IdNavigation { get; set; }
    }
}
