using System;
using System.Collections.Generic;
using System.Text;

namespace PedrosCantina
{
    public interface Iarbejder
    {
        List<Medarbejder> GetAllMedarbejdere();
        public Medarbejder GetMedarbejderById(int medarbejderId);
       public  void CreateMedarbejder(Medarbejder medarbejder);
       public  bool UpdateMedarbejder(Medarbejder medarbejder);
       public bool RemoveMedarbejder(int medarbejderId);
        public bool DeleteMedarbejder(int medarbejderId);
    }
}
