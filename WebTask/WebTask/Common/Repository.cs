using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Dapper;


namespace WebTask.Common
{
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		private IConnectionFactory connectionFactory;

		public Repository(IConnectionFactory connectionFactory)
		{
			this.connectionFactory = connectionFactory;
		}
		public IEnumerable<TEntity> GetEntitiesBySP(string storedProcedureName, Dictionary<string, Tuple<string, DbType, ParameterDirection>> parameters)
		{
			try
			{
				DynamicParameters dynamicParameters = new DynamicParameters();

				foreach (KeyValuePair<string, Tuple<string, DbType, ParameterDirection>> entry in parameters)
				{
					if (entry.Value.Item2 == DbType.Guid)
					{
						Guid guid = new Guid(entry.Value.Item1);
						dynamicParameters.Add(entry.Key, guid, DbType.Guid, entry.Value.Item3);
					}
					else
					{
						dynamicParameters.Add(entry.Key, entry.Value.Item1, entry.Value.Item2, entry.Value.Item3);
					}
				}

				using (var connection = connectionFactory.GetConnection())
				{
					connection.Open();

					IEnumerable<TEntity> result = connection.Query<TEntity>(
											storedProcedureName,
											param: dynamicParameters,
											commandType: CommandType.StoredProcedure);

					return result;
				}

			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

		public int ExecuteSPWithInputOutput(string storedProcedureName, Dictionary<string, Tuple<string, DbType, ParameterDirection>> parameters)
		{

			DynamicParameters dynamicParameters = new DynamicParameters();
			DbType outputType;
			string outputName = "";

			foreach (KeyValuePair<string, Tuple<string, DbType, ParameterDirection>> entry in parameters)
			{
				if (entry.Value.Item2 == DbType.Guid)
				{
					Guid guid = new Guid(entry.Value.Item1);
					dynamicParameters.Add(entry.Key, guid, DbType.Guid, entry.Value.Item3);
				}
				else
				{
					dynamicParameters.Add(entry.Key, entry.Value.Item1, entry.Value.Item2, entry.Value.Item3);
				}

				if (entry.Value.Item3 == ParameterDirection.Output || entry.Value.Item3 == ParameterDirection.InputOutput)
				{
					outputType = entry.Value.Item2;
					outputName = entry.Key;
				}
			}

			using (var connection = connectionFactory.GetConnection())
			{
				connection.Open();

				object result = connection.Query<TEntity>(
				  storedProcedureName,
				  param: dynamicParameters,
				  commandType: CommandType.StoredProcedure).FirstOrDefault();

				return dynamicParameters.Get<int>(outputName);
			}
		}
		//public int ExecuteSPWithOutPut(string storedProcedureName, Dictionary<string, Tuple<string, DbType, ParameterDirection>> parameters)
		//{
		//	DynamicParameters dynamicParameters = new DynamicParameters();
		//	DbType outputType;
		//	string outputName = "";

		//	foreach (KeyValuePair<string, Tuple<string, DbType, ParameterDirection>> entry in parameters)
		//	{
		//		if (entry.Value.Item2 == DbType.Guid)
		//		{
		//			Guid guid = new Guid(entry.Value.Item1);
		//			dynamicParameters.Add(entry.Key, guid, DbType.Guid, entry.Value.Item3);
		//		}
		//		else
		//		{
		//			dynamicParameters.Add(entry.Key, entry.Value.Item1, entry.Value.Item2, entry.Value.Item3);
		//		}

		//		if (entry.Value.Item3 == ParameterDirection.Output)
		//		{
		//			outputType = entry.Value.Item2;
		//			outputName = entry.Key;
		//		}
		//	}
		//}
	}
}
