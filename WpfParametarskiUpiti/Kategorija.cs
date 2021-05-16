using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfParametarskiUpiti
{
    class Kategorija //entitetska klasa odgovara tabeli
    {
        public int KategorijaId { get; set; }
        public string NazivKategorije { get; set; }
        public string OpisKategorije { get; set; }

        public override string ToString()
        {
            return NazivKategorije;
        }
    }
}
