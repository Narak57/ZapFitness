using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using ZapFitness.Models;

namespace ZapFitness.Controllers
{
    public class SessionController : Controller
    {
        private string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ZapFitness;Integrated Security=True";

        // GET: Session
        public ActionResult Index()
        {
            List<Session> sessions = new List<Session>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT * FROM Session";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Session session = new Session
                            {
                                SessionId = reader.GetInt32(0),
                                SessionName = reader.GetString(1),
                                Schedule = reader.GetString(2),
                            };

                            sessions.Add(session);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error loading sessions: " + ex.Message);
            }

            return View(sessions);
        }
        public ActionResult Create()
        {
            return View();
        }

        // POST: Session/Create
        [HttpPost]
        public ActionResult Create(Session session)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "INSERT INTO Session (SessionName, Schedule) VALUES (@SessionName, @Schedule)";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@SessionName", session.SessionName);
                            command.Parameters.AddWithValue("@Schedule", session.Schedule);

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
            return View(session);
        }
        // GET: Session/Edit/5
        public ActionResult Edit(int id)
        {
            Session session = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Session WHERE SessionId = @SessionId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SessionId", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            session = new Session
                            {
                                SessionId = reader.GetInt32(0),
                                SessionName = reader.GetString(1),
                                Schedule = reader.GetString(2)
                            };
                        }
                    }
                }
            }

            if (session == null)
                return HttpNotFound();

            return View(session);
        }

        // POST: Session/Edit/5
        [HttpPost]
        public ActionResult Edit(Session session)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "UPDATE Session SET SessionName = @SessionName, Schedule = @Schedule WHERE SessionId = @SessionId";
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@SessionName", session.SessionName);
                            command.Parameters.AddWithValue("@Schedule", session.Schedule);
                            command.Parameters.AddWithValue("@SessionId", session.SessionId);
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

            return View(session);
        }

        // GET: Session/Delete/5
        public ActionResult Delete(int id)
        {
            Session session = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Session WHERE SessionId = @SessionId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@SessionId", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            session = new Session
                            {
                                SessionId = reader.GetInt32(0),
                                SessionName = reader.GetString(1),
                                Schedule = reader.GetString(2)
                            };
                        }
                    }
                }
            }

            if (session == null)
                return HttpNotFound();

            return View(session);
        }

        // POST: Session/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Session WHERE SessionId = @SessionId";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@SessionId", id);
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error deleting session: " + ex.Message);
                return RedirectToAction("Delete", new { id = id });
            }
        }
    }
}
