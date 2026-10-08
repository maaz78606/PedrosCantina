using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace PedrosCantina
{
    public class MedarbejderRepository : Iarbejder
    {
        private readonly string _connectionString;

        public MedarbejderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public List<Medarbejder> GetAllMedarbejdere()
        {
            var medarbejdere = new List<Medarbejder>();
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(
                "SELECT Id, Name, Phone FROM Medarbejder ORDER BY Id",
                connection))
            {
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        medarbejdere.Add(LæsMedarbejder(reader));
                    }
                }
            }
            return medarbejdere;
        }

        public Medarbejder? GetMedarbejderById(int medarbejderId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(
                "SELECT Id, Name, Phone FROM Medarbejder WHERE Id = @Id",
                connection))
            {
                command.Parameters.AddWithValue("@Id", medarbejderId);
                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return LæsMedarbejder(reader);
                    }
                }
            }
            return null;
        }

        public void CreateMedarbejder(Medarbejder medarbejder)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(
                "INSERT INTO Medarbejder (Name, Phone) OUTPUT INSERTED.Id VALUES (@Navn, @Telefon)",
                connection))
            {
                command.Parameters.AddWithValue("@Navn", medarbejder.Navn);
                command.Parameters.AddWithValue("@Telefon", medarbejder.TelefonNummer);
                connection.Open();

                medarbejder.MedarbejderId = (int)command.ExecuteScalar();
            }
        }

        public bool UpdateMedarbejder(Medarbejder medarbejder)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(
                "UPDATE Medarbejder SET Name = @Navn, Phone = @Telefon WHERE Id = @Id",
                connection))
            {
                command.Parameters.AddWithValue("@Navn", medarbejder.Navn);
                command.Parameters.AddWithValue("@Telefon", medarbejder.TelefonNummer);
                command.Parameters.AddWithValue("@Id", medarbejder.MedarbejderId);
                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }

        public bool RemoveMedarbejder(int medarbejderId)
        {
            return DeleteMedarbejder(medarbejderId);
        }

        public bool DeleteMedarbejder(int medarbejderId)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand("DELETE FROM Medarbejder WHERE Id = @Id", connection))
            {
                command.Parameters.AddWithValue("@Id", medarbejderId);
                connection.Open();

                return command.ExecuteNonQuery() > 0;
            }
        }

        // Kolonnerækkefølgen svarer til SELECT-sætningerne ovenfor
        private static Medarbejder LæsMedarbejder(SqlDataReader reader)
        {
            return new Medarbejder
            {
                MedarbejderId = reader.GetInt32(0),
                Navn = reader.GetString(1),
                TelefonNummer = reader.GetString(2)
            };
        }
    }
}
