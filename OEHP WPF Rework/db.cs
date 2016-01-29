using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace OEHP_WPF_Rework
{
    public class DB
    {
        public string Id { get; set; }

        public string PayerID { get; set; }

        public string ExpMM { get; set; }

        public string ExpYY { get; set; }

        public string Span { get; set; }

        public string TimeCreated { get; set; }

        public string TransactionType { get; set; }
    }
    public static class DBService
    {
        public static List<DB> ReadFile(string filepath)
        {
            var lines = File.ReadAllLines(filepath);

            var data = from l in lines
                       let split = l.Split(',')
                       select new DB
                       {
                           Id = split[0],
                           PayerID = split[1],
                           ExpMM = split[2],
                           ExpYY = split[3],
                           Span = split[4],
                           TimeCreated = split[5],
                           TransactionType = split[6]
                       };
            return data.ToList();
        }
    }

}
