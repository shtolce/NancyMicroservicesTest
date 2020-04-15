using Nancy;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart
{
    public interface IShoppingCartStore
    {
        IShoppingCartStore Get(int userId);
        void AddItems(string[] scIt,IEventStore evS);
        void Save(IShoppingCartStore i);
        void RemoveItems(int[] scIt, IEventStore evS);

    }
    public interface IProductcatalogClient
    {
        string[] Get(int userId);
        Task<string[]> GetShoppingCartItems(int[] ids);
    }
    public interface IEventStore
    {
        string Get(int userId);
    }
    
    public class ShoppingCartModule : NancyModule
    {
        public ShoppingCartModule(IShoppingCartStore shoppingCartStore,IProductcatalogClient productcatalog,IEventStore eventStore):base("/shoppingcart")
        {
            Get("/{userid:int}", parameters =>
            {
                var userId = (int)parameters.userid;
                return shoppingCartStore.Get(userId);

            });

            Post("/{userid:int}/items",async(parameters,_)=> 
            {
                var productcatalogIds = this.Bind<int[]>();
                var userId = (int)parameters.userid;
                var shoppingCart = shoppingCartStore.Get(userId);
                var shoppingCartItems = await productcatalog.GetShoppingCartItems(productcatalogIds).ConfigureAwait(false);
                shoppingCart.AddItems(shoppingCartItems, eventStore);
                shoppingCartStore.Save(shoppingCart);
                return shoppingCart;
            });

            Delete("/{userid:int}/items", parameters => 
            {
                var productcatalogIds = this.Bind<int[]>();
                var userId = (int)parameters.userid;
                var shoppingCart = shoppingCartStore.Get(userId);
                shoppingCart.RemoveItems(productcatalogIds, eventStore);
                return shoppingCart;
            });






        }
    }
}
