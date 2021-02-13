using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using MssqlTestApp.Entities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MssqlTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //load data from xls file
            //LoadDataToDb();
            //task #2
            //DividePaymentBySum();
            //task #2.1
            //CountReceivable();
            //task #2.2
            //CountDepthOfReceivable();
            //task #1
            //CountNextContactDate();


            Console.ReadKey();

        }

        public static void DividePaymentBySum()
        {
            using (OrdersContext context = new OrdersContext())
            {
                var customers = context.Orders.GroupBy(customer => customer.CustomerId).Select(selector => selector.Key);
                foreach (var customer in customers)
                {
                    IEnumerable<Order> customerOrders = context.Orders.Where(order => order.CustomerId == customer).OrderBy(a => a.OrderDate);
                    var paySum = context.PaySum.Where(paysum => paysum.CustomerId == customer).FirstOrDefault().Pay;
                    Console.WriteLine(customer + ": sum of orders -" + customerOrders.Sum(a => a.SumOrd) + " PaySum - " + paySum);
                    Console.WriteLine();
                }
            }
        }
        public static void CountReceivable()
        {
            using (OrdersContext context = new OrdersContext())
            {
                var customers = context.Orders.GroupBy(customer => customer.CustomerId).Select(selector => selector.Key);
                foreach (var customer in customers)
                {
                    IEnumerable<Order> customerOrders = context.Orders.Where(order => order.CustomerId == customer).OrderBy(a => a.OrderDate);
                    var paySum = context.PaySum.Where(paysum => paysum.CustomerId == customer).FirstOrDefault().Pay;
                    decimal receivable = paySum - customerOrders.Sum(a => a.SumOrd);
                    Console.WriteLine("Receivables of " + customer + ": " + (receivable < 0 ? receivable : 0));
                    Console.WriteLine();
                }
            }
        }
        public static void CountDepthOfReceivable()
        {
            using (OrdersContext context = new OrdersContext())
            {
                var customers = context.Orders.GroupBy(customer => customer.CustomerId).Select(selector => selector.Key);
                foreach (var customer in customers)
                {
                    IEnumerable<Order> customerOrders = context.Orders.Where(order => order.CustomerId == customer).OrderBy(a => a.OrderDate);
                    var paySum = context.PaySum.Where(paysum => paysum.CustomerId == customer).FirstOrDefault().Pay;
                    DateTime finDate = new DateTime(2019, 2, 1);
                    DateTime lastPaidDate = new DateTime();
                    foreach (var item in customerOrders)
                    {
                        if ((paySum -= item.SumOrd) < 0)
                        {
                            lastPaidDate = item.OrderDate;
                        }
                    }
                    if (lastPaidDate != default(DateTime))
                    {

                        TimeSpan timeSpan = finDate.Subtract(lastPaidDate);
                        Console.WriteLine("Custome's " + customer + " depth is " + timeSpan.Days + " days.");
                    }



                }
            }
        }
        public static void CountNextContactDate()
        {
            using (OrdersContext context = new OrdersContext())
            {
                var customers = context.Orders.GroupBy(customer => customer.CustomerId).Select(selector => selector.Key);
                foreach (var customer in customers)
                {
                    IEnumerable<Order> customerOrders = context.Orders.Where(order => order.CustomerId == customer).OrderBy(a => a.OrderDate);
                    int periodicitiDays = context.Periodicities.Where(periodiciti => periodiciti.CustomerId == customer).FirstOrDefault().Days;
                    DateTime dateOfPenultOrder = customerOrders.ElementAt(customerOrders.Count() - 2).OrderDate;
                    while (periodicitiDays > 0)
                    {
                        if (dateOfPenultOrder.DayOfWeek != DayOfWeek.Saturday &&
                            dateOfPenultOrder.DayOfWeek != DayOfWeek.Sunday &&
                            !context.Holidays.Any(a => a.DateH == dateOfPenultOrder))
                        {
                            dateOfPenultOrder = dateOfPenultOrder.AddDays(1);
                            periodicitiDays--;
                        }
                        else
                        {
                            dateOfPenultOrder = dateOfPenultOrder.AddDays(1);
                            continue;
                        }
                    }
                    Console.WriteLine(customer + " - "+ dateOfPenultOrder);



                }
            }
        }
        public static void LoadDataToDb()
        {
            using (OrdersContext Context = new OrdersContext())
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(Environment.CurrentDirectory + "\\data.xlsx")))
                {

                    var ordersSheet = excelPackage.Workbook.Worksheets[1];
                    var periodisitiSheet = excelPackage.Workbook.Worksheets[2];
                    var holidaySheet = excelPackage.Workbook.Worksheets[3];
                    var paySumSheet = excelPackage.Workbook.Worksheets[4];
                    for (int i = 2; i <= ordersSheet.Dimension.End.Row; i++)
                    {
                        Context.Orders.Add(new Order(ordersSheet.Cells[i, 1].Value.ToString(),
                            Convert.ToDateTime(ordersSheet.Cells[i, 2].Value),
                            ordersSheet.Cells[i, 3].Value.ToString(),
                            Convert.ToDecimal(ordersSheet.Cells[i, 4].Value)));
                    }

                    Context.SaveChanges();
                    for (int i = 2; i <= periodisitiSheet.Dimension.End.Row; i++)
                    {
                        Context.Periodicities.Add(new Periodicity(periodisitiSheet.Cells[i, 1].Value.ToString(), Convert.ToInt32(periodisitiSheet.Cells[i, 2].Value)));
                    }
                    Context.SaveChanges();

                    Context.SaveChanges();
                    for (int i = 2; i <= paySumSheet.Dimension.End.Row; i++)
                    {
                        Context.PaySum.Add(new PaySum(paySumSheet.Cells[i, 1].Value.ToString(), Convert.ToDecimal(paySumSheet.Cells[i, 2].Value)));
                    }
                    Context.SaveChanges();

                    for (int i = 2; i <= holidaySheet.Dimension.End.Row; i++)
                    {
                        Context.Holidays.Add(new Holiday(Convert.ToDateTime(holidaySheet.Cells[i, 1].Value)));
                    }
                    Context.SaveChanges();
                }
            }

        }
    }
}

