using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NUnit.Framework;
using TestContext = Microsoft.VisualStudio.TestTools.UnitTesting.TestContext;

namespace IsolatedByInheritanceAndOverride.Test
{
    /// <summary>
    ///     OrderServiceTest 的摘要描述
    /// </summary>
    [TestFixture]
    public class OrderServiceTest
    {
        private TestContext testContextInstance;

        /// <summary>
        ///     取得或設定提供目前測試回合
        ///     的相關資訊與功能的測試內容。
        /// </summary>
        public TestContext TestContext
        {
            get { return testContextInstance; }
            set { testContextInstance = value; }
        }

        public OrderServiceTest()
        {
            //
            // TODO:  在此加入建構函式的程式碼
            //
        }

        [Test]
        public void Test_SyncBookOrders_3_Orders_Only_2_book_order()
        {
            var orderServiceForTest = new OrderServiceForTest();
            orderServiceForTest.SetOrders(new List<Order>()
            {
                new Order() {Type = "Book"},
                new Order() {Type = "CD"},
                new Order() {Type = "Book"},
            });

            var mockBookDao = Substitute.For<IBookDao>();
            orderServiceForTest.SetBookDao(mockBookDao);

            orderServiceForTest.SyncBookOrders();

            mockBookDao.Received(2).Insert(Arg.Is<Order>(x => x.Type == "Book"));
        }
    }

    public class OrderServiceForTest : OrderService
    {
        private IBookDao _dao;
        private List<Order> _orders;

        public void SetOrders(List<Order> orders)
        {
            _orders = orders;
        }

        protected override IBookDao GetBookDao()
        {
            return _dao;
        }

        protected override List<Order> GetOrders()
        {
            return _orders;
        }

        public void SetBookDao(IBookDao mockBookDao)
        {
            _dao = mockBookDao;
        }
    }
}