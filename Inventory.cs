using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodOrdering
{
    public partial class Inventory : Form
    {
        private Conclass dbConnect;
        private MySqlCommand cmd;
        private MySqlDataReader myReader;
        string categoryID;
        DataTable cdt = new DataTable();
        DataTable pdt = new DataTable();
        public Inventory()
        {
            InitializeComponent();
        }

        private void categoryClick_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (categoryCb.Visible == true)
            {
                newCategory.Visible = true;
                categoryCb.SelectedIndex = 0;
                categoryCb.Visible = false;
                label6.Text = "Existing Category?";
                label6.Location = new System.Drawing.Point(360, label6.Location.Y);
            }
            else
            {
                newCategory.Visible = false;
                categoryCb.Visible = true;
                label6.Text = "New Category?";
                label6.Location = new System.Drawing.Point(380, label6.Location.Y);
            }
        }
        #region AUD
        private void addProduct_Click(object sender, EventArgs e)
        {
            if(newCategory.TextLength == 0 && categoryCb.SelectedIndex == 0 || productName.TextLength == 0 || addStocks.TextLength == 0)
            {
                MessageBox.Show("Please Complete the Form.", "Notice!");
            }
            else if (prodID.Text != "prodID")
            {
                MessageBox.Show("Product Already Exist. Update it.", "Notice!");
            }
            else
            {
                if (label6.Text == "Existing Category?")
                {
                    dbConnect = new Conclass();
                    dbConnect.OpenConnection();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM category WHERE categoryName = @cName", dbConnect.myconnect);
                    cmd.Parameters.AddWithValue("@cName", newCategory.Text);
                    myReader = cmd.ExecuteReader();
                    if (myReader.Read())
                    {
                        MessageBox.Show("Category Already Exist!", "Notice!");
                    }
                    else
                    {
                        createCategory();
                        dbConnect.CloseConnection();
                        dbConnect = new Conclass();
                        dbConnect.OpenConnection();
                        MySqlCommand cmd1 = new MySqlCommand("INSERT INTO product VALUES ('', @pName, @cID, @cStocks, @cPrice)", dbConnect.myconnect);
                        cmd1.Parameters.AddWithValue("@pName", productName.Text);
                        cmd1.Parameters.AddWithValue("@cID", categoryID);
                        cmd1.Parameters.AddWithValue("@cStocks", addStocks.Text);
                        cmd1.Parameters.AddWithValue("@cPrice", productPrice.Text);
                        int insert = cmd1.ExecuteNonQuery();
                        if(insert > 0)
                        {
                            MessageBox.Show("Product Added Successfully", "Notice!");
                            Front.instance.mngInventory_Click(null, null);
                        }
                    }
                }
                else
                {
                    category.Text = categoryCb.SelectedItem.ToString();
                    readCategory(category);
                    dbConnect.CloseConnection();
                    dbConnect = new Conclass();
                    dbConnect.OpenConnection();
                    MySqlCommand cmd1 = new MySqlCommand("INSERT INTO product VALUES ('', @pName, @cID, @cStocks, @cPrice)", dbConnect.myconnect);
                    cmd1.Parameters.AddWithValue("@pName", productName.Text);
                    cmd1.Parameters.AddWithValue("@cID", categoryID);
                    cmd1.Parameters.AddWithValue("@cStocks", addStocks.Text);
                    cmd1.Parameters.AddWithValue("@cPrice", productPrice.Text);
                    int insert = cmd1.ExecuteNonQuery();
                    if (insert > 0)
                    {
                        MessageBox.Show("Product Added Successfully", "Notice!");
                        Front.instance.mngInventory_Click(null, null);
                    }
                }
            }
        }

        private void updateProduct_Click(object sender, EventArgs e)
        {
            int first = Convert.ToInt32(currentStocks.Text);
            if (newCategory.TextLength == 0 && categoryCb.SelectedIndex == 0 || productName.TextLength == 0)
            {
                MessageBox.Show("Please Complete the Form.", "Notice!");
            }
            else if (prodID.Text == "prodID")
            {
                MessageBox.Show("Product Doesn't Exist. Add it.", "Notice!");
            }
            else
            {
                int second;
                if(addStocks.TextLength == 0)
                {
                    second = 0;
                }
                else
                {
                    second = Convert.ToInt32(addStocks.Text);
                }
                int result = first + second;
                if (label6.Text == "Existing Category?")
                {
                    dbConnect = new Conclass();
                    dbConnect.OpenConnection();
                    MySqlCommand cmd = new MySqlCommand("SELECT * FROM category WHERE categoryName = @cName", dbConnect.myconnect);
                    cmd.Parameters.AddWithValue("@cName", newCategory.Text);
                    myReader = cmd.ExecuteReader();
                    if (myReader.Read())
                    {
                        MessageBox.Show("Category Already Exist!", "Notice!");
                    }
                    else
                    {
                        createCategory();
                        dbConnect.CloseConnection();
                        dbConnect = new Conclass();
                        dbConnect.OpenConnection();
                        MySqlCommand cmd1 = new MySqlCommand("UPDATE product SET productName = @pName, categoryId = @cID, productStocks = @cStocks, productPrice = @cPrice WHERE productId = @pId", dbConnect.myconnect);
                        cmd1.Parameters.AddWithValue("@pId", prodID.Text);
                        cmd1.Parameters.AddWithValue("@pName", productName.Text);
                        cmd1.Parameters.AddWithValue("@cID", categoryID);
                        cmd1.Parameters.AddWithValue("@cStocks", result.ToString());
                        cmd1.Parameters.AddWithValue("@cPrice", productPrice.Text);
                        int update = cmd1.ExecuteNonQuery();
                        if (update > 0)
                        {
                            MessageBox.Show("Product Updated Successfully", "Notice!");
                            Front.instance.mngInventory_Click(null, null);
                        }
                    }
                }
                else
                {
                    category.Text = categoryCb.SelectedItem.ToString();
                    readCategory(category);
                    dbConnect.CloseConnection();
                    dbConnect = new Conclass();
                    dbConnect.OpenConnection();
                    MySqlCommand cmd1 = new MySqlCommand("UPDATE product SET productName = @pName, categoryId = @cID, productStocks = @cStocks, productPrice = @cPrice WHERE productId = @pId", dbConnect.myconnect);
                    cmd1.Parameters.AddWithValue("@pId", prodID.Text);
                    cmd1.Parameters.AddWithValue("@pName", productName.Text);
                    cmd1.Parameters.AddWithValue("@cID", categoryID);
                    cmd1.Parameters.AddWithValue("@cStocks", result.ToString());
                    cmd1.Parameters.AddWithValue("@cPrice", productPrice.Text);
                    int update = cmd1.ExecuteNonQuery();
                    if (update > 0)
                    {
                        MessageBox.Show("Product Updated Successfully", "Notice!");
                        Front.instance.mngInventory_Click(null, null);
                    }
                }
            }
        }

        private void deleteProduct_Click(object sender, EventArgs e)
        {
            if(prodID.Text == "prodID")
            {
                MessageBox.Show("Product doesn't exist", "Notice");
                Home.instance.OpenChildForm(new Inventory(), 5, 60);
            }
            else
            {
                if (MessageBox.Show("Are you sure you want to delete this product?", "Notice!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    dbConnect.CloseConnection();
                    dbConnect = new Conclass();
                    dbConnect.OpenConnection();
                    MySqlCommand cmd = new MySqlCommand("UPDATE product SET productStocks = '0' WHERE productId = @pId", dbConnect.myconnect);
                    cmd.Parameters.AddWithValue("@pId", prodID.Text);
                    int delete = cmd.ExecuteNonQuery();
                    if(delete > 0)
                    {
                        MessageBox.Show("Product Deleted Successfully", "Deletion Complete!");
                        Home.instance.OpenChildForm(new Inventory(), 5, 60);
                    }
                }
            }
        }
        private void deleteCategory_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this category?", "Notice!", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                if (MessageBox.Show("All products under this category will be deleted. Proceed?", "Notice!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    dbConnect = new Conclass();
                    dbConnect.OpenConnection();
                    MySqlCommand cmd = new MySqlCommand("UPDATE product SET productStocks = '0' WHERE categoryId = @categ", dbConnect.myconnect);
                    cmd.Parameters.AddWithValue("@categ", categID.Text);
                    cmd.ExecuteNonQuery();
                    MySqlCommand cmd1 = new MySqlCommand("UPDATE category SET isAvailable = '0' WHERE categoryId = @categId", dbConnect.myconnect);
                    cmd1.Parameters.AddWithValue("@categId", categID.Text);
                    int categ = cmd1.ExecuteNonQuery();
                    if (categ > 0)
                    {
                        MessageBox.Show("Category Deleted Successfully", "Deletion Successful!");
                        loadProducts();
                        loadCategory();
                        int cat = categoryCb.Items.Count;
                        for (int i = cat - 1; i > 0; i--)
                        {
                            categoryCb.Items.RemoveAt(i);
                        }
                        Front.instance.mngInventory_Click(null, null);
                    }
                }
            }
        }
        #endregion

        private void Inventory_Load(object sender, EventArgs e)
        {
            categoryPanel.Visible = false;
            productPanel.Visible = false;
            Home.instance.activeForm = true;
            newCategory.Visible = false;
            loadCategory();
            loadProducts();
            productCategories();
            label7.Visible = false;
            newProdlink.Visible = false;
        }
        #region DATA
        private void createCategory()
        {
            dbConnect.CloseConnection();
            dbConnect = new Conclass();
            dbConnect.OpenConnection();
            MySqlCommand cmd1 = new MySqlCommand("INSERT INTO category VALUES ('', @categname, '1')", dbConnect.myconnect);
            cmd1.Parameters.AddWithValue("@categname", newCategory.Text);
            cmd1.ExecuteNonQuery();
            category.Text = newCategory.Text;
            readCategory(category);
        }
        private void readCategory(Label searchCategory)
        {
            dbConnect = new Conclass();
            dbConnect.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("SELECT categoryId FROM category WHERE categoryName = @cname AND isAvailable = '1'", dbConnect.myconnect);
            cmd.Parameters.AddWithValue("@cname", searchCategory.Text);
            myReader = cmd.ExecuteReader();
            if (myReader.Read())
            {
                categoryID = myReader["categoryId"].ToString();
            }
            dbConnect.CloseConnection();
        }
        private void productCategories()
        {
            dbConnect = new Conclass();
            dbConnect.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM category WHERE isAvailable = '1'", dbConnect.myconnect);
            myReader = cmd.ExecuteReader();
            while (myReader.Read())
            {
                categoryCb.Items.Add(myReader["categoryName"].ToString());
            }
        }
        private void loadCategory()
        {
            categoryDgv.RowTemplate.Height = 30;
            categoryDgv.ReadOnly = true;
            dbConnect = new Conclass();
            dbConnect.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("SELECT categoryId, categoryName FROM category WHERE isAvailable = '1' ORDER BY categoryId ASC", dbConnect.myconnect);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(cdt);
            categoryDgv.DataSource = cdt;
            dbConnect.CloseConnection();
        }
        private void loadProducts()
        {
            productDgv.RowTemplate.Height = 30;
            productDgv.ReadOnly = true;
            dbConnect = new Conclass();
            dbConnect.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("SELECT product.productId, product.productName, category.categoryName, product.productStocks, product.productPrice FROM product RIGHT JOIN category ON product.categoryId = category.categoryId WHERE productStocks > 0 ORDER BY product.productId ASC", dbConnect.myconnect);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(pdt);
            productDgv.DataSource = pdt;
            dbConnect.CloseConnection();
        }
        #endregion
        private void currentStocks_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void productDgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {

            }
            else
            {
                if (label6.Text == "Existing Category?")
                {
                    categoryClick_LinkClicked(null, null);
                }
                prodID.Text = productDgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                categoryCb.SelectedItem = productDgv.Rows[e.RowIndex].Cells[2].Value.ToString();
                productName.Text = productDgv.Rows[e.RowIndex].Cells[1].Value.ToString();
                currentStocks.Text = productDgv.Rows[e.RowIndex].Cells[3].Value.ToString();
                productPrice.Text = productDgv.Rows[e.RowIndex].Cells[4].Value.ToString();
                newProdlink.Visible = true;
                label7.Visible = true;
            }
        }

        private void newProdlink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            prodID.Text = "prodID";
            productName.Clear();
            productPrice.Clear();
            currentStocks.Clear();
            addStocks.Clear();
            newProdlink.Visible = false;
            label7.Visible = false;
            if (label6.Text == "Existing Category?")
            {
                categoryClick_LinkClicked(null, null);
            }
            else
            {
                categoryCb.SelectedIndex = 0;
            }
        }

        private void categoryDgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.RowIndex < 0)
            {

            }
            else
            {
                categoryName.Text = categoryDgv.Rows[e.RowIndex].Cells[1].Value.ToString();
                categID.Text = categoryDgv.Rows[e.RowIndex].Cells[0].Value.ToString();
            }
        }

        private void categorySearch_TextChanged(object sender, EventArgs e)
        {
            searchCategory();
        }
        private void productSearch_TextChanged(object sender, EventArgs e)
        {
            searchProduct();
        }
        private void searchCategory()
        {
            cdt.Clear();
            categoryDgv.RowTemplate.Height = 30;
            categoryDgv.ReadOnly = true;
            dbConnect.CloseConnection();
            dbConnect = new Conclass();
            dbConnect.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM category WHERE CONCAT (categoryName) LIKE @item AND isAvailable = '1' ORDER BY categoryId ASC", dbConnect.myconnect);
            cmd.Parameters.AddWithValue("@item", "%" + categorySearch.Text + "%");
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(cdt);
            categoryDgv.DataSource = cdt;
            dbConnect.CloseConnection();
            if (categoryDgv.Rows.Count > 0)
            {
                categoryPanel.Visible = false;
            }
            else
            {
                categoryPanel.Visible = true;
            }
        }
        private void searchProduct()
        {
            pdt.Clear();
            productDgv.RowTemplate.Height = 30;
            productDgv.ReadOnly = true;
            dbConnect.CloseConnection();
            dbConnect = new Conclass();
            dbConnect.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("SELECT product.productId, product.productName, product.productStocks, product.productPrice, category.categoryName FROM product RIGHT JOIN category ON product.categoryId = category.categoryId WHERE CONCAT (product.productName) LIKE @item AND productStocks > '0' ORDER BY product.productId ASC", dbConnect.myconnect);
            cmd.Parameters.AddWithValue("@item", "%" + productSearch.Text + "%");
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(pdt);
            productDgv.DataSource = pdt;
            dbConnect.CloseConnection();
            if (productDgv.Rows.Count > 0)
            {
                productPanel.Visible = false;
            }
            else
            {
                productPanel.Visible = true;
            }
        }
    }
}
