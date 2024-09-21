using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework_9
{
    public class DatabaseService
    {
        DbContextOptions<ApplicationContext> options;

        public void EnsurePopulated()
        {

            var builder = new ConfigurationBuilder();
            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());
            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("appsettings.json");
            // создаем конфигурацию
            var config = builder.Build();
            // получаем строку подключения
            string connectionString = config.GetConnectionString("DefaultConnection");


            var optionsBuilder = new DbContextOptionsBuilder<ApplicationContext>();
            options = optionsBuilder.UseSqlServer(connectionString).Options;

            using (ApplicationContext db = new ApplicationContext(options))
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }

        }

        //1.	Добавить данные про станции и поезда.
        public void AddStation()
        {
            using (ApplicationContext db = new ApplicationContext(options))
            {
                db.Database.ExecuteSqlRaw("INSERT INTO Stations (Name) VALUES ('Station1')");
            }
        }

        public void AddTrain()
        {
            using (ApplicationContext db = new ApplicationContext(options))
            {
                db.Database.ExecuteSqlRaw("INSERT INTO Trains (Number, Model, TravelTime, ManufacturingDate, StationId)" +
                    " VALUES ('1230', 'Model1', '04:30:00', '2022-01-01', 1)");
            }
        }

        //2.	Поезда у которых длительность маршрута более 5 часов.
        public void TrainsLongerThan5Hours()
        {
            using (ApplicationContext db = new ApplicationContext(options))
            {
                var trainsLongerThan5Hours = db.Trains.FromSqlRaw("SELECT * FROM Trains WHERE TravelTime > '05:00:00'").ToList();
            }
        }

        //3.	Общую информация о станции и ее поездах.
        public void StationsWithTrains()
        {
            using (ApplicationContext db = new ApplicationContext(options))
            {
                var stationsWithTrains = db.Stations.FromSqlRaw("SELECT * FROM Stations").Include(x => x.Trains).ToList();
            }
        }

        //4.	Название станций у которой в наличии более 3-ех поездов.
        public void StationsWithMoreThan3Trains()
        {
            using (ApplicationContext db = new ApplicationContext(options))
            {
                var stationsWithMoreThan3Trains = db.Stations.FromSqlRaw("SELECT s.Name FROM Stations s INNER JOIN Trains t " +
                    "ON s.Id = t.StationId GROUP BY s.Name HAVING COUNT(*) > 1").ToList();
            }
        }

        //5.	Все поезда, модель которых начинается на подстроку «Pell».
        public void TrainWithModelStartingWithPell()
        {
            using (ApplicationContext db = new ApplicationContext(options))
            {
                var trainWithModelStartingWithPell = db.Trains.FromSqlRaw("SELECT * FROM Trains WHERE Model LIKE 'Pell%'").ToList();
            }
        }

        //6.	Все поезда, у которых возраст более 15 лет с текущей даты.
        public void TrainsOlderThan15Years()
        {
            using (ApplicationContext db = new ApplicationContext(options))
            {
                var currentDate = DateTime.Now.Date;
                var trainsOlderThan15Years = db.Trains.FromSqlRaw($"SELECT * FROM Trains WHERE " +
                    $"ManufacturingDate <= DATEADD(year, -15, '{currentDate}')").ToList();
            }
        }

        //7.	Получить станции, у которых в наличии хотя бы один поезд с длительность маршрутка менее 4 часов.
        public void StationWhithShortTravelTimeTrains()
        {
            using (ApplicationContext db = new ApplicationContext(options))
            {
                var stationWhithShortTravelTimeTrains = db.Stations.FromSqlRaw("SELECT DISTINCT s.* FROM Stations s INNER JOIN " +
                    "Trains t ON s.Id = t.StationId WHERE t.TravelTime < '04:00:00'").ToList();
            }
        }

        //8.	Вывести все станции без поездов (на которых не будет поездов при выполнении LEFT JOIN).
        public void StationWithoutTrains()
        {
            using (ApplicationContext db = new ApplicationContext(options))
            {
                var stationWithoutTrains = db.Stations.FromSqlRaw("SELECT s.Id, s.Name FROM Stations s LEFT JOIN Trains t " +
                    "ON s.Id = t.StationId WHERE t.StationId IS NULL").ToList();
            }
        }
    }
}
