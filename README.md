# Web_Task1
*Very First Please Execute the DBScript File

Version:.Net Core 3.1 Visual Studio 2019 SQL Server

#Get Methods http://localhost:7010/api/Main/GetCustomersNameList

http://localhost:7010/api/Main/GetCustomersDetailsByID?customerID=3

http://localhost:7010/api/Main/GetAllItemCode

http://localhost:7010/api/Main/GetItemsDetailsByID?itemID=1

http://localhost:7010/api/Main/GetAllOrders

http://localhost:7010/api/Main/GetOrdersById?id=24

#Post Methods http://localhost:7010/api/Main/SaveOrder

{ "ItemId": "", "CustomerId": "1", "InvoiceNo": "4444", "ReferenceNo": "4444", "Note": "ww", "InvoiceDate": "2020-09-11T05:05:08.730Z", "ItemList":[ { "ItemId": "1", "ItemCode": "S100", "ItemName": "0", "Price": "2500", "Description": "2500", "Note": "2500", "Quantity": "2" },{ "ItemId": "0", "ItemCode": "S200", "ItemName": "0", "Price": "2500", "Description": "2500", "Note": "2500", "Quantity": "2" },{ "ItemId": "3", "ItemCode": "S300", "ItemName": "0", "Price": "75000", "Description": "2500", "Note": "2500", "Quantity": "2" }]

}
