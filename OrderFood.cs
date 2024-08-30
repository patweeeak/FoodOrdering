using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FoodOrdering
{
    public partial class OrderFood : Form
    {
        private Conclass dbConnect;
        private MySqlCommand cmd;
        private MySqlDataReader myReader, myReader1;
        DataTable cdt = new DataTable();
        DataTable pdt = new DataTable();
        string category, finalPrice, customerID, addressID, productID, qty, orderTotal, categoryId, orderID;
        public OrderFood()
        {
            InitializeComponent();
        }

        private void OrderFood_Load(object sender, EventArgs e)
        {
            Home.instance.activeForm = true;
            loadCategory();
            loadProduct();
        }
        #region load Data
        private void loadCategory()
        {
            cdt.Clear();
            categoryDgv.RowTemplate.Height = 50;
            categoryDgv.ReadOnly = true;
            dbConnect = new Conclass();
            dbConnect.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("SELECT categoryName FROM category ORDER BY categoryId ASC", dbConnect.myconnect);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(cdt);
            categoryDgv.DataSource = cdt;
            dbConnect.CloseConnection();
        }
        private void loadProduct()
        {
            pdt.Clear();
            productDgv.RowTemplate.Height = 50;
            productDgv.ReadOnly = true;
            dbConnect = new Conclass();
            dbConnect.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("SELECT productName, productStocks, productPrice FROM product WHERE productStocks > 0 ORDER BY productId ASC", dbConnect.myconnect);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmd;
            da.Fill(pdt);
            productDgv.DataSource = pdt;
            dbConnect.CloseConnection();
        }
        private void categoryProduct(string category)
        {
            pdt.Clear();
            dbConnect = new Conclass();
            dbConnect.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("SELECT categoryId FROM category WHERE categoryName = @cName", dbConnect.myconnect);
            cmd.Parameters.AddWithValue("@cName", category);
            myReader = cmd.ExecuteReader();
            if (myReader.Read())
            {
                string categID = myReader.GetInt32("categoryId").ToString();
                productPanel.Visible = false;
                dbConnect.CloseConnection();
                productDgv.RowTemplate.Height = 50;
                productDgv.ReadOnly = true;
                dbConnect = new Conclass();
                dbConnect.OpenConnection();
                MySqlCommand cmd1 = new MySqlCommand("SELECT productName, productStocks, productPrice FROM product WHERE categoryId = @id ORDER BY productId ASC", dbConnect.myconnect);
                cmd1.Parameters.AddWithValue("@id", categID);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmd1;
                da.Fill(pdt);
                productDgv.DataSource = pdt;
                dbConnect.CloseConnection();
            }
        }
        #endregion
        private void cancel_Click(object sender, EventArgs e)
        {
            productName.Clear();
            orderQty.Text = "0";
        }
        #region QTY Operation
        private void subtract_Click(object sender, EventArgs e)
        {
            if(orderQty.TextLength == 0)
            {

            }
            else
            {
                if(productName.TextLength == 0)
                {
                    if (orderQty.Text == "0")
                    {
                        orderQty.Text = "0";
                    }
                    else
                    {
                        int current = Convert.ToInt32(orderQty.Text);
                        int subtracted = current - 1;
                        orderQty.Text = subtracted.ToString();
                    }
                }
                else
                {
                    if (orderQty.Text == "1")
                    {
                        orderQty.Text = "1";
                    }
                    else
                    {
                        int current = Convert.ToInt32(orderQty.Text);
                        int subtracted = current - 1;
                        orderQty.Text = subtracted.ToString();
                    }
                }
            }
        }
        private void plus_Click(object sender, EventArgs e)
        {
            if (orderQty.TextLength == 0)
            {

            }
            else if (orderQty.Text == productStock.Text)
            {
                MessageBox.Show("Available stock(s) reached", "Notice!");
            }
            else
            {
                int current = Convert.ToInt32(orderQty.Text);
                int subtracted = current + 1;
                orderQty.Text = subtracted.ToString();
            }
        }
        #endregion

        private void orderQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        #region Cellclick
        private void categoryDgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {

            }
            else
            {
                category = categoryDgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                categoryProduct(category);
            }
        }
        private void productDgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                pdt.Clear();
                loadProduct();
            }
            else
            {
                productName.Text = productDgv.Rows[e.RowIndex].Cells[0].Value.ToString();
                productStock.Text = productDgv.Rows[e.RowIndex].Cells[1].Value.ToString();
                productPrice.Text = productDgv.Rows[e.RowIndex].Cells[2].Value.ToString();
                orderQty.Text = "1";
                readCategory();
            }
        }
        private void orderDgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            productName.Text = orderDgv.Rows[e.RowIndex].Cells[1].Value.ToString();
            orderQty.Text = orderDgv.Rows[e.RowIndex].Cells[3].Value.ToString();
            productPrice.Text = orderDgv.Rows[e.RowIndex].Cells[2].Value.ToString();
            int num1 = Convert.ToInt32(totalPrice.Text);
            int num2 = Convert.ToInt32(orderDgv.Rows[e.RowIndex].Cells[4].Value.ToString());
            int newTotal = num1 - num2;
            totalPrice.Text = newTotal.ToString();
            orderDgv.Rows.RemoveAt(e.RowIndex);
            //readCustomer();
            readOrderCategory();
            readOrderProduct();
            readOrder();
        }
        #endregion

        private void addProduct_Click(object sender, EventArgs e)
        {
/*            if (firstName.TextLength == 0 || lastName.TextLength == 0 || contactNumber.TextLength == 0 || municipality.TextLength == 0 || barangay.TextLength == 0 || street.TextLength == 0)
            {
                MessageBox.Show("Please complete the customer form", "Notice!");
            }
            else
            {
                readCustomer();
                readAddress();*/
                int num1 = Convert.ToInt32(orderQty.Text);
                int num2 = Convert.ToInt32(productPrice.Text);
                int num3 = Convert.ToInt32(totalPrice.Text);
                int total = num2 * num1;
                int ftotal = num3 + total;
                int rowId = orderDgv.Rows.Add();
                // Grab the new row!
                DataGridViewRow row = orderDgv.Rows[rowId];
                // Add the data
                row.Cells["Column5"].Value = category;
                row.Cells["Column6"].Value = productName.Text;
                row.Cells["Column7"].Value = productPrice.Text;
                row.Cells["Column8"].Value = total;
                row.Cells["Column9"].Value = orderQty.Text;
                qty = orderQty.Text;
                totalPrice.Text = ftotal.ToString();
                orderTotal = ftotal.ToString();
                readProduct();
                productName.Clear();
                orderQty.Text = "0";
            //}
        }
        #region Read and Insert
        private void readCategory()
        {
            dbConnect = new Conclass();
            dbConnect.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("SELECT categoryId FROM product WHERE productName = @pName AND productStocks = @pStocks AND productPrice = @pPrice ", dbConnect.myconnect);
            cmd.Parameters.AddWithValue("@pName", productName.Text);
            cmd.Parameters.AddWithValue("@pStocks", productStock.Text);
            cmd.Parameters.AddWithValue("@pPrice", productPrice.Text);
            myReader = cmd.ExecuteReader();
            if(myReader.Read())
            {
                categoryId = myReader["categoryId"].ToString();
                dbConnect.CloseConnection();
                dbConnect = new Conclass();
                dbConnect.OpenConnection();
                MySqlCommand cmd1 = new MySqlCommand("SELECT categoryName FROM category WHERE categoryId = @cId", dbConnect.myconnect);
                cmd1.Parameters.AddWithValue("@cId", categoryId);
                myReader1 = cmd1.ExecuteReader();
                if(myReader1.Read())
                {
                    category = myReader1["categoryName"].ToString();
                }
            }
        }

        private void placeOrder_Click(object sender, EventArgs e)
        {
            if (orderDgv.Rows.Count < 1)
            {
                MessageBox.Show("Please add a product to order", "Notice!");
            }
            else
            {
                if (MessageBox.Show("Order successfully placed! Do you want to place another order?", "Success!", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    Home.instance.OpenChildForm(new OrderFood(), 5, 60);
                }
                else
                {
                    Home.instance.OpenChildForm(new Front(), 5, 60);
                }
            }
        }
        private void readProduct()
        {
            dbConnect = new Conclass();
            dbConnect.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("SELECT productId FROM product WHERE productName = @name AND categoryId = @categ AND productStocks = @stocks AND productPrice = @price", dbConnect.myconnect);
            cmd.Parameters.AddWithValue("@name", productName.Text);
            cmd.Parameters.AddWithValue("@categ", categoryId);
            cmd.Parameters.AddWithValue("@stocks", productStock.Text);
            cmd.Parameters.AddWithValue("@price", productPrice.Text);
            myReader = cmd.ExecuteReader();
            if (myReader.Read())
            {
                productID = Convert.ToString(myReader.GetInt32("productId"));
                newOrder();
            }
        }
        private void removeOrder()
        {
            dbConnect = new Conclass();
            dbConnect.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("DELETE FROM orders WHERE orderId = @id", dbConnect.myconnect);
            cmd.Parameters.AddWithValue("@id", orderID);
            int num1 = cmd.ExecuteNonQuery();
            if(num1 > 0)
            {
/*                MessageBox.Show("Success");*/
                addStock();
                loadProduct();
            }
        }
        private void newOrder()
        {
            DateTime date = DateTime.Now;
            dbConnect = new Conclass();
            dbConnect.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("INSERT INTO orders VALUES ('', @date, @pid, @qty, @total)", dbConnect.myconnect);
            cmd.Parameters.AddWithValue("@date", date.ToString("dd-MM-yyyy"));
            cmd.Parameters.AddWithValue("@pId", productID);
            cmd.Parameters.AddWithValue("@qty", qty);
            cmd.Parameters.AddWithValue("@total", orderTotal);
            int order = cmd.ExecuteNonQuery();
            if(order > 0)
            {
/*                MessageBox.Show("Order Placed", "Success");*/
                subtractStock();
                loadProduct();
            }
        }
        private void readOrder()
        {
            DateTime today = DateTime.Today;
            dbConnect = new Conclass();
            dbConnect.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("SELECT orderId FROM orders WHERE productId = @pId AND qty = @oqty AND orderDate =@date", dbConnect.myconnect);
            cmd.Parameters.AddWithValue("@pId", productID);
            cmd.Parameters.AddWithValue("@oqty", orderQty.Text);
            cmd.Parameters.AddWithValue("@date", today.ToString("dd-MM-yyyy"));
            myReader = cmd.ExecuteReader();
            if (myReader.Read())
            {
                orderID = Convert.ToString(myReader.GetInt32("orderId"));
                removeOrder();
            }
        }
        private void readOrderCategory()
        {
            dbConnect = new Conclass();
            dbConnect.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("SELECT categoryId FROM product WHERE productName = @pName AND productPrice = @pPrice ", dbConnect.myconnect);
            cmd.Parameters.AddWithValue("@pName", productName.Text);
            cmd.Parameters.AddWithValue("@pPrice", productPrice.Text);
            myReader = cmd.ExecuteReader();
            if (myReader.Read())
            {
                categoryId = myReader["categoryId"].ToString();
                dbConnect.CloseConnection();
                dbConnect = new Conclass();
                dbConnect.OpenConnection();
                MySqlCommand cmd1 = new MySqlCommand("SELECT categoryName FROM category WHERE categoryId = @cId", dbConnect.myconnect);
                cmd1.Parameters.AddWithValue("@cId", categoryId);
                myReader1 = cmd1.ExecuteReader();
                if (myReader1.Read())
                {
                    category = myReader1["categoryName"].ToString();
                }
            }
        }
        private void readOrderProduct()
        {
            dbConnect = new Conclass();
            dbConnect.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("SELECT productId FROM product WHERE productName = @name AND categoryId = @categ AND productPrice = @price", dbConnect.myconnect);
            cmd.Parameters.AddWithValue("@name", productName.Text);
            cmd.Parameters.AddWithValue("@categ", categoryId);
            cmd.Parameters.AddWithValue("@price", productPrice.Text);
            myReader = cmd.ExecuteReader();
            if (myReader.Read())
            {
                productID = Convert.ToString(myReader.GetInt32("productId"));
            }
        }
        private void subtractStock()
        {
            dbConnect = new Conclass();
            dbConnect.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("SELECT productStocks FROM product WHERE productId = @pID", dbConnect.myconnect);
            cmd.Parameters.AddWithValue("@pID", productID);
            myReader = cmd.ExecuteReader();
            if (myReader.Read())
            {
                int num1 = myReader.GetInt32("productStocks");
                int num2 = Convert.ToInt32(qty);
                int total = num1 - num2;
                dbConnect.CloseConnection();
                dbConnect = new Conclass();
                dbConnect.OpenConnection();
                MySqlCommand cmd1 = new MySqlCommand("UPDATE product SET productStocks = @uStocks WHERE productId = @uID", dbConnect.myconnect);
                cmd1.Parameters.AddWithValue("@uStocks", total);
                cmd1.Parameters.AddWithValue("@uID", productID);
                int update = cmd1.ExecuteNonQuery();
                if (update > 0)
                {
/*                    MessageBox.Show("Subtracted");*/
                }
            }
        }
        private void addStock()
        {
            dbConnect = new Conclass();
            dbConnect.OpenConnection();
            MySqlCommand cmd = new MySqlCommand("SELECT productStocks FROM product WHERE productId = @pID", dbConnect.myconnect);
            cmd.Parameters.AddWithValue("@pID", productID);
            myReader = cmd.ExecuteReader();
            if (myReader.Read())
            {
                int num1 = myReader.GetInt32("productStocks");
                int num2 = Convert.ToInt32(qty);
                int total = num1 + num2;
                dbConnect.CloseConnection();
                dbConnect = new Conclass();
                dbConnect.OpenConnection();
                MySqlCommand cmd1 = new MySqlCommand("UPDATE product SET productStocks = @uStocks WHERE productId = @uID", dbConnect.myconnect);
                cmd1.Parameters.AddWithValue("@uStocks", total);
                cmd1.Parameters.AddWithValue("@uID", productID);
                int update = cmd1.ExecuteNonQuery();
                if (update > 0)
                {
/*                    MessageBox.Show("Added");*/
                }
            }
        }
        #endregion
    }
}
