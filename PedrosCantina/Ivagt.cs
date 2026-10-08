using System;
using System.Collections.Generic;
using System.Text;

namespace PedrosCantina
{
    public interface Ivagt
    {
        List<Vagt> GetAllVagter();
       public Vagt GetVagtById(int vagtId);
        public void CreateVagt(Vagt vagt);
        public bool UpdateVagt(Vagt vagt);
       public bool DeleteVagt(int vagtId);
    }
}
