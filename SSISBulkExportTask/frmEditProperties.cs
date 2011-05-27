using System;
using System.Collections.Generic;
using System.Data;
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

            _connections = connections;

            //grdParameters.DataError += grdParameters_DataError(object sender, );

            _taskHost = taskHost;
            _isFirstLoad = true;

            try
            {
                Cursor = Cursors.WaitCursor;

                optFileVariable.CheckedChanged -= (optFileVariable_CheckedChanged_1);
                optFileVariable.Click -= (optFileVariable_Click);
                optFileConnection.CheckedChanged -= (optFileConnection_CheckedChanged_1);
                cmdFileVariable.Click -= (cmdFileVariable_Click);
                optFileFormatVariable.CheckedChanged -= (optFileFormatVariable_CheckedChanged_1);
                optFileFormatVariable.Click -= (optFileFormatVariable_CheckedChanged);
                optFileFormatConnection.CheckedChanged -= (optFileFormatConnection_CheckedChanged_1);
                optFileFormatConnection.ChangeUICues -= (optFileFormatConnection_CheckedChanged);

                LoadDbConnections();

                if (_taskHost.Properties[Keys.SQL_SERVER].GetValue(_taskHost) != null)
                {
                    cmbSQLServer.SelectedIndex = FindStringInComboBox(cmbSQLServer, _taskHost.Properties[Keys.SQL_SERVER].GetValue(_taskHost).ToString(), -1);

                    if (!string.IsNullOrEmpty(cmbSQLServer.Text))
                        LoadDataBaseObjects();
                }

                if (_taskHost.Properties[Keys.NETWORK_PACKET_SIZE].GetValue(_taskHost) != null)
                    txPacketSize.Text = _taskHost.Properties[Keys.NETWORK_PACKET_SIZE].GetValue(_taskHost).ToString();

                if (_taskHost.Properties[Keys.USE_REGIONAL_SETTINGS].GetValue(_taskHost) != null)
                {
                    bool isCheckedRegionalSettings;
                    Boolean.TryParse(_taskHost.Properties[Keys.USE_REGIONAL_SETTINGS].GetValue(_taskHost).ToString(),
                                     out isCheckedRegionalSettings);
                    chkRegionalSettings.Checked = isCheckedRegionalSettings;
                }

                if (_taskHost.Properties[Keys.FIRSTROW].GetValue(_taskHost) != null)
                    txFirstRow.Text = _taskHost.Properties[Keys.FIRSTROW].GetValue(_taskHost).ToString();

                if (_taskHost.Properties[Keys.LASTROW].GetValue(_taskHost) != null)
                    txLastRow.Text = _taskHost.Properties[Keys.LASTROW].GetValue(_taskHost).ToString();

                if (_taskHost.Properties[Keys.FIELD_TERMINATOR].GetValue(_taskHost) != null)
                    txFieldTerminator.Text = _taskHost.Properties[Keys.FIELD_TERMINATOR].GetValue(_taskHost).ToString();

                if (_taskHost.Properties[Keys.ROW_TERMINATOR].GetValue(_taskHost) != null)
                    txRowTerminator.Text = _taskHost.Properties[Keys.ROW_TERMINATOR].GetValue(_taskHost).ToString();

                if (_taskHost.Properties[Keys.SQL_STATEMENT].GetValue(_taskHost) != null)
                    txSQL.Text = _taskHost.Properties[Keys.SQL_STATEMENT].GetValue(_taskHost).ToString();

                if (_taskHost.Properties[Keys.DESTINATION_FILE_CONNECTION].GetValue(_taskHost) != null)
                {
                    if (_taskHost.Properties[Keys.DESTINATION_FILE_CONNECTION].GetValue(_taskHost).ToString() == Keys.TRUE)
                    {
                        optFileConnection.Checked = true;
                        LoadFileConnectionsDestination();
                    }
                    else
                    {
                        optFileVariable.Checked = true;
                        LoadFileVariablesDestination();
                    }
                }
                else
                {
                    optFileConnection.Checked = true;
                    LoadFileConnectionsDestination();
                }

                if (_taskHost.Properties[Keys.FORMAT_FILE_CONNECTION].GetValue(_taskHost) != null)
                {
                    if (_taskHost.Properties[Keys.FORMAT_FILE_CONNECTION].GetValue(_taskHost).ToString() == Keys.TRUE)
                    {
                        optFileFormatConnection.Checked = true;
                        LoadFileConnectionsFileFormat();
                    }
                    else
                    {
                        optFileFormatVariable.Checked = true;
                        LoadFileVariablesFileFormat();
                    }
                }
                else
                {
                    optFileFormatConnection.Checked = true;
                    LoadFileConnectionsFileFormat();
                }

                string selectedItem = string.Empty;

                cmbLogin.Items.Clear();
                cmbPassword.Items.Clear();
                cmbPassword = cmbLogin = LoadVariables("System.String", ref selectedItem);

                if (_taskHost.Properties[Keys.TRUSTED_CONNECTION].GetValue(_taskHost) != null)
                {
                    bool isCheckedTrustedConnection;
                    Boolean.TryParse(_taskHost.Properties[Keys.TRUSTED_CONNECTION].GetValue(_taskHost).ToString(), out isCheckedTrustedConnection);
                    chkTrustedConnection.Checked = isCheckedTrustedConnection;
                    label10.Enabled = label11.Enabled = cmbPassword.Enabled = cmbLogin.Enabled = !chkTrustedConnection.Checked;
                }

                if (_taskHost.Properties[Keys.SET_QUOTED_IDENTIFIERS_ON].GetValue(_taskHost) != null)
                {
                    bool isSET_QUOTED_IDENTIFIERS_ON;
                    Boolean.TryParse(_taskHost.Properties[Keys.SET_QUOTED_IDENTIFIERS_ON].GetValue(_taskHost).ToString(), out isSET_QUOTED_IDENTIFIERS_ON);
                    chkQuotes.Checked = isSET_QUOTED_IDENTIFIERS_ON;
                }

                if (_taskHost.Properties[Keys.UNICODE_CHR].GetValue(_taskHost) != null)
                {
                    bool isUnicode;
                    Boolean.TryParse(_taskHost.Properties[Keys.UNICODE_CHR].GetValue(_taskHost).ToString(), out isUnicode);
                    chkUnicode.Checked = isUnicode;
                }

                if (_taskHost.Properties[Keys.CHARACTER_DATA_TYPE].GetValue(_taskHost) != null)
                {
                    bool isChrDataType;
                    Boolean.TryParse(_taskHost.Properties[Keys.CHARACTER_DATA_TYPE].GetValue(_taskHost).ToString(), out isChrDataType);
                    chkChDataType.Checked = isChrDataType;
                }

                if (_taskHost.Properties[Keys.SQL_STATEMENT].GetValue(_taskHost) != null)
                    txSQL.Text = _taskHost.Properties[Keys.SQL_STATEMENT].GetValue(_taskHost).ToString();

                if (_taskHost.Properties[Keys.SQL_VIEW].GetValue(_taskHost) != null)
                    cmbViews.SelectedIndex = FindStringInComboBox(cmbViews, _taskHost.Properties[Keys.SQL_VIEW].GetValue(_taskHost).ToString(), -1);

                if (_taskHost.Properties[Keys.SQL_StoredProcedure].GetValue(_taskHost) != null)
                    cmbStoredProcedures.SelectedIndex = FindStringInComboBox(cmbStoredProcedures, _taskHost.Properties[Keys.SQL_StoredProcedure].GetValue(_taskHost).ToString(), -1);

                if (_taskHost.Properties[Keys.SQL_TABLE].GetValue(_taskHost) != null)
                    cmbTables.SelectedIndex = FindStringInComboBox(cmbTables, _taskHost.Properties[Keys.SQL_TABLE].GetValue(_taskHost).ToString(), -1);

                if (_taskHost.Properties[Keys.DESTINATION].GetValue(_taskHost) != null)
                    cmbDestination.SelectedIndex = FindStringInComboBox(cmbDestination, _taskHost.Properties[Keys.DESTINATION].GetValue(_taskHost).ToString(), -1);

                if (_taskHost.Properties[Keys.FORMAT_FILE].GetValue(_taskHost) != null)
                    cmbFormatFile.SelectedIndex = FindStringInComboBox(cmbFormatFile, _taskHost.Properties[Keys.FORMAT_FILE].GetValue(_taskHost).ToString(), -1);

                if (_taskHost.Properties[Keys.SRV_LOGIN].GetValue(_taskHost) != null)
                    cmbLogin.SelectedIndex = FindStringInComboBox(cmbLogin, _taskHost.Properties[Keys.SRV_LOGIN].GetValue(_taskHost).ToString(), -1);

                if (_taskHost.Properties[Keys.SRV_PASSWORD].GetValue(_taskHost) != null)
                    cmbPassword.SelectedIndex = FindStringInComboBox(cmbDestination, _taskHost.Properties[Keys.SRV_PASSWORD].GetValue(_taskHost).ToString(), -1);

                if (_taskHost.Properties[Keys.CODE_PAGE].GetValue(_taskHost) != null)
                    cmbCodePage.Text = _taskHost.Properties[Keys.CODE_PAGE].GetValue(_taskHost).ToString();

                if (_taskHost.Properties[Keys.ACTIVATE_CMDSHELL].GetValue(_taskHost) != null)
                    chkRightsCMDSHELL.Checked = (_taskHost.Properties[Keys.ACTIVATE_CMDSHELL].GetValue(_taskHost).ToString() == Keys.TRUE) ? true : false;

                if (_taskHost.Properties[Keys.DATA_SOURCE].GetValue(_taskHost) != null)
                    tabControl.SelectedTab = tabControl.TabPages.Cast<TabPage>().Where(tab => tab.Text == _taskHost.Properties[Keys.DATA_SOURCE].GetValue(_taskHost).ToString()).FirstOrDefault();

                optFileVariable.CheckedChanged += (optFileVariable_CheckedChanged_1);
                optFileVariable.Click += (optFileVariable_Click);
                optFileConnection.CheckedChanged += (optFileConnection_CheckedChanged_1);
                cmdFileVariable.Click += (cmdFileVariable_Click);
                optFileFormatVariable.CheckedChanged += (optFileFormatVariable_CheckedChanged_1);
                optFileFormatVariable.Click += (optFileFormatVariable_CheckedChanged);
                optFileFormatConnection.CheckedChanged += (optFileFormatConnection_CheckedChanged_1);
                optFileFormatConnection.ChangeUICues += (optFileFormatConnection_CheckedChanged);

                _isFirstLoad = false;
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

        private void LoadFileConnectionsDestination()
        {
            cmbDestination.Items.Clear();

            foreach (var connection in Connections.Cast<ConnectionManager>().Where(connection => connection.CreationName == "FILE"))
            {
                cmbDestination.Items.Add(connection.Name);
            }
        }

        private void LoadFileConnectionsFileFormat()
        {
            cmbFormatFile.Items.Clear();

            foreach (var connection in Connections.Cast<ConnectionManager>().Where(connection => connection.CreationName == "FILE"))
            {
                cmbFormatFile.Items.Add(connection.Name);
            }
        }

        private void LoadDbConnections()
        {
            cmbSQLServer.Items.Clear();

            foreach (var connection in Connections.Cast<ConnectionManager>().Where(connection => connection.CreationName.Contains("ADO.NET")))
            {
                cmbSQLServer.Items.Add(connection.Name);
            }
        }

        private void LoadFileVariablesDestination()
        {
            cmbDestination.Items.Clear();
            cmbDestination.Items.AddRange(LoadVariables("System.String").ToArray());
        }

        private void LoadFileVariablesFileFormat()
        {
            cmbFormatFile.Items.Clear();
            cmbFormatFile.Items.AddRange(LoadVariables("System.String").ToArray());
        }

        private List<string> LoadVariables(string parameterInfo)
        {
            return Variables.Cast<Variable>().Where(variable => Type.GetTypeCode(Type.GetType(parameterInfo)) == variable.DataType).Select(variable => string.Format("@[{0}::{1}]", variable.Namespace, variable.Name)).ToList();
        }

        private void LoadDataBaseObjects()
        {
            Cursor = Cursors.WaitCursor;

            using (var sqlConnection = new SqlConnection(EvaluateExpression(_connections[cmbSQLServer.Text].ConnectionString, _taskHost.VariableDispenser).ToString()))
            {
                sqlConnection.Open();
                {
                    using (var sqlCommand = new SqlCommand(QueryResources.TABLES, sqlConnection))
                    {
                        using (var sqlDataReaderTables = sqlCommand.ExecuteReader())
                        {
                            cmbTables.Items.Clear();

                            if (sqlDataReaderTables != null)
                                while (sqlDataReaderTables.Read())
                                {
                                    cmbTables.Items.Add(sqlDataReaderTables.GetString(0));
                                }
                        }
                    }
                }

                {
                    using (var sqlCommand = new SqlCommand(QueryResources.VIEWS, sqlConnection))
                    {
                        using (var sqlDataReaderTables = sqlCommand.ExecuteReader())
                        {
                            cmbViews.Items.Clear();

                            if (sqlDataReaderTables != null)
                                while (sqlDataReaderTables.Read())
                                {
                                    cmbViews.Items.Add(sqlDataReaderTables.GetString(0));
                                }
                        }
                    }
                }

                {
                    using (var sqlCommand = new SqlCommand(QueryResources.STORED_PROCEDURES, sqlConnection))
                    {
                        using (var sqlDataReaderTables = sqlCommand.ExecuteReader())
                        {
                            cmbStoredProcedures.Items.Clear();

                            if (sqlDataReaderTables != null)
                                while (sqlDataReaderTables.Read())
                                {
                                    cmbStoredProcedures.Items.Add(sqlDataReaderTables.GetString(0));
                                }
                        }
                    }
                }
                sqlConnection.Close();
            }

            Cursor = Cursors.Arrow;
        }

        private void LoadStoredProcedureParameters(string schema, string storedProcedureName)
        {
            Cursor = Cursors.WaitCursor;

            using (var sqlConnection = new SqlConnection(EvaluateExpression(_connections[cmbSQLServer.Text].ConnectionString, _taskHost.VariableDispenser).ToString()))
            {
                sqlConnection.Open();

                using (var sqlCommand = new SqlCommand(string.Format(QueryResources.STORED_PROCEDURE_PARAMETERS, schema, storedProcedureName), sqlConnection))
                {
                    using (var sqlDataReaderTables = sqlCommand.ExecuteReader())
                    {
                        grdParameters.Rows.Clear();

                        if (sqlDataReaderTables != null)
                            while (sqlDataReaderTables.Read())
                            {
                                int index = grdParameters.Rows.Add();

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

                sqlConnection.Close();
            }

            if (_isFirstLoad)
            {
                var mappingParams = (MappingParams)_taskHost.Properties[Keys.SQL_STORED_PROCEDURE_PARAMS].GetValue(_taskHost);

                if (mappingParams != null)
                    foreach (MappingParam mappingParam in mappingParams)
                    {
                        foreach (DataGridViewRow row in grdParameters.Rows.Cast<DataGridViewRow>().Where(row => row.Cells[0].Value.ToString() == mappingParam.Name))
                        {
                            row.Cells[2].Value = mappingParam.Value;
                        }
                    }
            }


            Cursor = Cursors.Arrow;
        }

        #endregion

        #region Events

        void grdParameters_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            //throw new NotImplementedException();
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            _taskHost.Properties[Keys.SQL_SERVER].SetValue(_taskHost, cmbSQLServer.Text);

            _taskHost.Properties[Keys.FORMAT_FILE_CONNECTION].SetValue(_taskHost, optFileFormatConnection.Checked ? Keys.TRUE : Keys.FALSE);
            _taskHost.Properties[Keys.FORMAT_FILE].SetValue(_taskHost, cmbFormatFile.Text);

            _taskHost.Properties[Keys.DESTINATION_FILE_CONNECTION].SetValue(_taskHost, optFileConnection.Checked ? Keys.TRUE : Keys.FALSE);
            _taskHost.Properties[Keys.DESTINATION].SetValue(_taskHost, cmbDestination.Text);

            _taskHost.Properties[Keys.TRUSTED_CONNECTION].SetValue(_taskHost, chkTrustedConnection.Checked ? Keys.TRUE : Keys.FALSE);
            _taskHost.Properties[Keys.SRV_LOGIN].SetValue(_taskHost, !chkTrustedConnection.Checked ? cmbLogin.Text : string.Empty);
            _taskHost.Properties[Keys.SRV_PASSWORD].SetValue(_taskHost, !chkTrustedConnection.Checked ? cmbPassword.Text : string.Empty);

            _taskHost.Properties[Keys.NATIVE_DB_DATATYPE].SetValue(_taskHost, chkNativeDatabase.Checked ? Keys.TRUE : Keys.FALSE);

            _taskHost.Properties[Keys.FIRSTROW].SetValue(_taskHost, txFirstRow.Text.Trim());
            _taskHost.Properties[Keys.LASTROW].SetValue(_taskHost, txLastRow.Text.Trim());

            _taskHost.Properties[Keys.FIELD_TERMINATOR].SetValue(_taskHost, txFieldTerminator.Text.Trim());
            _taskHost.Properties[Keys.ROW_TERMINATOR].SetValue(_taskHost, txRowTerminator.Text.Trim());

            _taskHost.Properties[Keys.ACTIVATE_CMDSHELL].SetValue(_taskHost, chkRightsCMDSHELL.Checked ? Keys.TRUE : Keys.FALSE);

            _taskHost.Properties[Keys.SQL_STATEMENT].SetValue(_taskHost, txSQL.Text.Trim());
            _taskHost.Properties[Keys.SQL_VIEW].SetValue(_taskHost, cmbViews.Text);
            _taskHost.Properties[Keys.SQL_StoredProcedure].SetValue(_taskHost, cmbStoredProcedures.Text);
            _taskHost.Properties[Keys.SQL_TABLE].SetValue(_taskHost, cmbTables.Text);

            _taskHost.Properties[Keys.CODE_PAGE].SetValue(_taskHost, cmbCodePage.Text.Trim());
            _taskHost.Properties[Keys.DATA_SOURCE].SetValue(_taskHost, tabControl.SelectedTab.Text);
            _taskHost.Properties[Keys.NETWORK_PACKET_SIZE].SetValue(_taskHost, txPacketSize.Text.Trim());
            _taskHost.Properties[Keys.USE_REGIONAL_SETTINGS].SetValue(_taskHost, chkRegionalSettings.Checked ? Keys.TRUE : Keys.FALSE);

            _taskHost.Properties[Keys.SET_QUOTED_IDENTIFIERS_ON].SetValue(_taskHost, chkQuotes.Checked ? Keys.TRUE : Keys.FALSE);
            _taskHost.Properties[Keys.CHARACTER_DATA_TYPE].SetValue(_taskHost, chkChDataType.Checked ? Keys.TRUE : Keys.FALSE);
            _taskHost.Properties[Keys.UNICODE_CHR].SetValue(_taskHost, chkUnicode.Checked ? Keys.TRUE : Keys.FALSE);

            var mappingParams = new MappingParams();
            mappingParams.AddRange(from DataGridViewRow row in grdParameters.Rows
                                   select new MappingParam
                                   {
                                       Name = row.Cells[0].Value.ToString(),
                                       Type = row.Cells[1].Value.ToString(),
                                       Value = row.Cells[5].Value.ToString()
                                   });

            _taskHost.Properties[Keys.SQL_STORED_PROCEDURE_PARAMS].SetValue(_taskHost, mappingParams);

            string val = (from db in _connections[cmbSQLServer.Text].ConnectionString.Split(';')
                          where db.Contains("Initial Catalog")
                          select db).FirstOrDefault();

            _taskHost.Properties[Keys.SQL_DATABASE].SetValue(_taskHost, string.Format("[{0}]", val.Split('=')[1]));

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
            label10.Enabled = label11.Enabled = cmbLogin.Enabled = cmbPassword.Enabled = (chkTrustedConnection.Checked) ? false : true;
        }

        private void optFileConnection_Click(object sender, EventArgs e)
        {
            LoadFileConnectionsDestination();
        }

        private void optFileVariable_Click(object sender, EventArgs e)
        {
            LoadFileVariablesDestination();
        }

        private void cmdFileVariable_Click(object sender, EventArgs e)
        {
            using (var expressionBuilder = ExpressionBuilder.Instantiate(_taskHost.Variables, _taskHost.VariableDispenser, Type.GetType("Sysytem.String"), cmbDestination.Text))
            {
                if (expressionBuilder.ShowDialog() == DialogResult.OK)
                {
                    cmbDestination.Text = expressionBuilder.Expression;
                }
            }
        }

        private void cmdFileFormatVariable_Click(object sender, EventArgs e)
        {
            using (var expressionBuilder = ExpressionBuilder.Instantiate(_taskHost.Variables, _taskHost.VariableDispenser, Type.GetType("Sysytem.String"), cmbFormatFile.Text))
            {
                if (expressionBuilder.ShowDialog() == DialogResult.OK)
                {
                    cmbFormatFile.Text = expressionBuilder.Expression;
                }
            }
        }

        private void cmbStoredProcedures_SelectedIndexChanged(object sender, EventArgs e)
        {
            var interValue = cmbStoredProcedures.Text.Split(new[] { "].[" }, StringSplitOptions.None);
            string schema = interValue[0].Replace("[", string.Empty).Replace("]", string.Empty);
            string storedProcedure = interValue[1].Replace("[", string.Empty).Replace("]", string.Empty); ;
            LoadStoredProcedureParameters(schema, storedProcedure);
        }

        private void optFileFormatConnection_CheckedChanged(object sender, EventArgs e)
        {
            LoadFileConnectionsFileFormat();
        }

        private void optFileFormatVariable_CheckedChanged(object sender, EventArgs e)
        {
            LoadFileVariablesFileFormat();
        }

        private void grdParameters_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case 3:
                    {
                        SqlDbType sqlDbType;
                        try
                        {
                            sqlDbType = (SqlDbType)Enum.Parse(typeof(SqlDbType), grdParameters.Rows[e.RowIndex].Cells[1].Value.ToString(), true);
                        }
                        catch
                        {
                            sqlDbType = SqlDbType.NVarChar;
                        }

                        using (ExpressionBuilder expressionBuilder = ExpressionBuilder.Instantiate(_taskHost.Variables,
                                                                                                _taskHost.VariableDispenser,
                                                                                                SQLGoodies.ConvertFromSqlDbType(sqlDbType),
                                                                                                string.Empty))
                        {
                            if (expressionBuilder.ShowDialog() == DialogResult.OK)
                            {
                                ((DataGridViewComboBoxCell)grdParameters.Rows[e.RowIndex].Cells[e.ColumnIndex - 1]).Items.Add(expressionBuilder.Expression);
                                grdParameters.Rows[e.RowIndex].Cells[e.ColumnIndex - 1].Value = expressionBuilder.Expression;
                            }
                        }
                    }

                    break;
            }
        }

        #endregion

        private void optFileConnection_CheckedChanged_1(object sender, EventArgs e)
        {
            cmdFileVariable.Enabled = false;
            LoadFileConnectionsDestination();
        }

        private void optFileVariable_CheckedChanged_1(object sender, EventArgs e)
        {
            cmdFileVariable.Enabled = true;
            LoadFileVariablesDestination();
        }

        private void optFileFormatVariable_CheckedChanged_1(object sender, EventArgs e)
        {
            cmdFileFormatVariable.Enabled = true;
            LoadFileVariablesFileFormat();
        }

        private void optFileFormatConnection_CheckedChanged_1(object sender, EventArgs e)
        {
            cmdFileFormatVariable.Enabled = false;
            LoadFileConnectionsFileFormat();
        }

        private void chkTrustedConnection_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
