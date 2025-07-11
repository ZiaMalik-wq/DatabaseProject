﻿using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Security.Cryptography;

namespace FYP_Management.HelperClasses
{
    public static class Project_Helper
    {
        public static void AddProject(string title, string description)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.AddProject", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@Description", description);
            con.Open();
            cmd.ExecuteNonQuery();
        }
        public static void UpdateProject(int id, string title, string desc)
        {
            using var con = Config.GetConnection();
            using var cmd = new SqlCommand("dbo.UpdateProject", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            cmd.Parameters.AddWithValue("@Id", id);
            cmd.Parameters.AddWithValue("@Title", title);
            cmd.Parameters.AddWithValue("@Description", desc);

            con.Open();
            cmd.ExecuteNonQuery();
        }

        public static DataTable GetProjectTable()
        {
            var con = Config.GetConnection();
            SqlCommand cmd = new SqlCommand("Select id,title AS Title,Description from Project", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            return dt;
        }
        public static DataTable Search(string str)
        {
            var con = Config.GetConnection();
            con.Open();
            using (DataTable dt = new DataTable("Person"))
            {
                using (SqlCommand cmd = new SqlCommand("Select id,title AS Title ,Description from Project " +
                    "where Title + Description like @str_ ", con))
                {
                    cmd.Parameters.AddWithValue("str_", string.Format("%{0}%", str));
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                return dt;
            }
        }
        public static DataTable GetProjectNotAssigned(string str)
        {
            var con = Config.GetConnection();
            con.Open();
            using (DataTable dt = new DataTable("Person"))
            {
                using (SqlCommand cmd = new SqlCommand("Select id,title,Description from Project " +
                    "where Project.Id not in ( select s.ProjectId from GroupProject as s ) and Title + Description like @str_ ", con))
                {
                    cmd.Parameters.AddWithValue("str_", string.Format("%{0}%", str));
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                return dt;
            }
        }
        public static DataTable GetGroupAssigned()
        {
            var con = Config.GetConnection();
            con.Open();
            using (DataTable dt = new DataTable("Person"))
            {
                using (SqlCommand cmd = new SqlCommand("Select * from Project " +
                    "where Project.Id in ( select s.ProjectId from GroupProject as s ) ", con))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                return dt;
            }
        }
        public static DataTable GetAssignedProjectDetails(string str=null)
        {
            var con = Config.GetConnection();
            con.Open();
            using (DataTable dt = new DataTable("Person"))
            {
                using (SqlCommand cmd = new SqlCommand(" select GroupId,Project.Title ,(select Person.FirstName+' '+Person.LastName from ProjectAdvisor PA join Person on Person.Id= Pa.AdvisorId where GP.ProjectId = Pa.ProjectId and PA.AdvisorRole = 11 ) as [Main Advisor] ,(select Person.FirstName+' '+Person.LastName from ProjectAdvisor PA join Person on Person.Id= Pa.AdvisorId where GP.ProjectId = Pa.ProjectId and PA.AdvisorRole = 12 ) as [Co Advisor] ,(select Person.FirstName+' '+Person.LastName from ProjectAdvisor PA join Person on Person.Id= Pa.AdvisorId where GP.ProjectId = Pa.ProjectId and PA.AdvisorRole = 14 ) as [Industry Advisor] from GroupProject GP join Project on Project.Id = GP.ProjectId where project.Title like @str_ ", con))
                {
                    cmd.Parameters.AddWithValue("str_", string.Format("%{0}%", str));
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                return dt;
            }
        }
        
        public static DataTable GetProjectWithAdvisorsNames(string str=null)
        {
            var con = Config.GetConnection();
            con.Open();
            using (DataTable dt = new DataTable("Person"))
            {
                using (SqlCommand cmd = new SqlCommand(" select GroupId,Project.Title ,GP.AssignmentDate ,(select Person.FirstName+' '+Person.LastName from ProjectAdvisor PA join Person on Person.Id= Pa.AdvisorId where GP.ProjectId = Pa.ProjectId and PA.AdvisorRole = 11 ) as [Main Advisor] ,(select Person.FirstName+' '+Person.LastName from ProjectAdvisor PA join Person on Person.Id= Pa.AdvisorId where GP.ProjectId = Pa.ProjectId and PA.AdvisorRole = 12 ) as [Co Advisor] ,(select Person.FirstName+' '+Person.LastName from ProjectAdvisor PA join Person on Person.Id= Pa.AdvisorId where GP.ProjectId = Pa.ProjectId and PA.AdvisorRole = 14 ) as [Industry Advisor] from GroupProject GP join Project on Project.Id = GP.ProjectId where project.Title like @str_ ", con))
                {
                    cmd.Parameters.AddWithValue("str_", string.Format("%{0}%", str));
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                return dt;
            }
        }
        
        /*public static string getMainAdvisor(int GID,int PID)
        {
            var con = Config.GetConnection();
            con.Open();
            using (DataTable dt = new DataTable("Person"))
            {
                using (SqlCommand cmd = new SqlCommand("select Person.FirstName+' '+Person.LastName from ProjectAdvisor PA join Person on Person.Id= Pa.AdvisorId where GP.ProjectId = Pa.ProjectId and PA.AdvisorRole = 11 ) as [Main Advisor] ", con))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    adapter.Fill(dt);
                }
                return dt;
            }

            var con = Config.GetConnection();
            SqlCommand cmd = new SqlCommand($"select TotalMarks from Evaluation where id = {Eid}", con);
            con.Open();
            int id = (int)cmd.ExecuteScalar();
            con.Close();
            return id.ToString();
        }*/

    }
}
