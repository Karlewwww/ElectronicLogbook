﻿using ElectronicLogbookModel;
using ElectronicLogbookFunction;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.Routing;

namespace ElectronicLogbookWeb.Controllers
{
    public class EmployeeLogController : BaseController
    {
        private IFEmployeeLog _iFEmployeeLog;
        private AndersonCRMFunction.IFEmployee _iFEmployee;
        public EmployeeLogController()
        {
            _iFEmployeeLog = new FEmployeeLog();
            _iFEmployee = new AndersonCRMFunction.FEmployee();
            
        }
        #region Create
        [HttpGet]
        public ActionResult Create(EmployeeLog employeeLog)
        { 
                return View(employeeLog);
        }
        [HttpPost]
        public ActionResult CreateLog(EmployeeLog employeeLog)
        {
            var employee = _iFEmployee.Read(employeeLog.EmployeeNumber, employeeLog.Pin);
            var logname = _iFEmployeeLog.Readlogtype(employeeLog.LogTypeId);
            bool IsSuccess = employee.EmployeeId != 0 && employee.Pin == employeeLog.Pin;
            employeeLog.LogName = logname.Name;
            employeeLog.EmployeeId = employee.EmployeeId;
            employeeLog.SuccesLogin = IsSuccess;
            employeeLog = _iFEmployeeLog.Create(UserId, employeeLog);
            employeeLog.EmployeeImageBase64 = employee.EmployeeImageBase64;

            employeeLog.EmployeeNumber = string.Empty;
            employeeLog.Pin = string.Empty;
            //return View(employeeLog);
           return RedirectToAction("Create", new RouteValueDictionary(employeeLog));
        }
        #endregion
        #region Read
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Read()
        {
            return Json(_iFEmployeeLog.Read());
        }
        #endregion
        #region Update
        [HttpGet]
        public ActionResult Update(int employeeLogId)
        {
            return View(_iFEmployeeLog.Read(employeeLogId));
        }
        [HttpPost]
        public ActionResult Update(EmployeeLog employeeLog)
        {
            _iFEmployeeLog.Update(UserId, employeeLog);
            return RedirectToAction("Index");
        }
        #endregion
        #region Delete
        [HttpDelete]
        public JsonResult Delete(int id)
        {
            _iFEmployeeLog.Delete(id);
            return Json(string.Empty);
        }
        #endregion
    }
}
