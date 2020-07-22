using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ProjectCore.Entity.Model
{
    public class Statuses
    {
        static public int Active { get; set; }
        static public int Inactive { get; set; }

        public Statuses()
        {
            Active = 1;
            Inactive = 2;
        }

        static public List<Dictionary<string, int>> Get()
        {
            List<Dictionary<string, int>> returnList = new List<Dictionary<string, int>>();
            Statuses status = new Statuses();
            PropertyInfo[] infos = status.GetType().GetProperties();

            foreach (PropertyInfo info in infos)
            {
                Dictionary<string, int> temp = new Dictionary<string, int>();
                temp.Add(info.Name, (int)info.GetValue(status, null));
                returnList.Add(temp);
            }
            return returnList;
        }
    }
}
