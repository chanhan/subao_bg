using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Models.ViewModel
{
    public class FollballAlliance
    {
        /// <summary>
        /// 联盟名称
        /// </summary>
        public string AllianceName { get; set; }

        /// <summary>
        ///  显示名称
        /// </summary>
        private string ShowName1;

        public string ShowName
        {
            get { return ShowName1; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    ShowName1 = AllianceName;
                }
                else
                {
                    ShowName1 = value;
                }
            }
        }
    }
}
