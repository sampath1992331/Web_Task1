using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using WebTask.Common;
using WebTask.Models;

namespace WebTask.Services
{
    public class Main :IMain
    {
		private readonly IUnitOfWork unitOfWork;
		

		public Main(IUnitOfWork unitOfWork)
		{
			this.unitOfWork = unitOfWork;
		}

        public APIResponse GetCustomersNameList()
		{
            try
            {
				Dictionary<string, Tuple<string, DbType, ParameterDirection>> parameters = new Dictionary<string, Tuple<string, DbType, ParameterDirection>>
				{
				};
				
				var data	= unitOfWork.Repository<CustomerModel>().GetEntitiesBySP("[dbo].[GetCustomersNameList]", parameters);
                return APIResponseGenerator.GenerateResponseMessage("1", "Success", data);
            }
            catch (Exception ex)
            {

                return APIResponseGenerator.GenerateResponseMessage("Error", ex.ToString(), null) ;
            }
			
		}
		public APIResponse GetCustomersDetailsByID(int customerid)
		{
			try
			{
				Dictionary<string, Tuple<string, DbType, ParameterDirection>> parameters = new Dictionary<string, Tuple<string, DbType, ParameterDirection>>
				{
					{ "@Id", Tuple.Create(customerid.ToString(), DbType.Int32, ParameterDirection.Input) },
				};
				var data = unitOfWork.Repository<CustomerModel>().GetEntitiesBySP("[dbo].[GetCustomersDetailsByID]", parameters);
				return APIResponseGenerator.GenerateResponseMessage("1", "Success", data);
			}
			catch (Exception ex)
			{

				return APIResponseGenerator.GenerateResponseMessage("Error", ex.ToString(), null);
			}

		}
		public APIResponse GetItemsDetailsByID(int Itemid)
		{
			try
			{
				Dictionary<string, Tuple<string, DbType, ParameterDirection>> parameters = new Dictionary<string, Tuple<string, DbType, ParameterDirection>>
				{
					{ "@Id", Tuple.Create(Itemid.ToString(), DbType.Int32, ParameterDirection.Input) },
				};
				var data = unitOfWork.Repository<ItemModel>().GetEntitiesBySP("[dbo].[GetItemsDetailsByID]", parameters);
				return APIResponseGenerator.GenerateResponseMessage("1", "Success", data);
			}
			catch (Exception ex)
			{

				return APIResponseGenerator.GenerateResponseMessage("Error", ex.ToString(), null);
			}

		}

