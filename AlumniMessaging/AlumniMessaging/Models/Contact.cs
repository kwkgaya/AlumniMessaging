using System;

namespace AlumniMessaging.Models
{
    public class Contact
    {
        private string _mobile;
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Batch { get; set; }

        public string Mobile
        {
            get => _mobile;
            set => _mobile = value?.Trim().Replace("+94", "0");
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Contact contact)) return false;
            return string.Equals(Mobile, contact.Mobile);
        }

        public override int GetHashCode()
        {
            return Mobile.GetHashCode();
        }
    }
}