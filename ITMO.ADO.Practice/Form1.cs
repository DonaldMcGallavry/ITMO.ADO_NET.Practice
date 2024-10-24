using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;

namespace ITMO.ADO.Practice
{
    public partial class Form1 : Form
    {
        private static string con = ConfigurationManager.ConnectionStrings["NorthwindConnectionString"].ConnectionString;
        private SqlDataAdapter SqlDataAdapter3;
        private DataSet NorthwindDataset = new DataSet("Northwind");
        private DataTable CustomersTable = new DataTable("Employees");

        //private SqlConnection Connection = new SqlConnection(con);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataGridViewOrders.DataSource = dataSetOrders.Orders;
            sqlDataAdapter1.Fill(dataSetOrders.Orders);

            dataGridViewEmp.DataSource = dataSetEmp.Employees;
            sqlDataAdapter2.Fill(dataSetEmp.Employees);


        }

        private void BestEmpBtn_Click(object sender, EventArgs e)
        {
            CountOrders();
        }

        private void CountOrders()
        {
            //int maxOrderCount = 0;
            //int employeeIDWithMaxOrders = 0;

            var orderCounts = dataGridViewOrders.Rows.Cast<DataGridViewRow>()
                .GroupBy(row => row.Cells["EmployeeID"].Value)
                .Select(g => new { EmployeeID = g.Key, OrderCount = g.Count() })
                .OrderByDescending(x => x.OrderCount)
                .FirstOrDefault();

            if (orderCounts != null)
            {
                int employeeID = Convert.ToInt32(orderCounts.EmployeeID);
                string firstName = string.Empty;
                string lastName = string.Empty;

                foreach (DataGridViewRow row in dataGridViewEmp.Rows)
                {
                    if ((int)row.Cells["EmployeeID"].Value == employeeID)
                    {
                        firstName = row.Cells["FirstName"].Value.ToString();
                        lastName = row.Cells["LastName"].Value.ToString();
                        break;
                    }
                }

                string bestEmployeeText = $"{firstName} {lastName} стал сотрудником месяца!";
                BestEmpText.Text = bestEmployeeText;

                //Добавляем столбец CurrentBestEmp в dataGridViewEmp, если его нет
                AddColumn();

                // Обновляем столбец CurrentBestEmp
                foreach (DataGridViewRow row in dataGridViewEmp.Rows)
                {
                    if (row.Cells["EmployeeID"]?.Value != null && (int)row.Cells["EmployeeID"].Value == employeeID)
                    {
                        row.Cells["CurrentBestEmp"].Value = "*";
                    }
                    else
                    {
                        row.Cells["CurrentBestEmp"].Value = string.Empty;
                    }
                }
            }
        }

        private void AddColumn()
        {
            if (!dataGridViewEmp.Columns.Contains("CurrentBestEmp"))
            {
                dataGridViewEmp.Columns.Add("CurrentBestEmp", "CurrentBestEmp");
            }

        }
        private void UpdateButtonOrder_Click(object sender, EventArgs e)
        {
            sqlDataAdapter1.Update(dataSetOrders);
        }

    }
}
