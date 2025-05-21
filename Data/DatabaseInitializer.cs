using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.IO;
using System.Threading.Tasks;

namespace UBB_SE_2025_EUROTRUCKERS.Data
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:Elements should be documented", Justification = "<Pending>")]
    public class DatabaseInitializer
    {
        private readonly TransportDbContext _context;
        private readonly string _schemaScriptPath;
        private readonly string _mockDataScriptPath;

        public DatabaseInitializer(TransportDbContext context,
                                string schemaScriptPath = null,
                                string mockDataScriptPath = null)
        {
            _context = context;
            _schemaScriptPath = schemaScriptPath ?? GetDefaultScriptPath("schema.sql");
            _mockDataScriptPath = mockDataScriptPath ?? GetDefaultScriptPath("mock_data.sql");
        }

        private string GetDefaultScriptPath(string fileName)
        {
            return Path.Combine(AppContext.BaseDirectory, "Database", fileName);
        }

        public async Task InitializeDatabaseAsync()
        {
            await CreateDatabaseFromScriptsAsync();
        }

        private async Task CreateDatabaseFromScriptsAsync()
        {
            using var adminConnection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=postgres;Database=postgres");
            adminConnection.Open();

            var dbExistsCmd = new NpgsqlCommand("SELECT 1 FROM pg_database WHERE datname = 'transport_dev'", adminConnection);
            var exists = dbExistsCmd.ExecuteScalar() != null;

            if (!exists)
            {
                var createDbCmd = new NpgsqlCommand("CREATE DATABASE transport_dev WITH ENCODING 'UTF8'", adminConnection);
                createDbCmd.ExecuteNonQuery();
                Console.WriteLine("Database transport_dev created.");
            }
            else
            {
                Console.WriteLine("Database transport_dev already exists.");
            }

            adminConnection.Close();

            if (!exists) 
            // Nueva conexión a transport_dev
            using (var connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=postgres;Database=transport_dev"))
            {
                try
                {
                    connection.Open();
                    Console.WriteLine("Connection to transport_dev established.");

                    // Ejecutar schema.sql
                    var schemaScript = await File.ReadAllTextAsync(_schemaScriptPath);
                    Console.WriteLine("Executing schema script...");
                    await ExecuteScriptAsync(connection, schemaScript);

                    // Ejecutar mock_data.sql
                    var mockDataScript = await File.ReadAllTextAsync(_mockDataScriptPath);
                    Console.WriteLine("Executing mock data script...");
                    await ExecuteScriptAsync(connection, mockDataScript);

                    Console.WriteLine("Scripts executed successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error during script execution.");
                    Console.WriteLine(ex.Message);
                }
            }

        }


        private async Task ExecuteScriptAsync(NpgsqlConnection connection, string script)
        {
            // Divide script into individual commands
            var commands = script.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var commandText in commands)
            {
                if (string.IsNullOrWhiteSpace(commandText.Trim()))
                    continue;

                using (var command = new NpgsqlCommand(commandText, connection))
                {
                    try
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                    catch (Exception ex)
                    {
                        // Log error but continue
                        Console.WriteLine($"Error executing command: {commandText}");
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}
