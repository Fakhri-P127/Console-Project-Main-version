﻿using System;
using System.Collections.Generic;
using System.Text;
using Console_Project.Enums;
using Console_Project.Interfaces;
using Console_Project.Models;

namespace Console_Project.Operations
{
    class AcademyService : IAcademyServices
    {
        public List<Group> AllGroups { get; } = new List<Group>();
        public List<Student> AllStudents { get; } = new List<Student>();

        public void CreateGroup(Categories category, bool isonline)
        {                                              
            Group group = new Group(category, isonline);
            if (!CheckGroupNo(group))
            {
                ClearAndColor();
                Console.WriteLine($"{group.No.ToUpper().Trim()} already exists.");
                return;
            }            
            group.Limit = isonline ? group.Limit = 15 : group.Limit = 10;
            AllGroups.Add(group);
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Created GroupNo: {group.No.ToUpper().Trim()}\n");                       
        }
        public bool CheckGroupNo(Group currentGroup)
        {
            //if the groupNo of the group changed, this makes sure that you can't create group with the same groupNo
            //Example: created groupNo P100 then edited to P101. After this if we create P101 it will create another group with t
            foreach (Group group in AllGroups)
            {
                if (group.No.ToLower().Trim() == currentGroup.No.ToLower().Trim())
                {
                    return false;
                }                
            }
            return true;
        }        
        public void ShowAllGroups()
        {
            if (AllGroups.Count == 0)
            {
                ClearAndColor();
                Console.WriteLine("There's no group that exists");
                return;
            }
            foreach (Group group in AllGroups)
            {
                Console.WriteLine(group);                
            }
        }
        public void EditGroupNo(string no, string newNo)
        {            
            if (string.IsNullOrEmpty(no) || string.IsNullOrEmpty(newNo))
            {
                ClearAndColor();
                Console.WriteLine("Please enter something. You can't have empty value.");
                return;
            }
            if (string.IsNullOrWhiteSpace(no) || string.IsNullOrWhiteSpace(newNo))
            {
                ClearAndColor();
                Console.WriteLine("Please enter something. You can't have just white spaces.");
                return;
            }           
            Group existGroup = FindGroup(no);
            string groupNo = newNo.Trim();
            if (!CheckGroupNo(groupNo))
            {
                return;
            }
            if (existGroup == null)
            {
                ClearAndColor();
                Console.WriteLine("Please enter a groupNo that exists");
                return;
            }
            foreach (Group group in AllGroups)
            {
                if (group.No.ToLower().Trim() == groupNo.ToLower().Trim())
                {
                    ClearAndColor();
                    Console.WriteLine($"{group.No.ToUpper().Trim()} already exists");
                    return;
                }
            }            
            //For example: If you create S100 group, you can't change it to P200. They must be from the same category.(my own rule)
            switch (existGroup.Category)
            {
                case Categories.Programming:

                    if (groupNo.StartsWith('p') || groupNo.StartsWith('P'))
                    {
                        break;
                    }
                    else
                    {
                        ClearAndColor();
                        Console.WriteLine($"{groupNo} is not from the same category as {no}. You can only edit groups that are in the same category.");
                        return;
                    }                    
                case Categories.Design:
                    if (groupNo.StartsWith('d') || groupNo.StartsWith('D'))
                    {
                        break;
                    }
                    else
                    {
                        ClearAndColor();
                        Console.WriteLine($"{groupNo} is not from the same category as {no}. You can only edit groups that are in the same category.");
                        return;
                    }                    
                case Categories.SystemAdministration:
                    if (groupNo.StartsWith('s') || groupNo.StartsWith('S'))
                    {
                        break;
                    }
                    else
                    {
                        ClearAndColor();
                        Console.WriteLine($"{groupNo} is not from the same category as {no}. You can only edit groups that are in the same category.");
                        return;
                    }
                default:
                    ClearAndColor();
                    Console.WriteLine($"{groupNo} doesn't start with the category its in.");//Not possible to get to this error so we can delete default case but i kept it just in case.
                    return;
            }
            existGroup.No = groupNo;            

            //When calling ShowAllStudents() after EditGroupNo() it shows the old groupNo. This code is preventing that.                                                   
            foreach (Student student in existGroup.Students)
            {
                student.GroupNo = groupNo;
            }
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{no.ToUpper().Trim()} has been successfuly changed to {groupNo.ToUpper().Trim()}\n");
        }
        public static bool CheckGroupNo(string groupNo)
        {            
            if (groupNo.Length == 4 && char.IsLetter(groupNo[0]))
            {
                for (int i = 1; i < groupNo.Length; i++)
                {
                    if (!char.IsDigit(groupNo[i]))
                    {
                        ClearAndColor();
                        Console.WriteLine("Last 3 characters must be all digits");
                        return false;
                    }
                }
                return true;
            }
            ClearAndColor();
            Console.WriteLine("GroupNo needs to be 4 characters long.\nFirst character must be a letter and last 3 characters should be all digits");
            return false;
        }
        //public static bool CheckGroupNo(string groupno)
        //{
        //    string group = groupno.Trim();
        //    if ((group.Length >= 4 && group.Length <= 6) && char.IsLetter(group[0]) && char.IsDigit(group[1]) && char.IsDigit(group[2]) && char.IsDigit(group[3]))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        ClearAndColor();
        //        Console.WriteLine("GroupNo needs to be 4 character long. First character must be a letter and last 3 characters should be all digits");
        //        return false;
        //    }
        //}
        public Group FindGroup(string no)
        {            
            foreach (Group group in AllGroups)
            {
                if (group.No.ToLower().Trim() == no.ToLower().Trim())
                {
                    return group;
                }
            }
            return null;
        }       
        public void ShowStudentsInGroup(string no)
        {            
            Group group = FindGroup(no);
            if (group == null)
            {
                ClearAndColor();
                Console.WriteLine("Please enter valid groupNo.");
                return;
            }
            if (group.Students.Count == 0)
            {
                ClearAndColor();
                Console.WriteLine("There's no students in this group.");
                return;
            }
            string statusOnline = group.IsOnline ? "Online" : "Offline";
            if (no.ToLower().Trim() == group.No.ToLower().Trim())
            {
                foreach (Student student in group.Students)
                {                         
                    Console.WriteLine($"{student}, Status: {statusOnline}");                    
                }
            }       
        }        
        public void ShowAllStudents()
        {            
            if (AllStudents.Count == 0)
            {
                ClearAndColor();
                Console.WriteLine("There's no students.");
                return;
            }            
            foreach (Group group in AllGroups)
            {
                string statusOnline = group.IsOnline ? "Online" : "Offline";
                foreach (Student student in group.Students)
                {
                    Console.WriteLine($"{student}, Status: {statusOnline}");
                }                
            }                       
        }       
        public void CreateStudent(string fullname,string groupNo, bool type)
        {                                   
            Group group = FindGroup(groupNo);

            if (group == null)
            {
                ClearAndColor();
                Console.WriteLine("Enter a group that exists");
                return;
            }                                                                                        
            if (group.Limit-1 < group.Students.Count)
            {
                ClearAndColor();
                Console.WriteLine($"You passed the limit. Group {groupNo}'s max limit is {group.Limit}");
                return;
            }            
            Student student = new Student(fullname, groupNo, type);            
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Created Student {fullname.ToUpper().Trim()}\n");            
            AllStudents.Add(student);
            group.Students.Add(student);
            Console.WriteLine($"Students in group {groupNo.ToUpper().Trim()}:\n");
            string statusOnline = group.IsOnline ? "Online" : "Offline";
            foreach (Student stud in group.Students)
            {
                Console.WriteLine($"{stud}, Status: {statusOnline}");
            }            
        }
        public static bool CheckFullname(string fullname)
        {
            if (fullname.Length > 30)
            {
                ClearAndColor();
                Console.WriteLine($"{fullname.ToUpper().Trim()} is too long. Enter less than 30 characters.");
                return false;
            }
            foreach (char letter in fullname)
            {
                if (char.IsDigit(letter))
                {
                    ClearAndColor();
                    Console.WriteLine("No digits allowed.");
                    return false;
                }
            }            
            string[] splitFullname = fullname.Split(" ");            
            if ((splitFullname.Length == 2)) //2 because(name and surname)
            {
                if ((splitFullname[0].Length >= 3 && splitFullname[1].Length >= 3))
                {
                    return true;
                }
                ClearAndColor();
                Console.WriteLine("Both Name and Surname needs to be at least 3 characters.");
                return false;
            }
            ClearAndColor();
            Console.WriteLine("Fullname must include: Name + Space + Surname. Example(Fakhri Afandiyev)");
            return false;
        }
        public static void ClearAndColor()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
        }
    }
}