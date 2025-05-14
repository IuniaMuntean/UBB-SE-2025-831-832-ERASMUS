using Npgsql;
using UBB_SE_2025_EUROTRUCKERS.Models;

namespace UBB_SE_2025_EUROTRUCKERS.Services
{
    public interface IResourceServices
    {
        public void CreateDriver(Driver driver);
        public void UpdateDriver(Driver driver);
        public Driver ReadDriver(int id);
        public void DeleteDriver(int id);
        public void CreateTruck(Truck truck);
        public void UpdateTruck(Truck truck);
        public Truck ReadTruck(int id);
        public void DeleteTruck(int id);
    }

    public class ResourceServices : IResourceServices
    {
        private readonly string _connectionString = "Host=localhost;Username=postgres;Password=postgres;Database=postgres";

        public ResourceServices()
        {
        }

        public void CreateDriver(Driver driver)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                //using (var command = new NpgsqlCommand("INSERT INTO drivers (name, license_number) VALUES (@name, @licenseNumber)", connection))
                //{
                //    command.Parameters.AddWithValue("@name", driver.Name);
                //    command.Parameters.AddWithValue("@licenseNumber", driver.LicenseNumber);
                //    command.ExecuteNonQuery();
                //}
            }
        }

        public void UpdateDriver(Driver driver)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                //using (var command = new NpgsqlCommand("UPDATE drivers SET name = @name, license_number = @licenseNumber WHERE id = @id", connection))
                //{
                //    command.Parameters.AddWithValue("@id", driver.Id);
                //    command.Parameters.AddWithValue("@name", driver.Name);
                //    command.Parameters.AddWithValue("@licenseNumber", driver.LicenseNumber);
                //    command.ExecuteNonQuery();
                //}
            }
        }

        public Driver ReadDriver(int id)
        {
            using (var connection = new NpgsqlConnection(_connectionString))
            {
                connection.Open();
                //using (var command = new NpgsqlCommand("SELECT * FROM drivers WHERE id = @id", connection))
                //{
                //    command.Parameters.AddWithValue("@id", id);
                //    using (var reader = command.ExecuteReader())
                //    {
                //        if (reader.Read())
                //        {
                //            return new Driver
                //            {
                //                Id = reader.GetInt32(0),
                //                Name = reader.GetString(1),
                //                LicenseNumber = reader.GetString(2),
                //            };
                //        }
                //    }
                //}
            }
            return null;
        }

        public void DeleteDriver(int id)
        {
            //using (var connection = new NpgsqlConnection(_connectionString))
            //{
            //    connection.Open();
            //    using (var command = new NpgsqlCommand("DELETE FROM drivers WHERE id = @id", connection))
            //    {
            //        command.Parameters.AddWithValue("@id", id);
            //        command.ExecuteNonQuery();
            //    }
            //}
        }

        public void CreateTruck(Truck truck)
        {
            //using (var connection = new NpgsqlConnection(_connectionString))
            //{
            //    connection.Open();
            //    using (var command = new NpgsqlCommand("INSERT INTO trucks (model, capacity) VALUES (@model, @capacity)", connection))
            //    {
            //        command.Parameters.AddWithValue("@model", truck.Model);
            //        command.Parameters.AddWithValue("@capacity", truck.Capacity);
            //        command.ExecuteNonQuery();
            //    }
            //}
        }

        public void UpdateTruck(Truck truck)
        {
            //using (var connection = new NpgsqlConnection(_connectionString))
            //{
            //    connection.Open();
            //    using (var command = new NpgsqlCommand("UPDATE trucks SET model = @model, capacity = @capacity WHERE id = @id", connection))
            //    {
            //        command.Parameters.AddWithValue("@id", truck.Id);
            //        command.Parameters.AddWithValue("@model", truck.Model);
            //        command.Parameters.AddWithValue("@capacity", truck.Capacity);
            //        command.ExecuteNonQuery();
            //    }
            //}
        }

        public Truck ReadTruck(int id)
        {
            //using (var connection = new NpgsqlConnection(_connectionString))
            //{
            //    connection.Open();
            //    using (var command = new NpgsqlCommand("SELECT * FROM trucks WHERE id = @id", connection))
            //    {
            //        command.Parameters.AddWithValue("@id", id);
            //        using (var reader = command.ExecuteReader())
            //        {
            //            if (reader.Read())
            //            {
            //                return new Truck
            //                {
            //                    Id = reader.GetInt32(0),
            //                    Model = reader.GetString(1),
            //                    Capacity = reader.GetInt32(2)
            //                };
            //            }
            //        }
            //    }
            //}
            return null;
        }

        public void DeleteTruck(int id)
        {
            //using (var connection = new NpgsqlConnection(_connectionString))
            //{
            //    connection.Open();
            //    using (var command = new NpgsqlCommand("DELETE FROM trucks WHERE id = @id", connection))
            //    {
            //        command.Parameters.AddWithValue("@id", id);
            //        command.ExecuteNonQuery();
            //    }
            //}
        }
    }
}