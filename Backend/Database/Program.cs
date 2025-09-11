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

public class Admin
{
    public int AdminID { get; set; }
    public int UserID { get; set; }
    public string Permissions { get; set; }
}

public class Event
{
    public int EventID { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateOnly EventDate { get; set; }
    public int CreateBy { get; set; }
}

public class OfficeAttendadnce
{
    public int AttendenceID { get; set; }
    public int UserID { get; set; }
    public DateOnly Date { get; set; }
    public string Status { get; set; }
}

public class Room
{
    public int RoomID { get; set; }
    public string RoomName { get; set; }
    public int Capacity { get; set; }
    public string Location { get; set; }
}

public class RoomBooking
{
    
}