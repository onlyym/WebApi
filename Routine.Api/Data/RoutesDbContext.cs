using Microsoft.EntityFrameworkCore;
using Routine.Api.Entities;
using System;

namespace Routine.Api.Data
{
    public class RoutesDbContext : DbContext
    {
        public RoutesDbContext(DbContextOptions<RoutesDbContext> options)
            :base(options)
        {

        }
        public DbSet<Company> Companies  { get; set; }  
        public DbSet<Employee> Employees { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().Property(x => x.Name).IsRequired().HasMaxLength(100);
            modelBuilder.Entity<Company>().Property(x => x.Introduction).HasMaxLength(500);
            modelBuilder.Entity<Employee>().Property(x => x.EmployeeNo).IsRequired().HasMaxLength(10);
            modelBuilder.Entity<Employee>().Property(x => x.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Employee>().Property(x => x.LastName).IsRequired().HasMaxLength(50);
            modelBuilder.Entity<Employee>()
                .HasOne(x => x.Company)
                .WithMany(x => x.Employees)
                .HasForeignKey(x => x.CompanyId).OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Company>().HasData(
               new Company
               {
                   Id = Guid.Parse("3781029A-13CD-A93C-CC09-C5FA4B18889E"),
                   Name = "微软",
                   Introduction = "Good Company"

               },
                new Company
                {
                    Id = Guid.Parse("7EAD96F5-A8F5-99E3-C238-3E425A476E76"),
                    Name = "阿里",
                    Introduction = "中国 Company"

                },
                 new Company
                 {
                     Id = Guid.Parse("0AFF89A7-98B7-D258-FC8A-435E23F8B1D1"),
                     Name = "腾讯",
                     Introduction = "游戏 Company"

                 },
                  new Company
                  {
                      Id = Guid.Parse("d1f1f410-f563-4355-aa91-4774d693363f"),
                      Name = "test",
                      Introduction = "test"

                  }
                );

            modelBuilder.Entity<Employee>().HasData(
              //Microsoft employees
              new Employee
              {
                  Id = Guid.Parse("ca268a19-0f39-4d8b-b8d6-5bace54f8027"),
                  CompanyId = Guid.Parse("3781029A-13CD-A93C-CC09-C5FA4B18889E"),
                  DateOfBirth = new DateTime(1955, 10, 28),
                  EmployeeNo = "M001",
                  FirstName = "William",
                  LastName = "Gates",
                  Gender = Gender.男
              },
              new Employee
              {
                  Id = Guid.Parse("265348d2-1276-4ada-ae33-4c1b8348edce"),
                  CompanyId = Guid.Parse("3781029A-13CD-A93C-CC09-C5FA4B18889E"),
                  DateOfBirth = new DateTime(1998, 1, 14),
                  EmployeeNo = "M002",
                  FirstName = "Kent",
                  LastName = "Back",
                  Gender = Gender.男
              },
              //Google employees
              new Employee
              {
                  Id = Guid.Parse("47b70abc-98b8-4fdc-b9fa-5dd6716f6e6b"),
                  CompanyId = Guid.Parse("7EAD96F5-A8F5-99E3-C238-3E425A476E76"),
                  DateOfBirth = new DateTime(1986, 11, 4),
                  EmployeeNo = "G001",
                  FirstName = "Mary",
                  LastName = "King",
                  Gender = Gender.女
              },
              new Employee
              {
                  Id = Guid.Parse("059e2fcb-e5a4-4188-9b46-06184bcb111b"),
                  CompanyId = Guid.Parse("7EAD96F5-A8F5-99E3-C238-3E425A476E76"),
                  DateOfBirth = new DateTime(1977, 4, 6),
                  EmployeeNo = "G002",
                  FirstName = "Kevin",
                  LastName = "Richardson",
                  Gender = Gender.男
              },
              new Employee
              {
                  Id = Guid.Parse("910e7452-c05f-4bf1-b084-6367873664a1"),
                  CompanyId = Guid.Parse("7EAD96F5-A8F5-99E3-C238-3E425A476E76"),
                  DateOfBirth = new DateTime(1982, 3, 1),
                  EmployeeNo = "G003",
                  FirstName = "Frederic",
                  LastName = "Pullan",
                  Gender = Gender.男
              },
              //Alipapa employees
              new Employee
              {
                  Id = Guid.Parse("a868ff18-3398-4598-b420-4878974a517a"),
                  CompanyId = Guid.Parse("0AFF89A7-98B7-D258-FC8A-435E23F8B1D1"),
                  DateOfBirth = new DateTime(1964, 9, 10),
                  EmployeeNo = "A001",
                  FirstName = "Jack",
                  LastName = "Ma",
                  Gender = Gender.男
              },
              new Employee
              {
                  Id = Guid.Parse("2c3bb40c-5907-4eb7-bb2c-7d62edb430c9"),
                  CompanyId = Guid.Parse("0AFF89A7-98B7-D258-FC8A-435E23F8B1D1"),
                  DateOfBirth = new DateTime(1997, 2, 6),
                  EmployeeNo = "A002",
                  FirstName = "Lorraine",
                  LastName = "Shaw",
                  Gender = Gender.女
              },
              new Employee
              {
                  Id = Guid.Parse("e32c33a7-df20-4b9a-a540-414192362d52"),
                  CompanyId = Guid.Parse("0AFF89A7-98B7-D258-FC8A-435E23F8B1D1"),
                  DateOfBirth = new DateTime(2000, 1, 24),
                  EmployeeNo = "A003",
                  FirstName = "Abel",
                  LastName = "Obadiah",
                  Gender = Gender.女
              }
          );
        }

    }
}
