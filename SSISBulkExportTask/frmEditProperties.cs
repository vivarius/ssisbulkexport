using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Windows.Forms;
using Microsoft.DataTransformationServices.Controls;
using Microsoft.SqlServer.Dts.Runtime;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
using TaskHost = Microsoft.SqlServer.Dts.Runtime.TaskHost;
using Variable = Microsoft.SqlServer.Dts.Runtime.Variable;
using VariableDispenser = Microsoft.SqlServer.Dts.Runtime.VariableDispenser;

namespace SSISBulkExportTask100
{
    public partial class frmEditProperties : Form
    {
        #region Private Properties
        private readonly TaskHost _taskHost;
        private readonly Connections _connections;
        private readonly Dictionary<string, string> _paramsManager = new Dictionary<string, string>();
        private bool _isFirstLoad = false;
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets the variables.
        /// </summary>
        /// <value>The variables.</value>
        private Variables Variables
        {
            get { return _taskHost.Variables; }
        }

        /// <summary>
        /// Gets the connections.
        /// </summary>
        /// <value>The connections.</value>
        private Connections Connections
        {
            get { return _connections; }
        }

        #endregion

        #region .ctor
        public frmEditProperties(TaskHost taskHost, Connections connections)
        {
            InitializeComponent();

            _taskHost = taskHost;

            try
            {

                LoadDBConnections();
                LoadFileConnections();

                Cursor = Cursors.WaitCursor;
            }
            catch (Exception exception)
            {
                Cursor = Cursors.Arrow;
                MessageBox.Show(exception.Message);
            }
            finally
            {
                Cursor = Cursors.Arrow;
            }
        }

        void grdParameters_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Methods

        private ComboBox LoadVariables(string parameterInfo, ref string selectedText)
        {
            var comboBox = new ComboBox();

            comboBox.Items.Add(string.Empty);

            foreach (Variable variable in Variables.Cast<Variable>().Where(variable => Type.GetTypeCode(Type.GetType(parameterInfo)) == variable.DataType))
            {
                comboBox.Items.Add(string.Format("@[{0}::{1}]", variable.Namespace, variable.Name));
            }

            return comboBox;
        }

        public int FindStringInComboBox(ComboBox comboBox, string searchTextItem, int startIndex)
        {
            if (startIndex >= comboBox.Items.Count)
                return -1;

            int indexPosition = comboBox.FindString(searchTextItem, startIndex);

            if (indexPosition <= startIndex)
                return -1;

            return comboBox.Items[indexPosition].ToString() == searchTextItem
                                    ? indexPosition
                                    : FindStringInComboBox(comboBox, searchTextItem, indexPosition);
        }

        private static object EvaluateExpression(string mappedParam, VariableDispenser variableDispenser)
        {
            object variableObject;

            if (mappedParam.Contains("@"))
            {
                var expressionEvaluatorClass = new ExpressionEvaluatorClass
                                                   {
                                                       Expression = mappedParam
                                                   };

                expressionEvaluatorClass.Evaluate(DtsConvert.GetExtendedInterface(variableDispenser),
                                                  out variableObject,
                                                  false);
            }
            else
            {
                variableObject = mappedParam;
            }

            return variableObject;
        }

        private void LoadFileConnections()
        {
            cmbDestination.Items.Clear();

            foreach (var connection in Connections)
            {
                cmbDestination.Items.Add(connection.Name);
            }
        }

        private void LoadDBConnections()
        {
            cmbSQLServer.Items.Clear();

            foreach (var connection in Connections)
            {
                cmbSQLServer.Items.Add(connection.Name);
            }
        }

        private void LoadFileVariables()
        {
            cmbDestination.Items.Clear();
            cmbDestination.Items.AddRange(LoadVariables("System.String").ToArray());
        }

        private List<string> LoadVariables(string parameterInfo)
        {
            return Variables.Cast<Variable>().Where(variable => Type.GetTypeCode(Type.GetType(parameterInfo)) == variable.DataType).Select(variable => string.Format("@[{0}::{1}]", variable.Namespace, variable.Name)).ToList();
        }

