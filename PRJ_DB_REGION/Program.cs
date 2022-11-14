using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PRJ_DB_REGION
{
    internal partial class Program
    {
        static string connectionString = "Server = MICHELET; Database = PRJ_REGIONS; User Id = rNPC; Password = aaa;";

        static List<C_VILLE> Villes;
        static List<C_DEPARTEMENT> Departements;
        static List<C_REGION> Regions;

        static void Main(string[] args)
        {
            string Data_Json;

            Data_Json = File.ReadAllText("cities.json");
            Villes = JsonSerializer.Deserialize<List<C_VILLE>>(Data_Json);

            Data_Json = File.ReadAllText("departments.json");
            Departements = JsonSerializer.Deserialize<List<C_DEPARTEMENT>>(Data_Json);

            Data_Json = File.ReadAllText("regions.json");
            Regions = JsonSerializer.Deserialize<List<C_REGION>>(Data_Json);


            incTable_Regions();
            incTable_Departements();
            incTable_Villes();
        }




        static void incTable_Regions()
        {


            string queryString;
            foreach (var item in Regions)
            {
                queryString = $"INSERT INTO Regions(code, name) " +
                    $"values('{item.code}', '{item.slug}');";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    connection.Close();
                }
            }
        }



        static void incTable_Departements()
        {
            string queryString;
            int id;
            foreach (var item in Departements)
            {

                queryString = $"SELECT idRegion FROM Regions WHERE code = '{item.region_code}';";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    id = reader.GetInt32(0);
                    reader.Close();
                }

                queryString = $"INSERT INTO Departements(code, name, idRegion) " +
                    $"values('{item.code}', '{item.slug}', {id});";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    connection.Close();
                }
            }
        }

        static void incTable_Villes()
        {
            string queryString;
            int id;
            foreach (var item in Villes)
            {

                queryString = $"SELECT idDepartement FROM Departements WHERE code = '{item.department_code}';";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    reader.Read();
                    id = reader.GetInt32(0);
                    reader.Close();
                }

                //if(item.name == "L'Abergement-Cl\u00e9menciat") { itemName = }
                queryString = $"INSERT INTO Villes(insee_code, zip_code, name, gps_lat, gps_lng, idDepartement) " +
                    $"values('{item.insee_code}', '{item.zip_code}', '{item.slug}', '{item.gps_lat}', '{item.gps_lng}', {id});";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    connection.Close();
                }
            }
        }

    }
}
