using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace InventoryManager
{
   public  class ProductDAL
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }   
        public int Quantity { get; set; }
        public decimal Price { get; set; }

        public string strC { get; set; }

        private readonly string _connectionString;

        public ProductDAL()
        {
            _connectionString = "Data Source=DESKTOP-0R3B99K;Initial Catalog=InventoryDB;Integrated Security=True";
        }

        // Add Product
        public void AddProduct(string productName, string category, int quantity, decimal price)
        {
            try
            {

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("ADD_Product", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Product_Name", productName);
                        cmd.Parameters.AddWithValue("@Category", category);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@Price", price);

                        conn.Open();
                        cmd.ExecuteNonQuery();


                    }
                }
            }
            catch(SqlException ex)
            {
                Console.Error.WriteLine("Error Adding product to Database ", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected Error addind product ", ex.Message);
                throw;
            }

        }

        // Get All Products
        public DataTable GetProducts()
        {
                  DataTable dt = new DataTable();

            try
            {


                using (SqlConnection conn = new SqlConnection(_connectionString))
                        {
                            using (SqlCommand cmd = new SqlCommand("SEL_Products", conn))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                conn.Open();
                                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                                adapter.Fill(dt);
                            }
                        }
             }
             catch (SqlException ex)
             {
                    Console.Error.WriteLine("Error retrieving products from database",ex.Message);
                    throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected Error retrieving products ",ex.Message);
                throw;
            }
            return dt;
        }

      
        // Update Product
        public void UpdateProduct(int productID, string productName, string category, int quantity, decimal price)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("UPD_Products", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductId", productID);
                        cmd.Parameters.AddWithValue("@Product_Name", productName);
                        cmd.Parameters.AddWithValue("@Category", category);
                        cmd.Parameters.AddWithValue("@Quantity", quantity);
                        cmd.Parameters.AddWithValue("@Price", price);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.Error.WriteLine("Error updating products in database", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected Error updating products ", ex.Message);
                throw;
            }
        }


        // Delete Product
        public void DeleteProduct(int productID)
        {
            try
            {

                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("DEL_Products", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ProductId", productID);

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.Error.WriteLine("Error deleting products in database", ex.Message);
                throw;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected Error deleting products ", ex.Message);
                throw;
            }
        }

        //search Product
        
        public DataTable SearchProduct(string srchFilter = null)
        {

            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SearchProduct", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SearchFilter", (object)srchFilter ?? DBNull.Value);
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                    adapter.Fill(dt);
                    
                }
            }
            return dt;
        }

    }
}
