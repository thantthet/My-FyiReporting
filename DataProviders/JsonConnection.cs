using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace fyiReporting.Data
{
	public class JsonConnection : IDbConnection
	{
		string _Connection;  // the connection string; of format filepath= or url=
		bool bOpen = false;

		public JsonConnection(string conn)
		{
			ConnectionString = conn;
		}

		internal bool IsOpen
		{
			get { return bOpen; }
		}

		public void ChangeDatabase(string databaseName)
		{
			throw new NotImplementedException("ChangeDatabase method not supported.");
		}

		public IDbTransaction BeginTransaction(System.Data.IsolationLevel il)
		{
			throw new NotImplementedException("BeginTransaction method not supported.");
		}

		IDbTransaction System.Data.IDbConnection.BeginTransaction()
		{
			throw new NotImplementedException("BeginTransaction method not supported.");
		}

		public System.Data.ConnectionState State
		{
			get
			{
				throw new NotImplementedException("State not implemented");
			}
		}

		public string ConnectionString
		{
			get
			{
				return _Connection;
			}
			set
			{
				string c = value;
				// Now parse the connection string;
				Array args = c.Split(',');
				string filepath = null;
				Uri url = null;
				foreach (string arg in args)
				{
					if (arg.Trim().ToLower().StartsWith("filepath="))
					{
						filepath = arg.Trim().Split('=').GetValue(1) as string;
						_Connection = filepath;
					}
					if (arg.Trim().ToLower().StartsWith("url="))
					{
						url = new Uri(arg.Trim().Split('=').GetValue(1) as string);
						_Connection = url.ToString();
					}
				}
				//_Connection = value;
			}
		}

		public IDbCommand CreateCommand()
		{
			return new JsonCommand(this);
		}

		public void Open()
		{

			if (!this.ConnectionString.StartsWith("http"))
			{
				if (System.IO.File.Exists(this.ConnectionString) == false)
				{
					throw new System.IO.FileNotFoundException();
				}
			}

			bOpen = true;
		}

		public void Close()
		{
			bOpen = false;
		}

		public string Database
		{
			get
			{
				return null;
			}
		}

		public int ConnectionTimeout
		{
			get
			{
				return 0;
			}
		}

		public void Dispose()
		{
			this.Close();
		}

	}
}
