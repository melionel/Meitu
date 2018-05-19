using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace meitu
{
    public enum UserKind
    {
        Common,
        Black,
        Golden
    }

    public class Goods
    {
        public string Id;
        public double Price;
        public double Tax;
        public double Income;
        public string Shop;
        public string Source;
        public string Brand;
        

        public Goods(string id, double price, double tax, double income, string shop, string source, string brand)
        {
            Id = id;
            Price = price;
            Tax = tax;
            Income = income;
            Shop = shop;
            Source = source;
            Brand = brand;
        }
    }

    public class User
    {
        public string Id;
        public UserKind Kind;
        public string Address;

        public User(string id, UserKind kind, string address)
        {
            Id = id;
            Kind = kind;
            Address = address;
        }
    }

    public class Coupon
    {
        public double Discount;
        public double MinPrice;

        public Func<User, Goods> IsStatisfiedGoods;

        public Coupon(double discount, double minPrice, Func<User, Goods> func)
        {
            Discount = discount;
            MinPrice = minPrice;
            IsStatisfiedGoods = func;
        }
    }

    public class Order
    {
        public List<Goods> GoodsList;
        public List<Coupon> CouponList;

        public Order()
        {
            GoodsList = new List<Goods>();
            CouponList = new List<Coupon>();
        }

        public double GetTotalPrice()
        {
            return GoodsList.Sum(g => g.Price);
        }

        public double GetTotalDiscount()
        {
            return CouponList.Sum(c => c.Discount);
        }

        public double GetTotalIncome()
        {
            return GoodsList.Sum(g => g.Income);
        }

        public double GetTotalTax()
        {
            return GoodsList.Sum(g => g.Tax);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
        }
    }
}
