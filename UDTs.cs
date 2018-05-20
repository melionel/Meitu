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

    public enum GoodsType
    {
        Default,
        HealthCare,
        Cosme,
        SkinCare
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
        public GoodsType Type;

        public Goods(string id, double price, double tax, double income, string shop, string source, string brand, GoodsType type = GoodsType.Default)
        {
            Id = id;
            Price = price;
            Tax = tax;
            Income = income;
            Shop = shop;
            Source = source;
            Brand = brand;
            Type = type;
        }

        public override string ToString()
        {
            return string.Format("Id:{0}\tPrice:{1}\tTax:{2}\tIncome:{3}\n", Id, Price, Tax, Income);
        }
    }

    public class User
    {
        public string Id;
        public UserKind Kind;
        public string Address;

        public List<Goods> GoodsList;
        public List<Coupon> CouponList;

        public User(string id, UserKind kind, string address, List<Goods> goodses, List<Coupon> coupons)
        {
            Id = id;
            Kind = kind;
            Address = address;
            GoodsList = goodses;
            CouponList = coupons;
        }
    }

    public class Coupon
    {
        public double Discount;
        public double MinPrice;
        public List<Goods> RecommondGoodsList;

        public Func<Goods, DateTime, bool> IsStatisfiedGoods;

        public Coupon(double discount, double minPrice, List<Goods> recGoodsList, Func<Goods, DateTime, bool> func)
        {
            Discount = discount;
            MinPrice = minPrice;
            RecommondGoodsList = recGoodsList;
            IsStatisfiedGoods = func;
        }

        public override string ToString()
        {
            return string.Format("MinPrice:{0}\tDiscount:{1}\n", MinPrice, Discount);
        }
    }

    public class Order
    {
        public List<Goods> GoodsList;
        public List<Coupon> CouponList;
        public List<Goods> RecomendedGoodsList;

        public Order()
        {
            GoodsList = new List<Goods>();
            CouponList = new List<Coupon>();
            RecomendedGoodsList = new List<Goods>();
        }

        public double GetTotalPrice()
        {
            return GoodsList.Sum(g => g.Price) + RecomendedGoodsList.Sum(g => g.Price);
        }

        public double GetTotalDiscount()
        {
            return CouponList.Count > 0 ? CouponList.Sum(c => c.Discount) : 0.0;
        }

        public double GetTotalIncome()
        {
            return GoodsList.Sum(g => g.Income) + RecomendedGoodsList.Sum(g => g.Income);
        }

        public double GetTotalTax()
        {
            return GoodsList.Sum(g => g.Tax) + RecomendedGoodsList.Sum(g => g.Tax);
        }

        public double GetFinalPrice()
        {
            return GetTotalPrice() - GetTotalDiscount();
        }

        public double GetDiscountRate()
        {
            return GetTotalDiscount()/(GetTotalPrice() + GetTotalTax());
        }
    }
}
