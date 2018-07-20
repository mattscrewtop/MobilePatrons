using System;
using System.Data.SqlClient;
using System.Reflection;
using CodeGenerator.Helpers;
using CodeGenerator.Implementations;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MobilePatronsApp.WebApi.Tests
{
	[TestClass]
	public class CodeGenerationTests
	{
		private readonly String codeGenerationSchemaName;
		private readonly String databaseName;
		private readonly SqlConnection sqlConnection;
		private readonly Server server;
		private readonly Database database;
		private readonly String projectRootPath;
		private readonly String projectName;
		private readonly String connectionString;
		private readonly String nameSpace;

		public CodeGenerationTests()
		{
			codeGenerationSchemaName = @"API";
			databaseName = @"MobilePatronsApp";
			projectRootPath = @"C:\\ProjectStoreGit\\Solutia-MobilePatronsApp\\Application\\Server\\MobilePatronsApp.Service\\MobilePatronsApp.";
			projectName = @"MobilePatronsApp";
			connectionString = @"Data Source=localhost;Initial Catalog=MobilePatronsApp;Integrated Security=True";
			nameSpace = @"MobilePatronsApp";


			//codeGenerationSchemaName = @"API";
			//databaseName = @"BASICS1.0";
			//projectRootPath = @"C:\\ProjectStoreGit\\Solutia-CandidateTracking\\Application\\Server\\CandidateTracking.";
			//projectName = @"CandidateTracking";
			//connectionString = @"Data Source=23.253.119.242;Initial Catalog=BASICS1.0;Persist Security Info=True;User ID=CandidateTrackingUser; Password=CandidateTrackingUser";
			//nameSpace = @"CandidateTracking";			 



			sqlConnection = new SqlConnection(connectionString);
			sqlConnection.Open();

			server = new Server(new ServerConnection(sqlConnection));
			database = new Database(server, databaseName);
		}

		[TestMethod]
		public void TestGetStoredProcedureList()
		{
			//ASSEMBLE
			database.Refresh();

			//ACT
			var storedProcList = CodeGeneratorHelper.GetStoredProcedureList(database, codeGenerationSchemaName);

			//ASSERT
			Assert.IsNotNull(storedProcList);
		}

		[TestMethod]
		public void TestGetStoredProcCodeGenerationMetaData()
		{
			//ASSEMBLE
			database.Refresh();
			var storedProcList = CodeGeneratorHelper.GetStoredProcedureList(database, codeGenerationSchemaName);
			var schemaName = storedProcList[0].SchemaName;
			var storeProcName = storedProcList[0].StoredProcName;


			//ACT
			var codeGenerationTargetMetaData = new CodeGenerationTargetMetaData
			{
				ProjectRootPath = projectRootPath,
				ProjectName = projectName,
				ConnectionString = connectionString,
				DatabaseName = databaseName,
				SchemaName = schemaName,
				StoredProcName = storeProcName,
				NameSpace = nameSpace
			};

			var storedProcResultSetMetaData = CodeGeneratorHelper.ProcessStoredProcedure(codeGenerationTargetMetaData);

			var methodName = storedProcResultSetMetaData.MethodName;

			//ASSERT
			Assert.IsTrue(storedProcResultSetMetaData.IsStoredProcValid);
			Assert.IsNotNull(codeGenerationTargetMetaData);
			Assert.IsNotNull(storedProcResultSetMetaData);
		}

		[TestMethod]
		public void TestGetStoredProcCodeGenerationMetaDataByStoredProcName()
		{
			//ASSEMBLE
			database.Refresh();
			const string schemaName = "API";
			const string storeProcName = "GameByGameId";
			const string assemblyPath = @"C:\ProjectStoreGit\Solutia-MobilePatronsApp\Application\Server\MobilePatronsApp.Service\MobilePatronsApp.DataContracts\bin\Debug\MobilePatronsApp.DataContracts.dll";
			//var assemblyPath = @"C:\ProjectStoreGit\Solutia-CandidateTracking\Application\Server\CandidateTracking.DataContracts\bin\Debug\CandidateTracking.DataContracts.dll";
			byte[] data;

			using (var fs = System.IO.File.OpenRead(assemblyPath))
			{
				data = new byte[fs.Length];
				fs.Read(data, 0, Convert.ToInt32(fs.Length));
			}

			if (data == null || data.Length == 0)
			{
				throw new ApplicationException("Failed to load " + assemblyPath);
			}

			var interfaceAssembly = System.Reflection.Assembly.Load(data);	

			//ACT
			var codeGenerationTargetMetaData = new CodeGenerationTargetMetaData
			{
				InterfaceAssembly = interfaceAssembly,
				ProjectRootPath = projectRootPath,
				ProjectName = projectName,
				ConnectionString = connectionString,
				DatabaseName = databaseName,
				SchemaName = schemaName,
				StoredProcName = storeProcName,
				NameSpace = nameSpace
			};

			var storedProcResultSetMetaData = CodeGeneratorHelper.ProcessStoredProcedure(codeGenerationTargetMetaData);

			var methodName = storedProcResultSetMetaData.MethodName;

			//ASSERT
			Assert.IsTrue(storedProcResultSetMetaData.IsStoredProcValid);
			Assert.IsNotNull(codeGenerationTargetMetaData);
			Assert.IsNotNull(storedProcResultSetMetaData);
		}
	}
}