		public APIResponse GetAllItemCode()
		{
			try
			{
				Dictionary<string, Tuple<string, DbType, ParameterDirection>> parameters = new Dictionary<string, Tuple<string, DbType, ParameterDirection>>
				{

				};
				var data = unitOfWork.Repository<ItemModel>().GetEntitiesBySP("[dbo].[GetAllItemCode]", parameters);
				return APIResponseGenerator.GenerateResponseMessage("1", "Success", data);
			}
			catch (Exception ex)
			{

				return APIResponseGenerator.GenerateResponseMessage("Error", ex.ToString(), null);
			}

		}
		public APIResponse SaveOrder(OrderModel orderModel)
		{
			try
			{
				var statusCode = "-1";
				var message = "";
				Dictionary<string, Tuple<string, DbType, ParameterDirection>> parameters = new Dictionary<string, Tuple<string, DbType, ParameterDirection>>
				{
					{ "@Note", Tuple.Create(orderModel.Note.ToString(), DbType.String, ParameterDirection.Input) },
					{ "@InvoiceNo", Tuple.Create(orderModel.InvoiceNo.ToString(), DbType.String, ParameterDirection.Input) },
					{ "@InvoiceDate", Tuple.Create(orderModel.InvoiceDate.ToString(), DbType.DateTime, ParameterDirection.Input) },
					{ "@ReferenceNo", Tuple.Create(orderModel.ReferenceNo.ToString(), DbType.String, ParameterDirection.Input) },
					{ "@Status", Tuple.Create(0.ToString(), DbType.Int32, ParameterDirection.Output) },
				};
				var orderId = unitOfWork.Repository<string>().ExecuteSPWithInputOutput("[dbo].[SaveOrder]", parameters);
				if (orderId > 0)
				{
					int result = InsertItemsByOrder(orderModel, orderId);
					if (result > 0)
					{
						statusCode = "1";
						message = "Successfully Saved";
					}
				}
				else
				{
						statusCode = "-1";
						message = "Error, Not Saved";
				}
				return APIResponseGenerator.GenerateResponseMessage(statusCode, message, "");

			}
			catch (Exception ex)
			{
				return APIResponseGenerator.GenerateResponseMessage("Error", ex.ToString(), null);
			}

		}
		public APIResponse GetAllOrders()
		{
			try
			{
				Dictionary<string, Tuple<string, DbType, ParameterDirection>> parameters = new Dictionary<string, Tuple<string, DbType, ParameterDirection>>
				{

				};
				var data = unitOfWork.Repository<OrderModel>().GetEntitiesBySP("[dbo].[GetAllOrders]", parameters);
				return APIResponseGenerator.GenerateResponseMessage("1", "Success", data);
			}
			catch (Exception ex)
			{

				return APIResponseGenerator.GenerateResponseMessage("Error", ex.ToString(), null);
			}

		}
		public APIResponse GetOrdersById(int id)
		{
			try
			{
				Dictionary<string, Tuple<string, DbType, ParameterDirection>> parameters = new Dictionary<string, Tuple<string, DbType, ParameterDirection>>
				{
					{ "@Id", Tuple.Create(id.ToString(), DbType.String, ParameterDirection.Input) },
				};
				var data = unitOfWork.Repository<OrderModel>().GetEntitiesBySP("[dbo].[GetOrdersById]", parameters);
				return APIResponseGenerator.GenerateResponseMessage("1", "Success", data);
			}
			catch (Exception ex)
			{

				return APIResponseGenerator.GenerateResponseMessage("Error", ex.ToString(), null);
			}

		}
		private decimal CalcualteExclAmount(ItemModel itemModel) 
		{
			decimal exclAmount = 0;
			exclAmount = itemModel.Quantity * itemModel.Price;
			return exclAmount;
		}
		private decimal CalcualteTaxAmount(decimal taxrate,decimal exclAmount)
		{
			decimal taxAmount = 0;
			taxAmount = (exclAmount * taxrate) / 100;
			return taxAmount;
		}
		private decimal CalcualteInclAmount(decimal taxamount, decimal exclAmount)
		{
			decimal taxAmount = 0;
			taxAmount = exclAmount + taxamount;
			return taxAmount;
		}
		private int InsertItemsByOrder(OrderModel orderModel,int orderId)
		{
            try
            {
				int result = -100;
				int count = 0;
				foreach (var l in orderModel.ItemList)
				{
					decimal exclAmount = CalcualteExclAmount(l);
					decimal taxAmount = CalcualteTaxAmount(5, exclAmount);
					decimal inclAmount = CalcualteInclAmount(taxAmount, exclAmount);
					Dictionary<string, Tuple<string, DbType, ParameterDirection>> parameters = new Dictionary<string, Tuple<string, DbType, ParameterDirection>>
					{
						{ "@OrderId", Tuple.Create(orderId.ToString(), DbType.Int32, ParameterDirection.Input) },
						{ "@ItemCode", Tuple.Create(l.ItemCode.ToString(), DbType.String, ParameterDirection.Input) },
						{ "@Description", Tuple.Create(l.Description.ToString(), DbType.String, ParameterDirection.Input) },
						{ "@Note", Tuple.Create(l.Note.ToString(), DbType.String, ParameterDirection.Input) },
						{ "@Quantity", Tuple.Create(l.Quantity.ToString(), DbType.Int32, ParameterDirection.Input) },
						{ "@ExclAmount", Tuple.Create(exclAmount.ToString(), DbType.Decimal, ParameterDirection.Input) },
						{ "@TaxAmount", Tuple.Create(taxAmount.ToString(), DbType.Decimal, ParameterDirection.Input) },
						{ "@InclAmount", Tuple.Create(inclAmount.ToString(), DbType.Decimal, ParameterDirection.Input) },
						{ "@Price", Tuple.Create(l.Price.ToString(), DbType.Decimal, ParameterDirection.Input) },
						{ "@Status", Tuple.Create(0.ToString(), DbType.Int32, ParameterDirection.Output) },
					};
					var id = unitOfWork.Repository<string>().ExecuteSPWithInputOutput("[dbo].[SaveItemsByOrder]", parameters);
					if (id > 0)
					{
						count++;
					}
				}
				if (count == orderModel.ItemList.Count)
				{
					result = 100;
				}
				return result;
				//TODO:if itemsbyorder insert fail revert the order insert
			}
            catch (Exception ex)
            {

				return -100;
            }
			
		}

	}
}
