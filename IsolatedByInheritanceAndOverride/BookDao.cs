using System;

namespace IsolatedByInheritanceAndOverride
{
    public class BookDao : IBookDao
    {
        public void Insert(Order order)
        {
            // directly depend on some web service
            //var client = new HttpClient();
            //var response = client.PostAsync<Order>("http://api.joey.io/Order", order, new JsonMediaTypeFormatter()).Result; 
            //response.EnsureSuccessStatusCode();
            throw new NotImplementedException();
        }
    }
}