using Microsoft.EntityFrameworkCore;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace nookenApp.Helper
{
    public class DatabaseHelper
    {
        private string _connectionString;
        private string _bakFilePath;
        private string _databaseName;

        public DatabaseHelper()
        {
            _connectionString = Settings.ConnectionString;
            _databaseName = "AppDB";
            // Use the GetProjectDirectory method to construct the full path to the .bak file
            _bakFilePath = GetProjectDirectory("DB", "AppDB.bak");
        }

        // Method to get the project directory and construct the full path to the specified file
        private string GetProjectDirectory(params string[] paths)
        {
            string projectDirectory = Path.Combine(AppContext.BaseDirectory, "..", "..", "..");
            return Path.GetFullPath(Path.Combine(projectDirectory, Path.Combine(paths)));
        }

        public async Task InitializeDatabaseAsync()
        {
            try
            {
                using (var connection = new SqlConnection(Settings.MconnectionString))
                {
                    await connection.OpenAsync();

                    // Check if the database exists
                    var checkDbCommand = new SqlCommand($"SELECT database_id FROM sys.databases WHERE Name = '{_databaseName}'", connection);
                    var result = await checkDbCommand.ExecuteScalarAsync();

                    if (result != null)
                    {
                        // Restore the database from backup
                        string restoreDbScript = $@"
                            USE [master]
                            RESTORE DATABASE [{_databaseName}]
                            FROM DISK = N'{_bakFilePath}'
                            WITH FILE = 1, NOUNLOAD, STATS = 5";

                        Console.WriteLine($"Executing SQL: {restoreDbScript}"); // Логирование SQL скрипта

                        using (var restoreDbCommand = new SqlCommand(restoreDbScript, connection))
                        {
                            await restoreDbCommand.ExecuteNonQueryAsync();
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Database '{_databaseName}' already exists.");
                    }
                }

                // Apply migrations if using Entity Framework
                try
                {
                    using (var context = new AppDbContext())
                    {
                        await context.Database.MigrateAsync();
                    }
                }
                catch (Exception ex)
                {
                    // Логирование ошибки
                    Console.WriteLine($"Error applying migrations: {ex.Message}");
                    throw; // Перебросить исключение для дальнейшей обработки
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибки
                Console.WriteLine($"Error initializing database: {ex.Message}");
                throw; // Перебросить исключение для дальнейшей обработки
            }
        }
    }
}
