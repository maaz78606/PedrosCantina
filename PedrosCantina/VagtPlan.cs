using System;
using System.Collections.Generic;
using System.Text;

namespace PedrosCantina
{
    // Svarer til tabellen Vagtplan (Id, EmployeeId, ShiftId) - kobler en medarbejder til en vagt
    public class VagtPlan
    {
        public int VagtPlanId { get; set; }
        public int MedarbejderId { get; set; }
        public int VagtId { get; set; }

        public override string ToString()
        {
            return $"VagtPlanId: {VagtPlanId}, MedarbejderId: {MedarbejderId}, VagtId: {VagtId}";
        }
    }
}   
