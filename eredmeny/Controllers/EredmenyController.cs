using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Models = eredmeny.Models;
using eredmeny.Models.DTOs;

namespace eredmeny.Controllers
{
    [Route("eredmeny")]
    [ApiController]
    public class EredmenyController : ControllerBase
    {
        private readonly string ConnectionString = "Server=localhost;Database=sportolo13b;uid=root;Password=";

        [HttpGet]
        public List<Models.eredmeny> GetAllEredmeny()
        {
            List<Models.eredmeny> results = new();
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            string sql = "SELECT Id, Competition, Description, ResultTime, UpdateTime, SportoloId FROM eredmeny";

            var cmd = new MySqlCommand(sql, connector);
            var dataReader = cmd.ExecuteReader();

            while (dataReader.Read())
            {
                var e = new Models.eredmeny
                {
                    Id = dataReader.GetInt32(0),
                    Competition = dataReader.GetString(1),
                    Description = dataReader.GetString(2),
                    ResultTime = dataReader.GetDateTime(3),
                    UpdateTime = dataReader.GetDateTime(4),
                    SportoloId = dataReader.GetInt32(5)
                };
                results.Add(e);
            }
            connector.Close();
            return results;
        }

        [HttpPost]
        public Models.eredmeny AddNewEredmeny(AddEredmenyDTO dto)
        {
            var connector = new MySqlConnection(ConnectionString);

            connector.Open();

            var e = new Models.eredmeny
            {
                Competition = dto.Competition ?? string.Empty,
                Description = dto.Description ?? string.Empty,
                ResultTime = dto.ResultTime,
                UpdateTime = DateTime.Now,
                SportoloId = dto.SportoloId
            };

            var sql = "INSERT INTO `eredmeny`(`Competition`, `Description`, `ResultTime`, `UpdateTime`, `SportoloId`) VALUES(@competition, @description, @resultTime, @updateTime, @sportoloId)";

            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@competition", e.Competition);
            cmd.Parameters.AddWithValue("@description", e.Description);
            cmd.Parameters.AddWithValue("@resultTime", e.ResultTime);
            cmd.Parameters.AddWithValue("@updateTime", e.UpdateTime);
            cmd.Parameters.AddWithValue("@sportoloId", e.SportoloId);

            cmd.ExecuteNonQuery();
            connector.Close();
            return e;
        }

        [HttpPut]
        public object UpdateEredmeny([FromQuery] int id, [FromBody] UpdateEredmenyDTO dto)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var sql = "UPDATE `eredmeny` SET `Competition` = @competition, `Description` = @description, `ResultTime` = @resultTime, `SportoloId` = @sportoloId, `UpdateTime` = @updateTime WHERE `Id` = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@competition", dto.Competition);
            cmd.Parameters.AddWithValue("@description", dto.Description);
            cmd.Parameters.AddWithValue("@resultTime", dto.ResultTime);
            cmd.Parameters.AddWithValue("@sportoloId", dto.SportoloId);
            cmd.Parameters.AddWithValue("@updateTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            connector.Close();
            return new { message = "Eredmény frissítve!" };
        }

        [HttpDelete]
        public object DeleteEredmeny([FromQuery] int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var sql = "DELETE FROM `eredmeny` WHERE Id = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            connector.Close();
            return new { message = "Eredmény törölve!" };
        }
    }
}
