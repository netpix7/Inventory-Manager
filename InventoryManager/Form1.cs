using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InventoryManager
{
    public partial class Form1 : Form
    {
        private ProductDAL productDAL = new ProductDAL();
       

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
          
                LoadProducts();
                dgvProductList.Columns["productid"].Visible = false;
           


        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
           
                LoadProducts();
                ClearReset();
          
        }

        private void LoadProducts()
        {
           
                dgvProductList.DataSource = productDAL.GetProducts();
            
           
        }

        private void ClearReset()
        {
            txtProductName.Clear();
            txtCategory.Clear();
            txtQuantity.Clear();
            txtPrice.Clear();
        


        }
        //======================================================================
        private bool ValidateUserInput()
        {
            
            // Validate 
           
            String  productName = txtProductName.Text.Trim();
            string category = txtCategory.Text.Trim();
           
            if (string.IsNullOrWhiteSpace(productName))
            {
                MessageBox.Show("Product name is required.", "Inventory Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(category))
            {
                MessageBox.Show("Category is required.", "Inventory Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCategory.Focus();
                return false;
            }

//==========================================================================
            if (string.IsNullOrWhiteSpace(txtQuantity.Text))
            {
                MessageBox.Show("Quantity is required and cannot be left blank.", "Inventory Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return false;
            }

            try
            {
                int quantity = Convert.ToInt32(Convert.ToInt32(txtQuantity.Text));
                
                if (quantity <= 0)
                {
                    MessageBox.Show("Quantity must be positive", "Inventory Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtQuantity.Focus();
                    return false;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Quantity must be numbers "+ex.Message, "Inventory Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtQuantity.Focus();
                return false;


            }

            //==============================================
            if (string.IsNullOrWhiteSpace(txtPrice.Text))
            {
                MessageBox.Show("Price is required and cannot be left blank.", "Inventory Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return false;
            }



            try
            {
                decimal price = Convert.ToDecimal(Convert.ToDecimal(txtPrice.Text));
                if (price <= 0)
                {
                    MessageBox.Show("Price must be positive", "Inventory Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtPrice.Focus();
                    return false;
                }
            }
            catch (Exception ex)
            {
               MessageBox.Show("Price must be numbers "+ex.Message, "Inventory Manager", MessageBoxButtons.OK, MessageBoxIcon.Warning);
               txtPrice.Focus();
                return false;


            }

            return true;
        }


        //======================================================================
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (ValidateUserInput())
            {

                MessageBox.Show("All inputs are valid!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                try
                {
                    int quantity = Convert.ToInt32(Convert.ToInt32(txtQuantity.Text));
                    decimal price = Convert.ToDecimal(Convert.ToDouble(txtPrice.Text));

                    productDAL.AddProduct(txtProductName.Text, txtCategory.Text, quantity, price);
                    LoadProducts();
                    MessageBox.Show("Product record has been saved successfully", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearReset();

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to add products: {ex.Message}", "Inventory Manager: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }


               
               
            }

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateUserInput())
            {
                if (dgvProductList.SelectedRows.Count > 0)
                {
                    try
                    {

                        int quantity = Convert.ToInt32(Convert.ToInt32(txtQuantity.Text));
                        decimal price = Convert.ToDecimal(Convert.ToDouble(txtPrice.Text));
                        int productID = Convert.ToInt32(dgvProductList.SelectedRows[0].Cells["productid"].Value);
                        productDAL.UpdateProduct(productID, txtProductName.Text, txtCategory.Text, quantity, price);


                        LoadProducts();
                        MessageBox.Show("Product record has been updated successfully", "Inventory Manager", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        ClearReset();


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to update products: {ex.Message}", "Inventory Manager: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    // int quantity = Convert.ToInt32(Convert.ToInt32(txtQuantity.Text));

                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProductList.SelectedRows.Count > 0)
                {
                    int productID = Convert.ToInt32(dgvProductList.SelectedRows[0].Cells["productid"].Value);

                    DialogResult result = MessageBox.Show("Are You Sure You want to Delete this Record?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        productDAL.DeleteProduct(productID);

                        MessageBox.Show("Record Deleted successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadProducts();
                        ClearReset();
                    }

                }
               
            }
            catch(Exception ex)
            {
                
                MessageBox.Show($"Failed to update products: {ex.Message}", "Inventory Manager: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

           
            
            
        }

        private void DisplayCellValue()
        {
            try
            {
                if (dgvProductList.SelectedRows.Count > 0)
                {
                    int rowIndex = dgvProductList.SelectedRows[0].Index;
                    txtProductID.Text = dgvProductList.Rows[rowIndex].Cells["productid"].Value.ToString();
                    txtProductName.Text = dgvProductList.Rows[rowIndex].Cells["product_name"].Value.ToString();

                    txtCategory.Text = dgvProductList.Rows[rowIndex].Cells["category"].Value.ToString();
                    txtQuantity.Text = dgvProductList.Rows[rowIndex].Cells["quantity"].Value.ToString();
                    txtPrice.Text = dgvProductList.Rows[rowIndex].Cells["price"].Value.ToString();

                 }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to retrieve products: {ex.Message}", "Inventory Manager: Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void dgvProductList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           // DisplayCellValue();
        }

        private void dgvProductList_SelectionChange(Object sender, EventArgs e)
        {
            DisplayCellValue();
        }





        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            dgvProductList.DataSource = productDAL.SearchProduct(txtSearch.Text.Trim());
        }
           

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }
    }
}

