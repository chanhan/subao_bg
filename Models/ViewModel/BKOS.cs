using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.ViewModel
{
    public class BKOS
    {
        public List<BKOSSchedules> schedules { get; set; }
        public List<BKOSAlliance> alliance { get; set; }
        public List<BKOSAlliance> allianceSelect { get; set; }
    }
}
