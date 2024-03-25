using PracticalTask5;
using System;
using System.Collections.Generic;

namespace PracticalTask5
{
    class Vacation
    {
        public DateTime StartDate { get; set; }
        public int Duration { get; set; }
    }

    class Employee
    {
        public string Name { get; set; }
        public List<Vacation> Vacations { get; set; }

        public Employee(string name)
        {
            Name = name;
            Vacations = new List<Vacation>();
        }
    }
    internal class Program
    {
        public static bool CheckingVacationDates(DateTime startDate, int vacationDuration, Vacation vacation, int days)
        {
            for (DateTime verifiedDate = startDate; verifiedDate < startDate.AddDays(vacationDuration); verifiedDate = verifiedDate.AddDays(1))
            {
                for (DateTime busyVacationDay = vacation.StartDate; busyVacationDay < vacation.StartDate.AddDays(vacation.Duration); busyVacationDay = busyVacationDay.AddDays(1))
                {
                    if (Math.Abs((verifiedDate - busyVacationDay).Days) <= days)
                    {
                        return false;
                    }
                }
            }
            
            return true;
        }
        static void Main(string[] args)
        {
            List<Employee> employees = new List<Employee>();
            employees.Add(new Employee("Иванов Иван Иванович"));
            employees.Add(new Employee("Петров Петр Петрович"));
            employees.Add(new Employee("Юлина Юлия Юлиановна"));
            employees.Add(new Employee("Сидоров Сидор Сидорович"));
            employees.Add(new Employee("Павлов Павел Павлович"));
            employees.Add(new Employee("Георгиев Георг Георгиевич"));

            Random random = new Random();
            int currentYear = DateTime.Now.Year;
            DateTime endYear = new DateTime(currentYear, 12, 31);
            var listWorkingDays = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };

            foreach (Employee employee in employees)
            {
                int daysRemaining = 28;
                while (daysRemaining > 0)
                {
                    bool freeVacation = true;
                    int vacationDuration = (daysRemaining > 14) ? 14 : 7;
                    DateTime startDate = new DateTime(currentYear, 1, 1).AddDays(random.Next(365));
                    if (listWorkingDays.Contains(startDate.DayOfWeek.ToString()) && startDate.AddDays(vacationDuration) <= endYear)
                    {
                        foreach (Vacation vacation in employee.Vacations)
                        {
                            if (freeVacation)
                            {
                                freeVacation = CheckingVacationDates(startDate, vacationDuration, vacation, 30);
                            }
                            else
                            {
                                break;
                            }
                        }
                        foreach (Employee employee2 in employees)
                        {
                            foreach (Vacation vacation in employee2.Vacations)
                            {
                                if (freeVacation)
                                {
                                    freeVacation = CheckingVacationDates(startDate, vacationDuration, vacation, 3);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                        if (freeVacation)
                        {
                            employee.Vacations.Add(new Vacation { StartDate = startDate, Duration = vacationDuration });
                            daysRemaining -= vacationDuration;
                        }
                    }
                }

                employee.Vacations.Sort(delegate (Vacation x, Vacation y)
                {
                    return x.StartDate.CompareTo(y.StartDate);
                });
            }

            foreach (Employee employee in employees)
            {
                Console.WriteLine($"Сотрудник: {employee.Name}");
                foreach (Vacation vacation in employee.Vacations)
                {
                    Console.WriteLine($"Начало отпуска: {vacation.StartDate.ToShortDateString()}, Конец отпуска: {vacation.StartDate.AddDays(vacation.Duration-1).ToShortDateString()}, Продолжительность: {vacation.Duration} дней");
                }
                Console.WriteLine();
            }
        }
    }
}
