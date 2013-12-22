using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace fyiReporting.Data
{
	public class JsonCommand : IDbCommand
	{

		JsonConnection _jc; // connection we're running under
		string _cmd; // command to execute
		int _Timeout; // timeout limit on invoking json service
		string _Url; // url of json service
		ArrayList _Columns; // Columns specified for the request
		DataParameterCollection _Parameters = new DataParameterCollection();

		public JsonCommand(JsonConnection conn)
		{
			_jc = conn;
		}

		internal string Url
		{
			get
			{
				// Check to see if "Url" or "@Url" is a parameter
				IDbDataParameter dp = _Parameters["Url"] as IDbDataParameter;
				if (dp == null)
					dp = _Parameters["@Url"] as IDbDataParameter;
				// Then check to see if the Url value is a parameter?
				if (dp == null)
					dp = _Parameters[_Url] as IDbDataParameter;
				if (dp != null)
					return dp.Value != null ? dp.Value.ToString() : _Url;	// don't pass null; pass existing value
				return _Url;
			}
			set {
				_Url = value;
			}
		}

		internal ArrayList Columns
		{
			get { return _Columns; }
			set { _Columns = value; }
		}

		public void Cancel()
		{
			throw new NotImplementedException("Cancel not implemented");
		}

		public void Prepare()
		{
			return;
		}

		public System.Data.CommandType CommandType
		{
			get{
				throw new NotImplementedException("CommandType not implemented");
			}
			set{
				throw new NotImplementedException("CommandType not implemented");
			}
		}

		public IDataReader ExecuteReader(System.Data.CommandBehavior behavior)
		{
			if (!(behavior == CommandBehavior.SingleResult ||
				behavior == CommandBehavior.SchemaOnly))
			{
				throw new ArgumentException("ExecuteReader supports SingleResult and SchemaOnly only.");
			}
			return new JsonDataReader(behavior, _jc, this);
		}

		IDataReader System.Data.IDbCommand.ExecuteReader()
		{
			return ExecuteReader(System.Data.CommandBehavior.SingleResult);
		}

		public object ExecuteScalar()
		{
			throw new NotImplementedException("ExecuteScalar not implemented");
		}

		public int ExecuteNonQuery()
		{
			throw new NotImplementedException("ExecuteNonQuery not implemented");
		}

		public int CommandTimeout
		{
			get
			{
				return _Timeout;
			}
			set
			{
				_Timeout = value;
			}
		}

		public IDbDataParameter CreateParameter()
		{
			return new JsonDataParameter();
		}

		public IDbConnection Connection
		{
			get
			{
				return this._jc;
			}
			set
			{
				throw new NotImplementedException("Setting connection not implemented");
			}
		}

		public System.Data.UpdateRowSource UpdatedRowSource
		{
			get
			{
				throw new NotImplementedException("UpdateRowSource not implemented");
			}
			set
			{
				throw new NotImplementedException("UpdateRowSource not implemented");
			}
		}

		public string CommandText
		{
			get
			{
				return this._cmd;
			}
			set
			{
				// Parse the command string for keyword value pairs separated by ';'
				string[] args = value.Split(';');
				string url = null;
				string[] columns = null;
				foreach (string arg in args)
				{
					string[] param = arg.Trim().Split('=');
					if (param == null || param.Length != 2)
					{
						continue;
					}
					string key = param[0].Trim().ToLower();
					string val = param[1];
					switch (key)
					{
						case "url":
						case "file":
							url = val;
							break;
						case "columns":
							columns = val.Trim().Split(',');
							break;
						default:
							throw new ArgumentException(string.Format("{0} is an unknown parameter key", param[0]));
					}
				}
				if (url == null)
				{
					throw new ArgumentException("CommandText requires 'Url' parameters");
				}
				_cmd = value;
				_Url = url;
				if (columns != null)
				{
					_Columns = new ArrayList(columns);
				}
			}
		}

		public IDataParameterCollection Parameters
		{
			get
			{
				return _Parameters;
			}
		}

		public IDbTransaction Transaction
		{
			get
			{
				throw new NotImplementedException("Transaction not implemented");
			}
			set
			{
				throw new NotImplementedException("Transaction not implemented");
			}
		}

		public void Dispose()
		{
		}

	}
}
