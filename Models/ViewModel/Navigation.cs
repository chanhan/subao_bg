using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.ViewModel
{
    public class Navigation
    {
        public List<string> Level { get; set; }
        public bool HaveButton { get; set; }
        public string ButtonText { get; set; }
        public string Area { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public List<Parameter> Parameter { get; set; }

    }
    public class Parameter
    {
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public Parameter(string parameterName, string parameterValue)
        {
            ParameterName = parameterName;
            ParameterValue = parameterValue;
        }
        public override string ToString()
        {
            return string.Format("{0}={1}", ParameterName, ParameterValue);
        }
    }
}
