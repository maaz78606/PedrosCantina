using System;
using System.Collections.Generic;
using System.Text;

namespace PedrosCantina
{
    // Svarer til tabellen Vagt (Id, ShiftDate, StartTime, EndTime)
    public class Vagt
    {
        public int VagtId { get; set; }
        public DateOnly Dato { get; set; }
        public TimeOnly StartTid { get; set; }
        public TimeOnly SlutTid { get; set; }

        public Vagt(int vagtId, DateOnly dato, TimeOnly startTid, TimeOnly slutTid)
        {
            VagtId = vagtId;
            Dato = dato;
            StartTid = startTid;
            SlutTid = slutTid;
        }
        public override string ToString()
        {
            return $"VagtId: {VagtId}, Dato: {Dato:yyyy-MM-dd}, StartTid: {StartTid:HH\\:mm}, SlutTid: {SlutTid:HH\\:mm}";
        }
    }
}
