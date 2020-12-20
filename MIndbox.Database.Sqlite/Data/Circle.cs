#nullable disable

namespace Mindbox.Database.Sqlite.Data
{
    public partial class Circle
    {
        public long Idcircle { get; set; }
        public double Radius { get; set; }

        public virtual Figure IdcircleNavigation { get; set; }
    }
}
