namespace Homework_9
{
    public class Program
    {
        private static DatabaseService databaseService;
        static void Main()
        {
            databaseService = new DatabaseService();
            databaseService.EnsurePopulated();

            databaseService.AddStation();//1.1
            databaseService.AddTrain();//1.2
            databaseService.TrainsLongerThan5Hours();//2
            databaseService.StationsWithTrains();//3
            databaseService.StationsWithMoreThan3Trains();//4
            databaseService.TrainWithModelStartingWithPell();//5
            databaseService.TrainsOlderThan15Years();//6 
            databaseService.StationWhithShortTravelTimeTrains();//7
            databaseService.StationWithoutTrains();//8
        }
    }
}
