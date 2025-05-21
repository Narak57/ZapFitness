using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZapFitness.Models;    

namespace ZapFitness.Controllers
{
    public class MemberController : Controller
    {
        
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ZapFitness;Integrated Security=True";


        // Display All members
        public ActionResult Index()
        {
            List<Member_Model> members = new List<Member_Model>();
            try 
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Member";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Member_Model member = new Member_Model
                            {
                                MemberID = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2),
                                Phone = reader.GetString(3),
                            };
                            members.Add(member);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error loading members: " + ex.Message);
            }

            return View(members);
        }

        // Create a new member form
        public ActionResult Create()
        {
            return View();
        }

        //Add new member to database
        [HttpPost]
        public ActionResult Create(Member_Model member)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "INSERT INTO Member (FirstName, LastName, Phone) VALUES (@FirstName, @LastName, @Phone)";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@FirstName", member.FirstName);
                            command.Parameters.AddWithValue("@LastName", member.LastName);
                            command.Parameters.AddWithValue("@Phone", member.Phone);

                            command.ExecuteNonQuery();
                        }
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error: " + ex.Message);
                }
            }
                return View(member);
        }
        

        // Edit member form
        public ActionResult Edit(int id)
        {
            Member_Model member = null;

            try
            {

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Member WHERE MemberID = @MemberID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MemberID", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                member = new Member_Model
                                {
                                    MemberID = (int)reader["MemberID"],
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Phone = reader["Phone"].ToString()
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error loading member: " + ex.Message);
            }

            if (member == null)
            {
                return HttpNotFound();
            }

            return View(member);
        }

        //Update member in database
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Member_Model updateMember)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "UPDATE Member SET FirstName = @FirstName, LastName = @LastName, Phone = @Phone WHERE MemberID = @MemberID";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@FirstName", updateMember.FirstName);
                            command.Parameters.AddWithValue("@LastName", updateMember.LastName);
                            command.Parameters.AddWithValue("@Phone", updateMember.Phone);
                            command.Parameters.AddWithValue("@MemberID", updateMember.MemberID);
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                return RedirectToAction("Index");
                            }
                            else
                            {
                                ModelState.AddModelError("", "Unable to update member. Please try again.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Error updating member: " + ex.Message);

                }
            }

                return View(updateMember);
        }
   
           

        // Delete member
        public ActionResult Delete(int id)
        {
            Member_Model member = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Member WHERE MemberID = @MemberID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@MemberID", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                member = new Member_Model
                                {
                                    MemberID = (int)reader["MemberID"],
                                    FirstName = reader["FirstName"].ToString(),
                                    LastName = reader["LastName"].ToString(),
                                    Phone = reader["Phone"].ToString()
                                };
                            }
                        }
                    }
                }
                if (member == null)
                    return HttpNotFound();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error loading member: " + ex.Message);
            }

            return View(member);
           
        }


        // Delete member from database
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmation(int id)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Member WHERE MemberID = @MemberID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@MemberID", id);
                    command.ExecuteNonQuery();
                }
            }
            return RedirectToAction("Index");
        }
    }
}
