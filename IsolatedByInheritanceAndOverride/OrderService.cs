using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
//using System.Net.Http;
//using System.Net.Http.Formatting;
using System.Text;

namespace IsolatedByInheritanceAndOverride
{
    public class OrderService
    {
        private string _filePath = @"C:\temp\testOrders.csv";

        public void SyncBookOrders()
        {
            var orders = GetOrders();

            // only get orders of book
            var ordersOfBook = orders.Where(x => x.Type == "Book");

            var bookDao = GetBookDao();
            foreach (var order in ordersOfBook)
            {
                bookDao.Insert(order);
            }
        }

        protected virtual IBookDao GetBookDao()
        {
            return new BookDao();
        }

        protected virtual List<Order> GetOrders()
        {
            // parse csv file to get orders
            var result = new List<Order>();

            // directly depend on File I/O
            using (var sr = new StreamReader(_filePath, Encoding.UTF8))
            {
                var rowCount = 0;

                while (sr.Peek() > -1)
                {
                    rowCount++;

                    var content = sr.ReadLine();

                    // Skip CSV header line
                    if (rowCount > 1)
                    {
                        var line = content.Trim().Split(',');

                        result.Add(Mapping(line));
                    }
                }
            }

            return result;
        }

        private Order Mapping(string[] line)
        {
            var result = new Order
            {
                ProductName = line[0],
                Type = line[1],
                Price = Convert.ToInt32(line[2]),
                CustomerName = line[3]
            };

            return result;
        }
    }
}