using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace SSISBulkExportTask100
{
    public partial class frmPreview : Form
    {
        private readonly BackgroundWorker _backgroundWorker = new BackgroundWorker();
        private readonly string _connectionString;
        private readonly string _sourceType;
        private readonly string _executionSource;
        private readonly DataTable _dataTable = new DataTable();

        public frmPreview(string connectionString, string sourceType, string executionSource)
        {
            InitializeComponent();

            _backgroundWorker.DoWork += backgroundWorker_DoWork;
            _backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;

            _connectionString = connectionString;
            _sourceType = sourceType;
            _executionSource = executionSource;

            _backgroundWorker.RunWorkerAsync();
        }

        void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (dataGridView.InvokeRequired)
                dataGridView.Invoke(new MethodInvoker(delegate
                                                          {
                                                              dataGridView.DataSource = _dataTable;
                                                          }));
            else
                dataGridView.DataSource = _dataTable;
        }

        void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            using (var sqlConnection = new SqlConnection(_connectionString))
            {
                sqlConnection.Open();
                using (var sqlCommand = new SqlCommand
                                                   {
                                                       CommandTimeout = 30,
                                                       CommandType = CommandType.Text,
                                                       Connection = sqlConnection,
                                                       CommandText = _executionSource
                                                   })
                {
                    using (var sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                    {
                        sqlDataAdapter.Fill(_dataTable);
                    }
                }
            }
        }
    }
}
