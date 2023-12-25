using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Management.Pages.QuanLyCacPhong
{
    public class Room
    {
        public Room(int id, string name, int floor, string type, string status)
        {
            this.Id = id;
            this.Name = name;
            this.Floor = floor;
            this.Type = type;
            this.Status = status;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int Floor { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }

    }
}
