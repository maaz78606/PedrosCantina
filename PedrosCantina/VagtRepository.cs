using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace PedrosCantina
{
    public class VagtRepository : Ivagt
    {
        private readonly string _connectionString;

        public VagtRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Vagt> GetAllVagter()
        {
            var vagter = new List<Vagt>();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(
                "SELECT Id, ShiftDate, StartTime, EndTime FROM Vagt ORDER BY ShiftDate, StartTime",
                connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        vagter.Add(LæsVagt(reader));
                    }
                }
            }
            return vagter;
        }

        public Vagt? GetVagtById(int vagtId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(
                "SELECT Id, ShiftDate, StartTime, EndTime FROM Vagt WHERE Id = @Id",
                connection))
            {
                command.Parameters.AddWithValue("@Id", vagtId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return LæsVagt(reader);
                    }
                }
            }
            return null;
        }

        public void CreateVagt(Vagt vagt)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(
                "INSERT INTO Vagt (ShiftDate, StartTime, EndTime) OUTPUT INSERTED.Id VALUES (@Dato, @Start, @Slut)",
                connection))
            {
                command.Parameters.AddWithValue("@Dato", vagt.Dato);
                command.Parameters.AddWithValue("@Start", vagt.StartTid);
                command.Parameters.AddWithValue("@Slut", vagt.SlutTid);
                connection.Open();

                vagt.VagtId = (int)command.ExecuteScalar();
            }
        }

        public bool UpdateVagt(Vagt vagt)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(
                "UPDATE Vagt SET ShiftDate = @Dato, StartTime = @Start, EndTime = @Slut WHERE Id = @Id",
                connection))
            {
                command.Parameters.AddWithValue("@Dato", vagt.Dato);
                command.Parameters.AddWithValue("@Start", vagt.StartTid);
                command.Parameters.AddWithValue("@Slut", vagt.SlutTid);
                command.Parameters.AddWithValue("@Id", vagt.VagtId);
                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool DeleteVagt(int vagtId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("DELETE FROM Vagt WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", vagtId);
                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }

        // Kolonnerækkefølgen svarer til SELECT-sætningerne ovenfor
        private static Vagt LæsVagt(SqlDataReader reader)
        {
            return new Vagt(
                reader.GetInt32(0),
                reader.GetFieldValue<DateOnly>(1),
                reader.GetFieldValue<TimeOnly>(2),
                reader.GetFieldValue<TimeOnly>(3));
        }
    }
}
