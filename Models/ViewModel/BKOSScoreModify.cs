using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.ViewModel
{
    public class BKOSScoreModify
    {
        public string RunsA { get; set; }
        public string RunsB { get; set; }
        public List<string> ScoresA { get; set; }
        public List<string> ScoresB { get; set; }
        public int RA { get; set; }
        public int RB { get; set; }
        public string TeamAName { get; set; }
        public string TeamBName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string GameStates { get; set; }
        public string ModifyItem { get; set; }
        public int GID { get; set; }
        public int CtrlStates { get; set; }
        public string StatusText { get; set; }
    }
}
