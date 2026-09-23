using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Models = eredmeny.Models;
using eredmeny.Models.DTOs;

namespace eredmeny.Controllers
{
    [Route("sportolo")]
    [ApiController]
    public class SportoloController : ControllerBase
    {
        private readonly string ConnectionString = "Server=localhost;Database=sportolo13b;uid=root;Password=";

        [HttpGet]
        public List<Models.sportolo> GetAllSportolo()
        {
            var list = new List<Models.sportolo>();
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = "SELECT Id, name, email, age, password, registrationTime FROM sportolo";
            var cmd = new MySqlCommand(sql, connector);
            var dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                var s = new Models.sportolo
                {
                    Id = dr.GetInt32(0),
                    name = dr.IsDBNull(1) ? null : dr.GetString(1),
                    email = dr.IsDBNull(2) ? null : dr.GetString(2),
                    age = dr.GetInt32(3),
                    password = dr.IsDBNull(4) ? null : dr.GetString(4),
                    registrationTime = dr.GetDateTime(5)
                };
                list.Add(s);
            }
            connector.Close();
            return list;
        }

        [HttpPost]
        public Models.sportolo AddNewSportolo(AddSportoloDTO dto)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var s = new Models.sportolo
            {
                name = dto.Name,
                email = dto.Email,
                age = dto.Age,
                password = dto.Password,
                registrationTime = DateTime.Now
            };

            var sql = "INSERT INTO `sportolo`(`name`,`email`,`age`,`password`,`registrationTime`) VALUES(@name,@email,@age,@password,@registrationTime)";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@name", s.name);
            cmd.Parameters.AddWithValue("@email", s.email);
            cmd.Parameters.AddWithValue("@age", s.age);
            cmd.Parameters.AddWithValue("@password", s.password);
            cmd.Parameters.AddWithValue("@registrationTime", s.registrationTime);
            cmd.ExecuteNonQuery();
            connector.Close();
            return s;
        }

        [HttpPut]
        public object UpdateSportolo([FromQuery] int id, [FromBody] UpdateSportoloDTO dto)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();

            var sql = "UPDATE `sportolo` SET `name` = @name, `email` = @email, `age` = @age, `password` = @password WHERE `Id` = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@name", dto.Name);
            cmd.Parameters.AddWithValue("@email", dto.Email);
            cmd.Parameters.AddWithValue("@age", dto.Age);
            cmd.Parameters.AddWithValue("@password", dto.Password);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            connector.Close();
            return new { message = "Sportoló frissítve!" };
        }

        [HttpDelete]
        public object DeleteSportolo([FromQuery] int id)
        {
            var connector = new MySqlConnection(ConnectionString);
            connector.Open();
            var sql = "DELETE FROM `sportolo` WHERE Id = @id";
            var cmd = new MySqlCommand(sql, connector);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
            connector.Close();
            return new { message = "Sportoló törölve!" };
        }
    }
}
