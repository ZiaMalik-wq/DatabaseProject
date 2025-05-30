﻿using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace FYP_Management.HelperClasses
{
    public static class Evaluation_Helper
    {
        public static DataTable GetEvaluations(string str="")
        {
            var con = Config.GetConnection();
            SqlCommand cmd = new SqlCommand("Select * from Evaluation where name like @str", con);
            cmd.Parameters.AddWithValue("str", string.Format("%{0}%", str));
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            return dt;
        }
        public static void insertEvaluation(string name,int totalM,int tWeightage)
        {
            var con = Config.GetConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into evaluation values(@name,@tot,@weight)", con);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("tot", totalM);
            cmd.Parameters.AddWithValue("weight", tWeightage);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
        }
        
        public static void UpdateEvaluation(string name,int totalM,int tWeightage,int id)
        {
            var con = Config.GetConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand("update Evaluation set Name= @name , TotalMarks = @tot ,TotalWeightage = @weight where id = @id ", con);
            cmd.Parameters.AddWithValue("name", name);
            cmd.Parameters.AddWithValue("tot", totalM);
            cmd.Parameters.AddWithValue("weight", tWeightage);
            cmd.Parameters.AddWithValue("id", id);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
        }
        public static DataTable GetEvaluationFromGid (int Gid)
        {
            var con = Config.GetConnection();
            SqlCommand cmd = new SqlCommand("select E.Id,GE.GroupId, E.Name, GE.ObtainedMarks, E.TotalMarks, E.TotalWeightage,GE.EvaluationDate from Evaluation E join GroupEvaluation GE on E.Id = GE.EvaluationId where GE.GroupId = @Gid", con);
            cmd.Parameters.AddWithValue("Gid", Gid);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            dt.Load(sdr);
            con.Close();
            return dt;
        }
        public static List<string> getEvaluationName()
        {
            var con = Config.GetConnection();
            SqlCommand cmd = new SqlCommand("select name from Evaluation", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            List<string> list = new List<string>();
            while (sdr.Read())
            {
                string gender = sdr.GetString(0);
                list.Add(gender);
            }
            con.Close();
            return list;
        }
        public static int GetEvaluationIndex(int index)
        {
            var con = Config.GetConnection();
            SqlCommand cmd = new SqlCommand("select id from Evaluation", con);
            DataTable dt = new DataTable();
            con.Open();
            SqlDataReader sdr = cmd.ExecuteReader();
            List<int> list = new List<int>();
            while (sdr.Read())
            {
                int val = sdr.GetInt32(0);
                list.Add(val);
            }
            con.Close();
            return list[index];
        }
        public static void AddGroupEvaluation(int GId, int EId, int OMarks, DateTime dtime)
        {
            var con = Config.GetConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into Groupevaluation values(@gid,@eid,@omarks, @dtime)", con);
            cmd.Parameters.AddWithValue("gid", GId);
            cmd.Parameters.AddWithValue("eid", EId);
            cmd.Parameters.AddWithValue("omarks", OMarks);
            cmd.Parameters.AddWithValue("dtime", dtime);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
        }
        public static void UpdateGroupEvaluation(int GId, int EId, int OMarks, DateTime dtime)
        {
            var con = Config.GetConnection();
            con.Open();
            SqlCommand cmd = new SqlCommand("Update GroupEvaluation set obtainedmarks = @omarks , evaluationDate = @dtime where groupid = @gid and Evaluationid = @eid", con);
            cmd.Parameters.AddWithValue("gid", GId);
            cmd.Parameters.AddWithValue("eid", EId);
            cmd.Parameters.AddWithValue("dtime", dtime);
            cmd.Parameters.AddWithValue("omarks", OMarks);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            con.Close();
        }
        public static string GetTotalMarksofEvaluation(int Eid)
        {
            var con = Config.GetConnection();
            SqlCommand cmd = new SqlCommand($"select TotalMarks from Evaluation where id = {Eid}", con);
            con.Open();
            int id = (int)cmd.ExecuteScalar();
            con.Close();
            return id.ToString();
        }
        public static int GetAssigedPercentageEvaluations()
        {
            var con = Config.GetConnection();
            SqlCommand cmd = new SqlCommand("select sum(totalweightage) from Evaluation ", con);
            con.Open();
            var id = cmd.ExecuteScalar();
            con.Close();
            if(int.TryParse(id.ToString(),out int sum))
            {
                return sum;
            }
            return 0;
        }
    }
}
