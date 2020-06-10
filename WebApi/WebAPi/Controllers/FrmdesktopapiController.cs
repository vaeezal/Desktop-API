using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using WebAPi.Models;

namespace WebAPi.Controllers
{
    public class FrmdesktopapiController : ApiController
    {
        //This hard coded data is only sample when table is filled.    
        public static List<Frmdesktopapi> listData = new List<Frmdesktopapi>
        {
            new Frmdesktopapi { itemName = "Branch", itemContent = "{\"Branch\":[{\"branchid\":\"1\",\"branchname\":\"salatiga\"},{\"branchid\":\"2\",\"branchname\":\"semarang\"}]}", itemType = 1 },
            new Frmdesktopapi { itemName = "Employee", itemContent = "{\"Employee\":[{\"employeeid\":\"1\",\"employeename\":\"andi\"},{\"employeeid\":\"2\",\"employeename\":\"budi\"}]}", itemType = 1 },
            new Frmdesktopapi { itemName = "Supplier", itemContent = "<ArrayOfSupplier><Supplier><supplierid>1</supplierid><suppliername>Siemens</suppliername></Supplier><Supplier><supplierid>2</supplierid><suppliername>GE</suppliername></Supplier></ArrayOfSupplier>", itemType = 2 },
            new Frmdesktopapi { itemName = "Product", itemContent = "{\"Product\":[{\"productid\":\"1\",\"productname\":\"conveyorbot\"},{\"productid\":\"2\",\"productname\":\"pipetbot\"}]}", itemType = 1 },
            new Frmdesktopapi { itemName = "Customer", itemContent = "<ArrayOfCustomers><Customer><customerid>1</customerid><customername>Kalbe</customername></Customer><Customer><customerid>2</customerid><customername>Novartis</customername></Customer></ArrayOfCustomers>", itemType = 2 }
        };

        [HttpGet]
        public List<Frmdesktopapi> RetrieveList(string itemName)
        {
            List<Frmdesktopapi> retrieveList = null;
            if ((listData != null || listData.Find(r => (r.itemName == itemName)) != null) && itemName != null)
            {
                retrieveList = listData.FindAll(r => (r.itemName.ToUpper() == itemName.ToUpper())).ToList();
            }
            return retrieveList;
        }

        [HttpGet]
        public string Retrieve(string itemName)
        {
            string retrieveStr = null;
            if (itemName != null) { 
                if (listData.Find(r => (r.itemName.ToUpper() == itemName.ToUpper())) != null && itemName != null)
                {
                    retrieveStr = listData.Find(r => (r.itemName.ToUpper() == itemName.ToUpper())).itemName;
                }
            }
            return retrieveStr;
        }

        [HttpGet]
        public int GetType(string itemName)
        {
            int itemType = 0;
            if (itemName != null)
            {
                if (listData.Find(r => (r.itemName.ToUpper() == itemName.ToUpper())) != null)
                {
                    itemType = listData.Find(r => (r.itemName.ToUpper() == itemName.ToUpper())).itemType;
                }
            }
            return itemType;
        }

        [HttpGet]
        public List<Frmdesktopapi> Get()
        {
            return listData;
        }

        public Frmdesktopapi GetRow(string itemName)
        {
            return listData.Find((r) => r.itemName.ToUpper() == itemName.ToUpper());
        }

        [HttpPost]
        public void Register(string itemName, string itemContent, int itemType)
        {
            try
            {
                Frmdesktopapi listDataTemp = GetRow(itemName);
                if (listDataTemp == null)
                {
                    Frmdesktopapi rowData = new Frmdesktopapi();
                    rowData.itemName = itemName;
                    rowData.itemContent = itemContent;
                    rowData.itemType = itemType;
                    listData.Add(rowData);
                }
            }
            catch (Exception ex) //some other exception
            {
                Console.WriteLine(ex.ToString());
            }
        }

        //[HttpPost]
        //public bool Register(Frmdesktopapi rowData)
        //{
        //    bool trueOrFalse = false;
        //    try
        //    {
        //        Frmdesktopapi listDataTemp = GetRow(rowData.itemName);
        //        if (listDataTemp == null)
        //        {
        //            listData.Add(rowData);
        //            trueOrFalse = true;
        //        }
        //        return trueOrFalse;
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        [HttpPut]
        public bool Change(Frmdesktopapi rowData)
        {
            try
            {
                Boolean trueOrFalse = false;
                Frmdesktopapi listDataTemp = GetRow(rowData.itemName);
                listData.Remove(listDataTemp);

                if (listDataTemp != null)
                {
                    listData.Add(rowData);
                    trueOrFalse = true;
                }
                return trueOrFalse;
            }
            catch
            {
                return false;
            }
        }

        [HttpDelete]
        public void Deregister(string itemName)
        {
            try
            {
                var itemToRemove = listData.Find((r) => r.itemName == itemName);
                listData.Remove(itemToRemove);
            }
            catch (Exception ex) //some other exception
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}