        private void LoadDataBaseObjects()
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connections[cmbSQLServer.Text].ConnectionString))
            {
                {
                    using (SqlCommand sqlCommand = new SqlCommand(QueryResources.TABLES, sqlConnection))
                    {
                        using (SqlDataReader sqlDataReaderTables = sqlCommand.ExecuteReader())
                        {
                            cmbTables.Items.Clear();

                            while (sqlDataReaderTables.Read())
                            {
                                cmbTables.Items.Add(sqlDataReaderTables.GetString(0));
                            }
                        }
                    }
                }

                {
                    using (SqlCommand sqlCommand = new SqlCommand(QueryResources.VIEWS, sqlConnection))
                    {
                        using (SqlDataReader sqlDataReaderTables = sqlCommand.ExecuteReader())
                        {
                            cmbViews.Items.Clear();

                            while (sqlDataReaderTables.Read())
                            {
                                cmbViews.Items.Add(sqlDataReaderTables.GetString(0));
                            }
                        }
                    }
                }

                {
                    using (SqlCommand sqlCommand = new SqlCommand(QueryResources.STORED_PROCEDURES, sqlConnection))
                    {
                        using (SqlDataReader sqlDataReaderTables = sqlCommand.ExecuteReader())
                        {
                            cmbStoredProcedures.Items.Clear();

                            while (sqlDataReaderTables.Read())
                            {
                                cmbStoredProcedures.Items.Add(sqlDataReaderTables.GetString(0));
                            }
                        }
                    }
                }
            }
        }

        private void LoadStoredProcedureParameters(string schema, string storedProcedureName)
        {
            using (SqlConnection sqlConnection = new SqlConnection(_connections[cmbSQLServer.Text].ConnectionString))
            {
                using (SqlCommand sqlCommand = new SqlCommand(string.Format(QueryResources.STORED_PROCEDURE_PARAMETERS, schema, storedProcedureName), sqlConnection))
                {
                    using (SqlDataReader sqlDataReaderTables = sqlCommand.ExecuteReader())
                    {
                        grdParameters.Rows.Clear();

                        int index;

                        while (sqlDataReaderTables.Read())
                        {
                            index = grdParameters.Rows.Add();

                            DataGridViewRow row = grdParameters.Rows[index];

                            row.Cells["grdColParams"] = new DataGridViewTextBoxCell
                                                            {
                                                                Value = sqlDataReaderTables.GetString(1)
                                                            };

                            row.Cells["grdColDirection"] = new DataGridViewTextBoxCell
                                                               {
                                                                   Value = sqlDataReaderTables.GetString(2)
                                                               };

                            row.Cells["grdColVars"] = LoadCellVariables();

                            row.Cells["grdColExpression"] = new DataGridViewButtonCell();
                        }
                    }
                }
            }
        }

        #endregion

        #region Events

        private void btSave_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private DataGridViewComboBoxCell LoadCellVariables()
        {
            var comboBoxCell = new DataGridViewComboBoxCell();

            foreach (Variable variable in Variables)
            {
                comboBoxCell.Items.Add(string.Format("@[{0}::{1}]", variable.Namespace, variable.Name));
            }

            return comboBoxCell;
        }

        private void btGO_Click(object sender, EventArgs e)
        {
            LoadDataBaseObjects();
        }

        private void chkTrustedConnection_Click(object sender, EventArgs e)
        {
            cmbLogin.Enabled = cmbPassword.Enabled = (chkTrustedConnection.Checked) ? false : true;
        }

        private void optFileConnection_Click(object sender, EventArgs e)
        {
            LoadFileConnections();
        }

        private void optFileVariable_Click(object sender, EventArgs e)
        {
            LoadFileVariables();
        }

        private void cmdFileVariable_Click(object sender, EventArgs e)
        {
            using (var expressionBuilder = ExpressionBuilder.Instantiate(_taskHost.Variables, _taskHost.VariableDispenser, Type.GetType("Sysytem.String"), cmdFileVariable.Text))
            {
                if (expressionBuilder.ShowDialog() == DialogResult.OK)
                {
                    cmdFileVariable.Text = expressionBuilder.Expression;
                }
            }
        }

        private void cmbStoredProcedures_Click(object sender, EventArgs e)
        {
            var interValue = cmbStoredProcedures.Text.Split(new [] { "].[" }, StringSplitOptions.None);
            string schema = interValue[0].Replace("[", string.Empty).Replace("]", string.Empty);
            string storedProcedure = interValue[1].Replace("[", string.Empty).Replace("]", string.Empty); ;
            LoadStoredProcedureParameters(schema, storedProcedure);
        }

        #endregion
    }
}
