using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs_Compartidos.Models
{
    public class WebHook
    {
        public int id { get; set; }
        public bool live_mode { get; set; }
        public string type { get; set; }
        public string date_created { get; set; }
        public string application_id { get; set; }
        public string user_id { get; set; }
        public string api_version { get; set; }
        public string action { get; set; }
        public Data data { get; set; }

        public override string ToString()
        {
            string valores = $"id:{id} - user-id:{user_id} - action:{action} - idPreference: {data.id}";
            return valores;
        }
    }
}
