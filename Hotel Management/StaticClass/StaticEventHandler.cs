using Hotel_Management.MongoDatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace Hotel_Management.StaticEvents
{
    public static class StaticEventHandler
    {     
        public static event Action OnCustomerUpdated;
        public static readonly Regex _regex = new Regex("[^0-9.-]+");
        public static void OnCustomerUpdatedEvent()
        {
            OnCustomerUpdated?.Invoke();
        }
    }
}
