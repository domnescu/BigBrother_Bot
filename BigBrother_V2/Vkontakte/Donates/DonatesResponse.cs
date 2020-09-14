using System.Collections.Generic;

namespace BigBro.Vk.Donates
{
    class DonatesResponse
    {
        public int count { get; set; }
        public List<Donate> donates { get; set; }
        public bool success { get; set; }
    }
}
