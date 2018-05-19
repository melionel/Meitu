using System;
using System.Collections.Generic;
using System.Linq;

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

        public Func<User, Goods, DateTime, bool> IsStatisfiedGoods;

        public Coupon(double discount, double minPrice, List<Goods> recGoodsList, Func<User, Goods, DateTime, bool> func)
        {
            Discount = discount;
            MinPrice = minPrice;
            RecommondGoodsList = recGoodsList;
            IsStatisfiedGoods = func;
        }

        public override string ToString()
        {
            return string.Format("MinPrice:{0}\tDiscount{1}\n", MinPrice, Discount);
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
    }

    class Program
    {
        static void Main(string[] args)
        {
            var goodsList = new List<Goods>();
            var couponList = new List<Coupon>();
            var user = new User("a", UserKind.Black, "aaa", goodsList, couponList);

            var finalOrders = GetBestOrderPlan(user, true, DateTime.Now);
            foreach (var order in finalOrders)
            {
                PrintOneOrder(order);
            }
        }

        static List<Order> GetBestOrderPlan(User user, bool recommended, DateTime date)
        {
            var result = new List<Order>();

            var leftCoupons =
                GetAllAvailableCoupons(user, user.GoodsList, user.CouponList, date)
                    .OrderByDescending(c => c.Discount)
                    .ThenBy(c => c.MinPrice)
                    .ToList();
            var leftGoods = new List<Goods>(user.GoodsList); 

            while (leftGoods.Count > 0)
            {
                AddOneBestOrder(user, ref leftGoods, ref leftCoupons, date, true, ref result);
                // update avaliable left coupons
                leftCoupons =
                    GetAllAvailableCoupons(user, leftGoods, leftCoupons, date)
                        .OrderByDescending(c => c.Discount)
                        .ThenBy(c => c.MinPrice)
                        .ToList();
            }

            return result;
        }

        static List<Coupon> GetAllAvailableCoupons(User user, List<Goods> curGoods, List<Coupon> curCoupons, DateTime date)
        {
            var availableCoupons = new List<Coupon>();
            foreach (var c in curCoupons)
            {
                var availableGoods = curGoods.Where(g => c.IsStatisfiedGoods(user, g, date));
                if (availableGoods.Sum(g => g.Price) > 0.8*c.MinPrice)
                {
                    availableCoupons.Add(c);
                }
            }
            return availableCoupons;
        }

        static private void AddOneBestOrder(User user, ref List<Goods> leftGoods, ref List<Coupon> leftCoupons, DateTime date, bool recommended, ref List<Order> finalOrders)
        {
            var order = new Order();
            if (leftCoupons.Count == 0)
            {
                // no left coupons, add all goods to the order
                foreach (var g in leftGoods)
                {
                    order.GoodsList.Add(g);
                }
                
                finalOrders.Add(order);
                leftGoods.Clear();
            }
            else
            {
                var curCoupon = leftCoupons.First();

                // get all available goods for this coupon and order by the price descending
                var availableGoods = leftGoods.Where(g => curCoupon.IsStatisfiedGoods(user, g, date)).OrderByDescending(g => g.Price).ToList();
                // iterate on all goods to get enough goods to use this coupon
                var totalPrice = 0.0;
                var recommendedGoods = new List<Goods>();
                foreach (var g in availableGoods)
                {
                    order.GoodsList.Add(g);
                    totalPrice += g.Price;
                    if (totalPrice >= curCoupon.MinPrice)
                    {
                        // we have selected enough goods to use cur coupon
                        break;
                    }
                }

                // no enough, we can recommended goods for user
                if (totalPrice < curCoupon.MinPrice && recommended && curCoupon.RecommondGoodsList.Count > 0)
                {
                    // currently we only recommend one goods
                    var recGoods =
                        curCoupon.RecommondGoodsList.Where(g => g.Price >= (curCoupon.MinPrice - totalPrice))
                            .OrderBy(g => g.Price)
                            .FirstOrDefault();
                    if (recGoods != null)
                    {
                        recommendedGoods.Add(recGoods);
                        totalPrice += recGoods.Price;
                    }
                }

                // if enough goods have been selected, generate a order using this coupon; otherwise abandon this coupon
                if (totalPrice >= curCoupon.MinPrice)
                {
                    foreach (var g in order.GoodsList)
                    {
                        leftGoods.Remove(g);
                    }
                    leftCoupons.Remove(curCoupon);

                    foreach (var rg in recommendedGoods)
                    {
                        order.RecomendedGoodsList.Add(rg);
                    }

                    finalOrders.Add(order);
                }
                else
                {
                    leftCoupons.Remove(curCoupon);
                }
            }
        }

        private static void PrintOneOrder(Order order)
        {
            Console.WriteLine("All goods:");
            foreach (var g in order.GoodsList)
            {
                Console.WriteLine(g.ToString());
            }

            if (order.RecomendedGoodsList.Count > 0)
            {
                Console.WriteLine("Recomended goods:");
                foreach (var g in order.RecomendedGoodsList)
                {
                    Console.WriteLine(g.ToString());
                }
            }

            if (order.CouponList.Count == 0)
            {
                Console.WriteLine("No coupon used.");
            }
            else
            {
                Console.WriteLine("All coupons:");
                foreach (var c in order.CouponList)
                {
                    Console.WriteLine(c.ToString());
                }
            }

            Console.WriteLine("Order total price:" + order.GetTotalPrice());
            Console.WriteLine("Order total discount:" + order.GetTotalDiscount());
            Console.WriteLine("Order total tax:" + order.GetTotalTax());
            Console.WriteLine("Order total income:" + order.GetTotalIncome());
        }
    }
}
