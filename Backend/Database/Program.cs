using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class DatabaseContext : DbContext
{
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Group> Groups { get; set; }
}

public class Employee
{
    public int UserID { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Password { get; set; }
}

public class Group
{
    public int GroupID { get; set; }
    public string GroupName { get; set; }
    public string Description { get; set; }
}