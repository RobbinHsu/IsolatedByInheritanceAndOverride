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
        private IBookDao _mockBookDao;
        private OrderServiceForTest _orderServiceForTest;
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

        [SetUp]
        public void Setup()
        {
            _orderServiceForTest = new OrderServiceForTest();
            _mockBookDao = Substitute.For<IBookDao>();
        }

        [Test]
        public void Test_SyncBookOrders_3_Orders_Only_2_book_order()
        {
            GivenOrders(
                CreateOrder("Book"),
                CreateOrder("CD"),
                CreateOrder("Book")
            );

            _orderServiceForTest.SetBookDao(_mockBookDao);

            _orderServiceForTest.SyncBookOrders();

            BookDaoShouldInsertTimes(2);
        }

        private void GivenOrders(params Order[] orders)
        {
            _orderServiceForTest.SetOrders(orders.ToList());
        }


        private void BookDaoShouldInsertTimes(int times)
        {
            _mockBookDao.Received(times).Insert(Arg.Is<Order>(x => x.Type == "Book"));
        }

        private static Order CreateOrder(string type)
        {
            return new Order() {Type = type};
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