using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DruhaHodina
{
    class Program
    {
        static void Main(string[] args)
        {
            Employee employee = new Employee("Test", 3000, Department.Dep2);
            employee.DisplayInformation();
            Console.WriteLine();

            Manager manager = new Manager("TestManager", 3000, Department.Dep1, 1000);
            manager.DisplayInformation();
            Console.WriteLine();

            Dictionary<int, int> bossBonus = new Dictionary<int, int>();
            bossBonus.Add(3000, 1);
            bossBonus.Add(5000, 1);

            Boss boss = new Boss("TestBoss", 3000, 1000, bossBonus);
            boss.DisplayInformation();

            List<Employee> employees = new List<Employee>();
            employees.Add(employee);
            employees.Add(manager);
            employees.Add(boss);

            Company company = new Company(boss, employees, Type.As);
            company.Fire(manager, "Test", DateTime.Now);
            company.Print();

            Console.ReadLine();
        }
    }

    enum Department
    {
        None,
        Dep1,
        Dep2,
        Dep3,
        Company
    }

    enum Type
    {
        Sro,
        As,
        Osvc
    }

    class Company
    {
        public Boss boss;
        public List<Employee> employees;
        public Type type;
        public int Costs;
        public int Profit;
        public int ResultingState;
        public bool IsInProfit;

        public Company(Boss boss, List<Employee> employees, Type type)
        {
            if(boss == null)
            {
                Console.WriteLine("Nelze založit firmu");
                return;
            }
            this.boss = boss;
            this.employees = employees;
            this.type = type;
            if(employees.Count == 0)
            {
                this.type = Type.Osvc;
            }
            Costs = 0;
            employees.ForEach(employee =>
            {
                Costs += employee.Salary;
            });
            Profit = 1000000;
            ResultingState = Profit - Costs;
            IsInProfit = ResultingState > 0;
        }

        public void Fire(Employee employee, string Reason, DateTime Date)
        {
            if(!getTypeOfEmployee(employee).Equals("Boss"))
            {
                int SeverancePay = 10000;
                if (getTypeOfEmployee(employee).Equals("Manager")) SeverancePay = 30000;
                employees.Remove(employee);
                FiredEmployee newEmployee = new FiredEmployee(employee.Name, employee.Salary, Department.None, Reason, Date, SeverancePay);
                employees.Add(newEmployee);
            }
        }

        string getTypeOfEmployee(Employee employee)
        {
            return employee.GetType().ToString().Split('.')[employee.GetType().ToString().Split('.').Length - 1];
        }

        public void Print()
        {
            List<Employee> fired = new List<Employee>();
            Console.WriteLine("Zaměstnanci: ");
            employees.ForEach(employee =>
            {
                if(employee.department == Department.None)
                {
                    fired.Add(employee);
                } else
                {
                    Console.WriteLine(employee.Name + ", " + getTypeOfEmployee(employee) + ", " + employee.department);
                }
            });
            if(fired.Count > 0)
            {
                Console.WriteLine("\nVyhození:");
                fired.ForEach(firedEmployee =>
                {
                    Console.WriteLine(firedEmployee.Name);
                });
            }
        }
    }

    class FiredEmployee : Employee
    {
        public string Reason;
        public DateTime Date;
        public int SeverancePay;

        public FiredEmployee(string Name, int Salary, Department department, string Reason, DateTime Date, int SeverancePay) : base(Name, Salary, department)
        {
            this.Reason = Reason;
            this.Date = Date;
            this.SeverancePay = SeverancePay;
        }
    }

    class Employee
    {

        public string Name;
        public int Salary;
        public Department department;

        public Employee(string Name, int Salary, Department department)
        {
            if(Name == null || Name.Equals(""))
            {
                this.Name = "Franta";
            }
            else
            {
                this.Name = Name;
            }
            if(this.Name.Split(' ').Length == 1)
            {
                this.Name = this.Name + " prijmeni";
            }
            this.Salary = Salary;
            if (department.Equals(Department.Company))
            {
                this.department = Department.Dep1;
            }
            this.department = department;
        }

        public int CalculateMonthlySalary()
        {
            return Salary / 12;
        }

        public void DisplayInformation()
        {
            Console.WriteLine("Name: " + Name);
            Console.WriteLine("Salary: " + Salary);
            Console.WriteLine("Department: " + department);
            Console.WriteLine("Monthly Salary: " + CalculateMonthlySalary());
        }
    }

    class Manager : Employee
    {
        public int Bonus;

        public Manager(string Name, int Salary, Department department, int Bonus) : base(Name, Salary, department)
        {
            this.Bonus = Bonus;
        }

        public new int CalculateMonthlySalary()
        {
            return base.CalculateMonthlySalary() + Bonus/12;
        }

        public new void DisplayInformation()
        {

            Console.WriteLine("Name: " + Name);
            Console.WriteLine("Salary: " + Salary);
            Console.WriteLine("Department: " + department);
            Console.WriteLine("Bonus: " + Bonus);
            Console.WriteLine("Monthly Salary: " + CalculateMonthlySalary());
        }
    }

    class Boss : Manager
    {
        public Dictionary<int, int> StockOptions;

        public Boss(string Name, int Salary, int Bonus, Dictionary<int, int> StockOptions) : base(Name, Salary, Department.Company, Bonus)
        {
            this.StockOptions = StockOptions;
        }

        public int CalculateStockOptionsValue()
        {
            int StockOptionsValue = 0;
            for(int i = 0; i < StockOptions.Count; i++)
            {
                StockOptionsValue = StockOptions.Values.ToList<int>()[i] * StockOptions.Keys.ToList<int>()[i];
            }
            return StockOptionsValue;
        }

        public new int CalculateMonthlySalary()
        {
            return base.CalculateMonthlySalary() + CalculateStockOptionsValue()/12;
        }

        public new void DisplayInformation()
        {
            Console.WriteLine("Name: " + Name);
            Console.WriteLine("Salary: " + Salary);
            Console.WriteLine("Department: " + department);
            Console.WriteLine("Bonus: " + Bonus);
            Console.WriteLine("StockOptions: " + CalculateStockOptionsValue());
            Console.WriteLine("Monthly Salary: " + CalculateMonthlySalary());
        }
    }
}
