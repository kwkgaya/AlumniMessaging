using System;

namespace AlumniSms.Models
{
    public class Contact
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Batch { get; set; }
        public string Mobile { get; set; }
    }
}